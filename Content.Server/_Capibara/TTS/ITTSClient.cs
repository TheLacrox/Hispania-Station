// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Server._Capibara.TTS;

/// <summary>
/// Interface for the TTS Redis client that queues speech generation jobs
/// and receives audio chunks via pub/sub.
/// </summary>
public interface ITTSClient
{
    /// <summary>
    /// Initialize the Redis connection using the configured connection string.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Queue a TTS generation job and stream audio chunks back via callback.
    /// </summary>
    /// <param name="text">The text to synthesize.</param>
    /// <param name="voiceId">The voice ID to use.</param>
    /// <param name="effect">Audio effect to apply.</param>
    /// <param name="onChunkReceived">Callback for each audio chunk (data, isLast).</param>
    /// <param name="onError">Callback on error.</param>
    void GenerateTTS(string text, string voiceId, TTSEffect effect,
        Action<byte[], bool> onChunkReceived, Action<string>? onError = null);

    /// <summary>
    /// Whether the client is currently connected to Redis.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Shut down the Redis connection.
    /// </summary>
    void Shutdown();
}
