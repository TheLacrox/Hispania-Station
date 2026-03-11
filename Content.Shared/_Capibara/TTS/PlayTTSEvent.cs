// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.TTS;

/// <summary>
/// Sent server → client to begin a new TTS audio stream for an entity.
/// </summary>
[Serializable, NetSerializable]
public sealed class TTSHeaderEvent : EntityEventArgs
{
    /// <summary>
    /// Unique identifier for this TTS stream.
    /// </summary>
    public Guid StreamId { get; }

    /// <summary>
    /// The entity speaking.
    /// </summary>
    public NetEntity SourceUid { get; }

    /// <summary>
    /// Whether this is whispered speech (lower volume, shorter range).
    /// </summary>
    public bool IsWhisper { get; }

    public TTSHeaderEvent(Guid streamId, NetEntity sourceUid, bool isWhisper)
    {
        StreamId = streamId;
        SourceUid = sourceUid;
        IsWhisper = isWhisper;
    }
}

/// <summary>
/// Sent server → client with a chunk of OGG audio data for a TTS stream.
/// </summary>
[Serializable, NetSerializable]
public sealed class TTSChunkEvent : EntityEventArgs
{
    /// <summary>
    /// The stream this chunk belongs to.
    /// </summary>
    public Guid StreamId { get; }

    /// <summary>
    /// Raw OGG audio data.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// True if this is the final chunk in the stream.
    /// </summary>
    public bool IsLast { get; }

    public TTSChunkEvent(Guid streamId, byte[] data, bool isLast)
    {
        StreamId = streamId;
        Data = data;
        IsLast = isLast;
    }
}
