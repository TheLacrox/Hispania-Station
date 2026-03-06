# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

This is **Capibara Station**, a fork of Goob Station, which is itself a fork of Space Station 14 (SS14). It's a multiplayer game built on the **RobustToolbox** engine (git submodule). The codebase is C# 12 on .NET 9.0, using an Entity Component System (ECS) architecture. The station is bilingual (English and Spanish).

## Build & Run Commands

```bash
# First-time setup (initializes submodules + downloads engine)
python RUN_THIS.py

# Build (default DebugOpt configuration)
dotnet build

# Run server (default port 1212)
dotnet run --project Content.Server/Content.Server.csproj

# Run client
dotnet run --project Content.Client/Content.Client.csproj

# Run unit tests
dotnet test Content.Tests/Content.Tests.csproj

# Run integration tests
dotnet test Content.IntegrationTests/Content.IntegrationTests.csproj

# Run a single test (by name filter)
dotnet test Content.Tests/Content.Tests.csproj --filter "FullyQualifiedName~TestClassName.TestMethodName"

# YAML linter (validates all prototype YAML files)
dotnet run --project Content.YAMLLinter/Content.YAMLLinter.csproj
```

Build configurations: `Debug`, `DebugOpt` (default), `Release`, `Tools`.

**Important**: Kill running server/client processes before rebuilding — locked exe files cause MSB3027/MSB3021 errors. The server binds port 1212; check for stale processes if you get "port in use" errors.

## Architecture

### Three-Layer Content Split

All game content is split across three layers:

| Layer | Purpose |
|-------|---------|
| `Content.Shared` | Components, prototypes, enums, network messages — synced between client and server |
| `Content.Server` | Server-only systems, game logic, backend services |
| `Content.Client` | Client-only UI, rendering, input handling |

The same split exists for Goobstation extensions (`Content.Goobstation.Shared`, `.Server`, `.Client`, `.Common`, `.Maths`, `.UIKit`) and Capibara-specific code (`_Capibara/` subdirectories within each layer).

### ECS Pattern

RobustToolbox's ECS framework:
- **Components** — Data-only classes with `[RegisterComponent]`. Networked components use `[NetworkedComponent]` and `[AutoGenerateComponentState]` with `[AutoNetworkedField]` on fields.
- **Systems** — Inherit `EntitySystem`, subscribe to events via `SubscribeLocalEvent<TComp, TEvent>()`, query entities with `EntityQueryEnumerator<T1, T2>()`.
- **Partial classes** — Large systems split across files (e.g., `CapibaraBankSystem.cs` + `CapibaraBankSystem.ATM.cs` + `CapibaraBankSystem.SalaryConsole.cs`). `[Dependency]` fields must not duplicate across partials.
- Features are organized by domain directory, each with implementations across Shared/Server/Client.

### Prototype System

Game data is defined in **YAML prototype files** under `Resources/Prototypes/`. C# prototype classes use `[Prototype]`. Custom prototypes go in `Resources/Prototypes/_Capibara/`.

### UI System

Client UI uses XAML (RobustToolbox's UI framework, similar to Avalonia):
- XAML files define layout (`.xaml`), code-behind handles logic (`.xaml.cs`)
- `BoundUserInterface` classes bridge server state to client windows
- Server sends state via `_uiSystem.SetUiState()`, client receives in `UpdateState()`
- UI events use `BoundUserInterfaceMessage` subclasses

### Localization

Uses **Fluent** (`.ftl`) format. Entity names use `ent-{EntityId}` keys (e.g., `ent-CapibaraATM`). All new features must have both `en-US` and `es-ES` locale files.

### Guidebook System

In-game documentation uses XML files in `Resources/ServerInfo/Guidebook/`, registered via `guideEntry` YAML prototypes in `Resources/Prototypes/Guidebook/`. Supports rich markup: `[color]`, `[textlink]`, `[bold]`, `<GuideEntityEmbed>`, and Markdown-style headers (`#`, `##`).

## Fork Structure and Upstream Sync

This repo syncs upstream from Goob Station. To **minimize merge conflicts**, all Capibara-specific code MUST live in `_Capibara/` subdirectories that upstream will never touch.

### Where to put Capibara code (SAFE — no merge conflicts)

| Content type | Path |
|---|---|
| Server systems | `Content.Server/_Capibara/{Feature}/` |
| Shared components/events/prototypes | `Content.Shared/_Capibara/{Feature}/` |
| Client UI (XAML + BUI) | `Content.Client/_Capibara/{Feature}/` |
| YAML prototypes | `Resources/Prototypes/_Capibara/{Feature}/` |
| English locale strings | `Resources/Locale/en-US/_Capibara/{feature}/` |
| Spanish locale strings | `Resources/Locale/es-ES/_Capibara/{feature}/` |
| Guidebook XML content | `Resources/ServerInfo/Guidebook/_Capibara/` |
| Guidebook YAML registration | `Resources/Prototypes/_Capibara/Guidebook/` |
| Textures/sprites | `Resources/Textures/_Capibara/` |

### When you MUST modify upstream files

Sometimes you need to hook into existing upstream systems. These edits create merge conflict risk and should be **kept minimal**. Document each one clearly.

**Currently required upstream edits:**

| File | Why | Conflict risk |
|---|---|---|
| `Content.Server/Botany/Systems/PlantHolderSystem.cs` | Raise `PlantHarvestedEvent` for station objectives tracking | Low (small addition) |
| `Resources/Prototypes/game_presets.yml` | Add `StationObjectivesRule` to game presets | Medium (frequently edited) |
| `Resources/Prototypes/Guidebook/station.yml` | Add `CapibaraEconomy` to guidebook tree | Low (append to list) |
| `Resources/Locale/{en-US,es-ES}/guidebook/guides.ftl` | Add economy guidebook entry names | Low (append to end) |

**Rules for upstream edits:**

1. **Prefer events over direct modification.** Define a new event in `Content.Shared/_Capibara/` and raise it from the upstream file with a 2-3 line addition. Handle all logic in `_Capibara/` systems. (Example: `PlantHarvestedEvent` in botany.)
2. **Append, don't insert.** When adding to YAML lists or locale files, add at the end to minimize diff conflicts.
3. **Never restructure upstream code.** If upstream refactors a file you edited, your change should be easy to re-apply.
4. **Comment your additions.** Use `# Capibara` or a clear marker so edits are easy to find during merge.
5. **Track all upstream edits.** Keep the table above updated when adding new ones.

### Creating a new Capibara feature

Follow this folder structure (example for a feature called `MyFeature`):

```
Content.Shared/_Capibara/MyFeature/
├── Components/MyFeatureComponent.cs    # [RegisterComponent, NetworkedComponent]
├── Events/MyFeatureEvent.cs            # Shared events
├── MyFeaturePrototype.cs               # [Prototype] if needed
└── SharedMyFeatureSystem.cs            # Shared system (ItemSlot registration, etc.)

Content.Server/_Capibara/MyFeature/
└── MyFeatureSystem.cs                  # Server logic, event handlers

Content.Client/_Capibara/MyFeature/
├── UI/MyFeatureBoundUserInterface.cs   # BUI bridge
├── UI/MyFeatureWindow.xaml             # XAML layout
└── UI/MyFeatureWindow.xaml.cs          # Code-behind

Resources/Prototypes/_Capibara/MyFeature/
└── entities.yml                        # Entity prototypes

Resources/Locale/en-US/_Capibara/myfeature/
└── myfeature.ftl                       # English strings

Resources/Locale/es-ES/_Capibara/myfeature/
└── myfeature.ftl                       # Spanish strings
```

## Capibara Station Features

Current custom features:

- **Economy** — Bank accounts on ID cards, ATM machines, salary payroll system, vending machine pricing, salary management console (HOP/Captain)
- **Station Objectives** — Cooperative crew objectives with a 30-minute deadline that freezes salaries if unmet

### Other Fork Content

The repo also includes `_FarHorizons/` directories (a separate fork's content) with features like fission generators, machine linking, and research systems. These follow the same isolation pattern as `_Capibara/` but are not Capibara-specific code.

## Code Style

Enforced via `.editorconfig`:
- 4-space indentation, 120 char line limit, file-scoped namespaces
- `var` preferred everywhere
- Private fields: `_camelCase` prefix with underscore
- Public members, types, methods, properties, constants: `PascalCase`
- Interfaces: `IPascalCase`, type parameters: `TPascalCase`
- Allman-style braces (opening brace on new line)
- Space after cast: `(int) value`
- SPDX license headers on all files (`AGPL-3.0-or-later`)
- Modifier order: `public, private, protected, internal, new, abstract, virtual, sealed, override, static, readonly, extern, unsafe, volatile, async`

## CI Checks

PRs must pass: Build & Test (DebugOpt on Ubuntu), Test Packaging, YAML Linter, RGA/RSI/map validators.

## Key Gotchas

- `RobustToolbox/` is a git submodule — do not modify directly.
- `IdCardComponent.JobDepartments` (`List<ProtoId<DepartmentPrototype>>`) is the correct way to get departments from an ID card. Do NOT use `idCard.JobPrototype` for department lookup — it's a `ProtoId<AccessLevelPrototype>`, not a job ID.
- `DepartmentPrototype.Primary` is `false` for Command department, so `TryGetPrimaryDepartment()` will skip Captain, CMO, etc. Use `IdCardComponent.JobDepartments` directly instead.
- For click-on-entity interactions, use `InteractUsingEvent` from `Content.Shared.Interaction`.
- Button clicks in dynamic UI must use `Button.OnPressed`, not `PanelContainer.OnKeyBindDown`.

## License

AGPL-3.0-or-later for code. Most media: CC-BY-SA 3.0 (some CC-BY-NC-SA 3.0).
