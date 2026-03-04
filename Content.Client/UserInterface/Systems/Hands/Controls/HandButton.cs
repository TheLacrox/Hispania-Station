// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <jmaster9999@gmail.com>
// SPDX-FileCopyrightText: 2022 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Numerics;
using Content.Client.UserInterface.Controls;
using Content.Shared.Hands.Components;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Systems.Hands.Controls;

public sealed class HandButton : SlotControl
{
    public HandLocation HandLocation { get; }

    // Hispania: xenoborg port — placeholder icon for whitelisted empty hands
    private SpriteView? _placeholderView;

    public HandButton(string handName, HandLocation handLocation)
    {
        HandLocation = handLocation;
        Name = "hand_" + handName;
        SlotName = handName;
        SetBackground(handLocation);
    }

    // Hispania: xenoborg port — show a ghosted representative entity when hand is empty
    public void SetPlaceholder(EntityUid? entity)
    {
        if (entity == null)
        {
            if (_placeholderView != null)
                _placeholderView.Visible = false;
            return;
        }

        if (_placeholderView == null)
        {
            _placeholderView = new SpriteView
            {
                Scale = new Vector2(2, 2),
                SetSize = new Vector2(DefaultButtonSize, DefaultButtonSize),
                OverrideDirection = Direction.South,
                Modulate = new Color(1f, 1f, 1f, 0.4f),
            };
            AddChild(_placeholderView);
        }

        _placeholderView.SetEntity(entity);
        _placeholderView.Visible = Entity == null;
    }

    public void UpdatePlaceholderVisibility()
    {
        if (_placeholderView != null)
            _placeholderView.Visible = Entity == null;
    }

    private void SetBackground(HandLocation handLoc)
    {
        ButtonTexturePath = handLoc switch
        {
            HandLocation.Left => "Slots/hand_l",
            HandLocation.Middle => "Slots/hand_m",
            HandLocation.Right => "Slots/hand_r",
            _ => ButtonTexturePath
        };
    }
}