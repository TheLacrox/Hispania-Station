// SPDX-FileCopyrightText: 2025 Space Station 14 Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Power.Components;
using JetBrains.Annotations;

namespace Content.Shared.Power.EntitySystems;

/// <summary>
/// Generic system that handles entities with <see cref="PowerStateComponent"/>.
/// Used for simple machines that only need to switch between "idle" and "working" power states.
/// </summary>
public abstract class SharedPowerStateSystem : EntitySystem
{
    private EntityQuery<PowerStateComponent> _powerStateQuery;

    public override void Initialize()
    {
        base.Initialize();

        _powerStateQuery = GetEntityQuery<PowerStateComponent>();
    }

    /// <summary>
    /// Sets the working state of the entity, adjusting its power draw accordingly.
    /// </summary>
    [PublicAPI]
    public void SetWorkingState(Entity<PowerStateComponent?> ent, bool working)
    {
        if (!_powerStateQuery.Resolve(ent, ref ent.Comp))
            return;

        SetPowerLoad(ent, working ? ent.Comp.WorkingPowerDraw : ent.Comp.IdlePowerDraw);
        ent.Comp.IsWorking = working;
    }

    /// <summary>
    /// Tries to set the working state of the entity, adjusting its power draw accordingly.
    /// </summary>
    [PublicAPI]
    public void TrySetWorkingState(Entity<PowerStateComponent?> ent, bool working)
    {
        if (!_powerStateQuery.Resolve(ent, ref ent.Comp, false))
            return;

        SetWorkingState(ent, working);
    }

    /// <summary>
    /// Sets the power load on the entity. Implemented by the server-side system.
    /// </summary>
    protected virtual void SetPowerLoad(EntityUid uid, float load)
    {
    }
}
