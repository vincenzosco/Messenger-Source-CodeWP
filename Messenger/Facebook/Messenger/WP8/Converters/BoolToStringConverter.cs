using System;
using System.Globalization;
using System.Windows.Data;
using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Converters
{
	public class BoolToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = StringLibrary.Global_Notification_On;
			string text2 = StringLibrary.Global_Notification_Off;
			if (parameter != null)
			{
				string text3 = parameter as string;
				if (!string.IsNullOrEmpty(text3) && text3.Contains("|"))
				{
					string[] array = text3.Split('|');
					text = array[0];
					text2 = array[1];
				}
			}
			if ((bool)value)
			{
				return StringLibrary.ResourceManager.GetString(text) ?? text;
			}
			return StringLibrary.ResourceManager.GetString(text2) ?? text2;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
