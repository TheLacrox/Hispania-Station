#!/usr/bin/env python3
"""
TTS Worker for Capibara Station.

Consumes TTS jobs from the 'tts_jobs' Redis list and publishes
WAV audio back via the reply channel using Redis pub/sub.

Supports two backends:
  - Piper TTS (primary, offline, fast)
  - edge-tts (fallback, online, Microsoft Neural voices)

Usage:
    python tools/tts_worker.py [options]

Options:
    --redis-host    Redis host (default: 127.0.0.1)
    --redis-port    Redis port (default: 6380)
    --backend       Primary backend: piper or edge (default: piper)
    --fallback      Enable fallback backend (default: true)
    --voices-dir    Directory containing Piper voice models (default: tools/tts_voices)
    --piper-model   Default Piper model name (default: es_ES-davefx-medium)
    --edge-voice    Default edge-tts voice (default: es-ES-ElviraNeural)
    --list-edge-voices  List available edge-tts Spanish voices and exit
"""

import argparse
import asyncio
import io
import json
import logging
import struct
import sys
import time
import wave
from pathlib import Path

import redis

logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s [%(levelname)s] %(message)s",
    datefmt="%H:%M:%S",
)
log = logging.getLogger("tts_worker")

# Voice ID → Piper model mapping
# Maps game voice prototype IDs to Piper model files
# Piper only has 2 Spanish models, so we alternate between them
PIPER_VOICE_MAP: dict[str, str] = {
    "default": "es_ES-davefx-medium",
}

# Voice ID → edge-tts voice mapping
# Each voice maps to a unique Microsoft Neural voice for maximum variety
EDGE_VOICE_MAP: dict[str, str] = {
    "default": "es-ES-ElviraNeural",

    # Male voices — each a distinct accent/country
    "VoiceMaleAlvaro":    "es-ES-AlvaroNeural",      # Spain
    "VoiceMaleJorge":     "es-MX-JorgeNeural",       # Mexico
    "VoiceMaleTomas":     "es-AR-TomasNeural",        # Argentina
    "VoiceMaleGonzalo":   "es-CO-GonzaloNeural",     # Colombia
    "VoiceMaleLorenzo":   "es-CL-LorenzoNeural",     # Chile
    "VoiceMaleCarlos":    "es-HN-CarlosNeural",      # Honduras
    "VoiceMaleEmilio":    "es-DO-EmilioNeural",      # Dominican Republic
    "VoiceMaleAlex":      "es-PE-AlexNeural",        # Peru
    "VoiceMaleAlonso":    "es-US-AlonsoNeural",      # US Spanish
    "VoiceMaleManuel":    "es-CU-ManuelNeural",      # Cuba
    "VoiceMaleMarcelo":   "es-BO-MarceloNeural",     # Bolivia
    "VoiceMaleVictor":    "es-PR-VictorNeural",      # Puerto Rico
    "VoiceMaleSebastian": "es-VE-SebastianNeural",   # Venezuela
    "VoiceMaleMateo":     "es-UY-MateoNeural",       # Uruguay
    "VoiceMaleRodrigo":   "es-SV-RodrigoNeural",     # El Salvador

    # Female voices — each a distinct accent/country
    "VoiceFemaleElvira":    "es-ES-ElviraNeural",    # Spain
    "VoiceFemaleDalia":     "es-MX-DaliaNeural",     # Mexico
    "VoiceFemaleElena":     "es-AR-ElenaNeural",     # Argentina
    "VoiceFemaleSalome":    "es-CO-SalomeNeural",    # Colombia
    "VoiceFemaleCatalina":  "es-CL-CatalinaNeural",  # Chile
    "VoiceFemaleKarla":     "es-HN-KarlaNeural",     # Honduras
    "VoiceFemaleRamona":    "es-DO-RamonaNeural",    # Dominican Republic
    "VoiceFemaleCamila":    "es-PE-CamilaNeural",    # Peru
    "VoiceFemalePaloma":    "es-US-PalomaNeural",    # US Spanish
    "VoiceFemaleSofia":     "es-BO-SofiaNeural",     # Bolivia
    "VoiceFemaleXimena":    "es-ES-XimenaNeural",    # Spain (alt)
    "VoiceFemaleAndrea":    "es-EC-AndreaNeural",    # Ecuador
    "VoiceFemalePaola":     "es-VE-PaolaNeural",     # Venezuela
    "VoiceFemaleValentina": "es-UY-ValentinaNeural", # Uruguay
    "VoiceFemaleKarina":    "es-PR-KarinaNeural",    # Puerto Rico

    # Neutral voices
    "VoiceNeutralMaria": "es-CR-MariaNeural",        # Costa Rica
    "VoiceNeutralJuan":  "es-CR-JuanNeural",         # Costa Rica

    # Silicon voices — use more "robotic" sounding accents
    "VoiceSiliconJavier":   "es-GQ-JavierNeural",    # Equatorial Guinea
    "VoiceSiliconTeresa":   "es-GQ-TeresaNeural",    # Equatorial Guinea
    "VoiceSiliconFederico": "es-NI-FedericoNeural",  # Nicaragua
    "VoiceSiliconYolanda":  "es-NI-YolandaNeural",   # Nicaragua
}

# Voice ID → Piper speech rate (length_scale: lower = faster)
# Only used when piper is the backend (fallback)
PIPER_SPEED_MAP: dict[str, float] = {
    "default": 1.0,
}


def wav_to_ogg(wav_data: bytes) -> bytes | None:
    """Convert WAV bytes to OGG Vorbis bytes using ffmpeg."""
    import subprocess

    try:
        result = subprocess.run(
            [
                "ffmpeg", "-i", "pipe:0",
                "-f", "ogg",
                "-acodec", "libvorbis",
                "-ar", "22050",
                "-ac", "1",
                "-q:a", "4",
                "pipe:1",
            ],
            input=wav_data,
            capture_output=True,
            timeout=10,
        )
        if result.returncode != 0:
            log.error("ffmpeg WAV→OGG failed: %s", result.stderr.decode(errors="replace")[:200])
            return None
        return result.stdout
    except FileNotFoundError:
        log.error("ffmpeg not found — required for OGG conversion")
        return None
    except subprocess.TimeoutExpired:
        log.error("ffmpeg timed out during WAV→OGG conversion")
        return None


def mp3_to_ogg(mp3_data: bytes) -> bytes | None:
    """Convert MP3 bytes to OGG Vorbis bytes using ffmpeg."""
    import subprocess

    try:
        result = subprocess.run(
            [
                "ffmpeg", "-i", "pipe:0",
                "-f", "ogg",
                "-acodec", "libvorbis",
                "-ar", "22050",
                "-ac", "1",
                "-q:a", "4",
                "pipe:1",
            ],
            input=mp3_data,
            capture_output=True,
            timeout=10,
        )
        if result.returncode != 0:
            log.error("ffmpeg MP3→OGG failed: %s", result.stderr.decode(errors="replace")[:200])
            return None
        return result.stdout
    except FileNotFoundError:
        log.error("ffmpeg not found — required for OGG conversion")
        return None
    except subprocess.TimeoutExpired:
        log.error("ffmpeg timed out during MP3→OGG conversion")
        return None


class PiperBackend:
    """Offline TTS using Piper (rhasspy/piper)."""

    def __init__(self, voices_dir: str, default_model: str):
        self.voices_dir = Path(voices_dir)
        self.default_model = default_model
        self._loaded_voices: dict[str, object] = {}
        self.name = "piper"

    def _get_voice(self, model_name: str):
        """Load and cache a Piper voice model."""
        if model_name in self._loaded_voices:
            return self._loaded_voices[model_name]

        model_path = self.voices_dir / f"{model_name}.onnx"
        if not model_path.exists():
            log.warning("Piper model not found: %s", model_path)
            return None

        from piper import PiperVoice
        log.info("Loading Piper model: %s", model_path)
        voice = PiperVoice.load(str(model_path))
        self._loaded_voices[model_name] = voice
        return voice

    def synthesize(self, text: str, voice_id: str) -> bytes | None:
        """Generate OGG Vorbis audio from text. Returns OGG bytes or None on failure."""
        model_name = PIPER_VOICE_MAP.get(voice_id, PIPER_VOICE_MAP["default"])
        voice = self._get_voice(model_name)
        if voice is None:
            return None

        try:
            length_scale = PIPER_SPEED_MAP.get(voice_id, 1.0)

            buf = io.BytesIO()
            with wave.open(buf, "wb") as wav_file:
                voice.synthesize_wav(
                    text,
                    wav_file,
                    length_scale=length_scale,
                )
            wav_data = buf.getvalue()

            # Convert WAV to OGG Vorbis (RobustToolbox's LoadAudioOggVorbis)
            return wav_to_ogg(wav_data)
        except Exception as e:
            log.error("Piper synthesis failed: %s", e)
            return None


class EdgeTTSBackend:
    """Online TTS using Microsoft Edge Neural voices."""

    def __init__(self, default_voice: str):
        self.default_voice = default_voice
        self.name = "edge-tts"

    def synthesize(self, text: str, voice_id: str) -> bytes | None:
        """Generate OGG Vorbis audio from text. Returns OGG bytes or None on failure."""
        edge_voice = EDGE_VOICE_MAP.get(voice_id, EDGE_VOICE_MAP["default"])

        try:
            return asyncio.run(self._synthesize_async(text, edge_voice))
        except Exception as e:
            log.error("edge-tts synthesis failed: %s", e)
            return None

    async def _synthesize_async(self, text: str, voice: str) -> bytes | None:
        import edge_tts

        communicate = edge_tts.Communicate(text, voice)
        mp3_buf = io.BytesIO()

        async for chunk in communicate.stream():
            if chunk["type"] == "audio":
                mp3_buf.write(chunk["data"])

        mp3_data = mp3_buf.getvalue()
        if not mp3_data:
            return None

        # Convert MP3 to OGG Vorbis
        return mp3_to_ogg(mp3_data)


async def list_edge_voices():
    """List available Spanish edge-tts voices."""
    import edge_tts
    voices = await edge_tts.list_voices()
    spanish = [v for v in voices if v["Locale"].startswith("es-")]
    print(f"\nAvailable Spanish edge-tts voices ({len(spanish)}):\n")
    for v in sorted(spanish, key=lambda x: x["ShortName"]):
        print(f"  {v['ShortName']:30s}  {v['Gender']:8s}  {v['Locale']}")
    print()


def run_worker(args):
    """Main worker loop."""
    # Connect to Redis
    r = redis.Redis(host=args.redis_host, port=args.redis_port, decode_responses=False)
    r.ping()
    log.info("Connected to Redis at %s:%d", args.redis_host, args.redis_port)

    # Initialize backends
    primary = None
    fallback = None

    if args.backend == "piper":
        primary = PiperBackend(args.voices_dir, args.piper_model)
        if args.fallback:
            fallback = EdgeTTSBackend(args.edge_voice)
    else:
        primary = EdgeTTSBackend(args.edge_voice)
        if args.fallback:
            fallback = PiperBackend(args.voices_dir, args.piper_model)

    log.info("Primary backend: %s", primary.name)
    if fallback:
        log.info("Fallback backend: %s", fallback.name)
    else:
        log.info("No fallback backend configured")

    # Pre-load default Piper model
    if isinstance(primary, PiperBackend):
        log.info("Pre-loading default Piper model: %s", args.piper_model)
        primary._get_voice(args.piper_model)

    log.info("Waiting for TTS jobs on queue 'tts_jobs'...")

    while True:
        try:
            # BLPOP blocks until a job is available
            result = r.blpop("tts_jobs", timeout=5)
            if result is None:
                continue

            _, job_bytes = result
            job = json.loads(job_bytes)

            job_id = job.get("id", "?")
            text = job.get("text", "")
            voice_id = job.get("voice_id", "")
            reply_channel = job.get("reply_channel", "")

            if not reply_channel:
                log.warning("Job %s has no reply channel, skipping", job_id)
                continue

            if not text.strip():
                log.warning("Job %s has empty text, skipping", job_id)
                # Send end-of-stream so server doesn't hang
                r.publish(reply_channel, b"\x00")
                continue

            log.info(
                "Job %s: voice=%s, text=\"%s\" (%d chars)",
                job_id[:8], voice_id, text[:50], len(text),
            )

            # Try primary backend
            start = time.monotonic()
            audio = primary.synthesize(text, voice_id)
            elapsed = time.monotonic() - start

            if audio:
                log.info(
                    "  [%s] Generated %d bytes in %.2fs",
                    primary.name, len(audio), elapsed,
                )
            elif fallback:
                log.warning("  [%s] Failed, trying fallback...", primary.name)
                start = time.monotonic()
                audio = fallback.synthesize(text, voice_id)
                elapsed = time.monotonic() - start
                if audio:
                    log.info(
                        "  [%s] Generated %d bytes in %.2fs",
                        fallback.name, len(audio), elapsed,
                    )

            if audio:
                # Publish audio data
                r.publish(reply_channel, audio)
                # Send end-of-stream marker
                r.publish(reply_channel, b"\x00")
                log.info("  Sent to %s", reply_channel[:30] + "...")
            else:
                log.error("  All backends failed for job %s", job_id[:8])
                error_msg = f"ERROR:All TTS backends failed"
                r.publish(reply_channel, error_msg.encode("utf-8"))

        except redis.ConnectionError as e:
            log.error("Redis connection lost: %s — retrying in 5s", e)
            time.sleep(5)
            try:
                r.ping()
                log.info("Redis reconnected")
            except redis.ConnectionError:
                pass
        except KeyboardInterrupt:
            log.info("Shutting down...")
            break
        except Exception as e:
            log.error("Unexpected error: %s", e, exc_info=True)
            time.sleep(1)


def main():
    parser = argparse.ArgumentParser(description="Capibara Station TTS Worker")
    parser.add_argument("--redis-host", default="127.0.0.1", help="Redis host")
    parser.add_argument("--redis-port", type=int, default=6380, help="Redis port")
    parser.add_argument(
        "--backend", choices=["piper", "edge"], default="piper",
        help="Primary TTS backend (default: piper)",
    )
    parser.add_argument(
        "--no-fallback", dest="fallback", action="store_false",
        help="Disable fallback backend",
    )
    parser.add_argument(
        "--voices-dir", default="tools/tts_voices",
        help="Directory with Piper voice models",
    )
    parser.add_argument(
        "--piper-model", default="es_ES-davefx-medium",
        help="Default Piper model name",
    )
    parser.add_argument(
        "--edge-voice", default="es-ES-ElviraNeural",
        help="Default edge-tts voice",
    )
    parser.add_argument(
        "--list-edge-voices", action="store_true",
        help="List available Spanish edge-tts voices and exit",
    )
    args = parser.parse_args()

    if args.list_edge_voices:
        asyncio.run(list_edge_voices())
        return

    run_worker(args)


if __name__ == "__main__":
    main()
