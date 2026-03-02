// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Capibara.Economy.Components;

/// <summary>
/// Marks an entity as an ATM machine for the Capibara banking system.
/// Requires physical ID card or PDA insertion via an ItemSlot.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CapibaraATMComponent : Component
{
    public const string IdSlotId = "CapibaraATM-id";

    [DataField]
    public ItemSlot IdSlot = new();

    /// <summary>
    /// The entity prototype to spawn when withdrawing cash.
    /// </summary>
    [DataField]
    public EntProtoId CashPrototype { get; private set; } = "SpaceCash";

    [DataField]
    public SoundSpecifier ErrorSound { get; private set; } = new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");

    [DataField]
    public SoundSpecifier ConfirmSound { get; private set; } = new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg");
}
