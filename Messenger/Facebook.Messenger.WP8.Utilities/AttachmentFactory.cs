using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class AttachmentFactory
	{
		public static async Task<Facebook.Services.Api.Methods.SendMessage.Attachment> CreateAttachment(ImageResult imageResult)
		{
			if (imageResult == null || imageResult.Image == null || imageResult.Image.Source == null)
			{
				Logger.Error("Could not create attachment will null image", "CreateAttachment", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\AttachmentFactory.cs", 16);
				return null;
			}
			return CreateAttachment(await PhotoAttachmentHelper.PrepareImage(imageResult.Image), imageResult.Source);
		}

		public static Facebook.Services.Api.Methods.SendMessage.Attachment CreateAttachment(PhotoAttachmentHelper.PhotoAttachmentInfo info, MediaSource source)
		{
			if (info == null)
			{
				return null;
			}
			Facebook.Services.Api.Methods.SendMessage.Attachment attachment = new Facebook.Services.Api.Methods.SendMessage.Attachment();
			attachment.ThumbnailPath = info.ThumbnailPath;
			attachment.Path = info.ResizedPath;
			attachment.Width = info.ResizedWidth;
			attachment.Height = info.ResizedHeight;
			attachment.MimeType = FileUtil.GetImageMimeTypeFromFilePath(info.ResizedPath);
			attachment.AttachmentSizeBytes = info.ResizedFilesizeBytes;
			attachment.Name = Path.GetFileName(info.ResizedPath);
			attachment.Source = source;
			return attachment;
		}

		public static Facebook.Services.Api.Methods.SendMessage.Attachment CreateQuickCamVideoRecordingAttachment(VideoResult result)
		{
			if (result == null)
			{
				return null;
			}
			Facebook.Services.Api.Methods.SendMessage.Attachment attachment = new Facebook.Services.Api.Methods.SendMessage.Attachment();
			attachment.ThumbnailPath = result.ThumbnailPath;
			attachment.Path = result.Path;
			attachment.Height = result.Height;
			attachment.Width = result.Width;
			attachment.MimeType = FileUtil.GetVideoMimeTypeFromFilePath(result.Path);
			attachment.AttachmentSizeBytes = result.FileSize;
			attachment.Length = result.Length;
			attachment.Name = Path.GetFileName(result.Path);
			attachment.Rotation = result.Rotation;
			attachment.Source = MediaSource.QuickCam;
			return attachment;
		}

		public static Facebook.Services.Api.Methods.SendMessage.Attachment CreateVoiceRecordingAttachment(VoiceRecordingHelper.VoiceRecordingResult result)
		{
			Facebook.Services.Api.Methods.SendMessage.Attachment attachment = new Facebook.Services.Api.Methods.SendMessage.Attachment();
			attachment.Path = result.File.Path;
			attachment.MimeType = "audio/mp4";
			attachment.Name = result.File.Name;
			attachment.AudioDurationMs = result.DurationMs;
			return attachment;
		}
	}
}
