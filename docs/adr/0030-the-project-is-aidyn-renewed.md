# 0030. The project is Aidyn Renewed, a fork of HBHT, and cites no third-party decompilation's names

Status: Accepted · Oct 27, 2026 · Supersedes [0028](0028-the-project-is-hbht.md) · Amends [0015](0015-the-port-no-longer-reads-the-decomp.md) (the citations it kept as credit are removed)

## Context

The project was forked from [HBHT](https://github.com/Aquatakat/hbht), Hoo Boy, Here's Trouble, by Aquatakat, on Oct 27, 2026 at its commit 6ec85b3, under its MIT licence. The fork is a clean break from the upstream project's naming: the project's name, which was in the solution, every project and namespace, the data folder, the environment variables, the command line tool, the packages and the docs; the upstream author's name, which was in the packages' company, copyright, bundle identifiers and registry key; and the names of a third-party decompilation of the game, which [0015](0015-the-port-no-longer-reads-the-decomp.md) stopped the project reading but left cited in the code and docs as credit.

Where the work came from stays on record: the git history, the earlier ADRs in their own words, and the upstream copyright notice in `LICENSE`, which the MIT licence requires in every copy.

[0028](0028-the-project-is-hbht.md) moved the project off the game's name because "Aidyn" is a trademark. The fork takes the name its repository already has, Aidyn Renewed, which the README disclaims as unaffiliated with the trademark's holders. That is the owner's choice.

## Decision

- **The project is Aidyn Renewed.** The code is `AidynRenewed.*`: the solution is `AidynRenewed.slnx`, and the projects are `AidynRenewed.Rom`, `AidynRenewed.Plugins`, `AidynRenewed.Core`, `AidynRenewed.Cli`, `AidynRenewed.Views`, `AidynRenewed.Game` and `AidynRenewed.Editor`, with tests in `AidynRenewed.Core.Tests`, `AidynRenewed.Plugins.Tests`, `AidynRenewed.Rom.Tests` and `AidynRenewed.TestSupport`. Folders, csproj files, root namespaces and assembly names match. `AidynRenewed.Views` is not `AidynRenewed.Godot`, for the reason [0022](0022-views-library.md) gives. The earlier ADRs keep the names they were written with.
- **The tool is `aidyn-renewed`,** and the editor's executable `aidyn-renewed-editor`. The Godot projects are "Aidyn Renewed" and "Aidyn Renewed Editor", which are the windows' titles. Messages that tell a player to import say `aidyn-renewed import <rom>`.
- **The data folder is `AidynRenewed`.** `DataPaths.Root` is `AIDYN_RENEWED_DATA` if set, else `AidynRenewed` in the local application data folder. The tests' ROMs come from `AIDYN_RENEWED_ROMS`, and the other debugging variables are `AIDYN_RENEWED_*`. A player who imported under the old name moves the folder or points `AIDYN_RENEWED_DATA` at `%LOCALAPPDATA%\HBHT`; nothing is read from the old folder on its own.
- **The packages are the fork's own.** They are `AidynRenewed-alpha-N-<platform>…`, the releases "Aidyn Renewed alpha N", and the installer goes in `Program Files\Aidyn Renewed`. The company, copyright and manufacturer are mdoulos, the macOS bundle identifiers `com.mdoulos.aidynrenewed` and `com.mdoulos.aidynrenewed.editor`, the shortcuts' registry key `HKCU\Software\mdoulos\AidynRenewed`, and the installer's `UpgradeCode` is new, so installing Aidyn Renewed neither replaces nor refuses an installed HBHT.
- **What keeps "aidyn"** is what [0028](0028-the-project-is-hbht.md) kept, for the same reason: existing imports, saves and settings load without importing again, and `BasePlugin.ImporterVersion` is not bumped. The record ID namespace `aidyn:` and the paths made from it; the base plugin `aidyn.base` and its manifest ID `aidyn`; everything written into plugins, saves and settings; the game's own names; the Ghidra scripts' project name; and the test support's `TempPlugins.Aidyn`.
- **No third-party decompilation's names are cited.** [0015](0015-the-port-no-longer-reads-the-decomp.md) let the citations already written stay as credit. They are removed. A `<c>` tag or a backticked name gives the original's own name, from the debug build's strings, or the symbol map's, or a 1.0 address; where a doc contrasted that decompilation's reading with the machine code's, the machine code's finding stands alone; where it was the only source, the text says the point isn't yet checked against the machine code. The markers in `engine-behaviour.md` that read "(decomp)" read "(pseudocode)". The symbol map's rule is unchanged: the original's name from a string, else the port's, else one we coin, and no name from any third-party decompilation. 0015's rule that the project doesn't read one stands.
- **The licence keeps the upstream copyright notice,** with this project's beside it, and the README's provenance section names the upstream project as the fork's origin and the decompilation's part in its early research, without its names.

## Consequences

- Anything that built or ran the old paths (`src/HBHT.*`, `hbht import`) uses the new ones. The Godot projects' gitignored `.godot` folders move with their folders, and `.godot/mono` is deleted if a build complains of stale caches.
- The install is a separate product from HBHT's: a player with both has both.
- The earlier ADRs, 0028 among them, read as the history they are: `HBHT.*`, `hbht`, and the decompilation by name. A reader looking for the current names reads this record.
- `CLAUDE.md`'s glossary and ground rules say the same: the original's names are the ROM's or the symbol map's, and no third-party decompilation is read or cited.
