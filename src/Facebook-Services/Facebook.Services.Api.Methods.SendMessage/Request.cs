using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Facebook.Base;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SendMessage
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		private WeakReference<Action<bool>> _isCancellableAction;

		[DataMember]
		[JsonProperty(PropertyName = "users")]
		public List<MailUser> Users { get; set; }

		[JsonProperty(PropertyName = "retry_attempt")]
		[DataMember]
		public int RetryAttempt { get; set; }

		[JsonProperty(PropertyName = "message")]
		[DataMember]
		public string Message { get; set; }

		[JsonProperty(PropertyName = "offline_threading_id")]
		[DataMember]
		public string OfflineThreadingId { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "object_attachment_id")]
		public string ObjectAttachmentId { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "object_attachment_href")]
		public string ObjectAttachmentHref { get; set; }

		[JsonProperty(PropertyName = "server_thread_id")]
		[DataMember]
		public string ServerThreadId { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "single_recipient_id")]
		public string SingleRecipientId { get; set; }

		[JsonProperty(PropertyName = "copy_message_id")]
		[DataMember]
		public string CopyMessageId { get; set; }

		[JsonProperty(PropertyName = "copy_attachment_id")]
		[DataMember]
		public string CopyAttachmentId { get; set; }

		[DataMember]
		internal string LocalThreadId { get; set; }

		[JsonProperty(PropertyName = "location")]
		[DataMember]
		public FacebookPlace Location { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "file_paths")]
		public List<Attachment> FilePaths { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "media_fbids")]
		public List<string> MediaFbids { get; set; }

		[JsonProperty(PropertyName = "sender")]
		[DataMember]
		public MailUser Sender { get; set; }

		[JsonProperty(PropertyName = "sent_timestamp")]
		[DataMember]
		public long SentTimestamp { get; private set; }

		public bool HasSingleRecipientId()
		{
			return !string.IsNullOrEmpty(SingleRecipientId);
		}

		public Request()
		{
		}

		public Request(string threadId, string message, string offlineThreadingId)
		{
			ServerThreadId = threadId;
			Message = message;
			OfflineThreadingId = offlineThreadingId;
			Users = new List<MailUser>();
			SentTimestamp = DateTime.UtcNow.ToUnixTimeMilliseconds();
			FilePaths = new List<Attachment>();
		}

		public Request(List<MailUser> userIds, string message, string offlineThreadingId)
		{
			Users = userIds;
			Message = message;
			OfflineThreadingId = offlineThreadingId;
			SentTimestamp = DateTime.UtcNow.ToUnixTimeMilliseconds();
			FilePaths = new List<Attachment>();
		}

		public bool HasValidThreadId()
		{
			if (!HasServerThreadId())
			{
				return HasLocalThreadId();
			}
			return true;
		}

		public string GetValidThreadId()
		{
			if (HasServerThreadId())
			{
				return ServerThreadId;
			}
			if (HasLocalThreadId())
			{
				return LocalThreadId;
			}
			return null;
		}

		public bool IsValidSingleRecipientMessage()
		{
			if (Users != null && Users.Count == 1)
			{
				return !string.IsNullOrWhiteSpace(Users.First().Id);
			}
			return false;
		}

		public void SetIsCancellableAction(Action<bool> action)
		{
			_isCancellableAction = new WeakReference<Action<bool>>(action);
		}

		public void InvokeIsCancellableAction(bool isCancellable)
		{
			Action<bool> target;
			if (_isCancellableAction != null && _isCancellableAction.TryGetTarget(out target) && target != null)
			{
				target(isCancellable);
			}
		}

		public void ClearLocalAndServerIds()
		{
			ServerThreadId = null;
			LocalThreadId = null;
		}

		public bool HasServerThreadId()
		{
			return !string.IsNullOrWhiteSpace(ServerThreadId);
		}

		public bool HasLocalThreadId()
		{
			return !string.IsNullOrWhiteSpace(LocalThreadId);
		}

		public bool HasFiles()
		{
			if (FilePaths != null)
			{
				return FilePaths.Count > 0;
			}
			return false;
		}

		public bool HasUploadableMedia()
		{
			if (FilePaths != null)
			{
				return FilePaths.Any((Attachment a) => a.IsPhoto || a.IsVideo);
			}
			return false;
		}

		public bool HasVideo()
		{
			if (FilePaths != null)
			{
				return FilePaths.Any((Attachment f) => f.IsVideo);
			}
			return false;
		}

		public bool HasMedia()
		{
			if (MediaFbids != null)
			{
				return MediaFbids.Any();
			}
			return false;
		}

		public static MailUser CreateUserFromSession(FacebookContext context)
		{
			FacebookProfile profile = context.Session.Profile;
			MailUser mailUser = new MailUser();
			if (profile != null)
			{
				mailUser.Id = profile.Id;
				mailUser.Name = profile.Name;
			}
			return mailUser;
		}

		public static MailMessage CreateStubMessage(Request request, FacebookContext context, Response response = null)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.Id = ((response != null) ? response.MessageId : request.OfflineThreadingId);
			mailMessage.ThreadId = ((response != null) ? response.ThreadId : request.ServerThreadId);
			mailMessage.Body = request.Message;
			mailMessage.Sender = CreateUserFromSession(context);
			mailMessage.SendTimestamp = request.SentTimestamp;
			mailMessage.OfflineThreadingId = request.OfflineThreadingId;
			MailMessage mailMessage2 = mailMessage;
			if (request.ObjectAttachmentId != null && request.ObjectAttachmentHref != null)
			{
				MailShare.Media media = new MailShare.Media();
				media.Href = request.ObjectAttachmentHref;
				MailShare.Media item = media;
				MailShare mailShare = new MailShare();
				mailShare.StickerId = request.ObjectAttachmentId;
				mailShare.Href = request.ObjectAttachmentHref;
				mailShare.MediaList = new List<MailShare.Media> { item };
				MailShare item2 = mailShare;
				mailMessage2.Shares = new List<MailShare> { item2 };
			}
			if (request.Location != null && !string.IsNullOrWhiteSpace(request.Location.Latitude) && !string.IsNullOrWhiteSpace(request.Location.Longitude))
			{
				MailCoordinates mailCoordinates = new MailCoordinates();
				mailCoordinates.Latitude = double.Parse(request.Location.Latitude, CultureInfo.InvariantCulture);
				mailCoordinates.Longitude = double.Parse(request.Location.Longitude, CultureInfo.InvariantCulture);
				MailCoordinates coordinates = mailCoordinates;
				mailMessage2.Coordinates = coordinates;
			}
			if (request.FilePaths != null && request.FilePaths.Count > 0)
			{
				List<MailAttachment> list = new List<MailAttachment>();
				foreach (Attachment filePath in request.FilePaths)
				{
					string text = FileUtil.ConvertPathToUrl(filePath.Path);
					string text2 = (filePath.HasThumbnail ? FileUtil.ConvertPathToUrl(filePath.ThumbnailPath) : text);
					MailAttachment mailAttachment = new MailAttachment();
					mailAttachment.FileName = filePath.Name;
					mailAttachment.MimeType = filePath.MimeType;
					mailAttachment.LocalUrl = text;
					mailAttachment.LocalThumbnailUrl = text2;
					MailAttachment mailAttachment2 = mailAttachment;
					if (filePath.IsPhoto)
					{
						mailAttachment2.Image = new MailAttachment.ImageData
						{
							Url = text,
							PreviewUrl = text2,
							Width = filePath.Width,
							Height = filePath.Height
						};
					}
					else if (filePath.IsVideo)
					{
						mailAttachment2.Video = new MailAttachment.VideoData
						{
							Url = filePath.Path,
							PreviewUrl = filePath.ThumbnailPath,
							Width = filePath.Width,
							Height = filePath.Height,
							Length = filePath.Length,
							Rotation = filePath.Rotation
						};
					}
					list.Add(mailAttachment2);
				}
				mailMessage2.Attachments = list;
			}
			return mailMessage2;
		}

		public ExtraFailureReason GetExtraFailureReason(long startMs, ApplicationState applicationState)
		{
			bool flag = applicationState.IsTimestampBeforeLastStart(startMs);
			bool manuallyTerminated = applicationState.ManuallyTerminated;
			if (flag)
			{
				return ExtraFailureReason.AppBackgrounded;
			}
			if (manuallyTerminated)
			{
				return ExtraFailureReason.AppManuallyTerminated;
			}
			if (CancelToken.IsCancellationRequested)
			{
				return ExtraFailureReason.RequestManuallyCancelled;
			}
			return ExtraFailureReason.None;
		}
	}
}
