using System;
using System.Globalization;
using System.Windows.Data;

namespace Facebook.Messenger.WP8.Converters
{
	public class VisibilityToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool reverse = BoolToVisibilityConverter.IsReversed(parameter);
			return BoolToVisibilityConverter.ConvertBack(value, reverse);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			bool reverse = BoolToVisibilityConverter.IsReversed(parameter);
			return BoolToVisibilityConverter.Convert(value, reverse);
		}
	}
}
