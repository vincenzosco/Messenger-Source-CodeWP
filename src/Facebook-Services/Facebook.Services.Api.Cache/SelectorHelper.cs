using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Facebook.Base.Logging;

namespace Facebook.Services.Api.Cache
{
	public static class SelectorHelper
	{
		public static T SingleOrDefaultLogErrorIfNotSingle<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, [CallerMemberName] string caller = "unknown")
		{
			List<T> source = enumerable.Where(predicate).ToList();
			if (Enumerable.Count(source) > 1)
			{
				Logger.Error(string.Format("Found more than element of type {0} matching predicate from caller {1}", new object[2]
				{
					typeof(T),
					caller
				}), "SingleOrDefaultLogErrorIfNotSingle", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\SelectorHelper.cs", 16);
			}
			return source.FirstOrDefault();
		}
	}
}
