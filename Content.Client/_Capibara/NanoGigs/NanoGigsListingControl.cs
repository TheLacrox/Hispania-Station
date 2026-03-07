// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Capibara.NanoGigs;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.Utility;

namespace Content.Client._Capibara.NanoGigs;

public enum NanoGigsListingMode
{
    Board,
    MyJobs,
}

public sealed class NanoGigsListingControl : PanelContainer
{
    public event Action<int>? OnAccept;
    public event Action<int>? OnFinish;
    public event Action<int>? OnCancel;

    private static readonly Color CardBg = Color.FromHex("#252525d9");
    private static readonly Color CardBorder = Color.FromHex("#40404066");
    private static readonly Color AcceptedBg = Color.FromHex("#173717d9");
    private static readonly Color PayColor = Color.FromHex("#3dd425");
    private static readonly Color StatusAccepted = Color.FromHex("#ef973c");

    public NanoGigsListingControl(NanoGigListing listing, int viewerAccountId, NanoGigsListingMode mode)
    {
        HorizontalExpand = true;

        var isAccepted = listing.Status == NanoGigStatus.Accepted;
        PanelOverride = new StyleBoxFlat
        {
            BackgroundColor = isAccepted ? AcceptedBg : CardBg,
            ContentMarginLeftOverride = 8,
            ContentMarginRightOverride = 8,
            ContentMarginTopOverride = 6,
            ContentMarginBottomOverride = 6,
            BorderColor = CardBorder,
            BorderThickness = new Thickness(0, 0, 0, 1),
        };

        var content = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical,
            HorizontalExpand = true,
        };

        // Row 1: Title + Pay
        var titleRow = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Horizontal,
            HorizontalExpand = true,
        };

        var titleLabel = new RichTextLabel { HorizontalExpand = true };
        titleLabel.SetMessage(FormattedMessage.FromUnformatted(listing.Title));

        var payLabel = new Label
        {
            Text = Loc.GetString("nanogigs-ui-pay-amount", ("pay", listing.Pay)),
            Modulate = PayColor,
        };
        payLabel.AddStyleClass("LabelHeading");

        titleRow.AddChild(titleLabel);
        titleRow.AddChild(payLabel);
        content.AddChild(titleRow);

        // Row 2: Description (if any)
        if (!string.IsNullOrEmpty(listing.Description))
        {
            var descLabel = new RichTextLabel
            {
                HorizontalExpand = true,
                Margin = new Thickness(0, 1, 0, 0),
            };
            descLabel.SetMessage(FormattedMessage.FromUnformatted(listing.Description));
            content.AddChild(descLabel);
        }

        // Row 3: Location
        if (!string.IsNullOrEmpty(listing.Location))
        {
            var locLabel = new RichTextLabel { HorizontalExpand = true, Margin = new Thickness(0, 1, 0, 0) };
            locLabel.SetMessage(FormattedMessage.FromUnformatted(
                Loc.GetString("nanogigs-ui-location", ("location", listing.Location))));
            content.AddChild(locLabel);
        }

        // Row 4: Poster
        var posterLabel = new Label
        {
            Text = Loc.GetString("nanogigs-ui-posted-by", ("name", listing.PosterName)),
            Margin = new Thickness(0, 1, 0, 0),
        };
        posterLabel.AddStyleClass("LabelSubText");
        content.AddChild(posterLabel);

        // Taker info + status for My Jobs
        if (mode == NanoGigsListingMode.MyJobs && isAccepted)
        {
            var takerDisplay = string.IsNullOrEmpty(listing.TakerJobTitle)
                ? listing.TakerName ?? "?"
                : $"{listing.TakerName} ({listing.TakerJobTitle})";

            var takerRow = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Horizontal,
                HorizontalExpand = true,
                Margin = new Thickness(0, 3, 0, 0),
            };

            var takerLabel = new Label
            {
                Text = Loc.GetString("nanogigs-ui-taken-by", ("taker", takerDisplay)),
                HorizontalExpand = true,
                Modulate = StatusAccepted,
            };
            takerLabel.AddStyleClass("LabelSubText");
            takerRow.AddChild(takerLabel);
            content.AddChild(takerRow);
        }

        // Buttons
        var buttonRow = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Horizontal,
            HorizontalAlignment = HAlignment.Right,
            Margin = new Thickness(0, 4, 0, 0),
            SeparationOverride = 4,
        };

        var hasButtons = false;

        if (mode == NanoGigsListingMode.Board && listing.Status == NanoGigStatus.Available
            && listing.PosterAccountId != viewerAccountId)
        {
            var acceptBtn = new Button { Text = Loc.GetString("nanogigs-ui-accept") };
            acceptBtn.OnPressed += _ => OnAccept?.Invoke(listing.GigId);
            buttonRow.AddChild(acceptBtn);
            hasButtons = true;
        }

        if (mode == NanoGigsListingMode.MyJobs)
        {
            if (isAccepted)
            {
                var finishBtn = new Button { Text = Loc.GetString("nanogigs-ui-finish") };
                finishBtn.OnPressed += _ => OnFinish?.Invoke(listing.GigId);
                buttonRow.AddChild(finishBtn);
                hasButtons = true;
            }

            var cancelBtn = new Button { Text = Loc.GetString("nanogigs-ui-cancel") };
            cancelBtn.OnPressed += _ => OnCancel?.Invoke(listing.GigId);
            buttonRow.AddChild(cancelBtn);
            hasButtons = true;
        }

        if (hasButtons)
            content.AddChild(buttonRow);

        AddChild(content);
    }
}
