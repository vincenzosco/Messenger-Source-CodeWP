using System;
using System.Collections.Generic;

namespace Facebook.Models.Push;

public class FacebookPushUtil
{
	public static KeyValuePair<string, string>? TryParseMessagePushParts(string push)
	{
		int num = push.IndexOf(": ", StringComparison.OrdinalIgnoreCase);
		if (num >= 0)
		{
			return new KeyValuePair<string, string>(push.Substring(0, num), push.Substring(num + 2));
		}
		return null;
	}
}
