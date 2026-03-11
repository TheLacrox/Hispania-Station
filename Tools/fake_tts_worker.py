#!/usr/bin/env python3
"""
Fake TTS worker for testing the Redis TTS pipeline.
Consumes jobs from the 'tts_jobs' Redis list and publishes
a small test WAV audio response back via the reply channel.

Usage:
    python tools/fake_tts_worker.py [--host 127.0.0.1] [--port 6380]
"""

import argparse
import json
import struct
import math
import redis


def generate_test_tone(frequency=440, duration=0.5, sample_rate=22050):
    """Generate a small WAV file with a sine tone."""
    num_samples = int(sample_rate * duration)
    samples = []
    for i in range(num_samples):
        t = i / sample_rate
        # Sine wave with fade in/out
        envelope = min(i / 500, 1.0, (num_samples - i) / 500)
        value = int(32767 * envelope * 0.5 * math.sin(2 * math.pi * frequency * t))
        samples.append(struct.pack('<h', value))

    audio_data = b''.join(samples)

    # Build WAV header
    data_size = len(audio_data)
    wav = bytearray()
    wav.extend(b'RIFF')
    wav.extend(struct.pack('<I', 36 + data_size))
    wav.extend(b'WAVE')
    wav.extend(b'fmt ')
    wav.extend(struct.pack('<I', 16))       # chunk size
    wav.extend(struct.pack('<H', 1))        # PCM format
    wav.extend(struct.pack('<H', 1))        # mono
    wav.extend(struct.pack('<I', sample_rate))
    wav.extend(struct.pack('<I', sample_rate * 2))  # byte rate
    wav.extend(struct.pack('<H', 2))        # block align
    wav.extend(struct.pack('<H', 16))       # bits per sample
    wav.extend(b'data')
    wav.extend(struct.pack('<I', data_size))
    wav.extend(audio_data)

    return bytes(wav)


def main():
    parser = argparse.ArgumentParser(description='Fake TTS worker')
    parser.add_argument('--host', default='127.0.0.1')
    parser.add_argument('--port', type=int, default=6380)
    args = parser.parse_args()

    r = redis.Redis(host=args.host, port=args.port, decode_responses=False)
    r.ping()
    print(f"Connected to Redis at {args.host}:{args.port}")
    print("Waiting for TTS jobs...")

    while True:
        # BLPOP blocks until a job is available
        result = r.blpop('tts_jobs', timeout=0)
        if result is None:
            continue

        _, job_bytes = result
        job = json.loads(job_bytes)

        job_id = job.get('id', '?')
        text = job.get('text', '')
        voice = job.get('voice_id', '')
        reply_channel = job.get('reply_channel', '')

        print(f"Got job {job_id}: voice={voice}, text=\"{text}\"")

        if not reply_channel:
            print(f"  No reply channel, skipping")
            continue

        # Generate a test tone (different frequency per voice for fun)
        freq = hash(voice) % 400 + 300  # 300-700 Hz based on voice
        audio = generate_test_tone(frequency=freq, duration=0.3)

        print(f"  Sending {len(audio)} bytes of test audio to {reply_channel}")

        # Send audio as a single chunk
        r.publish(reply_channel, audio)

        # Send end-of-stream marker (single byte 0x00)
        r.publish(reply_channel, b'\x00')

        print(f"  Done!")


if __name__ == '__main__':
    main()
