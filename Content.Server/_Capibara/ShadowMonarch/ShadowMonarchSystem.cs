// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Chat.Systems;
using Content.Server.Popups;
using Content.Shared._Capibara.ShadowMonarch;
using Content.Shared._Capibara.ShadowMonarch.Components;
using Content.Shared._Capibara.ShadowMonarch.Events;
using Content.Shared.Actions;
using Content.Shared.Damage;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.NPC.Prototypes;
using Content.Shared.NPC.Systems;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server._Capibara.ShadowMonarch;

public sealed partial class ShadowMonarchSystem : SharedShadowMonarchSystem
{
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly NpcFactionSystem _faction = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private readonly ProtoId<NpcFactionPrototype> _shadowMonarchFaction = "ShadowMonarch";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShadowMonarchComponent, ShadowExtractSoldierEvent>(OnExtractSoldier);
        SubscribeLocalEvent<ShadowMonarchComponent, ShadowExtractTankEvent>(OnExtractTank);
        SubscribeLocalEvent<ShadowMonarchComponent, ShadowExtractMageEvent>(OnExtractMage);
        SubscribeLocalEvent<ShadowMonarchComponent, MobStateChangedEvent>(OnMonarchStateChanged);

        InitializeAbilities();
        InitializeArmy();
        InitializeAscension();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        UpdateArmy(frameTime);
        UpdateDomain(frameTime);
    }

    private void OnExtractSoldier(EntityUid uid, ShadowMonarchComponent component, ShadowExtractSoldierEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = HandleExtraction(uid, component, args.Target, ShadowSoldierType.Soldier);
    }

    private void OnExtractTank(EntityUid uid, ShadowMonarchComponent component, ShadowExtractTankEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = HandleExtraction(uid, component, args.Target, ShadowSoldierType.Tank);
    }

    private void OnExtractMage(EntityUid uid, ShadowMonarchComponent component, ShadowExtractMageEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = HandleExtraction(uid, component, args.Target, ShadowSoldierType.Mage);
    }

    private bool HandleExtraction(EntityUid uid, ShadowMonarchComponent component, EntityUid target, ShadowSoldierType type)
    {
        // Must be dead
        if (!TryComp<MobStateComponent>(target, out var mobState) || mobState.CurrentState != MobState.Dead)
        {
            _popup.PopupEntity(Loc.GetString("shadow-monarch-target-not-dead"), uid, uid, PopupType.SmallCaution);
            return false;
        }

        // Check if target is already a shadow soldier
        if (HasComp<ShadowSoldierComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("shadow-monarch-target-already-shadow"), uid, uid, PopupType.SmallCaution);
            return false;
        }

        // Check army size
        var activeCount = 0;
        foreach (var soldier in component.ShadowArmy)
        {
            if (Exists(soldier) && !TerminatingOrDeleted(soldier))
                activeCount++;
        }

        if (activeCount >= component.MaxArmySize)
        {
            _popup.PopupEntity(Loc.GetString("shadow-monarch-army-full"), uid, uid, PopupType.SmallCaution);
            return false;
        }

        // Get original name before transformation
        var originalName = Name(target);
        component.VictimNames.Add(originalName);

        // Transform the corpse into a shadow soldier
        TransformCorpseToSoldier(uid, target, component, originalName, type);

        // Increment extraction count and power
        component.ExtractionCount++;
        component.ShadowPower += ComputePowerGain(component.ExtractionCount - 1);

        // Update max army size
        component.MaxArmySize = ComputeMaxArmySize(component.ExtractionCount);

        _popup.PopupEntity(
            Loc.GetString("shadow-monarch-extraction-success", ("name", originalName)),
            uid, uid, PopupType.Medium);

        // Check phase transition
        if (component.CurrentPhase == ShadowMonarchPhase.Dormant && component.ExtractionCount >= 1)
        {
            component.CurrentPhase = ShadowMonarchPhase.Awakened;
            _popup.PopupEntity(Loc.GetString("shadow-monarch-awakening"), uid, uid, PopupType.LargeCaution);
        }

        // Check power thresholds for new abilities
        CheckPowerThresholds(uid, component);

        Dirty(uid, component);
        return true;
    }

    private void CheckPowerThresholds(EntityUid uid, ShadowMonarchComponent component)
    {
        // 2 extractions: Shadow Step
        if (component.ExtractionCount >= 2 && component.ActionStepEntity == null)
        {
            Actions.AddAction(uid, ref component.ActionStepEntity, component.ActionStep);
            _popup.PopupEntity(Loc.GetString("shadow-monarch-unlock-step"), uid, uid, PopupType.Medium);
        }

        // 3 extractions: Shadow Exchange, army cap: 4
        if (component.ExtractionCount >= 3 && component.ActionExchangeEntity == null)
        {
            Actions.AddAction(uid, ref component.ActionExchangeEntity, component.ActionExchange);
            _popup.PopupEntity(Loc.GetString("shadow-monarch-unlock-exchange"), uid, uid, PopupType.Medium);
        }

        // 4 extractions: Shadow Hide + Shadow Summon
        if (component.ExtractionCount >= 4 && component.ActionHideEntity == null)
        {
            Actions.AddAction(uid, ref component.ActionHideEntity, component.ActionHide);
            Actions.AddAction(uid, ref component.ActionSummonEntity, component.ActionSummon);
            _popup.PopupEntity(Loc.GetString("shadow-monarch-unlock-hide-summon"), uid, uid, PopupType.Medium);
        }

        // 5 extractions: Shadow Domain
        if (component.ExtractionCount >= 5 && component.ActionDomainEntity == null)
        {
            Actions.AddAction(uid, ref component.ActionDomainEntity, component.ActionDomain);
            _popup.PopupEntity(Loc.GetString("shadow-monarch-unlock-domain"), uid, uid, PopupType.Medium);
        }

        // 7 extractions: Ascend
        if (component.ExtractionCount >= 7 && component.ActionAscendEntity == null)
        {
            Actions.AddAction(uid, ref component.ActionAscendEntity, component.ActionAscend);
            _popup.PopupEntity(Loc.GetString("shadow-monarch-unlock-ascend"), uid, uid, PopupType.LargeCaution);
        }
    }

    private void OnMonarchStateChanged(EntityUid uid, ShadowMonarchComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead)
            return;

        DespawnAllSoldiers(uid, component);

        var ev = new ShadowMonarchDiedEvent { Monarch = uid };
        RaiseLocalEvent(ev);
    }
}
