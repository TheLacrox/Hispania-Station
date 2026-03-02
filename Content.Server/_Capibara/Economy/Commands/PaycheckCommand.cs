// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._Capibara.Economy.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class PaycheckCommand : IConsoleCommand
{
    public string Command => "paycheck";
    public string Description => "Triggers an immediate payroll cycle for all employees.";
    public string Help => "Usage: paycheck";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var payroll = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<CapibaraPayrollSystem>();
        payroll.ForcePayroll();
        shell.WriteLine("Payroll cycle triggered.");
    }
}
