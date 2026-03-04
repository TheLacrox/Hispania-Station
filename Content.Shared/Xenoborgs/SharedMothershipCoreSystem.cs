// SPDX-FileCopyrightText: 2025 Hispania Station contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.ActionBlocker;
using Content.Shared.Interaction.Events;
using Content.Shared.Movement.Events;
using Content.Shared.Xenoborgs.Components;

namespace Content.Shared.Xenoborgs;

/// <summary>
/// Handles mothership core movement blocking without using <see cref="Content.Shared.Interaction.Components.BlockMovementComponent"/>,
/// so that the core can still use, pick up, and drop items (e.g. materials for its lathe).
/// </summary>
public sealed class SharedMothershipCoreSystem : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MothershipCoreComponent, UpdateCanMoveEvent>(OnMoveAttempt);
        SubscribeLocalEvent<MothershipCoreComponent, ChangeDirectionAttemptEvent>(OnChangeDirectionAttempt);

        SubscribeLocalEvent<MothershipCoreComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<MothershipCoreComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnMoveAttempt(EntityUid uid, MothershipCoreComponent component, UpdateCanMoveEvent args)
    {
        args.Cancel();
    }

    private void OnChangeDirectionAttempt(EntityUid uid, MothershipCoreComponent component, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }

    private void OnStartup(EntityUid uid, MothershipCoreComponent component, ComponentStartup args)
    {
        _actionBlocker.UpdateCanMove(uid);
    }

    private void OnShutdown(EntityUid uid, MothershipCoreComponent component, ComponentShutdown args)
    {
        _actionBlocker.UpdateCanMove(uid);
    }
}
