// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Antag;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.Mind;
using Content.Server.Roles;
using Content.Server.Zombies;
using Content.Shared._Capibara.ShadowMonarch.Components;
using Content.Shared._Capibara.ShadowMonarch.Events;
using Content.Shared.GameTicking.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.NPC.Prototypes;
using Content.Shared.NPC.Systems;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server._Capibara.ShadowMonarch.Rules;

public sealed class ShadowMonarchRuleSystem : GameRuleSystem<ShadowMonarchRuleComponent>
{
    [Dependency] private readonly SharedRoleSystem _role = default!;
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly MobStateSystem _mob = default!;
    [Dependency] private readonly NpcFactionSystem _npc = default!;

    private readonly EntProtoId _mindRole = "MindRoleShadowMonarch";
    private readonly ProtoId<NpcFactionPrototype> _nanotrasenFactionId = "NanoTrasen";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShadowMonarchRuleComponent, AfterAntagEntitySelectedEvent>(OnSelectAntag);
        SubscribeLocalEvent<ShadowMonarchRoleComponent, GetBriefingEvent>(OnGetBriefing);

        SubscribeLocalEvent<ShadowMonarchAscendedEvent>(OnAscend);
        SubscribeLocalEvent<ShadowMonarchDiedEvent>(OnDeath);
    }

    private void OnDeath(ShadowMonarchDiedEvent args)
    {
        var rulesQuery = QueryActiveRules();
        while (rulesQuery.MoveNext(out _, out var rule, out _))
        {
            var monarchCount = 0;
            var monarchDead = 0;
            var query = EntityQueryEnumerator<ShadowMonarchComponent>();

            while (query.MoveNext(out var uid, out _))
            {
                monarchCount++;
                if (_mob.IsDead(uid) || _mob.IsInvalidState(uid))
                    monarchDead++;
            }

            if (monarchCount > 0 && monarchCount == monarchDead)
                rule.WinCondition = ShadowMonarchWinCondition.Failure;
        }
    }

    private void OnAscend(ShadowMonarchAscendedEvent args)
    {
        var rulesQuery = QueryActiveRules();
        while (rulesQuery.MoveNext(out _, out var rule, out _))
        {
            rule.WinCondition = ShadowMonarchWinCondition.Win;
            return;
        }
    }

    private void OnGetBriefing(EntityUid uid, ShadowMonarchRoleComponent component, ref GetBriefingEvent args)
    {
        args.Briefing = Loc.GetString("shadow-monarch-briefing");
    }

    private void OnSelectAntag(EntityUid uid, ShadowMonarchRuleComponent comp, ref AfterAntagEntitySelectedEvent args)
    {
        MakeShadowMonarch(args.EntityUid);
    }

    public bool MakeShadowMonarch(EntityUid target)
    {
        if (!_mind.TryGetMind(target, out var mindId, out var mind))
            return false;

        _role.MindAddRole(mindId, _mindRole, mind, true);

        _npc.RemoveFaction(target, _nanotrasenFactionId, false);

        var briefing = Loc.GetString("shadow-monarch-role-greeting");
        _antag.SendBriefing(target, briefing, Color.FromHex("#4B0082"), null);

        EnsureComp<ZombieImmuneComponent>(target);
        EnsureComp<ShadowMonarchComponent>(target);
        return true;
    }

    protected override void AppendRoundEndText(
        EntityUid uid,
        ShadowMonarchRuleComponent component,
        GameRuleComponent gamerule,
        ref RoundEndTextAppendEvent args)
    {
        base.AppendRoundEndText(uid, component, gamerule, ref args);

        var winText = Loc.GetString($"shadow-monarch-condition-{component.WinCondition.ToString().ToLower()}");
        args.AddLine(winText);
        args.AddLine(Loc.GetString("shadow-monarch-list-start"));

        var sessionData = _antag.GetAntagIdentifiers(uid);
        foreach (var (_, data, name) in sessionData)
        {
            var listing = Loc.GetString("shadow-monarch-list-name", ("name", name), ("user", data.UserName));
            args.AddLine(listing);
        }

        // Show extraction details for all monarchs
        var query = EntityQueryEnumerator<ShadowMonarchComponent>();
        while (query.MoveNext(out _, out var monarch))
        {
            args.AddLine(Loc.GetString("shadow-monarch-list-extractions",
                ("count", monarch.ExtractionCount)));

            foreach (var victim in monarch.VictimNames)
            {
                args.AddLine(Loc.GetString("shadow-monarch-list-victim", ("name", victim)));
            }
        }
    }
}
