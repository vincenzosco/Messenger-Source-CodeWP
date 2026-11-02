using System;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class MailAttachment
	{
		[JsonObject(MemberSerialization.OptIn)]
		[DataContract]
		public class ImageData
		{
			[DataMember]
			[JsonProperty("width")]
			public int Width { get; set; }

			[JsonProperty("height")]
			[DataMember]
			public int Height { get; set; }

			[DataMember]
			[JsonProperty("url")]
			public string Url { get; set; }

			[DataMember]
			[JsonProperty("preview_url")]
			public string PreviewUrl { get; set; }
		}

		[DataContract]
		[JsonObject(MemberSerialization.OptIn)]
		public class VideoData
		{
			public enum VideoSource
			{
				NonQuickCam = 1,
				QuickCam
			}

			[DataMember]
			[JsonProperty("width")]
			public int Width { get; set; }

			[JsonProperty("height")]
			[DataMember]
			public int Height { get; set; }

			[DataMember]
			[JsonProperty("length")]
			public int Length { get; set; }

			[DataMember]
			[JsonProperty("video_type")]
			public VideoSource Source { get; set; }

			[DataMember]
			[JsonProperty("url")]
			public string Url { get; set; }

			[JsonProperty("preview_url")]
			[DataMember]
			public string PreviewUrl { get; set; }

			[JsonProperty("rotation")]
			[DataMember]
			public int Rotation { get; set; }
		}

		private const string AudioClipPrefix = "audioclip-";

		private const string AudioClipNameFormat = "audioclip-{0}-{1}.mp4";

		[DataMember]
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("fbid")]
		[DataMember]
		public long Fbid { get; set; }

		[JsonProperty("local_type")]
		[DataMember]
		public FileUtil.FileType Type { get; set; }

		[JsonProperty("mime_type")]
		[DataMember]
		public string MimeType { get; set; }

		[DataMember]
		[JsonProperty("filename")]
		public string FileName { get; set; }

		[JsonProperty("image_data")]
		[DataMember]
		public ImageData Image { get; set; }

		[JsonProperty("video_data")]
		[DataMember]
		public VideoData Video { get; set; }

		[JsonProperty("file_size")]
		[DataMember]
		public long FileSize { get; set; }

		[JsonProperty("local_url")]
		[DataMember]
		public string LocalUrl { get; set; }

		[JsonProperty("local_thumbnail_url")]
		[DataMember]
		public string LocalThumbnailUrl { get; set; }

		public bool IsGenericFile()
		{
			if (!IsImage() && !IsAudioClip())
			{
				return !IsVideo();
			}
			return false;
		}

		private FileUtil.FileType GetFileType()
		{
			if (Type == FileUtil.FileType.Unknown)
			{
				return FileUtil.GetFileTypeFromMime(MimeType);
			}
			return Type;
		}

		public bool IsImage()
		{
			if (GetFileType() == FileUtil.FileType.Image)
			{
				return Image != null;
			}
			return false;
		}

		public bool IsVideo()
		{
			if (GetFileType() == FileUtil.FileType.Video)
			{
				return Video != null;
			}
			return false;
		}

		public bool HasValidImageData()
		{
			if (IsImage() && Image.Height != 0)
			{
				return Image.Width != 0;
			}
			return false;
		}

		public bool HasValidVideoData()
		{
			if (IsVideo() && Video.Width != 0)
			{
				return Video.Height != 0;
			}
			return false;
		}

		public bool IsAudioClip()
		{
			if (GetFileType() == FileUtil.FileType.Audio)
			{
				return FileName.StartsWith("audioclip-");
			}
			return false;
		}

		public TimeSpan GetAudioClipDuration()
		{
			string[] array = FileName.Split('-', '.');
			if (array.Length < 2)
			{
				return TimeSpan.Zero;
			}
			string s = array[array.Length - 2];
			int result;
			if (!int.TryParse(s, out result))
			{
				return TimeSpan.Zero;
			}
			return TimeSpan.FromMilliseconds(result);
		}

		public static string GenerateVoiceClipFileName(long timestamp, long duration)
		{
			return string.Format("audioclip-{0}-{1}.mp4", new object[2] { timestamp, duration });
		}
	}
}
