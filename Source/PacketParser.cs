using System.Data;
using System.Security.Cryptography;

namespace FileFragmentV2.Source;

public static class PacketParser
{
	private record Tail(byte[] Checksum, byte[] Data);

	private static Tail ParseTail(BinaryReader reader)
	{
		byte[] tail = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
		if (tail.Length < 5) throw new DataException("Tail too short");

		byte[] data = tail[0..^4];
		byte[] checksum = tail[^4..];

		return new(checksum, data);
	}

	private static FirstPacket ParseFirstPacket(BinaryReader reader)
	{
		ushort packetCount = reader.ReadUInt16();
		if (packetCount == 0) throw new DataException("Invalid packet count");

		byte[] fileChecksum = reader.ReadBytes(MD5.HashSizeInBytes);

		Tail tail = ParseTail(reader);
		byte[] data = tail.Data;

		FirstPacket packet = new(data, packetCount, fileChecksum);

		if (!packet.PacketChecksum.SequenceEqual(tail.Checksum))
			throw new DataException($"Invalid packet checksum. Expected: {HexString.ByteArrayToHexString(packet.PacketChecksum)}, Actual: {HexString.ByteArrayToHexString(tail.Checksum)}");

		return packet;
	}

	private static Packet ParsePacket(ushort id, BinaryReader reader)
	{
		Tail tail = ParseTail(reader);
		Packet packet = new(id, tail.Data);

		if (!packet.PacketChecksum.SequenceEqual(tail.Checksum))
			throw new DataException($"Invalid packet checksum. Expected: {HexString.ByteArrayToHexString(packet.PacketChecksum)}, Actual: {HexString.ByteArrayToHexString(tail.Checksum)}");

		return packet;
	}

	public static Packet GeneratePacket(Stream input)
	{
		using BinaryReader reader = new(input);

		byte[] magic = reader.ReadBytes(Global.MagicSequence.Length);
		if (!magic.SequenceEqual(Global.MagicSequence)) throw new DataException("Invalid magic sequence");

		byte version = reader.ReadByte();
		if (version != Global.Version) throw new DataException($"Incorrect version. Expected: {Global.Version}, Actual: {version}");

		ushort id = reader.ReadUInt16();

		if (id == 0) return ParseFirstPacket(reader);

		return ParsePacket(id, reader);
	}
}
