using System;
using System.Globalization;
using System.Windows.Data;

namespace Facebook.Messenger.WP8.Converters
{
	public class UniqueUriConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Uri uri = value as Uri;
			if (uri != null)
			{
				string text = uri.ToString();
				if (!string.IsNullOrWhiteSpace(text))
				{
					text += string.Format("{0}&ticks={1}", (!text.Contains("?")) ? "?" : string.Empty, DateTime.Now.Ticks);
					Uri result;
					if (Uri.TryCreate(text, uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative, out result))
					{
						return result;
					}
				}
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
