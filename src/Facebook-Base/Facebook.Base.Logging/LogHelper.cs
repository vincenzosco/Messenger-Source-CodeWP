using System;

namespace Facebook.Base.Logging
{
	public static class LogHelper
	{
		private const string FormatWithType = "[{0:yyyy-MM-dd HH\\:mm\\:ss\\:ffff}] [{1}] [{2}::{3}:{4}] --- {5}";

		private const string FormatWithoutType = "[{0:yyyy-MM-dd HH\\:mm\\:ss\\:ffff}] [{1}] [{2}:{3}] --- {4}";

		public static string CreateEntryWithType(DateTime date, LogLevel level, string className, string member, int line, string message)
		{
			return string.Format("[{0:yyyy-MM-dd HH\\:mm\\:ss\\:ffff}] [{1}] [{2}::{3}:{4}] --- {5}", date, level, className, member, line, message);
		}

		public static string CreateEntryWithoutType(DateTime date, LogLevel level, string member, int line, string message)
		{
			return string.Format("[{0:yyyy-MM-dd HH\\:mm\\:ss\\:ffff}] [{1}] [{2}:{3}] --- {4}", date, level, member, line, message);
		}
	}
}
