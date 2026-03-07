// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.ShadowMonarch;
using Content.Shared._Capibara.ShadowMonarch.Components;
using Content.Shared._Capibara.ShadowMonarch.Events;
using Content.Shared.Popups;
using Robust.Shared.Map;

namespace Content.Server._Capibara.ShadowMonarch;

public sealed partial class ShadowMonarchSystem
{
    private void InitializeAbilities()
    {
        SubscribeLocalEvent<ShadowMonarchComponent, ShadowStepEvent>(OnShadowStep);
        SubscribeLocalEvent<ShadowMonarchComponent, ShadowExchangeEvent>(OnShadowExchange);
        SubscribeLocalEvent<ShadowMonarchComponent, ShadowHideEvent>(OnShadowHide);
        SubscribeLocalEvent<ShadowMonarchComponent, ShadowSummonEvent>(OnShadowSummon);
        SubscribeLocalEvent<ShadowMonarchComponent, ShadowDomainEvent>(OnShadowDomain);
    }

    private void OnShadowStep(EntityUid uid, ShadowMonarchComponent component, ShadowStepEvent args)
    {
        if (args.Handled)
            return;

        var xform = Transform(uid);
        var targetCoords = args.Target;

        // Teleport monarch to target position
        xform.Coordinates = new EntityCoordinates(xform.ParentUid, targetCoords.Position);

        _popup.PopupEntity(Loc.GetString("shadow-monarch-step-used"), uid, uid, PopupType.Medium);
        args.Handled = true;
    }

    private void OnShadowExchange(EntityUid uid, ShadowMonarchComponent component, ShadowExchangeEvent args)
    {
        if (args.Handled)
            return;

        var target = args.Target;

        if (!HasComp<ShadowSoldierComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("shadow-monarch-exchange-not-soldier"), uid, uid, PopupType.SmallCaution);
            return;
        }

        // Swap positions
        var monarchXform = Transform(uid);
        var soldierXform = Transform(target);

        var monarchCoords = monarchXform.Coordinates;
        var soldierCoords = soldierXform.Coordinates;

        monarchXform.Coordinates = soldierCoords;
        soldierXform.Coordinates = monarchCoords;

        _popup.PopupEntity(Loc.GetString("shadow-monarch-exchange-used"), uid, uid, PopupType.Medium);
        args.Handled = true;
    }

    private void OnShadowHide(EntityUid uid, ShadowMonarchComponent component, ShadowHideEvent args)
    {
        if (args.Handled)
            return;

        var target = args.Target;

        if (!TryComp<ShadowSoldierComponent>(target, out var soldier))
        {
            _popup.PopupEntity(Loc.GetString("shadow-monarch-hide-not-soldier"), uid, uid, PopupType.SmallCaution);
            return;
        }

        if (soldier.Monarch != uid)
        {
            _popup.PopupEntity(Loc.GetString("shadow-monarch-hide-not-yours"), uid, uid, PopupType.SmallCaution);
            return;
        }

        // Store soldier data including type
        var data = new ShadowSoldierData
        {
            OriginalName = soldier.OriginalName,
            CombatStrength = soldier.CombatStrength,
            SoldierType = soldier.SoldierType,
        };
        component.HiddenSoldiers.Add(data);

        // Remove from army and delete
        component.ShadowArmy.Remove(target);
        QueueDel(target);

        _popup.PopupEntity(
            Loc.GetString("shadow-monarch-hide-success", ("name", soldier.OriginalName)),
            uid, uid, PopupType.Medium);

        args.Handled = true;
    }

    private void OnShadowSummon(EntityUid uid, ShadowMonarchComponent component, ShadowSummonEvent args)
    {
        if (args.Handled)
            return;

        if (component.HiddenSoldiers.Count == 0)
        {
            _popup.PopupEntity(Loc.GetString("shadow-monarch-summon-none"), uid, uid, PopupType.SmallCaution);
            return;
        }

        // Spawn ALL hidden soldiers at monarch's position
        var spawned = 0;
        foreach (var data in component.HiddenSoldiers)
        {
            // Check army size each time
            var activeCount = 0;
            foreach (var s in component.ShadowArmy)
            {
                if (Exists(s) && !TerminatingOrDeleted(s))
                    activeCount++;
            }

            if (activeCount >= component.MaxArmySize)
                break;

            SpawnHiddenSoldier(uid, component, data);
            spawned++;
        }

        component.HiddenSoldiers.Clear();

        _popup.PopupEntity(
            Loc.GetString("shadow-monarch-summon-success", ("count", spawned)),
            uid, uid, PopupType.Medium);

        args.Handled = true;
    }

    private void OnShadowDomain(EntityUid uid, ShadowMonarchComponent component, ShadowDomainEvent args)
    {
        if (args.Handled)
            return;

        // Spawn a domain entity at the monarch's position
        var coords = Transform(uid).Coordinates;
        var domain = Spawn("ShadowDomainEffect", coords);

        var domainComp = EnsureComp<ShadowDomainComponent>(domain);
        domainComp.Monarch = uid;
        domainComp.SpawnTime = _timing.CurTime;

        _popup.PopupEntity(Loc.GetString("shadow-monarch-domain-used"), uid, uid, PopupType.LargeCaution);
        args.Handled = true;
    }

    private void UpdateDomain(float frameTime)
    {
        var query = EntityQueryEnumerator<ShadowDomainComponent>();
        while (query.MoveNext(out var uid, out var domain))
        {
            if (_timing.CurTime > domain.SpawnTime + domain.Duration)
            {
                QueueDel(uid);
            }
        }
    }
}
