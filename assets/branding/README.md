# Branding

The user's two images ([ADR 0033](../../docs/adr/0033-the-project-is-aidyn-chronicles-enhanced.md)):

- `ace-icon.png`, ACE on the parchment: the apps' icon.
- `ace-logo-full.png`, the original's title logo with "Enhanced" under it: the full logo, on the boot splash, the title screen and the README.

`make.py` makes the rest from them with Pillow (`pip install pillow`, then `python assets/branding/make.py`), and copies them into the Godot projects. Make them again after changing either image.

| File | Made from | Used for |
| --- | --- | --- |
| `ace-icon.ico` (16–256 px), `ace-icon.icns` (to 1024) | The icon, cropped to what's opaque and centred on a square | The executables and the installer: `tools/package.ps1` copies them beside each project as `icon.ico` and `icon.icns` for the export, and gives the `.ico` to the MSI ([releases](../../docs/systems/releases.md)) |
| `ace-icon-256.png` | The same square at 256 px | Each project's `icon.png`, its `config/icon`, which is the window's icon |
| `ace-splash.png` | The full logo 960 px tall on black, 2048 × 1536 | Each project's `splash.png`, Godot's boot splash (`application/boot_splash`), fitted to the window on black. It's imported as Keep File, so the export carries the PNG as it is, which the boot splash reads, rather than a texture nothing uses |
| `ace-logo-full.png` itself | | The game's `logo.png`, the title screen's logo, imported with mipmaps as it's drawn smaller than it is ([ui](../../docs/systems/ui.md#in-the-port-1)) |

The projects' copies are committed, and `tools/package.ps1` refreshes any that differ before an export.
