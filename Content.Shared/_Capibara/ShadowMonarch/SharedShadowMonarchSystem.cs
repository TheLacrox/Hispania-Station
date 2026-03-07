// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.ShadowMonarch.Components;
using Content.Shared.Actions;

namespace Content.Shared._Capibara.ShadowMonarch;

public abstract class SharedShadowMonarchSystem : EntitySystem
{
    [Dependency] protected readonly SharedActionsSystem Actions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShadowMonarchComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, ShadowMonarchComponent component, MapInitEvent args)
    {
        Actions.AddAction(uid, ref component.ActionExtractionSoldierEntity, component.ActionExtractionSoldier);
        Actions.AddAction(uid, ref component.ActionExtractionTankEntity, component.ActionExtractionTank);
        Actions.AddAction(uid, ref component.ActionExtractionMageEntity, component.ActionExtractionMage);
    }

    public int ComputeMaxArmySize(int extractionCount)
    {
        return Math.Min(3 + extractionCount / 2, 10);
    }

    public int ComputePowerGain(int currentCount)
    {
        return 10 + 5 * currentCount;
    }
}
