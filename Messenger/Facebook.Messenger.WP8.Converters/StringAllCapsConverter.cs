using System;
using System.Globalization;
using System.Windows.Data;

namespace Facebook.Messenger.WP8.Converters
{
	public class StringAllCapsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			return text.ToUpper();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
