// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Capibara.Economy.Components;

/// <summary>
/// When present on a VendingMachine entity, items cost Spesos to purchase.
/// Vendors without this component remain free.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class VendingMachinePriceComponent : Component
{
    /// <summary>
    /// Default price for all items in this vendor unless overridden.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int DefaultPrice = 10;

    /// <summary>
    /// Per-item price overrides. Key is entity prototype ID.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<string, int> ItemPrices = new();

    /// <summary>
    /// If true, items without a specific price override are free.
    /// If false (default), unlisted items use DefaultPrice.
    /// </summary>
    [DataField]
    public bool FreeByDefault;
}
