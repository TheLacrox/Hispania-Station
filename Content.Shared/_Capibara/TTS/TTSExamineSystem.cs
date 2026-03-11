// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.TTS.Prototypes;
using Content.Shared.Examine;
using Robust.Shared.Prototypes;

namespace Content.Shared._Capibara.TTS;

/// <summary>
/// Shows the assigned TTS voice name when examining an entity.
/// </summary>
public sealed class TTSExamineSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TextToSpeechComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(EntityUid uid, TextToSpeechComponent component, ref ExaminedEvent args)
    {
        if (component.VoicePrototypeId == null)
            return;

        if (!_prototypeManager.TryIndex(component.VoicePrototypeId.Value, out var voice))
            return;

        args.PushMarkup(Loc.GetString("tts-examine-voice", ("voice", voice.Name)));
    }
}
