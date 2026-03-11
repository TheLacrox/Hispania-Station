// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Client.Audio;

namespace Content.Client._Capibara.TTS;

/// <summary>
/// Tracks active TTS audio playback on an entity.
/// Removed when playback completes.
/// </summary>
[RegisterComponent]
public sealed partial class ClientTTSAudioComponent : Component
{
    /// <summary>
    /// The currently playing TTS audio stream.
    /// </summary>
    public AudioStream? PlayingStream;
}
