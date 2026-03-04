// SPDX-FileCopyrightText: 2025 space-wizards contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._Wizden.Power.Components;
using Content.Server.Power.Components;
using Content.Shared.Power;
using Content.Shared.Verbs;

namespace Content.Server._Wizden.Power.EntitySystems;

public sealed class VoltageTogglerSystem : EntitySystem
{
    private static readonly VerbCategory VoltageLevel = new("verb-categories-voltage-level", null);

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<VoltageTogglerComponent, GetVerbsEvent<Verb>>(OnGetVerb);
    }

    private void OnGetVerb(Entity<VoltageTogglerComponent> entity, ref GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        var index = 0;
        foreach (var setting in entity.Comp.Settings)
        {
            // This is because Act wont work with index.
            // Needs it to be saved in the loop.
            var currIndex = index;
            var verb = new Verb
            {
                Priority = currIndex,
                Category = VoltageLevel,
                Disabled = entity.Comp.SelectedVoltageLevel == currIndex,
                Text = Loc.GetString(setting.Name),
                Act = () =>
                {
                    entity.Comp.SelectedVoltageLevel = currIndex;
                    ChangeVoltage(entity, setting);
                }
            };
            args.Verbs.Add(verb);
            index++;
        }
    }

    private void ChangeVoltage(Entity<VoltageTogglerComponent> entity, VoltageSetting setting)
    {
        // Hispania: We can't switch node groups at runtime (private setter),
        // so we change the charge rate on PowerNetworkBattery instead.
        if (TryComp<PowerNetworkBatteryComponent>(entity, out var netBattery))
        {
            netBattery.MaxChargeRate = setting.Wattage;
        }
    }
}
