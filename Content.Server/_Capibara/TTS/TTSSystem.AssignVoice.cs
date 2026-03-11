// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.TTS;
using Content.Shared._Capibara.TTS.Prototypes;
using Content.Shared.Humanoid;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server._Capibara.TTS;

/// <summary>
/// Partial for voice assignment logic.
/// </summary>
public sealed partial class TTSSystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    /// <summary>
    /// Cache of voice prototypes by sex for quick random assignment.
    /// </summary>
    private readonly Dictionary<Sex, List<VoicePrototype>> _voicesBySex = new();

    private VoicePrototype? _defaultVoice;

    private void InitializeAssignVoice()
    {
        _prototypeManager.PrototypesReloaded += OnPrototypesReloaded;
        CacheVoices();
    }

    private void OnPrototypesReloaded(PrototypesReloadedEventArgs args)
    {
        if (args.WasModified<VoicePrototype>())
            CacheVoices();
    }

    private void CacheVoices()
    {
        _voicesBySex.Clear();
        _defaultVoice = null;

        foreach (var voice in _prototypeManager.EnumeratePrototypes<VoicePrototype>())
        {
            if (voice.Silicon)
                continue;

            if (!_voicesBySex.TryGetValue(voice.Sex, out var list))
            {
                list = new List<VoicePrototype>();
                _voicesBySex[voice.Sex] = list;
            }

            list.Add(voice);
            _defaultVoice ??= voice;
        }
    }

    /// <summary>
    /// Get the voice ID for an entity, assigning one if needed.
    /// Priority: component voice > random sex-matched > default.
    /// </summary>
    private string? GetOrAssignVoice(EntityUid uid, TextToSpeechComponent component)
    {
        // If already assigned and valid, use it
        if (component.VoicePrototypeId != null &&
            _prototypeManager.HasIndex(component.VoicePrototypeId.Value))
        {
            return component.VoicePrototypeId.Value;
        }

        // Try to assign based on entity's sex
        var sex = Sex.Unsexed;
        if (TryComp<HumanoidAppearanceComponent>(uid, out var humanoid))
            sex = humanoid.Sex;

        VoicePrototype? selectedVoice = null;

        if (_voicesBySex.TryGetValue(sex, out var voices) && voices.Count > 0)
        {
            selectedVoice = _random.Pick(voices);
        }
        else if (_voicesBySex.TryGetValue(Sex.Unsexed, out var unsexedVoices) && unsexedVoices.Count > 0)
        {
            selectedVoice = _random.Pick(unsexedVoices);
        }

        selectedVoice ??= _defaultVoice;

        if (selectedVoice == null)
        {
            _log.Warning("No TTS voice prototypes available for entity {0}", uid);
            return null;
        }

        // Assign and network the voice
        component.VoicePrototypeId = selectedVoice.ID;
        Dirty(uid, component);

        return selectedVoice.ID;
    }
}
