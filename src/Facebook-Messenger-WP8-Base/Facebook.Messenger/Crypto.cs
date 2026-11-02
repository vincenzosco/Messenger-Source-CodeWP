using System.Security.Cryptography;
using System.Text;
using Facebook.Base;

namespace Facebook.Messenger
{
	public class Crypto : ICrypto
	{
		public string ToMD5HexString(string data)
		{
			return MD5Core.GetHashString(data).ToLower();
		}

		public byte[] ToMD5(string data)
		{
			return MD5Core.GetHash(data, Encoding.UTF8);
		}

		public byte[] ToSHA1(string data)
		{
			SHA1Managed sHA1Managed = new SHA1Managed();
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			return sHA1Managed.ComputeHash(bytes);
		}
	}
}
