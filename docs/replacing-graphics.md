# Replacing the game's graphics

This guide is for anyone who wants to change how Aidyn Chronicles Enhanced looks: repaint the menus, swap a model, or eventually ship a high-resolution texture pack. It explains how graphics mods work, what you can replace today, and what isn't possible yet.

## How it works

Aidyn Chronicles Enhanced never reads your ROM while you play. When you first run it, the importer turns the ROM into a folder of ordinary files called the **base plugin**: PNG images, 3D models in glTF format, sounds and data. The game draws everything from those files.

To change a graphic, you don't edit the base plugin. You make a **mod**: a second folder, next to the base plugin, holding only the files you want to replace. When the game looks for a file, it checks the mods first and falls back to the base plugin for everything they don't provide.

This keeps your work safe and separate:

- Importing the ROM again replaces the base plugin, but leaves your mods alone.
- Switching to another ROM, such as the Action Edition, keeps your mods too.
- Deleting a mod's folder turns it off.

## What you can replace today

| What it is | Where it's drawn | Can you replace it? |
| --- | --- | --- |
| UI images | Menus, the HUD, portraits, the title screen, the sky's gradients | Yes, at the original pixel size |
| Engine sprites | Particles such as fire and smoke, body shadows, the damage numbers in battle | Yes |
| Models | Props, buildings, characters, creatures and the scenery of each area | Yes, with care |
| Textures on models | The walls, ground, faces and clothes painted onto models | Not directly yet |

The last row is the one most people expect to work, and the [section on it](#textures-on-models) explains why it doesn't yet and how to work around it.

## Making a mod, step by step

### 1. Find the graphic's number

Every graphic has an ID made of a type and a number, such as `aidyn:image/0x0017`. The types you'll use are:

- `image`: UI images.
- `texture`: textures, including the engine's sprites.
- `scene`: models.

To find the one you want, open the editor and browse its list of textures, UI images and scenes. Each entry shows its ID, images open in a gallery, and models open in a viewer that draws them as the game does. The editor is `aidyn-enhanced-editor` beside the game in a release. From the source code, run:

```powershell
tools\godot.ps1 --path src\AidynEnhanced.Editor
```

The original files are also in the base plugin's folder, named by number, which is where you'll copy them from:

```
plugins\aidyn.base\assets\aidyn\image\0x0017.png
plugins\aidyn.base\assets\aidyn\texture\0x0003.png
plugins\aidyn.base\assets\aidyn\scene\0x0CF6.glb
```

### 2. Make the mod's folder

The plugins folder is in the game's data folder:

| Install | Plugins folder |
| --- | --- |
| Windows installer, or running from source | `%LOCALAPPDATA%\AidynEnhanced\plugins` |
| Windows portable zip | `data\plugins`, beside the game |
| Linux | `~/.local/share/AidynEnhanced/plugins` |
| macOS | `~/Library/Application Support/AidynEnhanced/plugins` |

If the `AIDYN_ENHANCED_DATA` environment variable is set, the data folder is wherever it points.

Make a folder there for your mod, and put a file named `plugin.json` in it:

```json
{ "id": "hd_ui", "name": "HD UI", "version": "1.0.0" }
```

Two rules catch people out:

- **The ID uses only lowercase letters, digits and underscores.** `hd_ui` works, and `hd-ui` or `HD UI` don't.
- **The folder's name must be exactly the ID.** This mod lives in `plugins\hd_ui`.

The name is what people see, and can be anything.

### 3. Add your files

Copy each original you want to change from the base plugin into the same place inside your mod's `assets` folder, then edit your copy. The `aidyn` folder in the path means "the base game's", because these files replace the base game's graphics.

```
plugins\
  aidyn.base\                         the imported game: don't edit it
  hd_ui\
    plugin.json
    assets\aidyn\image\0x0017.png     replaces UI image 0x0017
    assets\aidyn\texture\0x0003.png   replaces the body shadow
    assets\aidyn\scene\0x0CF6.glb     replaces model 0x0CF6
```

The file name, folder and extension have to match the original exactly, or the game won't find your file and draws the original instead.

### 4. Start the game

Start the game, or restart it if it's running. It loads the base plugin first and every mod after it. If two mods replace the same file, the one loaded last wins. Mods load in alphabetical order of their IDs unless one says otherwise: adding `"loadAfter": ["other_mod"]` to a mod's `plugin.json` makes it load after `other_mod`, so its files win.

If a mod is broken, for example its ID has a hyphen or its folder is misnamed, the game shows the problem on screen instead of starting. Fix it or move the folder out, and start again.

## Images: menus, the HUD and sprites

UI images and sprites are plain PNG files, and any PNG an image editor saves will load.

- **Keep the original's size in pixels.** The game lays out its menus and HUD by each image's size, so an image twice as wide is drawn twice as wide, not sharper. A repaint at the same size works perfectly. Higher-resolution UI needs a change to the game, described [below](#whats-planned).
- **The sky's gradients are data, not pictures.** Each is 12 columns by 37 rows. A column is one time of day, every two hours from midnight, and the game blends between neighbouring columns. The top 32 rows are the sky from the top of the screen down. The last 5 set the tint on the scenery and the colours of the fog, clouds and sun, so repainting them recolours the whole area, not just the sky. A replacement must be exactly 12 by 37, or the game ignores it.
- **Transparency comes from the alpha channel.** Keep the transparent areas transparent, or a portrait or icon gets a box around it.
- **Colours are drawn as you paint them.** The game shows the image's colours as they are, the way the N64 did, before any lighting or tint the game applies.
- **The title's logo isn't one of them.** The title screen shows Aidyn Chronicles Enhanced's own logo, which is part of the game rather than an image in a plugin, so replacing the original's logo, image `0x0103`, or its shadow, `0x0104`, changes nothing. PRESS START and the title's menu are images as usual.

## Models

Models are glTF binary files (`.glb`), which Blender and most 3D tools open. You can change a model's shape, its UV mapping and the textures inside it. The textures can be any size: texture coordinates are stored as fractions of the image, so a texture twice the size lines up exactly where the original did.

The importer also stores things in each model that the game depends on, and a replacement has to keep them:

- **The N64 settings on each material and on the model,** such as how a texture wraps, which textures scroll like water, and where particles come from. glTF calls these *extras*. Blender imports them as custom properties, and keeps them only if you tick **Custom Properties** under **Include** when you export.
- **The names of the model's parts.** The game finds each part by its node's name: `transform 0`, `transform 1` and so on for the parts themselves, `locator 0` and up for the points where heads, held weapons and particle emitters go, and `camera` and `light 0` and up for the model's camera and lights. Don't rename or delete them.
- **For characters and creatures, the animations and the parts they move.** The animations are inside the model file, named `borg6` and a number, and each moves the parts by their names. Keep the animations and the parts' names, and reshape the mesh around them. Props and scenery without animations are much more forgiving.

Collision is stored separately from the visible model. Reshaping a piece of scenery changes how it looks, but you still walk on the original's ground and bump into the original's walls. Keep the shape close where the player can reach it.

A round trip through Blender hasn't been tested yet. After replacing a model, open it in the editor's model viewer, which draws with the game's own renderer, and check it in the game.

### A skinned character

A character can also be replaced by a modern one: a single mesh that bends around a skeleton, in place of the original's rigid parts. The game needs no change for this. It loads the file through Godot's glTF importer, which skins the mesh, and it plays the animations by their names as it does any model's. A remodelled Alaron of 1,000 triangles and 23 bones has been tested this way: he walks and runs in trek and holds his sword and shield in battle.

The file must keep these from the original:

- **The root part, `transform 2`, at the top, with the original's channels in every animation.** The game moves the character by this part's animation and draws it with its position held, so copy its translation, rotation and scale channels unchanged and put the skeleton under it.
- **The animations, named `borg6` and a number, with the original's extras** (`borg6` and `length`), sampled 60 times a second as the original's are. Each must move the skeleton's bones as well as the root.
- **The locators** `locator 0` and up, each with its `locator N attachment` child, hung from the bones that stand for the original's parts: a held weapon, a shield, the head and spell effects are placed on them. Turn each attachment so it ends up facing as the original's does, or the weapon is held at the wrong angle.
- **The model's extras and its light**, copied from the original.

A character whose head is part of the new mesh still has its original head hung from locator 3, because the head is a separate model. Replace the head's model too, with one that keeps only its root node and has no geometry.

The animations can be new ones rather than the original's, as long as each keeps the original's length in ticks, since the game times its states by them. The game moves the character by the root's translation alone, so new animations can hold the root's rotation and scale at rest and carry the body's motion in the bones. A walk's or run's two steps are each half a loop, whose stride should carry the planted foot back as fast as the game moves the character (Alaron walks at 1.43 units a second, runs at 4.26 and sneaks at 1.24), or the feet slide; and an attack should reach its swing's fastest moment at the share of its length the game strikes at, 0.42 for Alaron and 0.4 for a bow shot or a throw ([combat](systems/combat.md)). A death holds its last frame. A mod can give its character the port's full run (request 26, on Shift) by patching the actor with states for it: a patch is `{"patch": "aidyn:actor/0x25F9", "states": [...]}`, and an actor's `states` are replaced whole, so the patch lists all of them, the original's 45 with their branches to the new states and the new ones after. Their animations are new ones in the model file, with IDs in the mod's own namespace (`alaron_remodel:animation/0x0001` is played as `borg6 1`), and their root's translation sets the run's speed. Alaron's remodel runs in full 20% further than the original's run goes, 5.11 units a second, in the same steps, so it outpaces the base game's run; plain movement is his lighter jog. A skinned character blends from one animation into the next under the Enhancements page's **Animation Blending**, so the joins between states needn't match exactly. The root's translation is also how fast the game moves the character, so a mod can make a gait slower or faster by scaling it: Alaron's remodel jogs at 89% of the original's run speed, 3.78 units a second, a little faster than its lighter stride covers, the user's choice. Alaron's remodel plays clips from Quaternius's CC0 Universal Animation Libraries fitted this way.

The skeleton can have bones the original has no part for. Alaron's remodel has three in each hand: two for the fingers, at the knuckles and at their middle, and one for the thumb, from its root by the wrist, with both outfits weighted to them, so that an animation can close his hands. The thumb's bone carries whatever stands forward of the palm between the wrist and the fingertips, which on the armour's gloves includes the point by the wrist. His jog closes his hands, as a casual jog's, the user's choice: his elbows bend about 90° and his upper arms swing through about 30°, where the clips bent them 50 to 65° and swung them through 60°, his fingers fold into fists, and his thumbs lie in across them rather than standing up. At about 1,000 triangles a thumb turned down toward the fingers crumples, so it turns in toward the palm. His full run closes his hands the same way and keeps the clips' arms; his walk keeps the clips' arms and open hands. The hands' bones are keyed in every animation, at rest wherever the hands are open, and they blend between animations as the other bones do, so his hands close as he starts to jog or run and open as he stops.

A character can have several **outfits**, each worn by what the character has on, rather than a model for every item. Put each outfit's mesh in the same file, all skinned to the one skeleton in the same rest pose so they share the animations, with a node name of its own, and list them in the actor's patch, the first whose conditions are met being worn:

```json
{"patch": "aidyn:actor/0x25F9", "outfits": [
  {"meshes": ["outfit armour", "outfit armour head"], "armored": true},
  {"meshes": ["outfit plain"]}]}
```

`"armored": true` is met while the character wears body armour that protects, which Regular Clothing doesn't (`false` is met while it doesn't), and `"items": ["aidyn:armor/0x0503"]` while one of the items is equipped. An outfit with neither is worn by anyone, and by a body that's no character's. The meshes the other outfits name are hidden. Alaron's remodel has two: plain clothes, and the armour, which is worn over any armour that protects. Both wear the same head: the armoured outfit's head is a copy of the plain one's faces above the collar, with the same weights and texture, so the face and hair never change between them. Its body was built from its own image-to-3D model, posed to the plain one's A-pose before skinning, its limbs turned to point as the plain one's do and its hands twisted so their palms face the same way, or the outfits would hold their hands differently. At about 1,000 triangles a pauldron or a flared sleeve standing off the body breaks up as the arm turns, the arm under it showing through, so the remodel fuses each to the arm under it and lets its texture carry the shape.

Animate the bones by rotation and, for the hips and the root, translation. A bone animated by scale hasn't been tried, and the game handles a part that's both turned and scaled specially.

The game draws the material's base colour texture with its N64 lighting. A normal map carries the detail the triangles leave out: with the Enhancements page's **Surface Detail** on Full, the default, the model is lit at each pixel through it, and on Original at each vertex as the game's own models are. Bake the normal map in the mesh's tangent space with +Y up the image, as Blender does, and include the mesh's tangents in the file. A metal-roughness map is ignored.

## Textures on models

The textures in the `texture` folder are only used for sprites the game draws itself: particles, shadows and the damage numbers. The textures painted on models aren't read from there. Each model file carries its own copy of every texture it uses, so putting a new `texture\0x0EAB.png` in a mod changes nothing on the models that use that texture.

Many textures belong to a single model, and those you can retexture today by replacing that one model, with the new texture inside it. Others are shared by many models: the most common is used by 85. Retexturing a shared texture today means replacing every model that uses it.

To find the models that use a texture, run this Python script with the texture's number. Each material in a model records the number of its texture, which is what it looks for:

```python
# Lists the models that use a texture, by its number: python models_using_texture.py 0x0EAB
import glob, json, os, struct, sys

texture = int(sys.argv[1], 16)
folder = os.path.expandvars(r"%LOCALAPPDATA%\AidynEnhanced\plugins\aidyn.base\assets\aidyn\scene")
for path in sorted(glob.glob(os.path.join(folder, "*.glb"))):
    with open(path, "rb") as f:
        data = f.read()
    length = struct.unpack("<I", data[12:16])[0]
    gltf = json.loads(data[20:20 + length])
    if any((m.get("extras") or {}).get("borg1") == texture for m in gltf.get("materials", [])):
        print(os.path.basename(path))
```

On Linux or macOS, change `folder` to your plugins folder.

## What's planned

Replacing assets is part of the mod tools in the [roadmap](roadmap.md)'s Phase 5, and two changes to the game would finish it for graphics:

1. **Textures on models from a mod's PNG.** When the game builds a model's materials, it would look for a mod's `texture` file with that texture's number and use it instead of the copy inside the model. Every material already records its texture's number, so this is a small change. A texture pack would then be a folder of PNGs at any resolution, with no models to touch.
2. **Higher-resolution UI images.** The game would lay out each image at the original's size and draw a larger replacement scaled into it, so a sharper menu stays the same size on screen.

The editor will eventually make and preview mods for you. For now, mods are made by hand as this guide describes.
