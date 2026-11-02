using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Converters
{
	public class AlternateColorConverter : IValueConverter
	{
		public SolidColorBrush DefaultColor { get; set; }

		public SolidColorBrush AltColor { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool) || !(bool)value)
			{
				return DefaultColor;
			}
			return AltColor;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
