using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class MessageDetailsUtils
	{
		public static readonly BitmapImage QueuedGrayImage = new BitmapImage(new Uri("/Assets/Threads/Queued.png", UriKind.Relative));

		public static readonly BitmapImage DeliveredGrayImage = new BitmapImage(new Uri("/Assets/Threads/Delivered.png", UriKind.Relative));

		public static readonly BitmapImage SentGrayImage = new BitmapImage(new Uri("/Assets/Threads/Sent.png", UriKind.Relative));

		public static readonly BitmapImage QueuedImage = new BitmapImage(new Uri("/Assets/Messages/DeliveryReceipts/Queued.png", UriKind.Relative));

		public static readonly BitmapImage DeliveredImage = new BitmapImage(new Uri("/Assets/Messages/DeliveryReceipts/Delivered.png", UriKind.Relative));

		public static readonly BitmapImage SentImage = new BitmapImage(new Uri("/Assets/Messages/DeliveryReceipts/Sent.png", UriKind.Relative));

		public static readonly BitmapImage FailedImage = new BitmapImage(new Uri("/Assets/Threads/inboxFailedIcon.png", UriKind.Relative));

		public static string GetMessageOriginText(MessageViewModel message)
		{
			List<string> tags = message.Tags;
			if (tags.IsNullOrEmpty() || !message.IsRegularMessage())
			{
				return string.Empty;
			}
			if (tags.Intersect(MessageTags.OriginMessengerTags).Any())
			{
				return StringLibrary.MessageStatus_Origin_Messenger;
			}
			if (tags.Intersect(MessageTags.OriginWebTags).Any())
			{
				return StringLibrary.MessageStatus_Origin_Web;
			}
			if (tags.Intersect(MessageTags.OriginMobileTags).Any())
			{
				return StringLibrary.MessageStatus_Origin_Mobile;
			}
			Logger.Info(string.Format("Unknown Message Origin. Message Tags: {0}", string.Join(", ", tags)), "GetMessageOriginText", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\MessageDetailsUtils.cs", 38);
			return string.Empty;
		}

		public static BitmapImage MessageStateImage(MailMessage.MessageState state)
		{
			switch (state)
			{
			case MailMessage.MessageState.Failed:
				return FailedImage;
			case MailMessage.MessageState.Queued:
				return QueuedImage;
			case MailMessage.MessageState.Delivered:
				return DeliveredImage;
			default:
				return SentImage;
			}
		}

		public static BitmapImage GrayMessageStateImage(MailMessage.MessageState state)
		{
			switch (state)
			{
			case MailMessage.MessageState.Failed:
				return FailedImage;
			case MailMessage.MessageState.Queued:
				return QueuedGrayImage;
			case MailMessage.MessageState.Delivered:
				return DeliveredGrayImage;
			default:
				return SentGrayImage;
			}
		}

		static MessageDetailsUtils()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected Obj, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected Obj, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected Obj, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected Obj, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected Obj, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected Obj, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected Obj, but got Unknown
		}
	}
}
