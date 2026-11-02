using System;
using System.Globalization;
using System.Windows.Data;
using Facebook.Base;

namespace Facebook.Messenger.WP8.Converters
{
	public class MessageSentTimeTimestampConverter : IValueConverter
	{
		private const string TimestampFormat = "f";

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			long unixTime = System.Convert.ToInt64(value);
			return DateExtensions.FromUnixTimeMilliseconds(unixTime).ToLocalTime().ToString("f");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
