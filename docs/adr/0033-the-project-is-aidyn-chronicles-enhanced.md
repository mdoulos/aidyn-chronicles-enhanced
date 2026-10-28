# 0033. The project is Aidyn Chronicles Enhanced, its code `AidynEnhanced.*`, with a logo and an icon of its own

Status: Accepted · Oct 28, 2026 · Partly supersedes [0030](0030-the-project-is-aidyn-renewed.md) (its names; the fork, its provenance and its rule on citations stand)

## Context

The owner has renamed the project from Aidyn Renewed, the name [0030](0030-the-project-is-aidyn-renewed.md) gave the fork, to Aidyn Chronicles Enhanced, and given it two images: a full logo, the original's title logo redrawn with "Enhanced" under it, and an icon, ACE on the same parchment. The full name is too long for code, so the code goes by Aidyn Enhanced. ACE is the branding's short form. These are the owner's choices.

0030's rename split the fork from another author's project. This one renames the same product, so an installed alpha is upgraded rather than kept beside the new one.

## Decision

- **The project is Aidyn Chronicles Enhanced.** That's the name a player sees: the Godot projects, "Aidyn Chronicles Enhanced" and "Aidyn Chronicles Enhanced Editor", which are the windows' titles and the executables' product names; the installer, its folder `Program Files\Aidyn Chronicles Enhanced` and its Start menu shortcuts; the macOS bundles `Aidyn Chronicles Enhanced.app` and `Aidyn Chronicles Enhanced Editor.app`; the releases, "Aidyn Chronicles Enhanced alpha N"; and the messages the apps and the importer show.
- **The code is `AidynEnhanced.*`.** The solution is `AidynEnhanced.slnx`, and the projects are `AidynEnhanced.Rom`, `AidynEnhanced.Plugins`, `AidynEnhanced.Core`, `AidynEnhanced.Cli`, `AidynEnhanced.Views`, `AidynEnhanced.Game` and `AidynEnhanced.Editor`, with tests in `AidynEnhanced.Core.Tests`, `AidynEnhanced.Plugins.Tests`, `AidynEnhanced.Rom.Tests` and `AidynEnhanced.TestSupport`. Folders, csproj files, root namespaces and assembly names match, as in 0030.
- **Every other name that's an identifier is Aidyn Enhanced's:** the tool `aidyn-enhanced`; the packaged executables `aidyn-enhanced` and `aidyn-enhanced-editor`; the data folder `AidynEnhanced`, which `AIDYN_ENHANCED_DATA` overrides; `AIDYN_ENHANCED_ROMS` for the tests' ROMs, and `AIDYN_ENHANCED_*` for the other debugging variables; the packages, `AidynEnhanced-alpha-N-<platform>…`; the bundle identifiers `com.mdoulos.aidynenhanced` and `com.mdoulos.aidynenhanced.editor`; and the shortcuts' registry key, `HKCU\Software\mdoulos\AidynEnhanced`.
- **ACE is the branding's.** `assets/branding/ace-icon.png` is the apps' icon: each Godot project's `config/icon`, which is the window's icon, the executables' icon (`ace-icon.ico`, `ace-icon.icns`), and the installer's in Add/Remove Programs. `ace-logo-full.png` is the full logo: Godot's boot splash in both apps, the head of the README, and the title screen, where it takes the place of the original's logo (Borg8 `0x0103` over its shadow `0x0104`). It's drawn at the screen's resolution rather than in the UI screen's 320 × 240, so its detail shows ([ui](../systems/ui.md#the-title)).
- **The installer upgrades Aidyn Renewed.** Its `UpgradeCode` stays the same, so installing Aidyn Chronicles Enhanced replaces an installed Aidyn Renewed alpha, in the new folder.
- **The data folder isn't moved for the player.** A player who imported under the old name renames `AidynRenewed` to `AidynEnhanced`, or points `AIDYN_ENHANCED_DATA` at the old folder. Nothing reads the old folder on its own, as in 0030.
- **What keeps "aidyn"** is what 0030 kept, for the same reason: the record ID namespace `aidyn:`, the base plugin `aidyn.base`, everything written into plugins, saves and settings, the game's own names, the Ghidra scripts' project name, and `TempPlugins.Aidyn`. `BasePlugin.ImporterVersion` isn't bumped. The repository keeps its URL, `github.com/mdoulos/aidyn-renewed`, which the installer's About link and the release notes' example name, until it's renamed on GitHub.

## Consequences

- Anything that built or ran the old paths (`src/AidynRenewed.*`, `aidyn-renewed import`) uses the new ones. The Godot projects' gitignored `.godot` folders move with their folders, and `.godot/mono` is deleted if a build complains of stale caches.
- The title's logo is the runtime's own file, not a plugin's image, so a mod that replaces Borg8 `0x0103` no longer changes it ([replacing graphics](../replacing-graphics.md)).
- The glTFs the importer writes name `aidyn-enhanced AidynEnhanced.Rom` as their generator. Earlier imports keep the old name, which nothing reads.
- The earlier ADRs, 0030 among them, read as the history they are. A reader looking for the current names reads this record.
