// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Capibara.Economy.Components;

/// <summary>
/// Stores a bank account balance on an ID card entity.
/// Attached to ID cards at player spawn time.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class BankAccountComponent : Component
{
    /// <summary>
    /// Current balance in Spesos.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int Balance;
}
