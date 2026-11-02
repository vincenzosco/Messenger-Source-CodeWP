using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Base.Logging;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Microsoft.Xna.Framework.Media;

namespace Facebook.Messenger.WP8.ViewModels;

public class GalleryImageDrawerItemViewModel : ImageDrawerItemViewModel
{
	private readonly Picture _picture;

	private ExifOrientation? _exifOrientation;

	public GalleryImageDrawerItemViewModel(Picture picture)
	{
		_picture = picture;
	}

	public override string GetName()
	{
		return _picture.Name;
	}

	public override PhotoData GetImageToSend()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected Obj, but got Unknown
		using Stream stream = _picture.SafeGetImageOrNull();
		if (stream == null)
		{
			return null;
		}
		BitmapImage val = new BitmapImage();
		((BitmapSource)val).SetSource(stream);
		PhotoData photoData = new PhotoData((BitmapSource)(object)val);
		photoData.SetExifOrientation(GetExifOrientation());
		return photoData;
	}

	public override ImageSource GetDisplayImage(int width, int height)
	{
		return ResizeImageUniformToFill(_picture, width, height);
	}

	private ExifOrientation GetExifOrientation()
	{
		if (_exifOrientation.HasValue)
		{
			return _exifOrientation.Value;
		}
		ExifOrientation value = ExifOrientation.NotFound;
		if (PhotoHelper.IsJpeg(_picture.Name))
		{
			using Stream stream = _picture.SafeGetImageOrNull();
			if (stream != null)
			{
				value = stream.GetExifOrientation();
			}
		}
		ExifOrientation? exifOrientation = (_exifOrientation = value);
		return exifOrientation.Value;
	}

	private ImageSource ResizeImageUniformToFill(Picture picture, int w, int h)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		BitmapImage val = new BitmapImage();
		int decodePixelWidth = w;
		int decodePixelHeight = h;
		int width = picture.Width;
		int height = picture.Height;
		if (width > 0 && height > 0)
		{
			if (width > height)
			{
				decodePixelWidth = width * h / height;
			}
			else
			{
				decodePixelHeight = height * w / width;
			}
		}
		val.DecodePixelWidth = decodePixelWidth;
		val.DecodePixelHeight = decodePixelHeight;
		Stream stream = picture.SafeGetPreviewImageOrNull();
		if (stream == null)
		{
			return null;
		}
		try
		{
			((BitmapSource)val).SetSource(stream);
			return (ImageSource)(object)val;
		}
		catch (Exception ex)
		{
			if (!ex.Message.Contains("The component cannot be found"))
			{
				throw;
			}
			Logger.Warning($"Failed to set bitmap source from Picture: {picture.Name}, height: {height}, width: {width}", "ResizeImageUniformToFill", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\ImageDrawerItemViewModel.cs", 165);
			return null;
		}
	}

	public override RotationAngle GetRotationAngle()
	{
		ExifOrientation exifOrientation = GetExifOrientation();
		return PhotoData.GetRotationAngleFromExifOrientation(exifOrientation);
	}
}
