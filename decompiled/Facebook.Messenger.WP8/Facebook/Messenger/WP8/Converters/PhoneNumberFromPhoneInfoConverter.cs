using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Converters;

public class PhoneNumberFromPhoneInfoConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is List<PhoneInfo> source) || !source.Any())
		{
			return string.Empty;
		}
		PhoneInfo phoneInfo = source.First();
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
