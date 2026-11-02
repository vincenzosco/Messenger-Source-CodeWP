using System.Collections.Generic;

namespace Facebook.Messenger.WP8.Text;

public class ParseTextHelper
{
	public static List<ParsedText> ParseText(string text)
	{
		List<ParsedText> list = ParseUrlHelper.ParseUrls(text);
		List<ParsedText> list2 = new List<ParsedText>();
		foreach (ParsedText item in list)
		{
			if (item.type == TextType.Text)
			{
				list2.AddRange(ParseEmoticonHelper.ParseEmots(item.text));
				continue;
			}
			list2.Add(new ParsedText
			{
				text = item.text,
				type = TextType.Url
			});
		}
		return list2;
	}
}
