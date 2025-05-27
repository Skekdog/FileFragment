namespace FileFragment.Core;

public static class HexString
{
	public static string ByteArrayToHexString(byte[] bytes)
	{
		return BitConverter.ToString(bytes).Replace("-", " ");
	}
}
