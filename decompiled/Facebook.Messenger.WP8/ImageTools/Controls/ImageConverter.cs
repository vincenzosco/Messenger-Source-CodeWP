using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ImageTools.Controls;

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
			if (value is string uriString)
			{
				ExtendedImage extendedImage2 = new ExtendedImage();
				extendedImage2.UriSource = new Uri(uriString, UriKind.RelativeOrAbsolute);
				return extendedImage2;
			}
			if (value is Stream source)
			{
				ExtendedImage extendedImage3 = new ExtendedImage();
				extendedImage3.SetSource(source);
				return extendedImage3;
			}
			WriteableBitmap val = (WriteableBitmap)((value is WriteableBitmap) ? value : null);
			if (val != null)
			{
				return val.ToImage();
			}
			if (value is ExtendedImage result)
			{
				return result;
			}
		}
		return unsetValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
