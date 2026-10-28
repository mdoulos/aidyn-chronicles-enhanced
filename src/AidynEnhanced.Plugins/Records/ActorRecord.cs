namespace AidynEnhanced.Plugins.Records;

/// <summary>
/// An animated model and its animation state machine (<c>aidyn:actor/0xHHHH</c>, a Borg7). Every
/// state's animation animates <see cref="Scene"/>, whose asset holds them as glTF animations named
/// <c>borg6 &lt;index&gt;</c>.
/// </summary>
/// <param name="RootTransform">
/// The scene transform drawn with its position zeroed. Its motion moves the actor instead.
/// </param>
/// <param name="Head">A head drawn on the actor, which it turns to look at things nearby.</param>
/// <param name="AlwaysAnimated">
/// Whether its body's animation is stepped in combat even while it idles 13 or more from the camera's aim, where
/// <c>Bodies.Process</c> (<c>0x80012784</c>) skips any other's (<c>Body.AlwaysAnimated</c>, <c>0x800CDA50</c>): the bat, wyvern, harpy,
/// dryads, Pandara, gryphon, manticore and scorpion, nine actors. The port steps every body's animation, so nothing reads it yet.
/// </param>
/// <param name="Outfits">
/// The looks its character wears by what it has on, a port addition for a mod's remodelled character; the original's actors have
/// none, and every mesh of their scenes is drawn.
/// </param>
public sealed record ActorRecord(RecordId Id, RecordId Scene, int RootTransform, int InitialState, IReadOnlyList<ActorState> States,
    ActorHead? Head = null, bool AlwaysAnimated = false, IReadOnlyList<ActorOutfit>? Outfits = null);

/// <summary>
/// A look of an actor's: the meshes of its scene drawn while its character wears it. The first outfit whose conditions the character
/// meets is worn, and one with none is met by anyone, a character or not; the meshes the others name are hidden.
/// </summary>
/// <param name="Meshes">The names of the scene's nodes it draws.</param>
/// <param name="Armored">
/// Met only while the character wears body armour that protects (<c>true</c>), which Regular Clothing doesn't, or only while it
/// doesn't (<c>false</c>); null for either.
/// </param>
/// <param name="Items">Met only while the character has one of these equipped; null for any.</param>
public sealed record ActorOutfit(IReadOnlyList<string> Meshes, bool? Armored = null, IReadOnlyList<RecordId>? Items = null);

/// <summary>
/// A head scene drawn at one of the actor's locators. Only Alaron's actor has one: his body's scene
/// has only a placeholder at the neck (<c>Actor::ChangeAppearance</c>).
/// </summary>
/// <param name="Locator">The locator of the actor's scene it sits on, and faces along.</param>
public sealed record ActorHead(RecordId Scene, int Locator);

/// <param name="Branches">Tried in order when the animation ends, or when the request changes.</param>
public sealed record ActorState(RecordId Animation, IReadOnlyList<ActorBranch> Branches);

/// <summary>
/// A way out of a state: taken for <see cref="Request"/> (255 is any) with a chance in 100.
/// </summary>
/// <param name="Interrupts">Rolled for as soon as the request changes, not only when the animation ends.</param>
public sealed record ActorBranch(int Request, int Chance, bool Interrupts, int State);
