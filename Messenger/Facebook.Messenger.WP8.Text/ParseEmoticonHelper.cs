using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Facebook.Messenger.WP8.Text
{
	public static class ParseEmoticonHelper
	{
		private static Regex _emoticonPattern;

		private static readonly Dictionary<string, string> BasicEmoticonToAsset = new Dictionary<string, string>
		{
			{ "(y)", "/Assets/Emoticons/EmoticonLike.png" },
			{ "(Y)", "/Assets/Emoticons/EmoticonLike.png" },
			{ ":like:", "/Assets/Emoticons/EmoticonLike.png" },
			{ "<3", "/Assets/Emoticons/emoji_2764_32.png" },
			{ "&lt;3", "/Assets/Emoticons/emoji_2764_32.png" },
			{ ":)", "/Assets/Emoticons/emoji_1f60a_32.png" },
			{ "(:", "/Assets/Emoticons/emoji_1f60a_32.png" },
			{ ":]", "/Assets/Emoticons/emoji_1f60a_32.png" },
			{ ":-)", "/Assets/Emoticons/emoji_1f60a_32.png" },
			{ "=)", "/Assets/Emoticons/emoji_1f60a_32.png" },
			{ ":(", "/Assets/Emoticons/emoji_1f61e_32.png" },
			{ ":[", "/Assets/Emoticons/emoji_1f61e_32.png" },
			{ "=(", "/Assets/Emoticons/emoji_1f61e_32.png" },
			{ ":-(", "/Assets/Emoticons/emoji_1f61e_32.png" },
			{ ">:(", "/Assets/Emoticons/emoji_1f620_32.png" },
			{ ">:-(", "/Assets/Emoticons/emoji_1f620_32.png" },
			{ ">:o", "/Assets/Emoticons/emoji_1f620_32.png" },
			{ ">:O", "/Assets/Emoticons/emoji_1f620_32.png" },
			{ ">:-o", "/Assets/Emoticons/emoji_1f620_32.png" },
			{ ">:-O", "/Assets/Emoticons/emoji_1f620_32.png" },
			{ ":o", "/Assets/Emoticons/emoji_1f632_32.png" },
			{ ":-o", "/Assets/Emoticons/emoji_1f632_32.png" },
			{ ":O", "/Assets/Emoticons/emoji_1f632_32.png" },
			{ ":-O", "/Assets/Emoticons/emoji_1f632_32.png" },
			{ ":p", "/Assets/Emoticons/emoji_1f61c_32.png" },
			{ ":-p", "/Assets/Emoticons/emoji_1f61c_32.png" },
			{ ":P", "/Assets/Emoticons/emoji_1f61c_32.png" },
			{ ":-P", "/Assets/Emoticons/emoji_1f61c_32.png" },
			{ "=P", "/Assets/Emoticons/emoji_1f61c_32.png" },
			{ "=D", "/Assets/Emoticons/emoji_1f603_32.png" },
			{ ":D", "/Assets/Emoticons/emoji_1f603_32.png" },
			{ ":-D", "/Assets/Emoticons/emoji_1f603_32.png" },
			{ ";)", "/Assets/Emoticons/emoji_1f609_32.png" },
			{ ";-)", "/Assets/Emoticons/emoji_1f609_32.png" },
			{ ":*", "/Assets/Emoticons/emoji_1f618_32.png" },
			{ ":-*", "/Assets/Emoticons/emoji_1f618_32.png" },
			{ ":/", "/Assets/Emoticons/emoji_1f615_32.png" },
			{ "=/", "/Assets/Emoticons/emoji_1f615_32.png" },
			{ "=\\", "/Assets/Emoticons/emoji_1f615_32.png" },
			{ ":-/", "/Assets/Emoticons/emoji_1f615_32.png" },
			{ ":\\", "/Assets/Emoticons/emoji_1f615_32.png" },
			{ ":-\\", "/Assets/Emoticons/emoji_1f615_32.png" },
			{ ":'(", "/Assets/Emoticons/emoji_1f622_32.png" },
			{ "^_^", "/Assets/Emoticons/emoji_1f604_32.png" },
			{ ">_<", "/Assets/Emoticons/emoji_1f620_32.png" },
			{ ">.<", "/Assets/Emoticons/emoji_1f620_32.png" },
			{ "3:)", "/Assets/Emoticons/emoji_1f608_32.png" },
			{ "3:-)", "/Assets/Emoticons/emoji_1f608_32.png" },
			{ "<(\")", "/Assets/Emoticons/emoji_1f608_32.png" },
			{ "(poo)", "/Assets/Emoticons/emoji_1f4a9_32.png" },
			{ ":poop:", "/Assets/Emoticons/emoji_1f4a9_32.png" },
			{ "o.O", "/Assets/Emoticons/emoji_emoticon_confused.png" },
			{ "O.o", "/Assets/Emoticons/emoji_emoticon_confused_rev.png" }
		};

		public static Regex GetEmoticonPattern()
		{
			if (_emoticonPattern == null)
			{
				string pattern = string.Join("|", from s in GetBasicEmoticonsList()
					select "(^|\\s)(" + Regex.Escape(s) + ")(\\s|$)");
				_emoticonPattern = new Regex(pattern, RegexOptions.IgnoreCase);
			}
			return _emoticonPattern;
		}

		private static bool TryGetValue(string text, out string assetUri)
		{
			return BasicEmoticonToAsset.TryGetValue(text, out assetUri);
		}

		private static IEnumerable<string> GetBasicEmoticonsList()
		{
			return BasicEmoticonToAsset.Keys;
		}

		public static InlineUIContainer GetEmoticonImage(string s)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected Obj, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected Obj, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected Obj, but got Unknown
			string assetUri;
			if (!TryGetValue(s, out assetUri))
			{
				return null;
			}
			Image val = new Image();
			((FrameworkElement)val).Width = 25.0;
			((FrameworkElement)val).Height = 25.0;
			BitmapImage val2 = new BitmapImage(new Uri(assetUri, UriKind.Relative));
			val2.DecodePixelHeight = 25;
			val2.DecodePixelWidth = 25;
			val.Source = (ImageSource)(object)val2;
			((FrameworkElement)val).Margin = new Thickness(2.0, 0.0, 2.0, -5.0);
			Image child = val;
			InlineUIContainer val3 = new InlineUIContainer();
			val3.Child = (UIElement)(object)child;
			return val3;
		}

		public static List<ParsedText> ParseEmots(string text)
		{
			List<ParsedText> list = new List<ParsedText>();
			IEnumerable<string> enumerable = from s in GetEmoticonPattern().Split(text)
				where s != string.Empty
				select s;
			foreach (string item in enumerable)
			{
				list.Add(new ParsedText
				{
					text = item,
					type = (IsEmoticon(item) ? TextType.Emoticon : TextType.Text)
				});
			}
			return list;
		}

		private static bool IsEmoticon(string text)
		{
			return GetEmoticonPattern().IsMatch(text);
		}
	}
}
