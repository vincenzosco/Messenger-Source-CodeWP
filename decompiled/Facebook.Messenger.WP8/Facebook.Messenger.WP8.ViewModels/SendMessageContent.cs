using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Facebook.Models;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class SendMessageContent
	{
		private string _text;

		public string StickerId { get; private set; }

		public string StickerUrl { get; private set; }

		public FacebookPlace Location { get; set; }

		public string Text
		{
			get
			{
				if (_text != null)
				{
					return _text.Trim();
				}
				return string.Empty;
			}
			private set
			{
				_text = value;
			}
		}

		public List<Facebook.Services.Api.Methods.SendMessage.Attachment> Attachments { get; private set; }

		public static SendMessageContent CreateStickerMessage(string stickerId, string stickerUrl)
		{
			SendMessageContent sendMessageContent = new SendMessageContent();
			sendMessageContent.StickerId = stickerId;
			sendMessageContent.StickerUrl = stickerUrl;
			return sendMessageContent;
		}

		public static SendMessageContent CreateImageMessage(Facebook.Services.Api.Methods.SendMessage.Attachment image)
		{
			List<Facebook.Services.Api.Methods.SendMessage.Attachment> list = new List<Facebook.Services.Api.Methods.SendMessage.Attachment>();
			list.Add(image);
			List<Facebook.Services.Api.Methods.SendMessage.Attachment> attachments = list;
			SendMessageContent sendMessageContent = new SendMessageContent();
			sendMessageContent.Attachments = attachments;
			return sendMessageContent;
		}

		public static SendMessageContent CreateVideoMessage(Facebook.Services.Api.Methods.SendMessage.Attachment video)
		{
			List<Facebook.Services.Api.Methods.SendMessage.Attachment> list = new List<Facebook.Services.Api.Methods.SendMessage.Attachment>();
			list.Add(video);
			List<Facebook.Services.Api.Methods.SendMessage.Attachment> attachments = list;
			SendMessageContent sendMessageContent = new SendMessageContent();
			sendMessageContent.Attachments = attachments;
			return sendMessageContent;
		}

		public static SendMessageContent CreateComposerBubbleMessage(string text, ICollection<Facebook.Services.Api.Methods.SendMessage.Attachment> attachments, FacebookPlace location)
		{
			if (string.IsNullOrWhiteSpace(text) && attachments.Count == 0)
			{
				return null;
			}
			string text2 = Regex.Replace(text ?? string.Empty, "\r(?<1>[^\n])", "\r\n$1");
			SendMessageContent sendMessageContent = new SendMessageContent();
			sendMessageContent.Text = text2;
			sendMessageContent.Attachments = attachments.ToList();
			sendMessageContent.Location = location;
			return sendMessageContent;
		}

		public static SendMessageContent CreateVoiceClipMessage(Facebook.Services.Api.Methods.SendMessage.Attachment clip)
		{
			List<Facebook.Services.Api.Methods.SendMessage.Attachment> list = new List<Facebook.Services.Api.Methods.SendMessage.Attachment>();
			list.Add(clip);
			List<Facebook.Services.Api.Methods.SendMessage.Attachment> attachments = list;
			SendMessageContent sendMessageContent = new SendMessageContent();
			sendMessageContent.Attachments = attachments;
			return sendMessageContent;
		}

		public bool HasSticker()
		{
			return !string.IsNullOrEmpty(StickerId);
		}

		private SendMessageContent()
		{
		}
	}
}
