// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Capibara.ShadowMonarch.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class ShadowSoldierComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid Monarch;

    [DataField, AutoNetworkedField]
    public string OriginalName = string.Empty;

    [DataField]
    public float CombatStrength = 1.0f;

    [DataField, AutoNetworkedField]
    public ShadowSoldierType SoldierType = ShadowSoldierType.Soldier;
}
