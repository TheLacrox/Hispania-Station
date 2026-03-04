// SPDX-FileCopyrightText: 2025 space-wizards contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._Wizden.Antag.Components;
using Content.Server.Antag;
using Robust.Shared.Random;

namespace Content.Server._Wizden.Antag;

public sealed class AntagMultipleRoleSpawnerSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ILogManager _log = default!;

    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AntagMultipleRoleSpawnerComponent, AntagSelectEntityEvent>(OnSelectEntity);

        _sawmill = _log.GetSawmill("antag_multiple_spawner");
    }

    private void OnSelectEntity(Entity<AntagMultipleRoleSpawnerComponent> ent, ref AntagSelectEntityEvent args)
    {
        if (args.AntagRoles.Count != 1)
        {
            _sawmill.Fatal($"Antag multiple role spawner had more than one antag ({args.AntagRoles.Count})");
            return;
        }

        var role = args.AntagRoles[0];

        var entProtos = ent.Comp.AntagRoleToPrototypes[role];

        if (entProtos.Count == 0)
            return;

        args.Entity = Spawn(ent.Comp.PickAndTake ? _random.PickAndTake(entProtos) : _random.Pick(entProtos));
    }
}
