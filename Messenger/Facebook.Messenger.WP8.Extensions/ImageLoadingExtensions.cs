using System;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;

namespace Facebook.Messenger.WP8.Extensions
{
	public static class ImageLoadingExtensions
	{
		public static PhotoData PhotoDataFromPhotoResult(this FacebookMessengerViewModel viewModel, PhotoResult result)
		{
			if (result == null)
			{
				return null;
			}
			PhotoData result2 = null;
			try
			{
				result2 = PhotoHelper.PhotoDataFromPhotoResult(result);
			}
			catch (OutOfMemoryException ex)
			{
				Logger.Error(ex.ToString(), "PhotoDataFromPhotoResult", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImageLoadingExtensions.cs", 33);
				viewModel.OnError(new Error
				{
					Message = StringLibrary.Error_Gallery_Out_Of_Memory
				});
			}
			return result2;
		}

		public static PhotoData PhotoDataFromPicture(this FacebookMessengerViewModel viewModel, Picture picture)
		{
			if (picture == (Picture)null)
			{
				return null;
			}
			PhotoData result = null;
			try
			{
				result = PhotoHelper.PhotoDataFromPicture(picture);
			}
			catch (OutOfMemoryException ex)
			{
				Logger.Error(ex.ToString(), "PhotoDataFromPicture", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImageLoadingExtensions.cs", 55);
				viewModel.OnError(new Error
				{
					Message = StringLibrary.Error_Gallery_Out_Of_Memory
				});
			}
			return result;
		}

		public static async Task<PhotoAttachmentHelper.PhotoAttachmentInfo> PrepareImageInfoAsync(this FacebookMessengerViewModel viewModel, PhotoData image, string filename = null)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				return await PhotoAttachmentHelper.PrepareImage(image, filename);
			}
			catch (Exception ex)
			{
				string message;
				if (ex is OutOfMemoryException)
				{
					message = StringLibrary.Error_Gallery_Out_Of_Memory;
				}
				else
				{
					if (!(ex is FileNotFoundException) && !(ex is UnauthorizedAccessException) && !(ex is ArgumentException))
					{
						throw;
					}
					message = StringLibrary.Error_Cannot_Attach_File;
				}
				Logger.Error(string.Format("Unable to create resized photo from file: {0}\n{1}", filename, ex), "PrepareImageInfoAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImageLoadingExtensions.cs", 88);
				viewModel.OnError(new Error
				{
					Message = message
				});
				return null;
			}
		}

		public static async Task<Uri> PrepareImageUriAsync(this FacebookMessengerViewModel viewModel, PhotoData image, string filename = null)
		{
			PhotoAttachmentHelper.PhotoAttachmentInfo info = await viewModel.PrepareImageInfoAsync(image, filename);
			if (info == null)
			{
				return null;
			}
			Uri uri;
			Uri.TryCreate(info.ResizedPath, UriKind.Absolute, out uri);
			return uri;
		}
	}
}
