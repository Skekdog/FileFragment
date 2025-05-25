namespace FileFragmentV2.Source;

public static class HexString
{
	public static string ByteArrayToHexString(byte[] bytes)
	{
		return BitConverter.ToString(bytes).Replace("-", " ");
	}
}
