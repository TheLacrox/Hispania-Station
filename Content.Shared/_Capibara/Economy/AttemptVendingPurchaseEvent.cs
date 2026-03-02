// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.VendingMachines;

namespace Content.Shared._Capibara.Economy;

/// <summary>
/// Raised on a vending machine entity before an item is ejected.
/// Can be cancelled to prevent the vend (e.g., insufficient funds).
/// </summary>
[ByRefEvent]
public record struct AttemptVendingPurchaseEvent(
    EntityUid Uid,
    EntityUid Buyer,
    InventoryType Type,
    string ItemId,
    bool Cancelled = false);
