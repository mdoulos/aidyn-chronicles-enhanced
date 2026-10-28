using AidynRenewed.Core.Actors;
using AidynRenewed.Plugins;
using AidynRenewed.Plugins.Records;

namespace AidynRenewed.Core.Tests;

/// <summary>Which of an actor's outfits its character wears, by what it has on: a port addition for a mod's remodelled character.</summary>
public sealed class ActorOutfitsTests
{
    private static readonly ActorOutfit Robed = new(["robe"], Items: [ItemData.ArmorId(3)]);
    private static readonly ActorOutfit Armoured = new(["armour", "armour head"], Armored: true);
    private static readonly ActorOutfit Plain = new(["plain"]);

    private static readonly ActorRecord Actor = new(RecordId.Hex(RecordTypes.Actor, 0x25F9), RecordId.Hex(RecordTypes.Scene, 0x25CB), 2, 0, [],
        Outfits: [Robed, Armoured, Plain]);

    [Fact]
    public void TheFirstOutfitTheCharacterMeetsIsWorn()
    {
        var alaron = ItemData.Alaron();
        Assert.Same(Plain, ActorOutfits.Worn(Actor, alaron));

        alaron.Equipment.EquipArmor(ItemData.Inst(ItemData.NewArmor(1, protection: 2)));
        Assert.Same(Armoured, ActorOutfits.Worn(Actor, alaron));

        // Robed comes first, so wearing its item wins over the armour.
        alaron.Equipment.EquipArmor(ItemData.Inst(ItemData.NewArmor(3, protection: 4)));
        Assert.Same(Robed, ActorOutfits.Worn(Actor, alaron));
    }

    [Fact]
    public void ClothingThatDoesntProtectIsNoArmour()
    {
        // Regular Clothing is body armour of protection 0, which Alaron starts in.
        var alaron = ItemData.Alaron();
        alaron.Equipment.EquipArmor(ItemData.Inst(ItemData.NewArmor(0)));
        Assert.False(ActorOutfits.Armored(alaron));
        Assert.Same(Plain, ActorOutfits.Worn(Actor, alaron));

        // A shield protects, but isn't body armour.
        alaron.Equipment.EquipArmor(ItemData.Inst(ItemData.NewArmor(0)));
        alaron.Equipment.EquipShield(ItemData.Inst(ItemData.NewArmor(2, ArmorSlot.Shield, protection: 3, defense: 2)));
        Assert.Same(Plain, ActorOutfits.Worn(Actor, alaron));
    }

    [Fact]
    public void ABodyThatsNoCharactersWearsOnlyAnOutfitWithoutConditions()
    {
        Assert.Same(Plain, ActorOutfits.Worn(Actor, null));
        Assert.Null(ActorOutfits.Worn(Actor with { Outfits = [Robed, Armoured] }, null));
        Assert.Null(ActorOutfits.Worn(Actor with { Outfits = null }, ItemData.Alaron()));
        // Wearing no armour can be a condition too.
        Assert.Same(Plain, ActorOutfits.Worn(Actor with { Outfits = [new ActorOutfit(["x"], Armored: true), Plain] }, null));
        var unarmoured = new ActorOutfit(["bare"], Armored: false);
        Assert.Same(unarmoured, ActorOutfits.Worn(Actor with { Outfits = [unarmoured, Plain] }, null));
    }
}
