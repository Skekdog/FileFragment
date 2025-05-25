using System.IO.Hashing;

namespace FileFragmentV2.Source;

public class Packet(ushort id, byte[] data)
{
	public byte Version = Global.Version;
	public ushort Id = id;
	public byte[] Data = data;
	public virtual byte[] PacketChecksum
	{
		get => CalculateChecksum();
	}

	public static readonly int PacketOverhead = Global.MagicSequence.Length + sizeof(byte) + sizeof(ushort) + Global.PacketChecksumLength;

	public override string ToString()
	{
		return $"Version: {Version}, Id: {Id}, Data: {Data.Length} bytes";
	}

	public virtual byte[] GetBytes()
	{
		MemoryStream stream = new();
		using BinaryWriter writer = new(stream);
		writer.Write(Global.MagicSequence);
		writer.Write(Version);
		writer.Write(Id);
		writer.Write(Data);

		writer.Write(CalculateChecksum(stream));

		return stream.ToArray();
	}

	private byte[] CalculateChecksum(MemoryStream? stream = null)
	{
		if (stream is null)
		{
			stream = new MemoryStream();
			using BinaryWriter writer = new(stream);
			writer.Write(Global.MagicSequence);
			writer.Write(Version);
			writer.Write(Id);
			writer.Write(Data);
		}
		return Crc32.Hash(stream.ToArray());
	}
}

public class FirstPacket(byte[] data, ushort packetCount, byte[] fileChecksum) : Packet(0, data)
{
	public ushort PacketCount = packetCount;
	public byte[] FileChecksum = fileChecksum;

	public override byte[] PacketChecksum
	{
		get => CalculateChecksum();
	}

	public static new readonly int PacketOverhead = Packet.PacketOverhead + sizeof(ushort) + Global.FileChecksumLength;

	public override string ToString()
	{
		return $"Version: {Version}, Id: {Id}, Data: {Data.Length} bytes, PacketCount: {PacketCount}, FileChecksum: {FileChecksum.Length} bytes";
	}

	private byte[] CalculateChecksum(MemoryStream? stream = null)
	{
		if (stream is null)
		{
			stream = new MemoryStream();
			using BinaryWriter writer = new(stream);
			writer.Write(Global.MagicSequence);
			writer.Write(Version);
			writer.Write(Id);
			writer.Write(PacketCount);
			writer.Write(FileChecksum);
			writer.Write(Data);
		}
		return Crc32.Hash(stream.ToArray());
	}

	public override byte[] GetBytes()
	{
		MemoryStream stream = new();
		using BinaryWriter writer = new(stream);
		writer.Write(Global.MagicSequence);
		writer.Write(Version);
		writer.Write(Id);
		writer.Write(PacketCount);
		writer.Write(FileChecksum);
		writer.Write(Data);

		writer.Write(CalculateChecksum(stream));

		return stream.ToArray();
	}
}