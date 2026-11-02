using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Text;
using Facebook.Models;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ThreadSnippet
	{
		public readonly bool SnippetFromMe;

		public readonly long SnippetTimestamp;

		private readonly string _adminSnippet;

		private readonly string _body;

		private readonly bool _isCanonical;

		private readonly string _senderFirstName;

		private readonly Lazy<Paragraph> _displaySnippet;

		public Paragraph DisplaySnippet
		{
			get
			{
				return _displaySnippet.Value;
			}
		}

		public ThreadSnippet(FacebookUser sender, long timestamp, string body, bool snippetFromMe, bool isCanonical, string adminSnippet)
		{
			SnippetTimestamp = timestamp;
			SnippetFromMe = snippetFromMe;
			_adminSnippet = adminSnippet;
			_body = body ?? string.Empty;
			_isCanonical = isCanonical;
			_senderFirstName = ((sender != null) ? sender.FirstName : null);
			_displaySnippet = new Lazy<Paragraph>(CreateDisplaySnippet);
		}

		private Paragraph CreateDisplaySnippet()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected Obj, but got Unknown
			string text = CreateDisplayText().Replace("\r\n", " ").Replace('\r', ' ').Replace('\n', ' ')
				.Replace('\t', ' ');
			List<ParsedText> list = ParseTextHelper.ParseText(text);
			Paragraph val = new Paragraph();
			foreach (ParsedText item in list)
			{
				if (item.type == TextType.Emoticon)
				{
					InlineUIContainer emoticonImage = ParseEmoticonHelper.GetEmoticonImage(item.text);
					if (emoticonImage != null)
					{
						((PresentationFrameworkCollection<Inline>)(object)val.Inlines).Add((Inline)(object)emoticonImage);
					}
					else
					{
						val.Inlines.Add(item.text);
					}
				}
				else
				{
					val.Inlines.Add(item.text);
				}
			}
			return val;
		}

		private string CreateDisplayText()
		{
			if (!string.IsNullOrWhiteSpace(_adminSnippet))
			{
				return _adminSnippet;
			}
			bool flag = string.IsNullOrWhiteSpace(_senderFirstName);
			if (flag)
			{
				Logger.Warning(this, "Sender name is null or empty", "CreateDisplayText", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\ThreadSnippet.cs", 80);
			}
			if ((_isCanonical && !SnippetFromMe) || flag)
			{
				return _body;
			}
			string arg = (SnippetFromMe ? StringLibrary.MessageSnippetWhenSenderIsTheLoggedInUser : _senderFirstName);
			return string.Format("{0}: {1}", arg, _body);
		}
	}
}
