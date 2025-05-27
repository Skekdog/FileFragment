using System.IO.Hashing;
using System.Security.Cryptography;
using System.Text;

namespace FileFragment.Core;

public static class Global
{
	public const string FileExtension = ".sff";
	public static readonly byte[] MagicSequence = Encoding.ASCII.GetBytes("sff");

	public const byte Version = 1;

	public static readonly int PacketChecksumLength = new Crc32().HashLengthInBytes;
	public static readonly int FileChecksumLength = MD5.HashSizeInBytes;
}