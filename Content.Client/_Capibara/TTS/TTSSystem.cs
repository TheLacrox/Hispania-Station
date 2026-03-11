// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.IO;
using Content.Shared._Capibara.CCVar;
using Robust.Client.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Client._Capibara.TTS;

/// <summary>
/// Client-side TTS playback system. Receives assembled audio from
/// TextToSpeechStreamSystem and plays it at the entity's position.
/// </summary>
public sealed class TTSSystem : EntitySystem
{
    [Dependency] private readonly Robust.Client.Audio.AudioSystem _audio = default!;
    [Dependency] private readonly IAudioManager _audioManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ISharedPlayerManager _player = default!;

    private ISawmill _log = default!;
    private float _volume = 0.5f;
    private bool _enabled;

    public override void Initialize()
    {
        base.Initialize();
        _log = Logger.GetSawmill("capibara.tts.client");

        _cfg.OnValueChanged(CapibaraCCVars.TTSVolume, OnVolumeChanged, true);
        _cfg.OnValueChanged(CapibaraCCVars.TTSClientEnabled, OnEnabledChanged, true);

        SubscribeLocalEvent<TTSStreamReadyEvent>(OnStreamReady);
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _cfg.UnsubValueChanged(CapibaraCCVars.TTSVolume, OnVolumeChanged);
        _cfg.UnsubValueChanged(CapibaraCCVars.TTSClientEnabled, OnEnabledChanged);
    }

    private void OnEnabledChanged(bool enabled)
    {
        _enabled = enabled;
    }

    private void OnVolumeChanged(float volume)
    {
        _volume = volume;
    }

    private void OnStreamReady(TTSStreamReadyEvent ev)
    {
        if (!_enabled)
            return;

        if (ev.AudioData.Length == 0)
            return;

        if (!Exists(ev.SourceUid))
            return;

        try
        {
            // Detect format and load audio from raw bytes
            AudioStream audioStream;
            if (IsWavFormat(ev.AudioData))
                audioStream = _audioManager.LoadAudioWav(new MemoryStream(ev.AudioData));
            else
                audioStream = _audioManager.LoadAudioOggVorbis(new MemoryStream(ev.AudioData));

            // Volume modifier for whispers (like Starlight: 0.6x)
            var gain = ev.IsWhisper ? _volume * 0.6f : _volume;
            var volumeDb = SharedAudioSystem.GainToVolume(gain);
            var audioParams = AudioParams.Default.WithVolume(volumeDb);

            // Play at entity position, or globally if it's the local player
            if (ev.SourceUid != _player.LocalEntity)
                _audio.PlayEntity(audioStream, ev.SourceUid, null, audioParams);
            else
                _audio.PlayGlobal(audioStream, null, audioParams);
        }
        catch (Exception ex)
        {
            _log.Warning("Failed to play TTS audio for entity {0}: {1}", ev.SourceUid, ex.Message);
        }
    }

    private static bool IsWavFormat(byte[] data)
    {
        return data.Length > 4 && data[0] == 'R' && data[1] == 'I' && data[2] == 'F' && data[3] == 'F';
    }
}
