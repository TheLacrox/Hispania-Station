// SPDX-FileCopyrightText: 2025 Space Station 14 Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Power.Components;
using Content.Shared.Power.Components;
using Content.Shared.Power.EntitySystems;

namespace Content.Server.Power.EntitySystems;

public sealed class PowerStateSystem : SharedPowerStateSystem
{
    [Dependency] private readonly PowerReceiverSystem _powerReceiverSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PowerStateComponent, ComponentStartup>(OnComponentStartup);
    }

    private void OnComponentStartup(Entity<PowerStateComponent> ent, ref ComponentStartup args)
    {
        EnsureComp<ApcPowerReceiverComponent>(ent);
        SetWorkingState(ent.Owner, ent.Comp.IsWorking);
    }

    protected override void SetPowerLoad(EntityUid uid, float load)
    {
        if (TryComp<ApcPowerReceiverComponent>(uid, out var receiver))
            _powerReceiverSystem.SetLoad(receiver, load);
    }
}
