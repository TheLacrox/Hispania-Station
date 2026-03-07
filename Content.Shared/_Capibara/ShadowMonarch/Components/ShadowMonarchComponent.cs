// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.ShadowMonarch.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class ShadowMonarchComponent : Component
{
    [DataField, AutoNetworkedField]
    public ShadowMonarchPhase CurrentPhase = ShadowMonarchPhase.Dormant;

    [DataField, AutoNetworkedField]
    public int ExtractionCount;

    [DataField, AutoNetworkedField]
    public int ShadowPower;

    [DataField]
    public HashSet<EntityUid> ShadowArmy = new();

    [DataField]
    public List<ShadowSoldierData> HiddenSoldiers = new();

    [DataField]
    public int MaxArmySize = 3;

    // Extraction actions (3 types)
    [DataField]
    public EntProtoId ActionExtractionSoldier = "ActionShadowExtractSoldier";

    [DataField]
    public EntityUid? ActionExtractionSoldierEntity;

    [DataField]
    public EntProtoId ActionExtractionTank = "ActionShadowExtractTank";

    [DataField]
    public EntityUid? ActionExtractionTankEntity;

    [DataField]
    public EntProtoId ActionExtractionMage = "ActionShadowExtractMage";

    [DataField]
    public EntityUid? ActionExtractionMageEntity;

    // Other abilities
    [DataField]
    public EntProtoId ActionStep = "ActionShadowStep";

    [DataField]
    public EntityUid? ActionStepEntity;

    [DataField]
    public EntProtoId ActionExchange = "ActionShadowExchange";

    [DataField]
    public EntityUid? ActionExchangeEntity;

    [DataField]
    public EntProtoId ActionHide = "ActionShadowHide";

    [DataField]
    public EntityUid? ActionHideEntity;

    [DataField]
    public EntProtoId ActionSummon = "ActionShadowSummon";

    [DataField]
    public EntityUid? ActionSummonEntity;

    [DataField]
    public EntProtoId ActionDomain = "ActionShadowDomain";

    [DataField]
    public EntityUid? ActionDomainEntity;

    [DataField]
    public EntProtoId ActionAscend = "ActionShadowAscend";

    [DataField]
    public EntityUid? ActionAscendEntity;

    [DataField]
    public EntProtoId ObjectiveAscend = "ShadowMonarchAscendObjective";

    /// <summary>
    /// Names of victims whose shadows were extracted, for round-end summary.
    /// </summary>
    [DataField]
    public List<string> VictimNames = new();
}

[NetSerializable, Serializable]
public enum ShadowMonarchPhase : byte
{
    Dormant,
    Awakened,
    Ascended,
}
