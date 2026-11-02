using System.Collections.Generic;
using System.Linq;
using Facebook.Base.Logging;

namespace Facebook.Services.Api.Cache
{
	public class CacheRequestStateUtil
	{
		public enum RequestState
		{
			UseCache,
			UpdateFromWeb,
			UpdateFromWebUseCachedActionId,
			UpdateFromWebDoNotStoreResults
		}

		public static RequestState GetRequestState(long requestActionId, long requestMaxTimestamp, long requestRecentPushActionId, long maxSavedActionId, bool isCacheStale, IEnumerable<object> cachedData)
		{
			if (cachedData == null)
			{
				return RequestState.UpdateFromWebDoNotStoreResults;
			}
			if (requestActionId > 0)
			{
				if (!isCacheStale)
				{
					return RequestState.UseCache;
				}
				if (maxSavedActionId < requestActionId)
				{
					Logger.Warning(string.Format("Threads database is out of sync with UI: {0}, db: {1}", new object[2] { requestActionId, maxSavedActionId }), "GetRequestState", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\CacheRequestStateUtil.cs", 48);
					return RequestState.UpdateFromWebDoNotStoreResults;
				}
				return RequestState.UpdateFromWebUseCachedActionId;
			}
			if (!cachedData.Any())
			{
				return RequestState.UpdateFromWeb;
			}
			bool flag = requestMaxTimestamp <= 0;
			if (isCacheStale && flag)
			{
				return RequestState.UpdateFromWebUseCachedActionId;
			}
			return RequestState.UseCache;
		}

		public static bool IsCacheStale(long requestRecentPushActionId, bool requestForceToServer, long largestSavedActionId, long lastKnownActionId)
		{
			if (largestSavedActionId <= 0 || lastKnownActionId <= 0 || requestForceToServer)
			{
				return true;
			}
			bool flag = requestRecentPushActionId > 0;
			bool result = flag && requestRecentPushActionId > lastKnownActionId;
			bool flag2 = flag && requestRecentPushActionId > largestSavedActionId;
			if (lastKnownActionId <= largestSavedActionId && !flag2)
			{
				return result;
			}
			return true;
		}
	}
}
