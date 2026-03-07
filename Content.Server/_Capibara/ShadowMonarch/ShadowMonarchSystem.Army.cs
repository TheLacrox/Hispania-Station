// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Ghost.Roles.Components;
using Content.Server.NPC;
using Content.Server.NPC.HTN;
using Content.Server.NPC.Systems;
using Content.Shared._Capibara.ShadowMonarch;
using Content.Shared._Capibara.ShadowMonarch.Components;
using Content.Shared.CombatMode;
using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.NPC.Prototypes;
using Content.Shared.Weapons.Melee;
using Robust.Shared.Prototypes;

namespace Content.Server._Capibara.ShadowMonarch;

public sealed partial class ShadowMonarchSystem
{
    [Dependency] private readonly NPCSystem _npc = default!;
    [Dependency] private readonly SharedCombatModeSystem _combat = default!;
    [Dependency] private readonly MetaDataSystem _metaData = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeed = default!;
    [Dependency] private readonly MobThresholdSystem _mobThreshold = default!;

    private static readonly ProtoId<NpcFactionPrototype> NanoTrasenFaction = "NanoTrasen";

    private void InitializeArmy()
    {
        SubscribeLocalEvent<ShadowSoldierComponent, MobStateChangedEvent>(OnSoldierStateChanged);
    }

    private void UpdateArmy(float frameTime)
    {
        var query = EntityQueryEnumerator<ShadowMonarchComponent>();
        while (query.MoveNext(out var uid, out var monarch))
        {
            monarch.ShadowArmy.RemoveWhere(s => !Exists(s) || TerminatingOrDeleted(s));
        }
    }

    /// <summary>
    /// Transforms a dead corpse into a shadow soldier in-place.
    /// </summary>
    public void TransformCorpseToSoldier(EntityUid monarch, EntityUid corpse, ShadowMonarchComponent monarchComp, string originalName, ShadowSoldierType type)
    {
        // Add shadow soldier component
        var soldier = EnsureComp<ShadowSoldierComponent>(corpse);
        soldier.Monarch = monarch;
        soldier.OriginalName = originalName;
        soldier.SoldierType = type;

        // Set up faction
        _faction.ClearFactions(corpse, dirty: false);
        _faction.AddFaction(corpse, _shadowMonarchFaction);

        // Set up combat mode
        _combat.SetInCombatMode(corpse, true);

        // Apply type-specific stats
        ApplyTypeStats(corpse, type, originalName);

        // Heal to full and revive
        if (TryComp<DamageableComponent>(corpse, out var damageable))
            _damageable.SetAllDamage(corpse, damageable, 0);
        _mobState.ChangeMobState(corpse, MobState.Alive);

        // Set up NPC AI
        var htnTask = type == ShadowSoldierType.Mage ? "SimpleRangedHostileCompound" : "SimpleHostileCompound";
        var htn = EnsureComp<HTNComponent>(corpse);
        htn.RootTask = new HTNCompoundTask { Task = htnTask };
        htn.Blackboard.SetValue(NPCBlackboard.Owner, corpse);
        _npc.WakeNPC(corpse, htn);

        // Add to army
        monarchComp.ShadowArmy.Add(corpse);

        // Set up ghost role so dead players can take over
        SetupGhostRole(corpse, type);
    }

    private void ApplyTypeStats(EntityUid uid, ShadowSoldierType type, string originalName)
    {
        var melee = EnsureComp<MeleeWeaponComponent>(uid);
        melee.AltDisarm = false;

        switch (type)
        {
            case ShadowSoldierType.Soldier:
                melee.Damage = new DamageSpecifier
                {
                    DamageDict = new Dictionary<string, FixedPoint2>
                    {
                        ["Slash"] = 12,
                        ["Cold"] = 3,
                    }
                };
                melee.Range = 1.2f;
                _metaData.SetEntityName(uid, Loc.GetString("shadow-monarch-soldier-name", ("name", originalName)));
                _mobThreshold.SetMobStateThreshold(uid, FixedPoint2.New(0), MobState.Alive);
                _mobThreshold.SetMobStateThreshold(uid, FixedPoint2.New(60), MobState.Dead);
                break;

            case ShadowSoldierType.Tank:
                melee.Damage = new DamageSpecifier
                {
                    DamageDict = new Dictionary<string, FixedPoint2>
                    {
                        ["Slash"] = 8,
                    }
                };
                melee.Range = 1.2f;
                _metaData.SetEntityName(uid, Loc.GetString("shadow-monarch-tank-name", ("name", originalName)));
                _mobThreshold.SetMobStateThreshold(uid, FixedPoint2.New(0), MobState.Alive);
                _mobThreshold.SetMobStateThreshold(uid, FixedPoint2.New(120), MobState.Dead);
                // Slower speed (4.5 * 0.7 = 3.15)
                _movementSpeed.ChangeBaseSpeed(uid, 3.15f, 3.15f, MovementSpeedModifierComponent.DefaultAcceleration);
                break;

            case ShadowSoldierType.Mage:
                melee.Damage = new DamageSpecifier
                {
                    DamageDict = new Dictionary<string, FixedPoint2>
                    {
                        ["Slash"] = 5,
                    }
                };
                melee.Range = 1.2f;
                _metaData.SetEntityName(uid, Loc.GetString("shadow-monarch-mage-name", ("name", originalName)));
                _mobThreshold.SetMobStateThreshold(uid, FixedPoint2.New(0), MobState.Alive);
                _mobThreshold.SetMobStateThreshold(uid, FixedPoint2.New(40), MobState.Dead);
                break;
        }

        Dirty(uid, melee);
    }

    private void SetupGhostRole(EntityUid uid, ShadowSoldierType type)
    {
        var ghostRole = EnsureComp<GhostRoleComponent>(uid);
        ghostRole.RoleName = type switch
        {
            ShadowSoldierType.Tank => Loc.GetString("shadow-soldier-ghost-role-tank-name"),
            ShadowSoldierType.Mage => Loc.GetString("shadow-soldier-ghost-role-mage-name"),
            _ => Loc.GetString("shadow-soldier-ghost-role-name"),
        };
        ghostRole.RoleDescription = Loc.GetString("shadow-soldier-ghost-role-description");
        ghostRole.RoleRules = Loc.GetString("shadow-soldier-ghost-role-rules");
        ghostRole.MindRoles = new List<EntProtoId> { "MindRoleShadowSoldier" };

        EnsureComp<GhostTakeoverAvailableComponent>(uid);
    }

    /// <summary>
    /// Spawns a shadow soldier from hidden data at the monarch's position.
    /// </summary>
    public EntityUid? SpawnHiddenSoldier(EntityUid monarch, ShadowMonarchComponent monarchComp, ShadowSoldierData data)
    {
        var coords = Transform(monarch).Coordinates;

        var protoId = data.SoldierType switch
        {
            ShadowSoldierType.Tank => "MobShadowTank",
            ShadowSoldierType.Mage => "MobShadowMage",
            _ => "MobShadowSoldier",
        };

        var soldier = Spawn(protoId, coords);

        var soldierComp = EnsureComp<ShadowSoldierComponent>(soldier);
        soldierComp.Monarch = monarch;
        soldierComp.OriginalName = data.OriginalName;
        soldierComp.CombatStrength = data.CombatStrength;
        soldierComp.SoldierType = data.SoldierType;

        var nameKey = data.SoldierType switch
        {
            ShadowSoldierType.Tank => "shadow-monarch-tank-name",
            ShadowSoldierType.Mage => "shadow-monarch-mage-name",
            _ => "shadow-monarch-soldier-name",
        };
        _metaData.SetEntityName(soldier, Loc.GetString(nameKey, ("name", data.OriginalName)));

        _faction.ClearFactions(soldier, dirty: false);
        _faction.AddFaction(soldier, _shadowMonarchFaction);

        monarchComp.ShadowArmy.Add(soldier);

        // Ghost role for spawned soldiers too
        SetupGhostRole(soldier, data.SoldierType);

        return soldier;
    }

    private void OnSoldierStateChanged(EntityUid uid, ShadowSoldierComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead)
            return;

        // Remove from monarch's army
        if (TryComp<ShadowMonarchComponent>(component.Monarch, out var monarch))
        {
            monarch.ShadowArmy.Remove(uid);
        }
    }

    public void DespawnAllSoldiers(EntityUid monarch, ShadowMonarchComponent component)
    {
        foreach (var soldier in component.ShadowArmy)
        {
            if (Exists(soldier) && !TerminatingOrDeleted(soldier))
                QueueDel(soldier);
        }
        component.ShadowArmy.Clear();
    }
}
