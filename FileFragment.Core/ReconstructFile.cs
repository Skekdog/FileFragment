using System.Security.Cryptography;

namespace FileFragment.Core;

public static class ReconstructFile
{
	public static MemoryStream Reconstruct(List<Packet> packets)
	{
		FirstPacket firstPacket = packets.Find(p => p is FirstPacket) as FirstPacket ?? throw new Exception("First packet not found");

		if (firstPacket.PacketCount != packets.Count) throw new Exception($"Invalid packet count. Expected: {firstPacket.PacketCount}, Actual: {packets.Count}");

		MemoryStream stream = new();
		using BinaryWriter writer = new(stream);

		packets.OrderBy(p => p.Id).ToList().ForEach(p => writer.Write(p.Data));

		byte[] fileChecksum = MD5.HashData(stream.ToArray());
		if (!firstPacket.FileChecksum.SequenceEqual(fileChecksum))
			throw new Exception($"Invalid file checksum. Expected: {HexString.ByteArrayToHexString(firstPacket.FileChecksum)}, Actual: {HexString.ByteArrayToHexString(fileChecksum)}");

		return stream;
	}
}