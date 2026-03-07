// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.ShadowMonarch.Components;
using Content.Shared.CombatMode;

namespace Content.Server._Capibara.ShadowMonarch;

public sealed class ShadowSoldierSystem : EntitySystem
{
    [Dependency] private readonly SharedCombatModeSystem _combat = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShadowSoldierComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, ShadowSoldierComponent component, ComponentStartup args)
    {
        _combat.SetInCombatMode(uid, true);
    }
}
