using System;
using System.Globalization;
using System.Windows.Data;

namespace Facebook.Messenger.WP8.Converters
{
	public class StringLengthToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			string value2 = value as string;
			bool flag = !string.IsNullOrEmpty(value2);
			if (parameter is string && "1".Equals(parameter))
			{
				flag = !flag;
			}
			return VisibilityUtil.FromBool(flag);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
