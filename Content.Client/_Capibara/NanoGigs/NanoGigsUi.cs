// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Client.UserInterface.Fragments;
using Content.Shared._Capibara.NanoGigs;
using Content.Shared.CartridgeLoader;
using Robust.Client.UserInterface;

namespace Content.Client._Capibara.NanoGigs;

public sealed partial class NanoGigsUi : UIFragment
{
    private NanoGigsUiFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        _fragment = new NanoGigsUiFragment();

        _fragment.OnPostJob += (title, description, location, pay) =>
        {
            userInterface.SendMessage(new CartridgeUiMessage(
                new NanoGigsUiMessageEvent(new NanoGigsPostJob(title, description, location, pay))));
        };

        _fragment.OnAcceptJob += gigId =>
        {
            userInterface.SendMessage(new CartridgeUiMessage(
                new NanoGigsUiMessageEvent(new NanoGigsAcceptJob(gigId))));
        };

        _fragment.OnFinishJob += gigId =>
        {
            userInterface.SendMessage(new CartridgeUiMessage(
                new NanoGigsUiMessageEvent(new NanoGigsFinishJob(gigId))));
        };

        _fragment.OnCancelJob += gigId =>
        {
            userInterface.SendMessage(new CartridgeUiMessage(
                new NanoGigsUiMessageEvent(new NanoGigsCancelJob(gigId))));
        };

        _fragment.OnToggleAlerts += () =>
        {
            userInterface.SendMessage(new CartridgeUiMessage(
                new NanoGigsUiMessageEvent(new NanoGigsToggleAlerts())));
        };
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is not NanoGigsUiState gigsState)
            return;

        _fragment?.UpdateState(gigsState);
    }
}
