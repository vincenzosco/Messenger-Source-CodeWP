using System.Net;

namespace Facebook.Base;

public static class Url
{
	public static string Encode(string value)
	{
		if (value == null)
		{
			return null;
		}
		string text = WebUtility.UrlEncode(value);
		text = text.Replace("+", "%20");
		text = text.Replace("!", "%21");
		text = text.Replace(")", "%29");
		text = text.Replace("(", "%28");
		return text.Replace("*", "%2A");
	}

	public static string Decode(string value)
	{
		return WebUtility.UrlDecode(value);
	}
}
