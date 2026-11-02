using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Facebook.Base.Logging;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;
using Windows.Storage;

namespace Facebook.Messenger.WP8.Utilities
{
	public class PhotoAttachmentHelper
	{
		public class PhotoAttachmentInfo
		{
			public readonly string ResizedPath;

			public readonly int ResizedWidth;

			public readonly int ResizedHeight;

			public readonly long ResizedFilesizeBytes;

			public readonly string ThumbnailPath;

			public PhotoAttachmentInfo(string resizedPath, int resizedWidth, int resizedHeight, long resizedFilesizeBytes, string thumbnailPath)
			{
				ResizedPath = resizedPath;
				ResizedWidth = resizedWidth;
				ResizedHeight = resizedHeight;
				ResizedFilesizeBytes = resizedFilesizeBytes;
				ThumbnailPath = thumbnailPath;
			}
		}

		private const int OutputQuality = 85;

		private const int PhotoMaxWidthHeight = 960;

		private const string FilenameResizedPrefix = "Resized_";

		public static async Task<PhotoAttachmentInfo> PrepareImage(PhotoData photoData, string filename = null)
		{
			if (photoData == null || photoData.Source == null)
			{
				return null;
			}
			filename = EnsureFilename(filename);
			try
			{
				WriteableBitmap writeableBitmap = new WriteableBitmap(photoData.Source);
				WriteableBitmap imageToSend = ResizeForSend(writeableBitmap);
				RotationAngle angle = photoData.GetRotationAngle();
				imageToSend = RotateForSend(imageToSend, angle);
				FileInfo resizedInfo = await SaveAsJpeg(imageToSend, "Resized_" + filename);
				return new PhotoAttachmentInfo(resizedInfo.FullName, ((BitmapSource)imageToSend).PixelWidth, ((BitmapSource)imageToSend).PixelHeight, resizedInfo.Length, null);
			}
			catch (OutOfMemoryException ex)
			{
				Logger.Error($"Caught OutOfMemoryException while preparing image: {ex.StackTrace}", "PrepareImage", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\PhotoAttachmentHelper.cs", 59);
				return null;
			}
		}

		private static string EnsureFilename(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				filename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			}
			return Path.ChangeExtension(filename, "jpeg");
		}

		private static WriteableBitmap ResizeForSend(WriteableBitmap source)
		{
			double num = Math.Min(960.0 / (double)((BitmapSource)source).PixelWidth, 960.0 / (double)((BitmapSource)source).PixelHeight);
			if (num < 1.0)
			{
				int width = (int)((double)((BitmapSource)source).PixelWidth * num);
				int height = (int)((double)((BitmapSource)source).PixelHeight * num);
				source = source.Resize(width, height, WriteableBitmapExtensions.Interpolation.Bilinear);
			}
			return source;
		}

		private static WriteableBitmap RotateForSend(WriteableBitmap source, RotationAngle angle)
		{
			if (angle != RotationAngle.Portrait)
			{
				source = source.Rotate(angle.ToDegrees());
			}
			return source;
		}

		private static async Task<FileInfo> SaveAsJpeg(WriteableBitmap source, string outputName)
		{
			FacebookContext context = ServiceLocator.Current.GetInstance<FacebookContext>();
			StorageFile outputFile = await (await StorageFolder.GetFolderFromPathAsync(await context.Settings.GetUserFolderPath())).CreateFileAsync(outputName, (CreationCollisionOption)1);
			using (Stream stream = await ((IStorageFile)(object)outputFile).OpenStreamForWriteAsync())
			{
				source.SaveJpeg(stream, ((BitmapSource)source).PixelWidth, ((BitmapSource)source).PixelHeight, 0, 85);
				return new FileInfo(outputFile.Path);
			}
		}
	}
}
