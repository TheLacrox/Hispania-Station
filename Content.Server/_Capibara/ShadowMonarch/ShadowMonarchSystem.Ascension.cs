// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.ShadowMonarch.Components;
using Content.Shared._Capibara.ShadowMonarch.Events;
using Content.Shared.Damage;
using Content.Shared.Mobs.Components;
using Content.Shared.Popups;

namespace Content.Server._Capibara.ShadowMonarch;

public sealed partial class ShadowMonarchSystem
{
    private void InitializeAscension()
    {
        SubscribeLocalEvent<ShadowMonarchComponent, ShadowMonarchAscendEvent>(OnAscend);
    }

    private void OnAscend(EntityUid uid, ShadowMonarchComponent component, ShadowMonarchAscendEvent args)
    {
        if (args.Handled)
            return;

        if (component.ExtractionCount < 7)
        {
            _popup.PopupEntity(Loc.GetString("shadow-monarch-ascend-not-ready"), uid, uid, PopupType.SmallCaution);
            return;
        }

        if (component.CurrentPhase == ShadowMonarchPhase.Ascended)
            return;

        // Phase transition
        component.CurrentPhase = ShadowMonarchPhase.Ascended;

        // Buff all current shadow soldiers (heal to full)
        foreach (var soldier in component.ShadowArmy)
        {
            if (!Exists(soldier) || TerminatingOrDeleted(soldier))
                continue;

            if (TryComp<DamageableComponent>(soldier, out var damageable))
                _damageable.SetAllDamage(soldier, damageable, 0);
        }

        // Halve all 3 extraction cooldowns
        if (component.ActionExtractionSoldierEntity != null)
            Actions.SetCooldown(component.ActionExtractionSoldierEntity.Value, TimeSpan.FromSeconds(5));

        if (component.ActionExtractionTankEntity != null)
            Actions.SetCooldown(component.ActionExtractionTankEntity.Value, TimeSpan.FromSeconds(5));

        if (component.ActionExtractionMageEntity != null)
            Actions.SetCooldown(component.ActionExtractionMageEntity.Value, TimeSpan.FromSeconds(5));

        // Station announcement
        _chat.DispatchGlobalAnnouncement(
            Loc.GetString("shadow-monarch-ascension-announcement"),
            Loc.GetString("shadow-monarch-ascension-sender"),
            colorOverride: Color.FromHex("#4B0082"));

        _popup.PopupEntity(Loc.GetString("shadow-monarch-ascended"), uid, uid, PopupType.LargeCaution);

        // Remove the ascend action (one-time use)
        if (component.ActionAscendEntity != null)
        {
            Actions.RemoveAction(uid, component.ActionAscendEntity);
            component.ActionAscendEntity = null;
        }

        // Raise event for rule system
        var ev = new ShadowMonarchAscendedEvent { Monarch = uid };
        RaiseLocalEvent(ev);

        Dirty(uid, component);
        args.Handled = true;
    }
}
