// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Text.Json.Serialization;

namespace Content.Server._Capibara.TTS;

/// <summary>
/// JSON-serializable job struct sent to the TTS worker via Redis queue.
/// </summary>
public sealed class TtsJob
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("voice_id")]
    public string VoiceId { get; set; } = string.Empty;

    [JsonPropertyName("effect")]
    public string Effect { get; set; } = "none";

    [JsonPropertyName("reply_channel")]
    public string ReplyChannel { get; set; } = string.Empty;
}
