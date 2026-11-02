using System;
using System.IO;
using Facebook.Base.Logging;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;

namespace Facebook.Messenger.WP8.Extensions
{
	public static class PictureExtensions
	{
		public static Stream SafeGetImageOrNull(this Picture picture)
		{
			try
			{
				return picture.GetImage();
			}
			catch (InvalidOperationException)
			{
				Logger.Warning(string.Format("Failed to GetImage from Picture: {0}", picture.Name), "SafeGetImageOrNull", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\PictureExtensions.cs", 20);
				return null;
			}
		}

		public static Stream SafeGetPreviewImageOrNull(this Picture picture)
		{
			try
			{
				return MediaLibraryExtensions.GetPreviewImage(picture);
			}
			catch (InvalidOperationException)
			{
				Logger.Warning(string.Format("Failed to GetPreviewImage from Picture: {0}", picture.Name), "SafeGetPreviewImageOrNull", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\PictureExtensions.cs", 35);
				return null;
			}
		}
	}
}
