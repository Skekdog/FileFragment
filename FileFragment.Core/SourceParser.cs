using System.Security.Cryptography;

namespace FileFragment.Core;

public class SourceParser
{
	public static List<Packet> ParseSource(Stream input)
	{
		if (input.Length < Config.PacketSize * 2) throw new Exception("Too small file");

		List<Packet> packets = [];
		using BinaryReader reader = new(input);

		var task = MD5.HashDataAsync(input).AsTask();
		task.Wait();
		byte[] fileChecksum = task.Result;

		reader.BaseStream.Position = 0;

		FirstPacket firstPacket = new(reader.ReadBytes((int)Config.PacketSize - FirstPacket.PacketOverhead), 0, fileChecksum);

		packets.Add(firstPacket);

		ushort i = 0;
		while (reader.BaseStream.Position < input.Length)
		{
			i++;
			packets.Add(new Packet(i, reader.ReadBytes((int)Config.PacketSize - Packet.PacketOverhead)));
		}

		firstPacket.PacketCount = (ushort)packets.Count;

		return packets;
	}
}
