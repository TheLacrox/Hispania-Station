// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Server._Capibara.ShadowMonarch.Rules;

[RegisterComponent, Access(typeof(ShadowMonarchRuleSystem))]
public sealed partial class ShadowMonarchRuleComponent : Component
{
    [DataField]
    public ShadowMonarchWinCondition WinCondition = ShadowMonarchWinCondition.Draw;
}

public enum ShadowMonarchWinCondition : byte
{
    Draw,
    Win,
    Failure
}
