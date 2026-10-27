# 0031. A data edit of NA 1.0 imports as 1.0

Status: Accepted · Oct 27, 2026 · Supersedes part of [0007](0007-rom-versions.md) (refusing every ROM hack)

## Context

[0007](0007-rom-versions.md) refuses every ROM it doesn't know by its SHA-1, hacks included, because guessing where an unknown ROM keeps its tables risks a subtly wrong `aidyn.base`.

The project's owner makes ROM hacks of NA 1.0: the Action Edition and the Doulos Edit, 26 builds from March 2023 to February 2026. Every one changed only bytes inside tables the layout already places: the entity, weapon, armor, item, chest and shop databases, and the code segment's potion prices, potion recipes and training costs. In every one the code, the Borg archive, its index, the boot code and every byte between the tables are 1.0's, and every table is where 1.0 has it. Such a ROM needs no guessing: it is 1.0 with other data in 1.0's tables, and the importer builds its records from those tables.

## Decision

- **A data edit of NA 1.0 is NA 1.0 under its own name and hash.** For a ROM whose SHA-1 is unknown, `RomVersions.FindDataEdit` zeroes the header's two checksums (`0x10`–`0x17`) and every table NA 1.0's layout places, and compares the SHA-1 of what's left, the ROM's frame, with 1.0's (`RomVersions.Na10Frame`). If they match, the ROM's version is NA 1.0's, with 1.0's layout, named "NA 1.0 with edited data", with the ROM's own SHA-1 and `DataEdit` set. Anything else is refused, as 0007 says.
- **It imports as 1.0 does.** The records come from the edited tables, so the edits are in the records. The base plugin's manifest records the edit's SHA-1 and version name, and the editor's import page and `aidyn-renewed import` take it.
- **What the original's code keys by position or ID, the port keys the same way for an edit.** The importer checks that a fact it holds names the same thing as 1.0's data: a flying weapon's speed for its position in the weapon DB, Spikes' place, a spell's effects for its ID. Those name checks catch a misread table in 1.0, and apply to 1.0 alone. An edit that puts another weapon at a bow's position gives it that bow's flight, and a renamed spell keeps its effects, as both do on the console. Checks of structure, such as a weapon list that doesn't end or a spell area keyed by the wrong kind, still fail an edit that breaks them.
- **The tests stay the versions' own.** `TestRoms` passes over a data edit, so the real-data tests check 1.0, 1.1 and the debug ROM. The identification is tested on 1.0 with bytes changed inside and outside the tables.

## Consequences

- An edit that moves a table, grows a string table past its range, or changes a Borg, the code, the boot code or any byte the layout doesn't place as a table is refused with its hash, as before. Editing assets or code would need a decision of its own.
- A table added to the layout widens what an edit may change, and changes 1.0's frame: `RomImageTests.Na10sFrameIsTheOneItsDataEditsShare` fails until `Na10Frame` is updated.
- The facts the port holds because 1.0 keeps them in code rather than data, such as the spells' effects and the missiles' speeds, are 1.0's for an edit too, which is what the edit plays with on the console.
- `BasePlugin.ImporterVersion` is unchanged: an import of 1.0 is byte for byte what it was.
- Only one base plugin is installed at a time. Switching between 1.0 and an edit is an import, or a second data folder (`AIDYN_RENEWED_DATA`).
