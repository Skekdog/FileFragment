using FileFragment.Core;

namespace FileFragment.Tests;

public sealed class ConfigTests : IDisposable
{
    private readonly uint _originalPacketSize = Config.PacketSize;

    public ConfigTests()
    {
        _originalPacketSize = Config.PacketSize;
    }

    public void Dispose()
    {
        Config.PacketSize = _originalPacketSize;
    }

    [Fact]
    public void SetPacketSize_CannotBeLessThanOrEqualToOverhead()
    {
        Assert.Throws<Exception>(() =>
        {
            Config.PacketSize = (uint)FirstPacket.PacketOverhead;
        });
    }

    [Fact]
    public void SetPacketSize_CanBeGreaterThanOverhead()
    {
        Config.PacketSize = (uint)FirstPacket.PacketOverhead + 1;
        Assert.Equal((uint)FirstPacket.PacketOverhead + 1, Config.PacketSize);
    }
}
