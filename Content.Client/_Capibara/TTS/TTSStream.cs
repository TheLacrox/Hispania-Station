// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Client._Capibara.TTS;

/// <summary>
/// Local event raised when a TTS stream has been fully assembled and is ready to play.
/// </summary>
public sealed class TTSStreamReadyEvent
{
    /// <summary>
    /// The entity that spoke.
    /// </summary>
    public EntityUid SourceUid { get; }

    /// <summary>
    /// Assembled OGG audio data.
    /// </summary>
    public byte[] AudioData { get; }

    /// <summary>
    /// Whether this was whispered speech.
    /// </summary>
    public bool IsWhisper { get; }

    public TTSStreamReadyEvent(EntityUid sourceUid, byte[] audioData, bool isWhisper)
    {
        SourceUid = sourceUid;
        AudioData = audioData;
        IsWhisper = isWhisper;
    }
}
