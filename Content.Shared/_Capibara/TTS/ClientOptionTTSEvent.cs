// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.TTS;

/// <summary>
/// Client → server message to toggle TTS opt-out.
/// </summary>
[Serializable, NetSerializable]
public sealed class ClientOptionTTSEvent : EntityEventArgs
{
    public bool Enabled { get; }

    public ClientOptionTTSEvent(bool enabled)
    {
        Enabled = enabled;
    }
}
