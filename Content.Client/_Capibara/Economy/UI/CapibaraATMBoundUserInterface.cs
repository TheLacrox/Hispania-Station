// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.Economy;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._Capibara.Economy.UI;

[UsedImplicitly]
public sealed class CapibaraATMBoundUserInterface : BoundUserInterface
{
    private CapibaraATMWindow? _window;

    public CapibaraATMBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = this.CreateWindow<CapibaraATMWindow>();
        _window.OnWithdraw += amount => SendMessage(new CapibaraATMWithdrawMessage(amount));
        _window.OnDeposit += () => SendMessage(new CapibaraATMDepositMessage());
        _window.OnInsert += () => SendMessage(new CapibaraATMInsertMessage());
        _window.OnEject += () => SendMessage(new CapibaraATMEjectMessage());
        _window.OnCreateAccount += () => SendMessage(new CapibaraATMCreateAccountMessage());
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (state is CapibaraATMState atmState)
            _window?.UpdateState(atmState);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
            _window?.Dispose();
    }
}
