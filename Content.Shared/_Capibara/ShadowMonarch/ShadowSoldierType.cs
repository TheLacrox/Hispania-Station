// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.ShadowMonarch;

[NetSerializable, Serializable]
public enum ShadowSoldierType : byte
{
    Soldier,
    Tank,
    Mage,
}
