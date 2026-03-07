// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared._Capibara.NanoGigs;

[Serializable, NetSerializable]
public sealed class NanoGigsUiMessageEvent : CartridgeMessageEvent
{
    public readonly INanoGigsPayload Payload;

    public NanoGigsUiMessageEvent(INanoGigsPayload payload)
    {
        Payload = payload;
    }
}

public interface INanoGigsPayload;

[Serializable, NetSerializable, DataRecord]
public sealed partial class NanoGigsPostJob : INanoGigsPayload
{
    public readonly string Title;
    public readonly string Description;
    public readonly string Location;
    public readonly int Pay;

    public NanoGigsPostJob(string title, string description, string location, int pay)
    {
        Title = title;
        Description = description;
        Location = location;
        Pay = pay;
    }
}

[Serializable, NetSerializable, DataRecord]
public sealed partial class NanoGigsAcceptJob : INanoGigsPayload
{
    public readonly int GigId;
    public NanoGigsAcceptJob(int gigId) { GigId = gigId; }
}

[Serializable, NetSerializable, DataRecord]
public sealed partial class NanoGigsFinishJob : INanoGigsPayload
{
    public readonly int GigId;
    public NanoGigsFinishJob(int gigId) { GigId = gigId; }
}

[Serializable, NetSerializable, DataRecord]
public sealed partial class NanoGigsCancelJob : INanoGigsPayload
{
    public readonly int GigId;
    public NanoGigsCancelJob(int gigId) { GigId = gigId; }
}

[Serializable, NetSerializable]
public sealed class NanoGigsToggleAlerts : INanoGigsPayload;
