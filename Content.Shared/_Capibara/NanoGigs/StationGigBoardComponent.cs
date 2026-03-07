// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Capibara.NanoGigs;

/// <summary>
/// Stores all gig listings for a station. Attached to station entities.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class StationGigBoardComponent : Component
{
    [DataField]
    public List<NanoGigListing> Jobs = new();

    [DataField]
    public int NextGigId = 1;
}
