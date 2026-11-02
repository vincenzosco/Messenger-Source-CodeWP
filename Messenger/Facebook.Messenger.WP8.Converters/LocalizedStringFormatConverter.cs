using System;
using System.Globalization;
using System.Windows.Data;
using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Converters
{
	public class LocalizedStringFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = string.Empty;
			if (value == null || parameter == null)
			{
				return result;
			}
			if (parameter is string)
			{
				string text = StringLibrary.ResourceManager.GetString(parameter as string, culture);
				if (text != null)
				{
					result = string.Format(text, value);
				}
			}
			else
			{
				result = value.ToString();
			}
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
