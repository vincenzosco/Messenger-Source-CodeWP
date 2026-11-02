using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ImageTools.Controls
{
	public class ImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object unsetValue = DependencyProperty.UnsetValue;
			if (value != null)
			{
				Uri uri = value as Uri;
				if (uri != null)
				{
					ExtendedImage extendedImage = new ExtendedImage();
					extendedImage.UriSource = uri;
					return extendedImage;
				}
				string text = value as string;
				if (text != null)
				{
					ExtendedImage extendedImage2 = new ExtendedImage();
					extendedImage2.UriSource = new Uri(text, UriKind.RelativeOrAbsolute);
					return extendedImage2;
				}
				Stream stream = value as Stream;
				if (stream != null)
				{
					ExtendedImage extendedImage3 = new ExtendedImage();
					extendedImage3.SetSource(stream);
					return extendedImage3;
				}
				WriteableBitmap val = (WriteableBitmap)((value is WriteableBitmap) ? value : null);
				if (val != null)
				{
					return val.ToImage();
				}
				ExtendedImage extendedImage4 = value as ExtendedImage;
				if (extendedImage4 != null)
				{
					return extendedImage4;
				}
			}
			return unsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
