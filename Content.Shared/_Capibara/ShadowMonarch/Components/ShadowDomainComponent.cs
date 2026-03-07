// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Shared._Capibara.ShadowMonarch.Components;

/// <summary>
/// Marker component for the Shadow Domain AoE entity.
/// </summary>
[RegisterComponent]
public sealed partial class ShadowDomainComponent : Component
{
    [DataField]
    public EntityUid Monarch;

    [DataField]
    public float Radius = 5f;

    [DataField]
    public TimeSpan Duration = TimeSpan.FromSeconds(15);

    [DataField]
    public TimeSpan SpawnTime;
}
