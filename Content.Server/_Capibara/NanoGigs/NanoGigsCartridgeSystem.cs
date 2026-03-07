// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._Capibara.Economy;
using Content.Server.CartridgeLoader;
using Content.Shared._Capibara.Economy.Components;
using Content.Shared._Capibara.NanoGigs;
using Content.Shared.Access.Components;
using Content.Shared.CartridgeLoader;
using Content.Shared.PDA;
using Content.Shared.Station.Components;

namespace Content.Server._Capibara.NanoGigs;

public sealed class NanoGigsCartridgeSystem : EntitySystem
{
    [Dependency] private readonly CartridgeLoaderSystem _cartridgeLoader = default!;
    [Dependency] private readonly CapibaraBankSystem _bank = default!;

    private const int MaxTitleLength = 50;
    private const int MaxDescriptionLength = 150;
    private const int MaxLocationLength = 50;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<NanoGigsCartridgeComponent, CartridgeMessageEvent>(OnUiMessage);
        SubscribeLocalEvent<NanoGigsCartridgeComponent, CartridgeUiReadyEvent>(OnUiReady);
    }

    private void OnUiReady(Entity<NanoGigsCartridgeComponent> ent, ref CartridgeUiReadyEvent args)
    {
        UpdateUiState(ent, args.Loader);
    }

    private void OnUiMessage(Entity<NanoGigsCartridgeComponent> ent, ref CartridgeMessageEvent args)
    {
        if (args is not NanoGigsUiMessageEvent message)
            return;

        var success = message.Payload switch
        {
            NanoGigsPostJob post => HandlePostJob(ent, message.Actor, post),
            NanoGigsAcceptJob accept => HandleAcceptJob(ent, message.Actor, accept),
            NanoGigsFinishJob finish => HandleFinishJob(ent, message.Actor, finish),
            NanoGigsCancelJob cancel => HandleCancelJob(ent, message.Actor, cancel),
            NanoGigsToggleAlerts => HandleToggleAlerts(ent),
            _ => false,
        };

        if (success)
            UpdateAllUIs(ent);
    }

    private bool HandlePostJob(EntityUid cartridge, EntityUid actor, NanoGigsPostJob post)
    {
        var idCard = _bank.GetIdCardFromPlayer(actor);
        if (idCard == null)
        {
            SendError(cartridge, Loc.GetString("nanogigs-error-no-id"));
            return false;
        }

        // Validate inputs
        var title = post.Title.Trim();
        var description = post.Description.Trim();
        var location = post.Location.Trim();

        if (string.IsNullOrEmpty(title) || title.Length > MaxTitleLength)
        {
            SendError(cartridge, Loc.GetString("nanogigs-error-invalid-title"));
            return false;
        }

        if (description.Length > MaxDescriptionLength)
        {
            SendError(cartridge, Loc.GetString("nanogigs-error-invalid-description"));
            return false;
        }

        if (location.Length > MaxLocationLength)
        {
            SendError(cartridge, Loc.GetString("nanogigs-error-invalid-location"));
            return false;
        }

        if (post.Pay <= 0)
        {
            SendError(cartridge, Loc.GetString("nanogigs-error-invalid-pay"));
            return false;
        }

        // Escrow: withdraw pay from poster
        if (!_bank.TryWithdraw(idCard.Value, post.Pay, out _))
        {
            SendError(cartridge, Loc.GetString("nanogigs-error-insufficient-funds"));
            return false;
        }

        if (!TryComp<BankAccountComponent>(idCard.Value, out var bankAccount))
            return false;

        var posterName = TryComp<IdCardComponent>(idCard.Value, out var card) ? card.FullName ?? "Unknown" : "Unknown";

        var board = GetOrCreateBoard(cartridge);
        if (board == null)
            return false;

        var listing = new NanoGigListing(
            board.NextGigId++,
            title,
            description,
            location,
            post.Pay,
            posterName,
            bankAccount.AccountId);

        board.Jobs.Add(listing);

        // Notify users with alerts enabled
        NotifyNewJob(listing, bankAccount.AccountId);
        return true;
    }

    private bool HandleAcceptJob(EntityUid cartridge, EntityUid actor, NanoGigsAcceptJob accept)
    {
        var idCard = _bank.GetIdCardFromPlayer(actor);
        if (idCard == null)
        {
            SendError(cartridge, Loc.GetString("nanogigs-error-no-id"));
            return false;
        }

        if (!TryComp<BankAccountComponent>(idCard.Value, out var bankAccount))
            return false;

        var board = GetOrCreateBoard(cartridge);
        if (board == null)
            return false;

        var job = board.Jobs.Find(j => j.GigId == accept.GigId);
        if (job == null || job.Status != NanoGigStatus.Available)
            return false;

        // Can't accept your own job
        if (job.PosterAccountId == bankAccount.AccountId)
        {
            SendError(cartridge, Loc.GetString("nanogigs-error-own-job"));
            return false;
        }

        var takerName = TryComp<IdCardComponent>(idCard.Value, out var card) ? card.FullName ?? "Unknown" : "Unknown";
        var takerJobTitle = card?.JobTitle ?? "";

        job.Status = NanoGigStatus.Accepted;
        job.TakerName = takerName;
        job.TakerJobTitle = takerJobTitle;
        job.TakerAccountId = bankAccount.AccountId;

        // Notify poster
        var displayName = string.IsNullOrEmpty(takerJobTitle)
            ? takerName
            : $"{takerName} ({takerJobTitle})";
        NotifyByAccountId(cartridge, job.PosterAccountId,
            Loc.GetString("nanogigs-program-name"),
            Loc.GetString("nanogigs-notify-accepted", ("taker", displayName), ("title", job.Title)));
        return true;
    }

    private bool HandleFinishJob(EntityUid cartridge, EntityUid actor, NanoGigsFinishJob finish)
    {
        var idCard = _bank.GetIdCardFromPlayer(actor);
        if (idCard == null)
            return false;

        if (!TryComp<BankAccountComponent>(idCard.Value, out var bankAccount))
            return false;

        var board = GetOrCreateBoard(cartridge);
        if (board == null)
            return false;

        var job = board.Jobs.Find(j => j.GigId == finish.GigId);
        if (job == null || job.Status != NanoGigStatus.Accepted)
            return false;

        // Only poster can finish
        if (job.PosterAccountId != bankAccount.AccountId)
            return false;

        // Pay the taker by finding their ID card
        if (job.TakerAccountId != null)
        {
            var takerIdCard = FindIdCardByAccountId(job.TakerAccountId.Value);
            if (takerIdCard != null)
            {
                _bank.TryDeposit(takerIdCard.Value, job.Pay, out _);
            }

            // Notify taker
            NotifyByAccountId(cartridge, job.TakerAccountId.Value,
                Loc.GetString("nanogigs-program-name"),
                Loc.GetString("nanogigs-notify-paid", ("pay", job.Pay), ("title", job.Title)));
        }

        board.Jobs.Remove(job);
        return true;
    }

    private bool HandleCancelJob(EntityUid cartridge, EntityUid actor, NanoGigsCancelJob cancel)
    {
        var idCard = _bank.GetIdCardFromPlayer(actor);
        if (idCard == null)
            return false;

        if (!TryComp<BankAccountComponent>(idCard.Value, out var bankAccount))
            return false;

        var board = GetOrCreateBoard(cartridge);
        if (board == null)
            return false;

        var job = board.Jobs.Find(j => j.GigId == cancel.GigId);
        if (job == null)
            return false;

        if (job.PosterAccountId == bankAccount.AccountId)
        {
            // Poster cancelling — refund escrow
            var posterIdCard = FindIdCardByAccountId(job.PosterAccountId);
            if (posterIdCard != null)
                _bank.TryDeposit(posterIdCard.Value, job.Pay, out _);

            // Notify taker if was accepted
            if (job.Status == NanoGigStatus.Accepted && job.TakerAccountId != null)
            {
                NotifyByAccountId(cartridge, job.TakerAccountId.Value,
                    Loc.GetString("nanogigs-program-name"),
                    Loc.GetString("nanogigs-notify-cancelled", ("title", job.Title)));
            }

            board.Jobs.Remove(job);
            return true;
        }

        if (job.TakerAccountId == bankAccount.AccountId && job.Status == NanoGigStatus.Accepted)
        {
            // Taker abandoning
            var takerName = job.TakerName ?? "Someone";
            job.Status = NanoGigStatus.Available;
            job.TakerName = null;
            job.TakerJobTitle = null;
            job.TakerAccountId = null;

            NotifyByAccountId(cartridge, job.PosterAccountId,
                Loc.GetString("nanogigs-program-name"),
                Loc.GetString("nanogigs-notify-abandoned", ("taker", takerName), ("title", job.Title)));
            return true;
        }

        return false;
    }

    private bool HandleToggleAlerts(Entity<NanoGigsCartridgeComponent> ent)
    {
        ent.Comp.AlertsEnabled = !ent.Comp.AlertsEnabled;
        return true;
    }

    private StationGigBoardComponent? GetOrCreateBoard(EntityUid cartridge)
    {
        // Find the station this cartridge belongs to
        var query = EntityQueryEnumerator<StationDataComponent>();
        while (query.MoveNext(out var stationUid, out _))
        {
            return EnsureComp<StationGigBoardComponent>(stationUid);
        }

        return null;
    }

    private EntityUid? FindIdCardByAccountId(int accountId)
    {
        var query = EntityQueryEnumerator<BankAccountComponent>();
        while (query.MoveNext(out var uid, out var bank))
        {
            if (bank.AccountId == accountId)
                return uid;
        }

        return null;
    }

    private void NotifyNewJob(NanoGigListing listing, int posterAccountId)
    {
        var header = Loc.GetString("nanogigs-program-name");
        var message = Loc.GetString("nanogigs-notify-new-job", ("title", listing.Title), ("pay", listing.Pay));

        var query = EntityQueryEnumerator<NanoGigsCartridgeComponent, CartridgeComponent>();
        while (query.MoveNext(out _, out var gigsComp, out var cartComp))
        {
            if (!gigsComp.AlertsEnabled || cartComp.LoaderUid == null)
                continue;

            var loaderUid = cartComp.LoaderUid.Value;

            if (!TryComp<PdaComponent>(loaderUid, out var pda) || pda.ContainedId == null)
                continue;

            if (!TryComp<BankAccountComponent>(pda.ContainedId.Value, out var bank))
                continue;

            // Don't notify the poster themselves
            if (bank.AccountId == posterAccountId)
                continue;

            _cartridgeLoader.SendNotification(loaderUid, header, message);
        }
    }

    private void NotifyByAccountId(EntityUid sourceCartridge, int targetAccountId, string header, string message)
    {
        var query = EntityQueryEnumerator<NanoGigsCartridgeComponent, CartridgeComponent>();
        while (query.MoveNext(out _, out _, out var cartComp))
        {
            if (cartComp.LoaderUid == null)
                continue;

            var loaderUid = cartComp.LoaderUid.Value;

            // Get the PDA's contained ID card
            if (!TryComp<PdaComponent>(loaderUid, out var pda) || pda.ContainedId == null)
                continue;

            if (!TryComp<BankAccountComponent>(pda.ContainedId.Value, out var bank))
                continue;

            if (bank.AccountId == targetAccountId)
            {
                _cartridgeLoader.SendNotification(loaderUid, header, message);
                return;
            }
        }
    }

    private void SendError(EntityUid cartridge, string error)
    {
        if (!TryComp<CartridgeComponent>(cartridge, out var cartComp) || cartComp.LoaderUid == null)
            return;

        var state = BuildUiState(cartComp.LoaderUid.Value, error);
        _cartridgeLoader.UpdateCartridgeUiState(cartComp.LoaderUid.Value, state);
    }

    private void UpdateUiState(Entity<NanoGigsCartridgeComponent> ent, EntityUid loaderUid)
    {
        var state = BuildUiState(loaderUid);
        _cartridgeLoader.UpdateCartridgeUiState(loaderUid, state);
    }

    private void UpdateAllUIs(EntityUid sourceCartridge)
    {
        var query = EntityQueryEnumerator<NanoGigsCartridgeComponent, CartridgeComponent>();
        while (query.MoveNext(out _, out _, out var cartComp))
        {
            if (cartComp.LoaderUid == null)
                continue;

            var state = BuildUiState(cartComp.LoaderUid.Value);
            _cartridgeLoader.UpdateCartridgeUiState(cartComp.LoaderUid.Value, state);
        }
    }

    private NanoGigsUiState BuildUiState(EntityUid loaderUid, string? error = null)
    {
        var viewerAccountId = 0;
        var viewerBalance = 0;
        var alertsEnabled = false;
        var hasIdCard = false;

        if (TryComp<PdaComponent>(loaderUid, out var pda) && pda.ContainedId != null)
        {
            hasIdCard = true;

            if (TryComp<BankAccountComponent>(pda.ContainedId.Value, out var bank))
            {
                viewerAccountId = bank.AccountId;
                viewerBalance = bank.Balance;
            }
        }

        // Find the alerts setting for this PDA's cartridge
        var cartQuery = EntityQueryEnumerator<NanoGigsCartridgeComponent, CartridgeComponent>();
        while (cartQuery.MoveNext(out _, out var gigsComp, out var cartComp))
        {
            if (cartComp.LoaderUid == loaderUid)
            {
                alertsEnabled = gigsComp.AlertsEnabled;
                break;
            }
        }

        // Get jobs from the first station's board
        var jobs = new List<NanoGigListing>();
        var stationQuery = EntityQueryEnumerator<StationGigBoardComponent>();
        if (stationQuery.MoveNext(out _, out var board))
        {
            jobs = board.Jobs;
        }

        return new NanoGigsUiState(jobs, viewerAccountId, viewerBalance, alertsEnabled, hasIdCard, error);
    }
}
