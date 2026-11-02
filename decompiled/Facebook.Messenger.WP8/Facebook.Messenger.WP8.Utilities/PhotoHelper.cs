using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Facebook.Messenger.WP8.Extensions;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class PhotoHelper
	{
		public static PhotoData PhotoDataFromPicture(Picture picture)
		{
			return PhotoDataFromStreamAndDispose(picture.SafeGetImageOrNull(), picture.Name);
		}

		public static PhotoData PhotoDataFromPhotoResult(PhotoResult result)
		{
			return PhotoDataFromStreamAndDispose(result.ChosenPhoto, result.OriginalFileName);
		}

		public static PhotoData PhotoDataFromStreamAndDispose(Stream stream, string originalFileName)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected Obj, but got Unknown
			if (stream == null)
			{
				return null;
			}
			using (stream)
			{
				BitmapImage val = new BitmapImage();
				((BitmapSource)val).SetSource(stream);
				PhotoData photoData = new PhotoData((BitmapSource)(object)val);
				if (IsJpeg(originalFileName))
				{
					if (stream.CanSeek && CanStreamSeekWithoutError(stream))
					{
						SetExifOrientation(photoData, stream);
					}
					else
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							stream.Position = 0L;
							stream.CopyTo(memoryStream);
							SetExifOrientation(photoData, memoryStream);
						}
					}
				}
				return photoData;
			}
		}

		private static void SetExifOrientation(PhotoData photoData, Stream stream)
		{
			stream.Position = 0L;
			ExifOrientation exifOrientation = stream.GetExifOrientation();
			photoData.SetExifOrientation(exifOrientation);
		}

		public static bool IsJpeg(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			string text = path.Split('.').LastOrDefault();
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (!text.Equals("jpg", StringComparison.InvariantCultureIgnoreCase))
			{
				return text.Equals("jpeg", StringComparison.InvariantCultureIgnoreCase);
			}
			return true;
		}

		public static bool CanStreamSeekWithoutError(Stream stream)
		{
			stream.Position = 0L;
			stream.Seek(1L, SeekOrigin.Current);
			return stream.Position == 1;
		}
	}
}
