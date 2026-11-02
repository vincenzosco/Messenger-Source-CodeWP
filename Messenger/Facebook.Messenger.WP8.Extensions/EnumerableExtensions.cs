using System;
using System.Collections.Generic;
using System.Linq;

namespace Facebook.Messenger.WP8.Extensions
{
	public static class EnumerableExtensions
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable != null)
			{
				return !enumerable.Any();
			}
			return true;
		}

		public static bool NotNullAndAny<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			if (enumerable != null)
			{
				return enumerable.Any(predicate);
			}
			return false;
		}
	}
}
