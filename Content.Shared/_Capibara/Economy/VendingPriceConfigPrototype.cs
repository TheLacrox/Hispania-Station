// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Capibara.Economy;

/// <summary>
/// Defines vending machine prices. Maps entity prototype IDs to default prices.
/// The CapibaraVendingSystem adds VendingMachinePriceComponent dynamically
/// to matching vending machines on MapInit.
/// </summary>
[Prototype]
public sealed partial class VendingPriceConfigPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Maps vending machine entity prototype IDs to their default price.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, int> VendorPrices { get; private set; } = new();
}
