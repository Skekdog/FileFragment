namespace FileFragment.Core;

public static class Config
{
	private static uint _packetSize = 50;
	public static uint PacketSize
	{
		get => _packetSize;
		set
		{
			if (value <= FirstPacket.PacketOverhead) throw new Exception($"Packet size must be at least {FirstPacket.PacketOverhead + 1} bytes");
			_packetSize = value;
		}
	}
}