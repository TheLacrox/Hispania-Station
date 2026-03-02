// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Popups;
using Content.Shared._Capibara.Economy;
using Content.Shared._Capibara.Economy.Components;
using Content.Shared.VendingMachines;
using Robust.Shared.Prototypes;

namespace Content.Server._Capibara.Economy;

/// <summary>
/// Handles vending machine purchases by charging the player's bank account.
/// Dynamically adds <see cref="VendingMachinePriceComponent"/> to vendors
/// listed in <see cref="VendingPriceConfigPrototype"/> on MapInit.
/// </summary>
public sealed class CapibaraVendingSystem : EntitySystem
{
    [Dependency] private readonly CapibaraBankSystem _bankSystem = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;

    private Dictionary<string, int> _vendorPrices = new();

    public override void Initialize()
    {
        base.Initialize();

        // Load price config
        if (_protoManager.TryIndex<VendingPriceConfigPrototype>("DefaultVendingPrices", out var config))
            _vendorPrices = config.VendorPrices;

        // Add VendingMachinePriceComponent to matching vendors on startup
        SubscribeLocalEvent<VendingMachineComponent, ComponentStartup>(OnVendingStartup);

        // Handle purchase attempts
        SubscribeLocalEvent<VendingMachinePriceComponent, AttemptVendingPurchaseEvent>(OnAttemptPurchase);
    }

    private void OnVendingStartup(EntityUid uid, VendingMachineComponent component, ComponentStartup args)
    {
        // Check if this vendor's prototype is in our price config
        if (!TryComp<MetaDataComponent>(uid, out var meta))
            return;

        var protoId = meta.EntityPrototype?.ID;
        if (protoId == null || !_vendorPrices.TryGetValue(protoId, out var defaultPrice))
            return;

        // Add the price component dynamically
        var priceComp = EnsureComp<VendingMachinePriceComponent>(uid);
        priceComp.DefaultPrice = defaultPrice;
        Dirty(uid, priceComp);
    }

    private void OnAttemptPurchase(EntityUid uid, VendingMachinePriceComponent priceComp, ref AttemptVendingPurchaseEvent args)
    {
        if (args.Cancelled)
            return;

        // Don't charge if the machine can't actually dispense right now
        if (!CanVendItem(uid, args.Type, args.ItemId))
        {
            args.Cancelled = true;
            return;
        }

        // Determine the price for this item
        var price = GetItemPrice(priceComp, args.ItemId);

        // Free items pass through
        if (price <= 0)
            return;

        // Try to charge the player
        if (!_bankSystem.TryBankWithdrawFromPlayer(args.Buyer, price))
        {
            _popup.PopupEntity(
                Loc.GetString("capibara-vending-insufficient-funds", ("price", price)),
                uid, args.Buyer);
            args.Cancelled = true;
            return;
        }

        _popup.PopupEntity(
            Loc.GetString("capibara-vending-purchased", ("price", price)),
            uid, args.Buyer);
    }

    /// <summary>
    /// Checks if the vending machine can actually dispense an item right now.
    /// Prevents charging when the machine is busy ejecting or the item is out of stock.
    /// </summary>
    private bool CanVendItem(EntityUid uid, InventoryType type, string itemId)
    {
        if (!TryComp<VendingMachineComponent>(uid, out var vendComp))
            return false;

        // Machine is currently ejecting another item
        if (vendComp.Ejecting)
            return false;

        // Machine is broken
        if (vendComp.Broken)
            return false;

        // Look up the item entry and check stock
        var inventory = type switch
        {
            InventoryType.Contraband => vendComp.ContrabandInventory,
            InventoryType.Emagged => vendComp.EmaggedInventory,
            _ => vendComp.Inventory,
        };

        if (!inventory.TryGetValue(itemId, out var entry))
            return false;

        // Out of stock
        if (entry.Amount <= 0)
            return false;

        return true;
    }

    private int GetItemPrice(VendingMachinePriceComponent priceComp, string itemId)
    {
        // Check per-item override first
        if (priceComp.ItemPrices.TryGetValue(itemId, out var itemPrice))
            return itemPrice;

        // If free by default, unlisted items are free
        if (priceComp.FreeByDefault)
            return 0;

        return priceComp.DefaultPrice;
    }
}
