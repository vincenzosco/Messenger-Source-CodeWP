using System;

namespace Facebook.Base
{
	public static class DateExtensions
	{
		public static DateTime FromUnixTimeMilliseconds(long unixTime)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTime);
		}

		public static bool IsDayOfYearBefore(DateTime first, DateTime second)
		{
			if (first.Month >= second.Month)
			{
				if (first.Month == second.Month)
				{
					return first.Day < second.Day;
				}
				return false;
			}
			return true;
		}

		public static bool IsDayOfYearAfter(DateTime first, DateTime second)
		{
			if (first.Month <= second.Month)
			{
				if (first.Month == second.Month)
				{
					return first.Day >= second.Day;
				}
				return false;
			}
			return true;
		}

		public static long ToUnixTimeMilliseconds(this DateTime date, bool convertToUtc = true)
		{
			DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			double totalMilliseconds = (convertToUtc ? date.ToUniversalTime() : date).Subtract(value).TotalMilliseconds;
			return Convert.ToInt64(totalMilliseconds);
		}

		public static DateTime FromUnixTimeSeconds(long unixTime)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
		}

		public static long ToUnixTimeSeconds(this DateTime date, bool convertToUtc = true)
		{
			DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			double totalSeconds = (convertToUtc ? date.ToUniversalTime() : date).Subtract(value).TotalSeconds;
			return Convert.ToInt64(totalSeconds);
		}

		public static long MSecsSinceEpoch()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
		}

		public static long SecsSinceEpoch()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		public static bool IsSameDayAs(this DateTime date, DateTime other)
		{
			if (date.Year == other.Year && date.Month == other.Month)
			{
				return date.Day == other.Day;
			}
			return false;
		}

		public static bool IsEmptyValue(this DateTime date)
		{
			return date == DateTime.MinValue;
		}

		public static double TotalMillisecondsSinceNow(this DateTime date)
		{
			return (DateTime.Now - date).TotalMilliseconds;
		}
	}
}
