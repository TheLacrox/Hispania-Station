// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.Economy.Components;
using Content.Shared.Containers.ItemSlots;

namespace Content.Shared._Capibara.Economy;

/// <summary>
/// Shared base for the Capibara banking system.
/// Registers the ATM ItemSlot on both client and server (same pattern as SharedIdCardConsoleSystem).
/// </summary>
public abstract partial class SharedCapibaraBankSystem : EntitySystem
{
    [Dependency] protected readonly ItemSlotsSystem _itemSlotsSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CapibaraATMComponent, ComponentInit>(OnATMInit);
        SubscribeLocalEvent<CapibaraATMComponent, ComponentRemove>(OnATMRemove);
    }

    private void OnATMInit(EntityUid uid, CapibaraATMComponent comp, ComponentInit args)
    {
        _itemSlotsSystem.AddItemSlot(uid, CapibaraATMComponent.IdSlotId, comp.IdSlot);
    }

    private void OnATMRemove(EntityUid uid, CapibaraATMComponent comp, ComponentRemove args)
    {
        _itemSlotsSystem.RemoveItemSlot(uid, comp.IdSlot);
    }
}
