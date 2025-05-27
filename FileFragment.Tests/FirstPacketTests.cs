using FileFragment.Core;

namespace FileFragment.Tests;

public class FirstPacketTests
{
	[Fact]
	public void Constructor_SetsIdVersionAndDataCorrectly()
	{
		byte[] data = [0, 1, 2, 255, 67];
		FirstPacket packet = new(data, 0, []);

		Assert.Equal(0, packet.Id);
		Assert.Equal(Global.Version, packet.Version);
		Assert.Equal(data, packet.Data);
	}

	[Fact]
	public void ToString_ReturnsExpectedString()
	{
		byte[] data = [0, 1, 2, 255, 67];
		FirstPacket packet = new(data, 0, []);

		string expected = $"Version: {Global.Version}, Id: 0, Data: 5 bytes, PacketCount: 0, FileChecksum: 0 bytes";
		Assert.Equal(expected, packet.ToString());
	}

	[Fact]
	public void PacketOverhead_IsCorrect()
	{
		int expected = Packet.PacketOverhead + sizeof(ushort) + Global.FileChecksumLength;
		Assert.Equal(expected, FirstPacket.PacketOverhead);
	}
}