using FileFragment.Core;

namespace FileFragment.Tests;

public class HexStringTests
{
    [Theory]
    [InlineData("30 31 32", new byte[] { 48, 49, 50 })]
    [InlineData("00 01 02", new byte[] { 0, 1, 2 })]
    [InlineData("", new byte[] { })]
    [InlineData("FF 00 FF FF", new byte[] { 255, 0, 255, 255 })]
    [InlineData("FF", new byte[] { 255 })]
    [InlineData("00", new byte[] { 0 })]
    public void ByteArrayToHexString_ConvertsArrayToCorrectString(string expected, byte[] bytes)
    {
        Assert.Equal(expected, HexString.ByteArrayToHexString(bytes));
    }
}