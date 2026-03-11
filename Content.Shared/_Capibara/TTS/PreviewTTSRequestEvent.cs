// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.TTS;

/// <summary>
/// Client → server request to preview a TTS voice.
/// </summary>
[Serializable, NetSerializable]
public sealed class PreviewTTSRequestEvent : EntityEventArgs
{
    public string VoiceId { get; }

    public PreviewTTSRequestEvent(string voiceId)
    {
        VoiceId = voiceId;
    }
}
