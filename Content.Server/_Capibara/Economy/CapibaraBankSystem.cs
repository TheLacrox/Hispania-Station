// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Inventory;
using Content.Shared._Capibara.Economy;
using Content.Shared._Capibara.Economy.Components;
using Content.Shared.Access.Components;
using Content.Shared.GameTicking;
using Content.Shared.Inventory;
using Content.Shared.PDA;
using Robust.Shared.Prototypes;

namespace Content.Server._Capibara.Economy;

/// <summary>
/// Core banking system for the Capibara economy.
/// Manages bank accounts on ID cards, handles deposits/withdrawals,
/// and sets initial balances at player spawn.
/// </summary>
public sealed partial class CapibaraBankSystem : SharedCapibaraBankSystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;

    private ISawmill _log = default!;

    public override void Initialize()
    {
        base.Initialize();
        _log = Logger.GetSawmill("capibara.bank");

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);

        InitializeATM();
    }

    /// <summary>
    /// When a player spawns, find their ID card and set up a bank account with starting balance.
    /// </summary>
    private void OnPlayerSpawnComplete(PlayerSpawnCompleteEvent ev)
    {
        var idCard = GetIdCardFromPlayer(ev.Mob);
        if (idCard == null)
            return;

        var bank = EnsureComp<BankAccountComponent>(idCard.Value);

        // Look up starting balance from salary prototype
        var startingBalance = GetStartingBalance(ev.JobId);
        bank.Balance = startingBalance;
        Dirty(idCard.Value, bank);

        var cardName = TryComp<IdCardComponent>(idCard.Value, out var card) ? card.FullName : "Unknown";
        _log.Info($"Bank account created for {cardName} (job: {ev.JobId}) with {startingBalance} Spesos.");
    }

    /// <summary>
    /// Attempt to withdraw an amount from a bank account on an ID card.
    /// </summary>
    public bool TryWithdraw(EntityUid idCard, int amount, out int newBalance)
    {
        newBalance = 0;

        if (amount <= 0)
            return false;

        if (!TryComp<BankAccountComponent>(idCard, out var bank))
            return false;

        if (bank.Balance < amount)
            return false;

        bank.Balance -= amount;
        newBalance = bank.Balance;
        Dirty(idCard, bank);
        return true;
    }

    /// <summary>
    /// Deposit an amount into a bank account on an ID card.
    /// </summary>
    public bool TryDeposit(EntityUid idCard, int amount, out int newBalance)
    {
        newBalance = 0;

        if (amount <= 0)
            return false;

        if (!TryComp<BankAccountComponent>(idCard, out var bank))
            return false;

        bank.Balance += amount;
        newBalance = bank.Balance;
        Dirty(idCard, bank);
        return true;
    }

    /// <summary>
    /// Get the balance of a bank account on an ID card.
    /// </summary>
    public bool TryGetBalance(EntityUid idCard, out int balance)
    {
        balance = 0;

        if (!TryComp<BankAccountComponent>(idCard, out var bank))
            return false;

        balance = bank.Balance;
        return true;
    }

    /// <summary>
    /// Attempt to withdraw from a player's bank account by finding their ID card.
    /// </summary>
    public bool TryBankWithdrawFromPlayer(EntityUid mob, int amount)
    {
        var idCard = GetIdCardFromPlayer(mob);
        if (idCard == null)
            return false;

        return TryWithdraw(idCard.Value, amount, out _);
    }

    /// <summary>
    /// Get the player's bank balance by finding their ID card.
    /// </summary>
    public bool TryGetBalanceFromPlayer(EntityUid mob, out int balance)
    {
        balance = 0;
        var idCard = GetIdCardFromPlayer(mob);
        if (idCard == null)
            return false;

        return TryGetBalance(idCard.Value, out balance);
    }

    /// <summary>
    /// Find a player's ID card entity by checking their "id" inventory slot.
    /// Handles the PDA -> ContainedId chain.
    /// </summary>
    public EntityUid? GetIdCardFromPlayer(EntityUid mob)
    {
        if (!_inventory.TryGetSlotEntity(mob, "id", out var idSlot))
            return null;

        // Check if it's a PDA containing an ID card
        if (TryComp<PdaComponent>(idSlot, out var pda) && pda.ContainedId != null)
            return pda.ContainedId.Value;

        // Maybe it's a raw ID card in the slot
        if (HasComp<IdCardComponent>(idSlot))
            return idSlot.Value;

        return null;
    }

    /// <summary>
    /// Get the starting balance for a given job ID from the salary prototype.
    /// </summary>
    private int GetStartingBalance(string? jobId)
    {
        if (!_prototypeManager.TryIndex<SalaryPrototype>("DefaultSalaries", out var salaryProto))
            return 100;

        if (jobId != null && salaryProto.Jobs.TryGetValue(jobId, out var jobData))
            return jobData.StartingBalance;

        return salaryProto.DefaultStartingBalance;
    }

    /// <summary>
    /// Get the salary amount for a given job ID from the salary prototype.
    /// </summary>
    public int GetSalary(string? jobId)
    {
        if (!_prototypeManager.TryIndex<SalaryPrototype>("DefaultSalaries", out var salaryProto))
            return 50;

        if (jobId != null && salaryProto.Jobs.TryGetValue(jobId, out var jobData))
            return jobData.Salary;

        return salaryProto.DefaultSalary;
    }
}
