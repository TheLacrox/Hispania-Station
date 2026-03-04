// SPDX-FileCopyrightText: 2025 space-wizards contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._Wizden.Power.Components;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;

namespace Content.Server._Wizden.Power.EntitySystems;

public sealed class SpawnOnBatteryLevelSystem : EntitySystem
{
    [Dependency] private readonly BatterySystem _battery = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpawnOnBatteryLevelComponent, ChargeChangedEvent>(OnBatteryChargeChange);
    }

    private void OnBatteryChargeChange(Entity<SpawnOnBatteryLevelComponent> entity, ref ChargeChangedEvent args)
    {
        if (!TryComp<BatteryComponent>(entity, out var battery))
            return;

        if (!TryComp(entity, out TransformComponent? xform))
            return;

        if (args.Charge >= entity.Comp.Charge)
        {
            Spawn(entity.Comp.Prototype, xform.Coordinates);

            _battery.ChangeCharge(entity, -entity.Comp.Charge, battery);
        }
    }
}
