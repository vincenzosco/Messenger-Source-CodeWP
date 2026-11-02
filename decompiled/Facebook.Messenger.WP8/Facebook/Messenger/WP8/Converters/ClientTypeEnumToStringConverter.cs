using System;
using System.Globalization;
using System.Windows.Data;
using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Converters;

public class ClientTypeEnumToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int? num = (int?)value;
		if (num.HasValue)
		{
			int? num2 = num & 4;
			if (num2.GetValueOrDefault() != 0 || !num2.HasValue || (num & 8) != 0)
			{
				return StringLibrary.Client_Type_Mobile;
			}
			return StringLibrary.Client_Type_Web;
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
