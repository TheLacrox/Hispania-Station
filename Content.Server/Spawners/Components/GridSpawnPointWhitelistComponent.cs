// SPDX-FileCopyrightText: 2025 Space Station 14 Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Whitelist;

namespace Content.Server.Spawners.Components;

/// <summary>
/// Whitelist component for grid spawn points.
/// When placed on a spawn point entity, limits which entities can use this spawn point.
/// </summary>
[RegisterComponent]
public sealed partial class GridSpawnPointWhitelistComponent : Component
{
    /// <summary>
    /// Whitelist for entities that can spawn at this point.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Blacklist for entities that cannot spawn at this point.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;
}
