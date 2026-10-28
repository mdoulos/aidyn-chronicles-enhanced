"""Makes the icons and the boot splash from ace-icon.png and ace-logo-full.png, with Pillow (README.md).

Run from anywhere: python assets/branding/make.py. It writes beside itself and copies into the Godot projects.
"""
import shutil
from pathlib import Path

from PIL import Image

here = Path(__file__).resolve().parent
projects = here.parent.parent / "src"


def square(path):
    """The image cropped to what's opaque, centred on a transparent square."""
    im = Image.open(path).convert("RGBA")
    im = im.crop(im.getchannel("A").getbbox())
    side = max(im.size)
    out = Image.new("RGBA", (side, side))
    out.paste(im, ((side - im.width) // 2, (side - im.height) // 2))
    return out


icon = square(here / "ace-icon.png").resize((1024, 1024), Image.LANCZOS)
icon.save(here / "ace-icon.ico", sizes=[(16, 16), (24, 24), (32, 32), (48, 48), (64, 64), (128, 128), (256, 256)])
icon.save(here / "ace-icon.icns")
icon.resize((256, 256), Image.LANCZOS).save(here / "ace-icon-256.png")

# The boot splash: the full logo 960 tall on a black 4:3, which Godot's Keep stretch fits to the window.
logo = Image.open(here / "ace-logo-full.png").convert("RGBA")
logo = logo.resize((round(logo.width * 960 / logo.height), 960), Image.LANCZOS)
splash = Image.new("RGBA", (2048, 1536), (0, 0, 0, 255))
splash.alpha_composite(logo, ((splash.width - logo.width) // 2, (splash.height - logo.height) // 2))
splash.convert("RGB").save(here / "ace-splash.png", optimize=True)

# The Godot projects' copies, which tools/package.ps1 also refreshes: each one's icon and splash, and the game's title logo.
for project in ("AidynEnhanced.Game", "AidynEnhanced.Editor"):
    shutil.copyfile(here / "ace-icon-256.png", projects / project / "icon.png")
    shutil.copyfile(here / "ace-splash.png", projects / project / "splash.png")
shutil.copyfile(here / "ace-logo-full.png", projects / "AidynEnhanced.Game" / "logo.png")
