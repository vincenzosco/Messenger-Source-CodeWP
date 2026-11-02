using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Facebook.Messenger.WP8.Text
{
	public static class ParseUrlHelper
	{
		private static Regex _urlPattern;

		public static Regex GetUrlPattern()
		{
			if (_urlPattern == null)
			{
				_urlPattern = new Regex("\\b((?:https?://|www\\d{0,3}[.]|[a-z0-9.\\-]+[.][a-z]{2,6}/|[a-z0-9.\\-]+[.]xn--[a-z]+/)(?:[^\\s()`\"{}<>«»“”‘’]+|\\((?:[^\\s()`\"{}<>«»“”‘’]+|\\([^\\s()`\"{}<>«»“”‘’]+\\))*\\))+(?:\\((?:[^\\s()`\"{}<>«»“”‘’]+|\\([^\\s()`\"{}<>«»“”‘’]+\\))*\\)|[^\\s()`\"{}<>«»“”‘’!\\[\\]'.?,:;]))", RegexOptions.IgnoreCase);
			}
			return _urlPattern;
		}

		public static List<ParsedText> ParseUrls(string text)
		{
			IEnumerable<string> enumerable = from s in GetUrlPattern().Split(text)
				where s != string.Empty
				select s;
			List<ParsedText> list = new List<ParsedText>();
			foreach (string item in enumerable)
			{
				list.Add(new ParsedText
				{
					text = item,
					type = (IsUrl(item) ? TextType.Url : TextType.Text)
				});
			}
			return list;
		}

		private static bool IsUrl(string text)
		{
			return GetUrlPattern().IsMatch(text);
		}
	}
}
