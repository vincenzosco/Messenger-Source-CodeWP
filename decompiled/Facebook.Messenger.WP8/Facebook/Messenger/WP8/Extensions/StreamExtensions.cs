using System.IO;
using ExifLib;

namespace Facebook.Messenger.WP8.Extensions
{
	public static class StreamExtensions
	{
		public static ExifOrientation GetExifOrientation(this Stream stream)
		{
			ExifOrientation result = ExifOrientation.NotFound;
			try
			{
				using (ExifReader exifReader = new ExifReader(stream))
				{
					exifReader.GetTagValue<ExifOrientation>(ExifTags.Orientation, out result);
				}
			}
			catch (ExifLibException)
			{
			}
			return result;
		}
	}
}
