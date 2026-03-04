// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2025 RatherUncreative <RatherUncreativeName@proton.me>
// SPDX-FileCopyrightText: 2025 Ted Lukin <66275205+pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Whatstone <whatston3@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Hands.Components;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.Borgs.Components;

/// <summary>
/// This is used for a <see cref="BorgModuleComponent"/> that provides items to the entity it's installed into.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedBorgSystem))]
public sealed partial class ItemBorgModuleComponent : Component
{
    /// <summary>
    /// The items that are provided (legacy format).
    /// </summary>
    [DataField]
    public List<EntProtoId> Items = new();

    // Hispania: xenoborg port — new hands format for modules with empty hand slots
    /// <summary>
    /// The hands that are provided (new format, supports empty hands for pickup).
    /// When non-empty, this takes priority over <see cref="Items"/>.
    /// </summary>
    [DataField]
    public List<BorgHand> Hands = new();

    /// <summary>
    /// The entities from <see cref="Items"/> that were spawned.
    /// </summary>
    [DataField("providedItems")]
    public SortedDictionary<string, EntityUid> ProvidedItems = new();

    // Hispania: xenoborg port — tracks empty hand IDs for cleanup
    /// <summary>
    /// Hand IDs for empty hands provided by this module (for cleanup on unselect).
    /// </summary>
    [DataField]
    public List<string> EmptyHandIds = new();

    /// <summary>
    /// A counter that ensures a unique hand ID.
    /// </summary>
    [DataField("handCounter")]
    public int HandCounter;

    /// <summary>
    /// Whether or not the items have been created and stored in <see cref="ProvidedContainer"/>
    /// </summary>
    [DataField("itemsCrated")]
    public bool ItemsCreated;

    /// <summary>
    /// A container where provided items are stored when not being used.
    /// This is helpful as it means that items retain state.
    /// </summary>
    [ViewVariables]
    public Container ProvidedContainer = default!;

    /// <summary>
    /// An ID for the container where provided items are stored when not used.
    /// </summary>
    [DataField("providedContainerId")]
    public string ProvidedContainerId = "provided_container";

    /// <summary>
    /// Frontier: a module ID to check for equivalence
    /// </summary>
    [DataField(required: true)]
    public string ModuleId = default!;
}

// Hispania: xenoborg port — BorgHand struct for new hands format
/// <summary>
/// A single hand provided by a borg module.
/// </summary>
[DataDefinition, Serializable, NetSerializable]
public partial record struct BorgHand
{
    /// <summary>
    /// The item to spawn in the hand, if any. Null means an empty hand for pickup.
    /// </summary>
    [DataField]
    public EntProtoId? Item;

    /// <summary>
    /// The settings for the hand, including whitelist/blacklist.
    /// </summary>
    [DataField]
    public Hand Hand = new();

    /// <summary>
    /// If true, the spawned item can be removed from the hand.
    /// </summary>
    [DataField]
    public bool ForceRemovable = false;

    public BorgHand()
    {
    }

    public BorgHand(EntProtoId? item, Hand hand, bool forceRemovable = false)
    {
        Item = item;
        Hand = hand;
        ForceRemovable = forceRemovable;
    }
}
