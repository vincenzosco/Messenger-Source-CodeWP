using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.SendMessage
{
	public class Attachment
	{
		public string ThumbnailPath { get; set; }

		public string Path { get; set; }

		public string Name { get; set; }

		public string MimeType { get; set; }

		public long AttachmentSizeBytes { get; set; }

		public int Length { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public int Rotation { get; set; }

		public long AudioDurationMs { get; set; }

		public MediaSource Source { get; set; }

		public bool IsPhoto => FileUtil.GetFileTypeFromMime(MimeType) == FileUtil.FileType.Image;

		public bool IsVideo => FileUtil.GetFileTypeFromMime(MimeType) == FileUtil.FileType.Video;

		public bool IsAudio => FileUtil.GetFileTypeFromMime(MimeType) == FileUtil.FileType.Audio;

		public bool HasThumbnail => !string.IsNullOrEmpty(ThumbnailPath);
	}
}
