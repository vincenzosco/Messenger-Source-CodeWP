using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Converters
{
	public class PhoneNumberFromPhoneInfoConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<PhoneInfo> list = value as List<PhoneInfo>;
			if (list == null || !list.Any())
			{
				return string.Empty;
			}
			PhoneInfo phoneInfo = list.First();
			if (phoneInfo != null)
			{
				return phoneInfo.Number;
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
