// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.NanoGigs;

[Serializable, NetSerializable]
public enum NanoGigStatus : byte
{
    Available,
    Accepted,
    Completed,
    Cancelled,
}

[Serializable, NetSerializable, DataRecord]
public sealed partial class NanoGigListing
{
    public readonly int GigId;
    public readonly string Title;
    public readonly string Description;
    public readonly string Location;
    public readonly int Pay;
    public readonly string PosterName;
    public readonly int PosterAccountId;
    public NanoGigStatus Status;
    public string? TakerName;
    public string? TakerJobTitle;
    public int? TakerAccountId;

    public NanoGigListing(
        int gigId,
        string title,
        string description,
        string location,
        int pay,
        string posterName,
        int posterAccountId)
    {
        GigId = gigId;
        Title = title;
        Description = description;
        Location = location;
        Pay = pay;
        PosterName = posterName;
        PosterAccountId = posterAccountId;
        Status = NanoGigStatus.Available;
    }
}

[Serializable, NetSerializable]
public sealed class NanoGigsUiState : BoundUserInterfaceState
{
    public readonly List<NanoGigListing> Jobs;
    public readonly int ViewerAccountId;
    public readonly int ViewerBalance;
    public readonly bool AlertsEnabled;
    public readonly bool HasIdCard;
    public readonly string? ErrorMessage;

    public NanoGigsUiState(List<NanoGigListing> jobs, int viewerAccountId, int viewerBalance, bool alertsEnabled, bool hasIdCard, string? errorMessage = null)
    {
        Jobs = jobs;
        ViewerAccountId = viewerAccountId;
        ViewerBalance = viewerBalance;
        AlertsEnabled = alertsEnabled;
        HasIdCard = hasIdCard;
        ErrorMessage = errorMessage;
    }
}
