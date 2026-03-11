// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Text.RegularExpressions;
using Content.Server.Chat.Systems;
using Content.Shared.GameTicking;
using Content.Shared._Capibara.CCVar;
using Content.Shared._Capibara.TTS;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Server._Capibara.TTS;

/// <summary>
/// Server-side TTS system. Listens for entity speech, sends text to the TTS service
/// via Redis, and streams audio chunks to nearby clients.
/// </summary>
public sealed partial class TTSSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ITTSClient _ttsClient = default!;

    private ISawmill _log = default!;
    private bool _enabled;

    public override void Initialize()
    {
        base.Initialize();
        _log = Logger.GetSawmill("capibara.tts");

        _cfg.OnValueChanged(CapibaraCCVars.TTSEnabled, OnEnabledChanged, true);

        // TextToSpeechComponent is added to humanoids via SharedHumanoidAppearanceSystem.SetBarkVoice.
        // For non-humanoid players, we add it on spawn.
        SubscribeLocalEvent<TextToSpeechComponent, EntitySpokeEvent>(OnEntitySpoke);
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);

        InitializeAssignVoice();
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _cfg.UnsubValueChanged(CapibaraCCVars.TTSEnabled, OnEnabledChanged);
    }

    private void OnEnabledChanged(bool enabled)
    {
        _enabled = enabled;
    }

    private void OnPlayerSpawnComplete(PlayerSpawnCompleteEvent args)
    {
        // Ensure all players get the TTS component, regardless of race
        EnsureComp<TextToSpeechComponent>(args.Mob);
    }

    private void OnEntitySpoke(EntityUid uid, TextToSpeechComponent component, EntitySpokeEvent args)
    {
        if (!_enabled)
            return;

        // Skip radio messages for Phase 1
        if (args.Channel != null)
            return;

        var voiceId = GetOrAssignVoice(uid, component);
        if (voiceId == null)
            return;

        var text = CleanTextForTTS(args.Message);
        if (string.IsNullOrWhiteSpace(text))
            return;

        _log.Debug("TTS request for entity {0}: voice={1}, text=\"{2}\"", uid, voiceId, text);

        var streamId = Guid.NewGuid();
        var isWhisper = args.IsWhisper;
        var netUid = GetNetEntity(uid);

        // Send header to PVS clients
        var headerEvent = new TTSHeaderEvent(streamId, netUid, isWhisper);
        var filter = Filter.Pvs(uid);
        RaiseNetworkEvent(headerEvent, filter);

        // Queue TTS generation
        _ttsClient.GenerateTTS(text, voiceId, TTSEffect.None,
            (data, isLast) =>
            {
                // Stream chunks to clients
                var chunkEvent = new TTSChunkEvent(streamId, data, isLast);
                RaiseNetworkEvent(chunkEvent, filter);
            },
            error =>
            {
                _log.Warning("TTS generation failed for entity {0}: {1}", uid, error);
                // Send empty final chunk so client knows to stop waiting
                var endEvent = new TTSChunkEvent(streamId, Array.Empty<byte>(), true);
                RaiseNetworkEvent(endEvent, filter);
            });
    }

    /// <summary>
    /// Clean text for TTS processing: strip markup tags, convert numbers to words.
    /// </summary>
    private static string CleanTextForTTS(string text)
    {
        // Remove rich text markup tags like [color=...], [bold], etc.
        text = MarkupTagRegex().Replace(text, "");

        // Convert numbers to words for natural speech
        text = NumberConverter.ConvertNumbersToWords(text);

        // Collapse whitespace
        text = WhitespaceRegex().Replace(text, " ").Trim();

        return text;
    }

    [GeneratedRegex(@"\[/?[^\]]+\]")]
    private static partial Regex MarkupTagRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}
