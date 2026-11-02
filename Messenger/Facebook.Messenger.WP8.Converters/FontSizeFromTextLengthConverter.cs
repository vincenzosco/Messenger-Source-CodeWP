using System;
using System.Globalization;
using System.Windows.Data;

namespace Facebook.Messenger.WP8.Converters
{
	public class FontSizeFromTextLengthConverter : IValueConverter
	{
		private const double MaxFontSize = 50.0;

		private const double MinFontSize = 25.0;

		private const double FontSizeDecreasePercentage = 0.15;

		private const int TextLengthInterval = 20;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (text == null)
			{
				return 50.0;
			}
			double val = 50.0 * Math.Pow(0.85, text.Length / 20);
			val = Math.Max(val, 25.0);
			return val;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
