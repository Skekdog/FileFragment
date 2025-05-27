using System.CommandLine;
using FileFragment.Core;

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

fragmentCommand.SetHandler(DiskOp.Fragment, packetSizeOption, fragmentOutputOption, fragmentInputOption);

defragmentCommand.SetHandler(DiskOp.Defragment, defragmentInputOption, defragmentOutputOption);

rootCommand.AddCommand(fragmentCommand);
rootCommand.AddCommand(defragmentCommand);

return await rootCommand.InvokeAsync(args);