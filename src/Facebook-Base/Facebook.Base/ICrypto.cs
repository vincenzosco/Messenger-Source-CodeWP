namespace Facebook.Base
{
	public interface ICrypto
	{
		string ToMD5HexString(string data);

		byte[] ToMD5(string data);

		byte[] ToSHA1(string data);
	}
}
