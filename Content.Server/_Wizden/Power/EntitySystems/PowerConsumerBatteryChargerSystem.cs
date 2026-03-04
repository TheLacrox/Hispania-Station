// SPDX-FileCopyrightText: 2025 space-wizards contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._Wizden.Power.Components;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Power.Components;

namespace Content.Server._Wizden.Power.EntitySystems;

public sealed class PowerConsumerBatteryChargerSystem : EntitySystem
{
    [Dependency] private readonly BatterySystem _battery = default!;

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<PowerConsumerBatteryChargerComponent, PowerConsumerComponent, BatteryComponent, TransformComponent>();

        while (query.MoveNext(out var entity, out _, out var powerConsumerComp, out var battery, out var transform))
        {
            if (!transform.Anchored)
                continue;

            _battery.ChangeCharge(entity, powerConsumerComp.NetworkLoad.ReceivingPower * frameTime, battery);
        }
    }
}
