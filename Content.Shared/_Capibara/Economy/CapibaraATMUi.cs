// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.Economy;

[Serializable, NetSerializable]
public enum CapibaraATMUiKey : byte
{
    Key
}

/// <summary>
/// State sent from server to client to update the ATM UI.
/// </summary>
[Serializable, NetSerializable]
public sealed class CapibaraATMState : BoundUserInterfaceState
{
    public int Balance;
    public bool Enabled;
    public string? AccountName;
    public bool HasIdInserted;
    public bool HasIdButNoAccount;

    public CapibaraATMState(int balance, bool enabled, string? accountName, bool hasIdInserted, bool hasIdButNoAccount)
    {
        Balance = balance;
        Enabled = enabled;
        AccountName = accountName;
        HasIdInserted = hasIdInserted;
        HasIdButNoAccount = hasIdButNoAccount;
    }
}

/// <summary>
/// Client requests a withdrawal of the specified amount.
/// </summary>
[Serializable, NetSerializable]
public sealed class CapibaraATMWithdrawMessage : BoundUserInterfaceMessage
{
    public int Amount;

    public CapibaraATMWithdrawMessage(int amount)
    {
        Amount = amount;
    }
}

/// <summary>
/// Client requests to deposit the cash held in their active hand.
/// </summary>
[Serializable, NetSerializable]
public sealed class CapibaraATMDepositMessage : BoundUserInterfaceMessage
{
}

/// <summary>
/// Client requests to insert their ID card/PDA from their inventory into the ATM slot.
/// </summary>
[Serializable, NetSerializable]
public sealed class CapibaraATMInsertMessage : BoundUserInterfaceMessage
{
}

/// <summary>
/// Client requests to eject the ID card/PDA from the ATM slot back to their hand.
/// </summary>
[Serializable, NetSerializable]
public sealed class CapibaraATMEjectMessage : BoundUserInterfaceMessage
{
}

/// <summary>
/// Client requests to create a new bank account on the inserted ID card.
/// </summary>
[Serializable, NetSerializable]
public sealed class CapibaraATMCreateAccountMessage : BoundUserInterfaceMessage
{
}
