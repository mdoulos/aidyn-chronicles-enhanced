using System.Text;

namespace AidynRenewed.Rom.Tests;

public class RomImageTests
{
    /// <summary>A minimal image that passes byte-order detection but matches no known version.</summary>
    private static byte[] FakeRom()
    {
        var rom = new byte[0x1000];
        rom[0] = 0x80; rom[1] = 0x37; rom[2] = 0x12; rom[3] = 0x40;
        rom[0x08] = 0x80; rom[0x09] = 0x00; rom[0x0A] = 0x04; rom[0x0B] = 0x00;
        Encoding.ASCII.GetBytes("AIDYN CHRONICLES    ").CopyTo(rom, 0x20);
        Encoding.ASCII.GetBytes("NAYE").CopyTo(rom, 0x3B);
        rom[0x3F] = 1;
        return rom;
    }

    [Theory]
    [InlineData(new byte[] { 0x80, 0x37, 0x12, 0x40 }, RomByteOrder.BigEndian)]
    [InlineData(new byte[] { 0x37, 0x80, 0x40, 0x12 }, RomByteOrder.ByteSwapped)]
    [InlineData(new byte[] { 0x40, 0x12, 0x37, 0x80 }, RomByteOrder.LittleEndian)]
    public void DetectsByteOrderFromFirstWord(byte[] firstWord, RomByteOrder expected) =>
        Assert.Equal(expected, RomByteOrders.Detect(firstWord));

    [Fact]
    public void RejectsNonRom() =>
        Assert.Throws<InvalidDataException>(() => RomByteOrders.Detect([0x50, 0x4B, 0x03, 0x04]));

    [Theory]
    [InlineData(RomByteOrder.ByteSwapped, new byte[] { 2, 1, 4, 3, 6, 5, 8, 7 })]
    [InlineData(RomByteOrder.LittleEndian, new byte[] { 4, 3, 2, 1, 8, 7, 6, 5 })]
    public void SwapsAndSwapsBack(RomByteOrder order, byte[] swapped)
    {
        byte[] data = [1, 2, 3, 4, 5, 6, 7, 8];
        RomByteOrders.Swap(data, order);
        Assert.Equal(swapped, data);
        RomByteOrders.Swap(data, order);
        Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8], data);
    }

    [Fact]
    public void ParsesHeader()
    {
        var header = RomHeader.Parse(FakeRom());
        Assert.Equal("AIDYN CHRONICLES", header.Name);
        Assert.Equal("NAYE", header.GameCode);
        Assert.Equal(1, header.Revision);
        Assert.Equal(0x80000400u, header.EntryPoint);
    }

    [Theory]
    [InlineData(RomByteOrder.BigEndian)]
    [InlineData(RomByteOrder.ByteSwapped)]
    [InlineData(RomByteOrder.LittleEndian)]
    public void RefusesUnknownRomWithItsHash(RomByteOrder order)
    {
        var rom = FakeRom();
        RomByteOrders.Swap(rom, order);
        var e = Assert.Throws<UnsupportedRomException>(() => RomImage.FromBytes(rom));
        Assert.Matches("^[0-9a-f]{40}$", e.Sha1);
        Assert.Contains(e.Sha1, e.Message);
        Assert.Equal("NAYE", e.Header.GameCode);
    }

    [Theory]
    [InlineData(RomVersionId.Na10, "NAYE", 0)]
    [InlineData(RomVersionId.Na11, "NAYE", 1)]
    [InlineData(RomVersionId.Debug, "", 0)]
    public void IdentifiesKnownRoms(RomVersionId id, string gameCode, byte revision)
    {
        var rom = TestRoms.Get(id);
        Assert.Equal(id, rom.Version.Id);
        Assert.Equal(gameCode, rom.Header.GameCode);
        Assert.Equal(revision, rom.Header.Revision);
        Assert.Equal(id == RomVersionId.Debug, rom.Version.ResearchOnly);
    }

    [Theory]
    [InlineData(RomByteOrder.ByteSwapped)]
    [InlineData(RomByteOrder.LittleEndian)]
    public void IdentifiesSwappedDumps(RomByteOrder order)
    {
        var bytes = TestRoms.Get(RomVersionId.Na10).Data.ToArray();
        RomByteOrders.Swap(bytes, order);

        var rom = RomImage.FromBytes(bytes);
        Assert.Equal(RomVersionId.Na10, rom.Version.Id);
        Assert.Equal(order, rom.OriginalByteOrder);
    }

    [Fact]
    public void RefusesModifiedRom()
    {
        var bytes = TestRoms.Get(RomVersionId.Na10).Data.ToArray();
        bytes[0x1000000] ^= 0xFF;

        var e = Assert.Throws<UnsupportedRomException>(() => RomImage.FromBytes(bytes));
        Assert.NotEqual(RomVersions.Na10.Sha1, e.Sha1);
    }

    [Fact]
    public void Na10sFrameIsTheOneItsDataEditsShare()
    {
        var na10 = TestRoms.Get(RomVersionId.Na10);
        Assert.Equal(RomVersions.Na10Frame, RomVersions.FrameSha1(na10.Data, na10.Layout));
        Assert.False(na10.Version.DataEdit);
        Assert.Null(RomVersions.FindDataEdit(na10.Data, na10.Sha1));
    }

    /// <summary>
    /// A ROM hack that edits only tables NA 1.0's layout places, and makes the header's checksums again, is NA 1.0 under its own
    /// name and hash, with 1.0's layout (ADR 0031): here an entity's record, a weapon's and the potions' prices in the code segment.
    /// </summary>
    [Fact]
    public void IdentifiesADataEditOfNa10AsNa10()
    {
        var na10 = TestRoms.Get(RomVersionId.Na10);
        var bytes = na10.Data.ToArray();
        bytes[na10.Layout.DbTables[DbTable.Entities].Offset + 0x30] ^= 0xFF;
        bytes[na10.Layout.DbTables[DbTable.Weapons].Offset + 0x20] ^= 0x01;
        bytes[na10.Layout.CodeTables[CodeTable.PotionPrices].Offset + 1] ^= 0x04;
        bytes[0x10] ^= 0xFF;

        var rom = RomImage.FromBytes(bytes);
        Assert.Equal(RomVersionId.Na10, rom.Version.Id);
        Assert.True(rom.Version.DataEdit);
        Assert.False(rom.Version.ResearchOnly);
        Assert.Equal("NA 1.0 with edited data", rom.Version.Name);
        Assert.NotEqual(RomVersions.Na10.Sha1, rom.Sha1);
        Assert.Equal(rom.Sha1, rom.Version.Sha1);
        Assert.Same(RomVersions.Na10.Layout, rom.Layout);
    }

    /// <summary>A change to any byte the layout doesn't place as a table is no data edit: the code, the Borg archive, its index, the boot code.</summary>
    [Theory]
    [InlineData(0x1000)]
    [InlineData(0x40)]
    [InlineData(0x00E7220)]
    [InlineData(0x1F89C14)]
    public void RefusesAnEditOutsideTheTables(int offset)
    {
        var na10 = TestRoms.Get(RomVersionId.Na10);
        Assert.DoesNotContain(na10.Layout.CodeTables.Values.Concat(na10.Layout.DbTables.Values).Concat(na10.Layout.StringTables.Values),
            range => range.Offset <= offset && offset < range.Offset + range.Length);
        var bytes = na10.Data.ToArray();
        bytes[na10.Layout.DbTables[DbTable.Entities].Offset] ^= 0xFF;
        bytes[offset] ^= 0xFF;

        Assert.Throws<UnsupportedRomException>(() => RomImage.FromBytes(bytes));
    }
}
