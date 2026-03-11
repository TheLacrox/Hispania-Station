// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.TTS;

namespace Content.Client._Capibara.TTS;

/// <summary>
/// Receives TTSHeader and TTSChunk network events, assembles audio streams,
/// and raises TTSStreamReadyEvent when complete.
/// </summary>
public sealed class TextToSpeechStreamSystem : EntitySystem
{
    /// <summary>
    /// Active streams being assembled. Key = StreamId.
    /// </summary>
    private readonly Dictionary<Guid, StreamState> _activeStreams = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<TTSHeaderEvent>(OnTTSHeader);
        SubscribeNetworkEvent<TTSChunkEvent>(OnTTSChunk);
    }

    private void OnTTSHeader(TTSHeaderEvent ev)
    {
        var sourceUid = GetEntity(ev.SourceUid);

        _activeStreams[ev.StreamId] = new StreamState
        {
            SourceUid = sourceUid,
            IsWhisper = ev.IsWhisper,
            Chunks = new List<byte[]>(),
        };
    }

    private void OnTTSChunk(TTSChunkEvent ev)
    {
        if (!_activeStreams.TryGetValue(ev.StreamId, out var state))
            return;

        if (ev.Data.Length > 0)
            state.Chunks.Add(ev.Data);

        if (!ev.IsLast)
            return;

        // Assemble all chunks into a single byte array
        var totalLength = 0;
        foreach (var chunk in state.Chunks)
            totalLength += chunk.Length;

        if (totalLength == 0)
        {
            _activeStreams.Remove(ev.StreamId);
            return;
        }

        var assembled = new byte[totalLength];
        var offset = 0;
        foreach (var chunk in state.Chunks)
        {
            chunk.CopyTo(assembled.AsSpan(offset));
            offset += chunk.Length;
        }

        _activeStreams.Remove(ev.StreamId);

        // Raise local event for the playback system
        var readyEvent = new TTSStreamReadyEvent(state.SourceUid, assembled, state.IsWhisper);
        RaiseLocalEvent(readyEvent);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // Clean up stale streams (older than 30 seconds)
        var stale = new List<Guid>();
        foreach (var (id, state) in _activeStreams)
        {
            state.Age += frameTime;
            if (state.Age > 30f)
                stale.Add(id);
        }

        foreach (var id in stale)
            _activeStreams.Remove(id);
    }

    private sealed class StreamState
    {
        public EntityUid SourceUid;
        public bool IsWhisper;
        public List<byte[]> Chunks = new();
        public float Age;
    }
}
