using AidynRenewed.Core.Actors;
using AidynRenewed.Core.Characters;
using AidynRenewed.Plugins.Records;
using Godot;

namespace AidynRenewed.Views;

/// <summary>
/// Draws the outfit an actor's character wears (<see cref="ActorOutfits"/>): of the scene's nodes its outfits name, those of the
/// one worn are shown and the rest hidden. It looks again each frame, so a change of what's worn shows at once. A port addition.
/// </summary>
public partial class OutfitSwitch : Node
{
    private readonly ActorRecord _actor;
    private readonly (string Name, Node3D Node)[] _named;
    private ActorOutfit? _shown;
    private bool _dressed;

    public OutfitSwitch()
    {
        _actor = null!;
        _named = [];
    }

    private OutfitSwitch(ActorRecord actor, Node3D scene)
    {
        _actor = actor;
        var names = actor.Outfits!.SelectMany(o => o.Meshes).ToHashSet();
        _named = [.. Nodes.FindAll<Node3D>(scene).Where(n => names.Contains(n.Name)).Select(n => (n.Name.ToString(), n))];
        Name = nameof(OutfitSwitch);
        Dress();
    }

    /// <summary>The character whose outfit it draws, or null for a body that's no character's.</summary>
    public Character? Wearer
    {
        get;
        set
        {
            field = value;
            Dress();
        }
    }

    /// <summary>The switch for an actor's scene, added under it, or null for an actor without outfits.</summary>
    public static OutfitSwitch? Add(ActorRecord actor, Node3D scene)
    {
        if (actor.Outfits is not { Count: > 0 })
            return null;
        var outfits = new OutfitSwitch(actor, scene);
        scene.AddChild(outfits);
        return outfits;
    }

    /// <summary>The switch under an actor's scene, or null if its actor has no outfits.</summary>
    public static OutfitSwitch? Of(Node3D scene) => scene.GetNodeOrNull<OutfitSwitch>(nameof(OutfitSwitch));

    public override void _Process(double delta) => Dress();

    private void Dress()
    {
        var worn = ActorOutfits.Worn(_actor, Wearer);
        if (_dressed && worn == _shown)
            return;
        _shown = worn;
        _dressed = true;
        // Wearing none of them draws every mesh, as an actor without outfits does.
        foreach (var (name, node) in _named)
            node.Visible = worn is null || worn.Meshes.Contains(name);
    }
}
