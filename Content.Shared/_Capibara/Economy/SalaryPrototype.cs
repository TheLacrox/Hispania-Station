// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Capibara.Economy;

/// <summary>
/// Defines salary and starting balance configuration per job role.
/// </summary>
[Prototype]
public sealed partial class SalaryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Mapping of JobPrototype ID to salary data.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, JobSalaryData> Jobs { get; private set; } = new();

    /// <summary>
    /// Default starting balance for jobs not in the mapping.
    /// </summary>
    [DataField]
    public int DefaultStartingBalance { get; private set; } = 100;

    /// <summary>
    /// Default salary for jobs not in the mapping.
    /// </summary>
    [DataField]
    public int DefaultSalary { get; private set; } = 50;

    /// <summary>
    /// Salary payment interval in seconds. Default is 10 minutes (600s).
    /// </summary>
    [DataField]
    public float PayInterval { get; private set; } = 600f;
}

[DataDefinition]
public sealed partial class JobSalaryData
{
    [DataField]
    public int StartingBalance { get; private set; } = 100;

    [DataField]
    public int Salary { get; private set; } = 50;
}
