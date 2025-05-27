namespace FileFragment.Core;

public class DiskOp
{
	public static void Fragment(uint packetSize, string outputDirectory, string inputFile)
	{
		Config.PacketSize = packetSize;
		List<Packet> packets = SourceParser.ParseSource(File.OpenRead(inputFile));

		if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);

		packets.ForEach(packet =>
		{
			File.WriteAllBytes(Path.Combine(outputDirectory, $"Packet{packet.Id}.sff"), packet.GetBytes());
		});
	}

	public static void Defragment(string inputDirectory, string outputFile)
	{
		List<Packet> packets = [];
		Directory.EnumerateFiles(inputDirectory, "*.sff").ToList().ForEach(f =>
		{
			packets.Add(PacketParser.GeneratePacket(File.OpenRead(f)));
		});

		File.WriteAllBytes(outputFile, ReconstructFile.Reconstruct(packets).ToArray());
	}
}
