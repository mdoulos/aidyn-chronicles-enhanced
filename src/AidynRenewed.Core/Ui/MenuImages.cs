using AidynRenewed.Plugins;
using AidynRenewed.Plugins.Records;

namespace AidynRenewed.Core.Ui;

/// <summary>
/// UI chrome the menus draw with: images the engine's code names, which need no records and which
/// mods replace as assets under the same IDs (see <c>docs/hardcoded-content.md</c>).
/// </summary>
public static class MenuImages
{
    /// <summary>The Party tab's parchment, with the silhouette, frames and labels painted in.</summary>
    public static readonly RecordId PartyTab = Image(0x00D9);

    /// <summary>The System tab's plain parchment, with nothing painted in.</summary>
    public static readonly RecordId SystemTab = Image(0x00D8);

    /// <summary>
    /// The World tab's parchment, with the world map, the compass and the calendar's days painted in. The three tabs'
    /// images are one scroll: their torn edges run on from one to the next.
    /// </summary>
    public static readonly RecordId WorldTab = Image(0x00DA);

    /// <summary>
    /// The World tab's widgets: today's square on the calendar (drawn 17 square), Camp and
    /// Journal; and the journal's book and its arrows, up and down.
    /// </summary>
    public static readonly RecordId Today = Image(0x00DC), CampTitle = Image(0x00DD), JournalTitle = Image(0x00E6),
        Book = Image(0x0182), BookUp = Image(0x00D6), BookDown = Image(0x00D5);

    /// <summary>
    /// The System tab's titles: its list's "Menu", Options, Load Game, Save Game, the
    /// name entry's and the Theatre's "Theater" (<c>Theatre.Build</c>).
    /// </summary>
    public static readonly RecordId MenuTitle = Image(0x00F2), OptionsTitle = Image(0x00FD), LoadTitle = Image(0x00F1),
        SaveTitle = Image(0x00FF), EnterNameTitle = Image(0x00F9), TheatreTitle = Image(0x0100);

    /// <summary>The options' volume bar, a braided rope 152 × 18, and the name entry's key cursor, 15 square.</summary>
    public static readonly RecordId VolumeBar = Image(0x00D7), KeyCursor = Image(0x00F8);

    /// <summary>The parchment C-down's details, barter and training draw on, and its shadow.</summary>
    public static readonly RecordId Parchment = Image(0x00E2), ParchmentShadow = Image(0x00E4);

    /// <summary>The hit points bar, cut to the share of hit points left.</summary>
    public static readonly RecordId HealthBar = Image(0x00E0);

    /// <summary>The lists' titles.</summary>
    public static readonly RecordId InventoryTitle = Image(0x00E9), SkillsTitle = Image(0x00EC), SpellsTitle = Image(0x00ED),
        StatsTitle = Image(0x00EE);

    /// <summary>The barter window's titles: the loot's, a shop's stock and the party's list beside a shop.</summary>
    public static readonly RecordId TreasureTitle = Image(0x00F0), BuyTitle = Image(0x00E7), SellTitle = Image(0x00EB);

    /// <summary>The trainer's list's title.</summary>
    public static readonly RecordId TrainTitle = Image(0x00EF);

    /// <summary>The arrows beside a list's title that show it has rows above or below.</summary>
    public static readonly RecordId ScrollUp = Image(0x38D0), ScrollDown = Image(0x38CF);

    /// <summary>
    /// The red arrows that say there's more either way: under the Party tab's knotwork, both ways in trek and left
    /// only in combat; under the System tab's list, right only; and over a save's name.
    /// </summary>
    public static readonly RecordId SliderBoth = Image(0x3756), SliderLeft = Image(0x3755), SliderRight = Image(0x3757);

    /// <summary>
    /// Each equipment slot's image of what it takes, by <see cref="EquipmentSlot"/>, which shows while
    /// the slot is empty (<c>gItemSlotBorg8Indecies</c>).
    /// </summary>
    public static readonly IReadOnlyList<RecordId> EmptySlots =
        [.. new[] { 0x3601, 0x35F8, 0x35FA, 0x35F9, 0x35F3, 0x35F7, 0x35F4, 0x35F5, 0x35F6, 0x35FF, 0x3600, 0x35FB, 0x35FC, 0x35FD, 0x35FE }
            .Select(Image)];

    /// <summary>Lunar's and Solar's icons (<c>getAspectIcon</c>). An item or spell of no aspect has none.</summary>
    public static RecordId? AspectIcon(Aspect aspect) => aspect switch
    {
        Aspect.Lunar => Image(0x01A3),
        Aspect.Solar => Image(0x01A4),
        _ => null,
    };

    /// <summary>Each school's icon (<c>getSchoolIcon</c>). Unschooled has none.</summary>
    public static RecordId? SchoolIcon(MagicSchool school) => school switch
    {
        MagicSchool.Chaos => Image(0x01A9),
        MagicSchool.Elemental => Image(0x01A5),
        MagicSchool.Naming => Image(0x01A6),
        MagicSchool.Necromancy => Image(0x01A7),
        MagicSchool.Star => Image(0x01A8),
        _ => null,
    };

    private static RecordId Image(int borg8) => RecordId.Hex(RecordTypes.Image, borg8);
}

/// <summary>
/// The silhouette's 15 slots, in the order the original numbers them (<c>DollEquipmentMenu</c>). The
/// helmet's slot reads helmets, per the machine code.
/// </summary>
public enum EquipmentSlot
{
    Weapon,
    Gloves,
    Ring1,
    Helmet,
    Amulet,
    Cloak,
    Armor,
    Belt,
    Boots,
    Shield,
    Wand,
    Ring2,
    Scroll1,
    Scroll2,
    Scroll3,
}
