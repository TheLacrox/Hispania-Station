# TTS (Text-to-Speech) Setup Guide

## Overview

Capibara Station has a TTS system that converts in-game speech to audio. When players type in chat, their characters speak aloud with distinct voices based on accent/country.

The system has three components:
1. **Redis** — Message broker between the game server and TTS worker
2. **TTS Worker** (`tools/tts_worker.py`) — Generates speech audio from text
3. **Game Server** — Queues TTS jobs and streams audio to clients

## Prerequisites

- **Docker** (for Redis)
- **Python 3.11+** with pip
- **ffmpeg** in PATH (for audio format conversion)

## Quick Start

### 1. Start Redis

```bash
cd Estacion-Capibara
docker compose up -d
```

This starts Redis on port **6380** (not default 6379, to avoid conflicts with WSL Redis).

### 2. Install Python Dependencies

```bash
pip install redis piper-tts edge-tts
```

### 3. Download Piper Voice Models (for offline fallback)

```bash
mkdir -p tools/tts_voices
cd tools/tts_voices

# Spanish (Spain) voice
curl -L -o es_ES-davefx-medium.onnx "https://huggingface.co/rhasspy/piper-voices/resolve/main/es/es_ES/davefx/medium/es_ES-davefx-medium.onnx"
curl -L -o es_ES-davefx-medium.onnx.json "https://huggingface.co/rhasspy/piper-voices/resolve/main/es/es_ES/davefx/medium/es_ES-davefx-medium.onnx.json"

# Spanish (Mexico) voice
curl -L -o es_MX-ald-medium.onnx "https://huggingface.co/rhasspy/piper-voices/resolve/main/es/es_MX/ald/medium/es_MX-ald-medium.onnx"
curl -L -o es_MX-ald-medium.onnx.json "https://huggingface.co/rhasspy/piper-voices/resolve/main/es/es_MX/ald/medium/es_MX-ald-medium.onnx.json"
```

### 4. Start the TTS Worker

**With edge-tts as primary (best quality, needs internet):**
```bash
python tools/tts_worker.py --redis-port 6380 --backend edge
```

**With Piper as primary (offline, faster, lower quality):**
```bash
python tools/tts_worker.py --redis-port 6380 --backend piper
```

**Options:**
```
--redis-host     Redis host (default: 127.0.0.1)
--redis-port     Redis port (default: 6380)
--backend        Primary backend: piper or edge (default: piper)
--no-fallback    Disable fallback backend
--voices-dir     Piper voice models directory (default: tools/tts_voices)
--list-edge-voices  List available Spanish edge-tts voices
```

### 5. Configure the Game Server

In `bin/Content.Server/server_config.toml`:
```toml
[tts]
enabled = true
connection_string = "127.0.0.1:6380"
```

Or via server console:
```
cvar tts.enabled true
cvar tts.connection_string 127.0.0.1:6380
```

### 6. Start the Game Server

```bash
dotnet run --project Content.Server/Content.Server.csproj
```

## Architecture

```
Player speaks → EntitySpokeEvent → Server TTSSystem
  → Assigns voice (random per sex from 36 voice prototypes)
  → Cleans text (strips markup, numbers → words)
  → TTSClient pushes job to Redis queue "tts_jobs"
  → TTS Worker picks up job via BLPOP
  → Worker generates OGG audio (edge-tts or Piper + ffmpeg)
  → Worker publishes audio to Redis reply channel
  → Server receives audio via pub/sub
  → Server streams TTSHeaderEvent + TTSChunkEvent to PVS clients
  → Client assembles chunks → LoadAudioOggVorbis → PlayEntity/PlayGlobal
```

## TTS Backends

### edge-tts (Primary recommended)
- Uses Microsoft's free Neural TTS voices
- **45 distinct Spanish voices** from different countries (Argentina, Mexico, Spain, Colombia, Chile, etc.)
- Best quality, natural-sounding speech
- Requires internet connection
- No API key needed
- ~1s latency per request

### Piper TTS (Offline fallback)
- Fully offline, runs on CPU
- 2 Spanish models (es_ES, es_MX)
- Good quality but less variety than edge-tts
- Very fast (~0.3s per request)
- Models are ~60MB each

## Voice Prototypes

36 voices defined in `Resources/Prototypes/_Capibara/TTS/voices.yml`:
- **15 male** — each mapped to a different country accent
- **15 female** — each mapped to a different country accent
- **2 neutral** — Costa Rica voices
- **4 silicon** — for AI/robot characters

Voices are auto-assigned randomly based on character sex when they first speak.

## Client Settings

Players can control TTS in Options → Audio:
- **TTS Volume** slider
- **Enable TTS** checkbox

On first join, a popup asks the player if they want TTS enabled.

## CVars

| CVar | Default | Scope | Description |
|------|---------|-------|-------------|
| `tts.enabled` | `false` | Server | Master TTS enable on server |
| `tts.connection_string` | `localhost:6379` | Server | Redis connection string |
| `tts.client_enabled` | `true` | Client | Player's TTS preference |
| `tts.volume` | `0.5` | Client | TTS playback volume |
| `tts.popup_shown` | `false` | Client | Whether first-join popup was shown |

## Troubleshooting

### No TTS audio
1. Check Redis is running: `docker compose ps`
2. Check worker is running and processing jobs (worker window shows logs)
3. Check server config has `tts.enabled = true` and correct `connection_string`
4. Check client has TTS enabled in Options → Audio

### First message missing
Fixed by using `SubscribeAsync().Wait()` — ensures Redis subscription is confirmed before job is pushed.

### WSL Redis conflict (Windows)
If WSL has its own Redis on port 6379, it shadows Docker's Redis. That's why we use port **6380** for Docker Redis. If you still have issues, stop WSL Redis: `wsl -e sudo service redis-server stop`

### Worker shows "ffmpeg not found"
Install ffmpeg and ensure it's in PATH. On Windows: `winget install ffmpeg` or download from https://ffmpeg.org/

## Files

### Game Code
| Path | Purpose |
|------|---------|
| `Content.Shared/_Capibara/TTS/` | Components, prototypes, network events, CVars |
| `Content.Server/_Capibara/TTS/` | Server TTS system, Redis client, voice assignment |
| `Content.Client/_Capibara/TTS/` | Client audio playback, stream assembly, settings popup |
| `Resources/Prototypes/_Capibara/TTS/voices.yml` | Voice prototype definitions |
| `Resources/Locale/{en-US,es-ES}/_Capibara/tts/tts.ftl` | Locale strings |

### Infrastructure
| Path | Purpose |
|------|---------|
| `docker-compose.yml` | Docker Compose for Redis on port 6380 |
| `tools/tts_worker.py` | TTS worker (Piper + edge-tts) |
| `tools/tts_voices/` | Piper voice model files (.onnx) |
| `tools/fake_tts_worker.py` | Test worker that generates beep tones (for debugging) |
