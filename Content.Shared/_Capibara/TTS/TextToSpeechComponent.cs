// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.TTS.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Capibara.TTS;

/// <summary>
/// Marks an entity as having TTS voice capability.
/// Stores the assigned voice prototype ID for speech synthesis.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class TextToSpeechComponent : Component
{
    /// <summary>
    /// The voice prototype to use for this entity's TTS.
    /// If null, a voice will be auto-assigned based on sex.
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<VoicePrototype>? VoicePrototypeId;
}
