using AidynEnhanced.Core.Characters;
using AidynEnhanced.Plugins.Records;

namespace AidynEnhanced.Core.Actors;

/// <summary>
/// Which of an actor's outfits its character wears (<see cref="ActorRecord.Outfits"/>), a port addition: a mod's remodelled
/// character changes its whole look by what it has on, where the original draws one model whatever is worn.
/// </summary>
public static class ActorOutfits
{
    /// <summary>
    /// The first of the actor's outfits <paramref name="wearer"/> meets, or null if it has none or meets none. A body that's no
    /// character's meets only an outfit without conditions.
    /// </summary>
    public static ActorOutfit? Worn(ActorRecord actor, Character? wearer) =>
        actor.Outfits?.FirstOrDefault(outfit => Meets(outfit, wearer));

    /// <summary>Whether the character meets the outfit's conditions.</summary>
    public static bool Meets(ActorOutfit outfit, Character? wearer)
    {
        if (outfit.Armored is { } armored && Armored(wearer) != armored)
            return false;
        return outfit.Items is not { } items || wearer is not null && wearer.Equipment.All.Any(i => items.Contains(i.Id));
    }

    /// <summary>Whether the character wears body armour that protects: Regular Clothing, the armour slot's 0, doesn't.</summary>
    public static bool Armored(Character? wearer) => wearer?.Equipment.Armor is { Item.Protection: > 0 };
}
