using System;
using System.Globalization;
using System.Windows.Data;

namespace Facebook.Messenger.WP8.Converters
{
	public class InverseBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value is bool && (bool)value;
			return !flag;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
