// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Configuration;

namespace Content.Shared._Capibara.CCVar;

/// <summary>
/// CVars for the TTS (Text-to-Speech) system.
/// </summary>
[CVarDefs]
public sealed class CapibaraCCVars
{
    /// <summary>
    /// Whether TTS is enabled on the server.
    /// </summary>
    public static readonly CVarDef<bool> TTSEnabled =
        CVarDef.Create("tts.enabled", false, CVar.SERVER | CVar.REPLICATED | CVar.ARCHIVE);

    /// <summary>
    /// Redis connection string for the TTS service.
    /// </summary>
    public static readonly CVarDef<string> TTSConnectionString =
        CVarDef.Create("tts.connection_string", "localhost:6379", CVar.SERVERONLY | CVar.ARCHIVE);

    /// <summary>
    /// Whether TTS is enabled on this client (user preference).
    /// </summary>
    public static readonly CVarDef<bool> TTSClientEnabled =
        CVarDef.Create("tts.client_enabled", true, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Whether the TTS preference popup has been shown to this client.
    /// </summary>
    public static readonly CVarDef<bool> TTSPopupShown =
        CVarDef.Create("tts.popup_shown", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Client volume for TTS speech (0.0 to 1.0).
    /// </summary>
    public static readonly CVarDef<float> TTSVolume =
        CVarDef.Create("tts.volume", 0.5f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Client volume for TTS radio speech.
    /// </summary>
    public static readonly CVarDef<float> TTSRadioVolume =
        CVarDef.Create("tts.radio_volume", 0.5f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Client volume for TTS announcements.
    /// </summary>
    public static readonly CVarDef<float> TTSAnnounceVolume =
        CVarDef.Create("tts.announce_volume", 0.5f, CVar.CLIENTONLY | CVar.ARCHIVE);
}
