// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Humanoid;
using Robust.Shared.Prototypes;

namespace Content.Shared._Capibara.TTS.Prototypes;

/// <summary>
/// Defines a TTS voice that can be assigned to entities.
/// Maps to a voice ID understood by the external TTS service.
/// </summary>
[Prototype("ttsVoice")]
public sealed partial class VoicePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Human-readable display name for the voice.
    /// </summary>
    [DataField(required: true)]
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// The sex this voice is intended for (used for auto-assignment).
    /// </summary>
    [DataField(required: true)]
    public Sex Sex { get; private set; } = Sex.Unsexed;

    /// <summary>
    /// Whether this voice is intended for silicon/AI entities.
    /// </summary>
    [DataField]
    public bool Silicon { get; private set; }
}
