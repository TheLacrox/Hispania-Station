// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Collections.Concurrent;
using System.Text.Json;
using Content.Shared._Capibara.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Log;
using StackExchange.Redis;

namespace Content.Server._Capibara.TTS;

/// <summary>
/// Redis-based TTS client. Queues jobs to a Redis list and receives
/// audio chunks back via pub/sub. Matches Starlight's protocol.
/// </summary>
public sealed class TTSClient : ITTSClient
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private const string JobQueueKey = "tts_jobs";
    private const string ReplyChannelPrefix = "tts_reply:";

    private ISawmill _log = default!;
    private ConnectionMultiplexer? _redis;
    private ISubscriber? _subscriber;
    private IDatabase? _db;
    private bool _enabled;
    private string _connectionString = "localhost:6379";

    /// <summary>
    /// Tracks pending TTS jobs by their reply channel, mapping to chunk callbacks.
    /// </summary>
    private readonly ConcurrentDictionary<string, Action<byte[], bool>> _pendingJobs = new();

    /// <summary>
    /// Tracks error callbacks for pending jobs.
    /// </summary>
    private readonly ConcurrentDictionary<string, Action<string>?> _errorCallbacks = new();

    public bool IsConnected => _redis?.IsConnected ?? false;

    public void Initialize()
    {
        IoCManager.InjectDependencies(this);
        _log = Logger.GetSawmill("capibara.tts");

        _cfg.OnValueChanged(CapibaraCCVars.TTSEnabled, OnEnabledChanged, true);
        _cfg.OnValueChanged(CapibaraCCVars.TTSConnectionString, OnConnectionStringChanged, true);

        if (_enabled)
            Connect();
    }

    private void OnEnabledChanged(bool enabled)
    {
        _enabled = enabled;

        if (_enabled && !IsConnected)
            Connect();
        else if (!_enabled && IsConnected)
            Disconnect();
    }

    private void OnConnectionStringChanged(string connectionString)
    {
        _connectionString = connectionString;

        if (_enabled)
        {
            Disconnect();
            Connect();
        }
    }

    private void Connect()
    {
        try
        {
            _log.Info("Connecting to Redis at {0}...", _connectionString);
            var options = ConfigurationOptions.Parse(_connectionString);
            options.AbortOnConnectFail = false;
            options.ConnectTimeout = 5000;

            _redis = ConnectionMultiplexer.Connect(options);
            _db = _redis.GetDatabase();
            _subscriber = _redis.GetSubscriber();

            if (_redis.IsConnected)
                _log.Info("Connected to Redis for TTS.");
            else
                _log.Warning("Redis connection pending — TTS will be unavailable until connected.");

            _redis.ConnectionFailed += (_, args) =>
                _log.Warning("Redis connection failed: {0}", args.Exception?.Message ?? "unknown");

            _redis.ConnectionRestored += (_, _) =>
                _log.Info("Redis connection restored for TTS.");
        }
        catch (Exception e)
        {
            _log.Warning("Failed to connect to Redis for TTS: {0}", e.Message);
            _redis = null;
            _db = null;
            _subscriber = null;
        }
    }

    private void Disconnect()
    {
        try
        {
            _subscriber = null;
            _db = null;
            _redis?.Dispose();
            _redis = null;
            _pendingJobs.Clear();
            _errorCallbacks.Clear();
        }
        catch (Exception e)
        {
            _log.Warning("Error disconnecting from Redis: {0}", e.Message);
        }
    }

    public void GenerateTTS(string text, string voiceId, TTSEffect effect,
        Action<byte[], bool> onChunkReceived, Action<string>? onError = null)
    {
        if (!_enabled || _db == null || _subscriber == null || !IsConnected)
        {
            _log.Warning("TTS GenerateTTS skipped: enabled={0}, db={1}, sub={2}, connected={3}",
                _enabled, _db != null, _subscriber != null, IsConnected);
            onError?.Invoke("TTS not connected");
            return;
        }

        _log.Debug("Queuing TTS job: voice={0}, text=\"{1}\"", voiceId, text);

        var jobId = Guid.NewGuid().ToString();
        var replyChannel = ReplyChannelPrefix + jobId;

        var job = new TtsJob
        {
            Id = jobId,
            Text = text,
            VoiceId = voiceId,
            Effect = effect.ToString().ToLowerInvariant(),
            ReplyChannel = replyChannel,
        };

        _pendingJobs[replyChannel] = onChunkReceived;
        _errorCallbacks[replyChannel] = onError;

        try
        {
            // Subscribe to the reply channel and wait for confirmation before pushing the job.
            // Using SubscribeAsync().Wait() ensures Redis has acknowledged the subscription
            // before we push the job, preventing a race where the worker replies before
            // the subscription is active (which causes the first message to be missed).
            _subscriber.SubscribeAsync(RedisChannel.Literal(replyChannel), (_, message) =>
            {
                HandleReply(replyChannel, message);
            }).Wait();

            // Push the job onto the Redis queue
            var jobJson = JsonSerializer.Serialize(job);
            _db.ListRightPush(JobQueueKey, jobJson);
        }
        catch (Exception e)
        {
            _log.Warning("Failed to queue TTS job: {0}", e.Message);
            _pendingJobs.TryRemove(replyChannel, out _);
            _errorCallbacks.TryRemove(replyChannel, out _);
            onError?.Invoke(e.Message);
        }
    }

    private void HandleReply(string replyChannel, RedisValue message)
    {
        if (!_pendingJobs.TryGetValue(replyChannel, out var callback))
            return;

        if (message.IsNullOrEmpty)
        {
            // Empty message = error or end signal
            CleanupJob(replyChannel);
            return;
        }

        var bytes = (byte[]) message!;

        // Convention: a single byte with value 0 means "end of stream"
        if (bytes.Length == 1 && bytes[0] == 0)
        {
            callback(Array.Empty<byte>(), true);
            CleanupJob(replyChannel);
            return;
        }

        // Convention: a message starting with "ERROR:" is an error
        if (bytes.Length > 6)
        {
            var prefix = System.Text.Encoding.UTF8.GetString(bytes, 0, Math.Min(6, bytes.Length));
            if (prefix == "ERROR:")
            {
                var errorMsg = System.Text.Encoding.UTF8.GetString(bytes, 6, bytes.Length - 6);
                _log.Warning("TTS job error: {0}", errorMsg);
                if (_errorCallbacks.TryGetValue(replyChannel, out var errorCallback))
                    errorCallback?.Invoke(errorMsg);
                CleanupJob(replyChannel);
                return;
            }
        }

        callback(bytes, false);
    }

    private void CleanupJob(string replyChannel)
    {
        _pendingJobs.TryRemove(replyChannel, out _);
        _errorCallbacks.TryRemove(replyChannel, out _);

        try
        {
            _subscriber?.Unsubscribe(RedisChannel.Literal(replyChannel));
        }
        catch
        {
            // Best effort cleanup
        }
    }

    public void Shutdown()
    {
        _cfg.UnsubValueChanged(CapibaraCCVars.TTSEnabled, OnEnabledChanged);
        _cfg.UnsubValueChanged(CapibaraCCVars.TTSConnectionString, OnConnectionStringChanged);
        Disconnect();
    }
}
