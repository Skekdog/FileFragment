using System.CommandLine;
using FileFragmentV2.Source;

var rootCommand = new RootCommand();

var fragmentCommand = new Command("fragment", "Fragment a file");

var packetSizeOption = new Option<uint>("--packet-size", "Packet size in bytes") { IsRequired = true };
fragmentCommand.AddOption(packetSizeOption);

packetSizeOption.AddValidator(result =>
{
	var value = result.GetValueOrDefault<uint>();
	if (value <= FirstPacket.PacketOverhead) result.ErrorMessage = $"Packet size must be at least {FirstPacket.PacketOverhead + 1} bytes";
});

var fragmentOutputOption = new Option<string>("--output", "Output directory") { IsRequired = true };
fragmentCommand.AddOption(fragmentOutputOption);

var fragmentInputOption = new Option<string>("--input", "Input file") { IsRequired = true };
fragmentCommand.AddOption(fragmentInputOption);

var defragmentCommand = new Command("defragment", "Defragment packets");

var defragmentInputOption = new Option<string>("--input", "Input directory") { IsRequired = true };
defragmentCommand.AddOption(defragmentInputOption);

var defragmentOutputOption = new Option<string>("--output", "Output file") { IsRequired = true };
defragmentCommand.AddOption(defragmentOutputOption);

fragmentCommand.SetHandler((packetSize, outputDirectory, inputFile) =>
{
	Config.PacketSize = packetSize;

	List<Packet> packets = SourceParser.ParseSource(File.OpenRead(inputFile));

	if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);

	packets.ForEach(packet =>
	{
		File.WriteAllBytes(Path.Combine(outputDirectory, $"Packet{packet.Id}.sff"), packet.GetBytes());
	});
}, packetSizeOption, fragmentOutputOption, fragmentInputOption);

defragmentCommand.SetHandler((inputDirectory, outputFile) =>
{
	List<Packet> readPackets = [];
	Directory.EnumerateFiles(inputDirectory, "*.sff").ToList().ForEach(f =>
	{
		readPackets.Add(PacketParser.GeneratePacket(File.OpenRead(f)));
	});

	File.WriteAllBytes(outputFile, ReconstructFile.Reconstruct(readPackets).ToArray());
}, defragmentInputOption, defragmentOutputOption);

rootCommand.AddCommand(fragmentCommand);
rootCommand.AddCommand(defragmentCommand);

return await rootCommand.InvokeAsync(args);