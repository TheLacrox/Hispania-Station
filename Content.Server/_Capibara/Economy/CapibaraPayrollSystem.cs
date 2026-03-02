// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Chat.Managers;
using Content.Shared._Capibara.Economy;
using Content.Shared._Capibara.Economy.Components;
using Content.Shared.Access.Components;
using Content.Server.GameTicking.Events;
using Content.Shared.Mobs.Components;
using Robust.Server.Player;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
namespace Content.Server._Capibara.Economy;

using CancellationTokenSource = System.Threading.CancellationTokenSource;

/// <summary>
/// Pays salaries to all players with bank accounts at regular intervals.
/// Uses Timer.SpawnRepeating instead of per-frame Update for efficiency.
/// </summary>
public sealed class CapibaraPayrollSystem : EntitySystem
{
    [Dependency] private readonly CapibaraBankSystem _bankSystem = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    private ISawmill _log = default!;
    private CancellationTokenSource? _timerCancel;

    public override void Initialize()
    {
        base.Initialize();
        _log = Logger.GetSawmill("capibara.payroll");
        SubscribeLocalEvent<RoundStartingEvent>(OnRoundStarted);
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _timerCancel?.Cancel();
        _timerCancel?.Dispose();
    }

    private void OnRoundStarted(RoundStartingEvent ev)
    {
        // Cancel any existing timer from a previous round
        _timerCancel?.Cancel();
        _timerCancel?.Dispose();
        _timerCancel = new CancellationTokenSource();

        var interval = 600f;
        if (_protoManager.TryIndex<SalaryPrototype>("DefaultSalaries", out var salaryProto))
            interval = salaryProto.PayInterval;

        Timer.SpawnRepeating(TimeSpan.FromSeconds(interval), PayAllEmployees, _timerCancel.Token);
    }

    /// <summary>
    /// Triggers an immediate payroll cycle. Used by admin commands.
    /// </summary>
    public void ForcePayroll()
    {
        PayAllEmployees();
    }

    private void PayAllEmployees()
    {
        var count = 0;

        // Iterate over all entities with BankAccountComponent and IdCardComponent (ID cards)
        var query = EntityQueryEnumerator<BankAccountComponent, IdCardComponent>();
        while (query.MoveNext(out var idCardUid, out var bank, out var idCard))
        {
            // Look up salary based on job
            var jobId = idCard.JobPrototype?.Id;
            var salary = _bankSystem.GetSalary(jobId);

            if (salary <= 0)
                continue;

            if (!_bankSystem.TryDeposit(idCardUid, salary, out var newBalance))
                continue;

            count++;

            // Try to notify the player holding this ID card
            NotifyPlayer(idCardUid, salary, newBalance);
        }

        if (count > 0)
            _log.Info($"Payroll: Paid {count} employees.");
    }

    private void NotifyPlayer(EntityUid idCardUid, int salary, int newBalance)
    {
        // The ID card is inside a PDA, which is in a player's inventory slot.
        // Walk up the container hierarchy to find the player mob.
        var holder = FindHolder(idCardUid);
        if (holder == null)
            return;

        if (!_playerManager.TryGetSessionByEntity(holder.Value, out var session))
            return;

        _chatManager.DispatchServerMessage(session,
            Loc.GetString("capibara-payroll-received",
                ("amount", salary),
                ("balance", newBalance)));
    }

    /// <summary>
    /// Walk up the container parents to find the mob entity holding this item.
    /// </summary>
    private EntityUid? FindHolder(EntityUid entity)
    {
        var current = entity;

        // Walk up at most 5 levels (idCard -> PDA -> inventory container -> mob)
        for (var i = 0; i < 5; i++)
        {
            if (HasComp<MobStateComponent>(current))
                return current;

            if (!TryComp<TransformComponent>(current, out var xform) || xform.ParentUid == EntityUid.Invalid)
                return null;

            current = xform.ParentUid;
        }

        return null;
    }
}
