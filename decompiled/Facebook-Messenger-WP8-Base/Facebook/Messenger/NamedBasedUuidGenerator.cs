using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Facebook.Messenger;

public class NamedBasedUuidGenerator
{
	public enum Namespace
	{
		Dns,
		Url,
		ObjectId,
		X500
	}

	public enum Version
	{
		MD5,
		SHA1
	}

	public static Guid Generate(string name, Namespace ns, Version version)
	{
		byte[] array = GetNamespaceGuid(ns).ToByteArray();
		AlternateUuidNetworkByteOrder(array);
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		byte[] data = array.Concat(bytes).ToArray();
		byte[] array2 = HashData(version, data);
		byte[] array3 = new byte[16];
		Array.Copy(array2, 0, array3, 0, 4);
		Array.Copy(array2, 4, array3, 4, 2);
		Array.Copy(array2, 6, array3, 6, 2);
		array3[6] = (byte)((array3[6] & 0xF) | (GetVersionNumber(version) << 4));
		array3[8] = array2[8];
		array3[8] = (byte)((array3[8] & 0x3F) | 0x80);
		array3[9] = array2[9];
		Array.Copy(array2, 10, array3, 10, 6);
		AlternateUuidNetworkByteOrder(array3);
		return new Guid(array3);
	}

	private static Guid GetNamespaceGuid(Namespace ns)
	{
		return ns switch
		{
			Namespace.Dns => new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"), 
			Namespace.Url => new Guid("6ba7b811-9dad-11d1-80b4-00c04fd430c8"), 
			Namespace.ObjectId => new Guid("6ba7b812-9dad-11d1-80b4-00c04fd430c8"), 
			Namespace.X500 => new Guid("6ba7b814-9dad-11d1-80b4-00c04fd430c8"), 
			_ => throw new ArgumentException("Namespace not handled"), 
		};
	}

	private static int GetVersionNumber(Version version)
	{
		return version switch
		{
			Version.MD5 => 3, 
			Version.SHA1 => 5, 
			_ => throw new ArgumentException("Version is not handled"), 
		};
	}

	private static void AlternateUuidNetworkByteOrder(byte[] data)
	{
		SwapBytes(data, 0, 3);
		SwapBytes(data, 1, 2);
		SwapBytes(data, 4, 7);
		SwapBytes(data, 5, 6);
	}

	private static void SwapBytes(byte[] data, int a, int b)
	{
		byte b2 = data[a];
		data[a] = data[b];
		data[b] = b2;
	}

	private static byte[] HashData(Version version, byte[] data)
	{
		switch (version)
		{
		case Version.SHA1:
		{
			using SHA1Managed sHA1Managed = new SHA1Managed();
			return sHA1Managed.ComputeHash(data);
		}
		case Version.MD5:
			return MD5Core.GetHash(data);
		default:
			throw new ArgumentException("Version is not handled");
		}
	}
}
