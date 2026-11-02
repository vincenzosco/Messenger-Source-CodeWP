using System;
using System.Collections.Generic;
using System.IO;
using Facebook.Base.Logging;

namespace Facebook.Base
{
	public static class FileUtil
	{
		public enum FileType
		{
			Unknown,
			Image,
			Audio,
			Video,
			File,
			AnimatedImage
		}

		public const string MimeTypeAudioMp4 = "audio/mp4";

		private const string MimeTypePrefixImage = "image/";

		private const string MimeTypePrefixAudio = "audio/";

		private const string MimeTypePrefixVideo = "video/";

		private static readonly IReadOnlyDictionary<string, string> ImageMimetypes = new Dictionary<string, string>
		{
			{ ".jpg", "image/jpeg" },
			{ ".jpeg", "image/jpeg" },
			{ ".bmp", "image/bmp" },
			{ ".png", "image/png" },
			{ ".gif", "image/gif" },
			{ ".tiff", "image/tiff" },
			{ ".tif", "image/tiff" }
		};

		private static readonly IReadOnlyDictionary<string, string> VideoMimeTypes = new Dictionary<string, string>
		{
			{ ".flv", "video/x-flv" },
			{ ".mp4", "video/mp4" },
			{ ".3gp", "video/3gpp" },
			{ ".avi", "video/x-msvideo" },
			{ ".wmv", "video/x-ms-wmv" }
		};

		public static string ConvertUrlToPath(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return string.Empty;
			}
			Uri result;
			if (!Uri.TryCreate(url, UriKind.Absolute, out result))
			{
				return string.Empty;
			}
			return result.LocalPath;
		}

		public static string ConvertPathToUrl(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return string.Empty;
			}
			return $"file:///{path}".Replace('\\', '/');
		}

		public static string GetName(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return string.Empty;
			}
			return Path.GetFileName(path);
		}

		private static string GetMimeTypeFromFilePath(string filePath, string defaultMimeType, IReadOnlyDictionary<string, string> mimeTypes)
		{
			string text = Path.GetExtension(filePath).ToLowerInvariant();
			if (string.IsNullOrEmpty(text) || !mimeTypes.ContainsKey(text))
			{
				Logger.Warning("File mime type not recognized: " + filePath, "GetMimeTypeFromFilePath", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\FileUtil.cs", 78);
				return defaultMimeType;
			}
			return mimeTypes[text];
		}

		public static string GetImageMimeTypeFromFilePath(string filePath)
		{
			return GetMimeTypeFromFilePath(filePath, "image/jpeg", ImageMimetypes);
		}

		public static string GetVideoMimeTypeFromFilePath(string filePath)
		{
			return GetMimeTypeFromFilePath(filePath, "video/mp4", VideoMimeTypes);
		}

		public static FileType GetFileTypeFromMime(string mime)
		{
			if (mime == null)
			{
				return FileType.Unknown;
			}
			if (mime.StartsWith("image/"))
			{
				return FileType.Image;
			}
			if (mime.StartsWith("audio/"))
			{
				return FileType.Audio;
			}
			if (mime.StartsWith("video/"))
			{
				return FileType.Video;
			}
			return FileType.Unknown;
		}

		public static FileType GetFileTypeFromAttachmentType(string type)
		{
			switch (type)
			{
			case "MessageAudio":
				return FileType.Audio;
			case "MessageImage":
				return FileType.Image;
			case "MessageVideo":
				return FileType.Video;
			case "MessageFile":
				return FileType.File;
			case "MessageAnimatedImage":
				return FileType.AnimatedImage;
			default:
				return FileType.Unknown;
			}
		}
	}
}
