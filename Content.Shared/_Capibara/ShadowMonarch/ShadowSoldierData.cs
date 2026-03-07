// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.ShadowMonarch;

[DataDefinition, NetSerializable, Serializable]
public sealed partial class ShadowSoldierData
{
    [DataField]
    public string OriginalName = string.Empty;

    [DataField]
    public float CombatStrength = 1.0f;

    [DataField]
    public ShadowSoldierType SoldierType = ShadowSoldierType.Soldier;
}
