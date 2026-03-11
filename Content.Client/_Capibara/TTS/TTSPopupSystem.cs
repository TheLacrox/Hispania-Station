// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Client.Gameplay;
using Content.Shared._Capibara.CCVar;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Shared.Configuration;

namespace Content.Client._Capibara.TTS;

/// <summary>
/// Shows a one-time popup asking the player if they want TTS enabled.
/// Only shown once — tracked via the tts.popup_shown CVar.
/// </summary>
public sealed class TTSPopupSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IStateManager _stateManager = default!;
    [Dependency] private readonly IUserInterfaceManager _ui = default!;

    private bool _popupShown;
    private bool _displayed;

    public override void Initialize()
    {
        base.Initialize();
        _cfg.OnValueChanged(CapibaraCCVars.TTSPopupShown, OnPopupShownChanged, true);
        _stateManager.OnStateChanged += OnStateChanged;
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _cfg.UnsubValueChanged(CapibaraCCVars.TTSPopupShown, OnPopupShownChanged);
        _stateManager.OnStateChanged -= OnStateChanged;
    }

    private void OnPopupShownChanged(bool shown)
    {
        _popupShown = shown;
    }

    private void OnStateChanged(StateChangedEventArgs args)
    {
        // Show popup when entering gameplay for the first time
        if (args.NewState is not GameplayState)
            return;

        if (_popupShown || _displayed)
            return;

        _displayed = true;
        ShowPopup();
    }

    private void ShowPopup()
    {
        var popup = new TTSPopup();
        popup.OnChoice += enabled =>
        {
            _cfg.SetCVar(CapibaraCCVars.TTSClientEnabled, enabled);
            _cfg.SetCVar(CapibaraCCVars.TTSPopupShown, true);
            _cfg.SaveToFile();
        };
        popup.OpenCentered();
    }
}
