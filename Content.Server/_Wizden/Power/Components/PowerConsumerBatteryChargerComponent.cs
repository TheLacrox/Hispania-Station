// SPDX-FileCopyrightText: 2025 space-wizards contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Power.Components;

namespace Content.Server._Wizden.Power.Components;

/// <summary>
/// Charges the battery from an entity with <see cref="PowerConsumerComponent"/>.
/// </summary>
[RegisterComponent]
public sealed partial class PowerConsumerBatteryChargerComponent : Component;
