using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Facebook.Messenger.WP8.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return Convert(value, IsReversed(parameter));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ConvertBack(value, IsReversed(parameter));
		}

		public static bool IsReversed(object parameter)
		{
			if (parameter != null && parameter is string && (string)parameter == "1")
			{
				return true;
			}
			return false;
		}

		public static Visibility Convert(object value, bool reverse = false)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			Visibility val = Visibility.Collapsed;
			if (value == null)
			{
				return val;
			}
			if (value is bool)
			{
				val = (!(bool)value ? Visibility.Collapsed : Visibility.Visible);
			}
			else if (value is int)
			{
				val = (((int)value <= 0) ? Visibility.Collapsed : Visibility.Visible);
			}
			if (reverse)
			{
				val = ((int)val == 0 ? Visibility.Collapsed : Visibility.Visible);
			}
			return val;
		}

		public static bool ConvertBack(object value, bool reverse = false)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Visibility? val = value as Visibility?;
			bool flag = false;
			if (val.HasValue && (int)val.Value == 0)
			{
				flag = true;
			}
			if (reverse)
			{
				flag = !flag;
			}
			return flag;
		}
	}
}
