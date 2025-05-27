using FileFragment.Core;

namespace FileFragment.Tests;

public class PacketTests
{
	[Fact]
	public void Constructor_SetsIdVersionAndDataCorrectly()
	{
		byte[] data = [0, 1, 2, 255, 67];
		Packet packet = new(2, data);

		Assert.Equal(2, packet.Id);
		Assert.Equal(Global.Version, packet.Version);
		Assert.Equal(data, packet.Data);
	}

	[Fact]
	public void PacketOverhead_IsCorrect()
	{
		int expected = Global.MagicSequence.Length + sizeof(byte) + sizeof(ushort) + Global.PacketChecksumLength;
		Assert.Equal(expected, Packet.PacketOverhead);
	}

	[Fact]
	public void ToString_ReturnsExpectedString()
	{
		byte[] data = [0, 1, 2, 255, 67];
		Packet packet = new(2, data);
		string expected = $"Version: {Global.Version}, Id: 2, Data: 5 bytes";
		Assert.Equal(expected, packet.ToString());
	}

	[Fact]
	public void PacketChecksum_IsConsistent()
	{
		byte[] data = [0, 1, 2, 255, 67];
		Packet packet1 = new(2, data);
		Packet packet2 = new(2, data);
		Assert.Equal(packet1.PacketChecksum, packet2.PacketChecksum);
	}

	[Fact]
	public void PacketChecksum_MatchesKnownChecksum()
	{
		byte[] data = [0, 1, 2, 255, 67];
		Packet packet = new(1, data);

		Assert.Equal("7E 5C D3 52", HexString.ByteArrayToHexString(packet.PacketChecksum));
	}

	[Fact]
	public void Data_CanBeFragmentedAndReassembled()
	{
		byte[] data = [.. Enumerable.Range(0, 256).Select(i => (byte)i)];

		List<Packet> packets = SourceParser.ParseSource(new MemoryStream(data));
		byte[] reconstructed = ReconstructFile.Reconstruct(packets).ToArray();

		Assert.Equal(data, reconstructed);
	}
}