# 0032. Aidyn ReChronicled is read for its findings and designs, with credit

Status: Accepted · Oct 27, 2026

## Context

[Aidyn ReChronicled](https://github.com/spitzdano/AidynReChronicled), by Spitzdano, is a static recompilation of NA 1.0 into a native Linux and Windows game, made with N64Recomp and N64ModernRuntime and drawn by the RT64 renderer. It adds widescreen, higher resolutions, 60 fps, longer draw distances, quicksaves, combat aids, louder sound, newly painted art and support for the Aidyn Patch and Aidyn Plus ROM hacks. Its code is GPL-3.0. Its README says nearly all of its code, tools and docs were written by Claude Code under the author's direction, and its art was made with an AI image generator and upscaler from the game's own pictures. The painted art, the HD texture packs and the restored instrument samples are in private repositories and ship only in its releases.

On Oct 27, 2026, the project's owner asked Spitzdano on Reddit whether he minded the owner cloning the repository and exploring it with Claude, "to see what's possible with the decomp and work done by you and others so far". Spitzdano answered "go for it". The owner reports permission to use any of it.

Much of its work is the plumbing a recompilation needs to run the original's code on a PC: memory, display lists, frame interpolation, the original's own queues and counters. The port doesn't run that code, so this doesn't apply. What does apply:

- **Fixes to the original's own bugs and to its frame-rate assumptions.**
- **How it solved rendering problems the port shares,** such as pop-in and the seams between meshes.
- **Its sound processing and its designs for options and play aids.**
- **An art and sound pipeline keyed as the port keys the same assets:**
  - world textures by Borg1 index;
  - portraits and icons by the names the ROM gives its Borg8 images;
  - instrument samples by a hash of the Borg11 sample's own data.

It also bundles the third-party decompilation that [0015](0015-the-port-no-longer-reads-the-decomp.md) stopped this project reading, as a submodule (`decomp/`), and its build takes that decompilation's names into its own symbol files. 0015's reason was that decompilation's author's view of projects written by LLMs. That view isn't Spitzdano's to waive.

## Decision

- **The clone lives beside the project, without its submodules,** in `../AidynReChronicled`.
  - `decomp/` is never initialised or read. Neither are the files that carry the decompilation's names: `split/symbol_addrs.txt`, `split/reloc_addrs.txt`, `tools/symbol_*.txt` and `tools/prepare_symbols.py`.
  - A subagent sent into it is told the same, and to describe the original's code by what it does and its addresses, not by the names its patches use.
- **The project takes its findings, designs and approaches, rebuilt in its own code.**
  - A fact about the original's code that ReChronicled gives is confirmed in 1.0's machine code before the port relies on it, and cited by address and the symbol map's names, as 0015 requires of every fact.
  - The system's doc may say ReChronicled found it first, naming the commit.
  - A fix it makes to the original's behaviour still follows [0018](0018-the-port-fixes-bugs-and-sets-enhancements.md): the port takes it as the user's choice, a setting where it changes what the original does.
- **Its code, art, HD packs and samples wait for a clearer yes.**
  - The permission shown above covers cloning and studying the repository. Copying its code into this MIT project, or using its art, packs or restored samples in a mod, waits for Spitzdano's explicit agreement to that use.
  - That agreement is recorded here when given.
  - Art and sound made from the game's data are never committed, whoever made them, as for anything derived from the ROM.
- **It is credited.** The README's credits name Aidyn ReChronicled and Spitzdano for what the project takes from it.

## Consequences

- The work this opens is drafted as a plan, which becomes roadmap tasks as the owner picks them:
  - quick fixes and settings;
  - the painted art, through textures on models from a mod's PNG and higher-resolution UI images, which [replacing graphics](../replacing-graphics.md) plans already;
  - quicksaves and combat aids;
  - rendering checks.
- The painted art and restored samples need two things before any of that work uses them:
  - Spitzdano's agreement to use them;
  - a copy of them, from the owner of the private repositories or from a release.
- Research that starts from a ReChronicled finding costs one extra step, the check against the machine code, and gains the finding's ROM and RAM addresses as a head start.
