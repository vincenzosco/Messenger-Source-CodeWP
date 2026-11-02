using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol
{
	public class MessageDeltaUtil
	{
		private const double ThriftToClientCoordinatesScale = 1000000.0;

		private const int ImageUriMapFullResolutionKey = 1;

		private const int ImageUriMapPreviewResolutionKey = 2;

		public static async Task<MailMessage> CreateMessageForThread(ThreadCache cache, MessageMetadata metadata, MailMessage.Type messageType)
		{
			MailThread thread = await ThreadDeltaUtil.GetThread(cache, metadata.ThreadKey);
			string senderFbidString = metadata.ActorFbId.ToString();
			MailUser senderParticipant = GetSenderParticipant(thread, senderFbidString);
			long actionId = ActionIdUtil.CreateActionIdFromTimestamp(metadata.Timestamp);
			return new MailMessage
			{
				ThreadId = thread.Id,
				ServerTimestamp = metadata.Timestamp,
				OfflineThreadingId = metadata.OfflineThreadingId.ToString(),
				Sender = senderParticipant,
				ActionId = actionId,
				Id = ThreadDeltaUtil.PrepareMessageId(metadata.MessageId, actionId),
				State = MailMessage.MessageState.Sent,
				Tags = metadata.Tags,
				Body = (metadata.AdminText ?? string.Empty),
				MessageType = messageType,
				Source = MailMessage.MessageSource.Sync
			};
		}

		private static MailUser GetSenderParticipant(MailThread thread, string senderId)
		{
			MailUser mailUser = null;
			if (thread != null)
			{
				mailUser = thread.Participants.SingleOrDefault((MailUser p) => p.Id == senderId);
			}
			MailUser mailUser2 = mailUser;
			if (mailUser2 == null)
			{
				MailUser mailUser3 = new MailUser();
				mailUser3.Id = senderId;
				mailUser2 = mailUser3;
			}
			return mailUser2;
		}

		public static List<MailShare> CreateStickerShares(long stickerId)
		{
			List<MailShare> list = new List<MailShare>();
			list.Add(new MailShare
			{
				StickerId = stickerId.ToString()
			});
			return list;
		}

		public static MailCoordinates CreateCoordinates(Coordinates coordinates)
		{
			if (coordinates == null)
			{
				return null;
			}
			MailCoordinates mailCoordinates = new MailCoordinates();
			mailCoordinates.Latitude = (double)coordinates.Latitude / 1000000.0;
			mailCoordinates.Longitude = (double)coordinates.Longitude / 1000000.0;
			return mailCoordinates;
		}

		public static List<MailAttachment> CreateAttachments(IEnumerable<Facebook.Models.SyncProtocol.Attachment> attachments)
		{
			if (attachments == null)
			{
				return null;
			}
			return attachments.Select(CreateAttachment).ToList();
		}

		private static MailAttachment CreateAttachment(Facebook.Models.SyncProtocol.Attachment attachment)
		{
			MailAttachment mailAttachment = new MailAttachment();
			mailAttachment.Id = attachment.Id;
			mailAttachment.MimeType = attachment.MimeType;
			mailAttachment.FileName = attachment.Filename;
			MailAttachment mailAttachment2 = mailAttachment;
			if (attachment.__isset.fbid)
			{
				mailAttachment2.Fbid = attachment.Fbid;
			}
			if (attachment.__isset.fileSize)
			{
				mailAttachment2.FileSize = attachment.FileSize;
			}
			mailAttachment2.Image = CreateImageData(attachment.ImageMetadata);
			mailAttachment2.Video = CreateVideoData(attachment.VideoMetadata);
			return mailAttachment2;
		}

		private static MailAttachment.ImageData CreateImageData(ImageMetadata metadata)
		{
			if (metadata == null)
			{
				return null;
			}
			MailAttachment.ImageData imageData = new MailAttachment.ImageData();
			imageData.Width = metadata.Width;
			imageData.Height = metadata.Height;
			MailAttachment.ImageData imageData2 = imageData;
			if (metadata.ImageURIMap != null)
			{
				string value;
				metadata.ImageURIMap.TryGetValue(1, out value);
				string value2;
				metadata.ImageURIMap.TryGetValue(2, out value2);
				imageData2.Url = value ?? value2;
				imageData2.PreviewUrl = value2 ?? value;
			}
			return imageData2;
		}

		private static MailAttachment.VideoData CreateVideoData(VideoMetadata metadata)
		{
			if (metadata == null)
			{
				return null;
			}
			MailAttachment.VideoData videoData = new MailAttachment.VideoData();
			videoData.Width = metadata.Width;
			videoData.Height = metadata.Height;
			videoData.Length = metadata.DurationMs / 1000;
			videoData.PreviewUrl = metadata.ThumbnailUri;
			videoData.Url = metadata.VideoUri;
			videoData.Rotation = metadata.Rotation;
			videoData.Source = ConvertVideoSource(metadata.Source);
			return videoData;
		}

		private static MailAttachment.VideoData.VideoSource ConvertVideoSource(VideoSource source)
		{
			switch (source)
			{
			case VideoSource.NON_QUICKCAM:
				return MailAttachment.VideoData.VideoSource.NonQuickCam;
			case VideoSource.QUICKCAM:
				return MailAttachment.VideoData.VideoSource.QuickCam;
			default:
				throw new ArgumentOutOfRangeException("source");
			}
		}
	}
}
