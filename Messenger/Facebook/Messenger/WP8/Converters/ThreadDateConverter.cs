using System;
using System.Globalization;
using System.Windows.Data;
using Facebook.Base;

namespace Facebook.Messenger.WP8.Converters
{
	public class ThreadDateConverter : IValueConverter
	{
		private struct DateFormatter
		{
			private static readonly string TimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("tt", "t");

			public string DateFormat { get; set; }

			public bool ShowTime { private get; set; }

			public string FormatOffset(DateTime time)
			{
				DateTimeOffset dateTimeOffset = new DateTimeOffset(time.ToLocalTime());
				string text = (ShowTime ? dateTimeOffset.ToString(TimeFormat) : string.Empty);
				text = text.ToLower(CultureInfo.CurrentCulture).Replace(" ", string.Empty);
				if (!string.IsNullOrEmpty(DateFormat))
				{
					return string.Join(" ", dateTimeOffset.ToString(DateFormat), text);
				}
				return text;
			}
		}

		private static readonly DateFormatter DateFormatter_Time = new DateFormatter
		{
			DateFormat = null,
			ShowTime = true
		};

		private static readonly DateFormatter DateFormatter_DayOfWeek = new DateFormatter
		{
			DateFormat = "ddd",
			ShowTime = false
		};

		private static readonly DateFormatter DateFormatter_DayOfWeek_Time = new DateFormatter
		{
			DateFormat = DateFormatter_DayOfWeek.DateFormat,
			ShowTime = true
		};

		private static readonly DateFormatter DateFormatter_Month_Day = new DateFormatter
		{
			DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern.Replace("MMMM", "MMM"),
			ShowTime = false
		};

		private static readonly DateFormatter DateFormatter_Month_Day_Time = new DateFormatter
		{
			DateFormat = DateFormatter_Month_Day.DateFormat,
			ShowTime = true
		};

		private static readonly DateFormatter DateFormatter_Year_Month_Day = new DateFormatter
		{
			DateFormat = "d",
			ShowTime = false
		};

		private static readonly DateFormatter DateFormatter_Year_Month_Day_Time = new DateFormatter
		{
			DateFormat = DateFormatter_Year_Month_Day.DateFormat,
			ShowTime = true
		};

		public static string FormatDateAsString(long timestamp)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime time = DateExtensions.FromUnixTimeMilliseconds(timestamp);
			DateTime date = utcNow.Date;
			DateTime date2 = time.Date;
			int day = date.Day;
			int day2 = date2.Day;
			int month = date.Month;
			int month2 = date2.Month;
			int year = date.Year;
			int year2 = date2.Year;
			long num = utcNow.ToUnixTimeMilliseconds() - timestamp;
			DateFormatter dateFormatter;
			if (num < 86400000)
			{
				dateFormatter = DateFormatter_Time;
			}
			else if (((day <= day2 + 3) & (month == month2)) && year == year2)
			{
				dateFormatter = DateFormatter_DayOfWeek;
			}
			else if (year > year2)
			{
				int num2 = month + 12 - month2;
				dateFormatter = ((year - year2 != 1 || num2 > 6) ? DateFormatter_Year_Month_Day : DateFormatter_Month_Day);
			}
			else
			{
				dateFormatter = DateFormatter_Month_Day;
			}
			return dateFormatter.FormatOffset(time);
		}

		public static string FormatMessageDateAsString(long timestamp)
		{
			DateTime now = DateTime.Now;
			DateTime time = DateExtensions.FromUnixTimeMilliseconds(timestamp).ToLocalTime();
			DateTime date = now.Date;
			DateTime date2 = time.Date;
			int day = date.Day;
			int day2 = date2.Day;
			int month = date.Month;
			int month2 = date2.Month;
			int year = date.Year;
			int year2 = date2.Year;
			long num = now.ToUnixTimeMilliseconds() - timestamp;
			DateFormatter dateFormatter;
			if (num < 86400000)
			{
				dateFormatter = ((day != day2) ? DateFormatter_DayOfWeek_Time : DateFormatter_Time);
			}
			else if (day <= day2 + 3 && month == month2 && year == year2)
			{
				dateFormatter = DateFormatter_DayOfWeek_Time;
			}
			else if (year > year2)
			{
				int num2 = month + 12 - month2;
				dateFormatter = ((year - year2 != 1 || num2 > 6) ? DateFormatter_Year_Month_Day_Time : DateFormatter_Month_Day_Time);
			}
			else
			{
				dateFormatter = DateFormatter_Month_Day_Time;
			}
			return dateFormatter.FormatOffset(time);
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			long timestamp = System.Convert.ToInt64(value);
			return FormatDateAsString(timestamp);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
