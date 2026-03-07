// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Capibara.NanoGigs;

/// <summary>
/// Marker component for the NanoGigs PDA cartridge program.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class NanoGigsCartridgeComponent : Component
{
    [DataField]
    public bool AlertsEnabled;
}
