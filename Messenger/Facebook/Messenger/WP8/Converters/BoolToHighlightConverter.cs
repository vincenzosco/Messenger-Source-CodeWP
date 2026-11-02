using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Converters
{
	public class BoolToHighlightConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			if (!(value is bool) || !(bool)value)
			{
				return (object)new SolidColorBrush(Colors.Black);
			}
			return Application.Current.Resources[(object)"MessengerBlueBrush"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
