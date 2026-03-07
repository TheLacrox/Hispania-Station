// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Actions;

namespace Content.Shared._Capibara.ShadowMonarch.Events;

// Extraction — target a corpse to raise as shadow soldier (3 types)
public sealed partial class ShadowExtractSoldierEvent : EntityTargetActionEvent;
public sealed partial class ShadowExtractTankEvent : EntityTargetActionEvent;
public sealed partial class ShadowExtractMageEvent : EntityTargetActionEvent;

// Shadow Step — teleport to a world position
public sealed partial class ShadowStepEvent : WorldTargetActionEvent;

// Shadow Exchange — swap position with a shadow soldier
public sealed partial class ShadowExchangeEvent : EntityTargetActionEvent;

// Shadow Hide — click a soldier to hide in monarch's shadow
public sealed partial class ShadowHideEvent : EntityTargetActionEvent;

// Shadow Summon — summon all hidden soldiers at once
public sealed partial class ShadowSummonEvent : InstantActionEvent;

// Shadow Domain — create AoE darkness zone
public sealed partial class ShadowDomainEvent : InstantActionEvent;

// Ascend — final transformation
public sealed partial class ShadowMonarchAscendEvent : InstantActionEvent;

// Broadcast events for rule system
public sealed class ShadowMonarchAscendedEvent : EntityEventArgs
{
    public EntityUid Monarch;
}

public sealed class ShadowMonarchDiedEvent : EntityEventArgs
{
    public EntityUid Monarch;
}
