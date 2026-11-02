using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.FetchThreads
{
	[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
	internal class CacheFilter : Filter<Request, Response>
	{
		public CacheFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			long actionId = request.ActionId;
			long maxTimestamp = request.MaxTimestamp;
			string folder = request.Folder;
			Response cacheResponse = await QueryCache(actionId, maxTimestamp, folder, request.ThreadLimit);
			if (!request.RequestFromSyncProtocolService && maxTimestamp == 0)
			{
				cacheResponse.ReceivedThreadsFromWebOrSync = true;
				return cacheResponse;
			}
			CacheRequestStateUtil.RequestState cacheRequestState = GetCacheRequestState(request, cacheResponse);
			switch (cacheRequestState)
			{
			case CacheRequestStateUtil.RequestState.UseCache:
				return cacheResponse;
			case CacheRequestStateUtil.RequestState.UpdateFromWebUseCachedActionId:
				request.ActionId = cacheResponse.Counts.LargestSavedThreadActionId;
				break;
			default:
				throw new NotImplementedException("State not implemented");
			case CacheRequestStateUtil.RequestState.UpdateFromWeb:
			case CacheRequestStateUtil.RequestState.UpdateFromWebDoNotStoreResults:
				break;
			}
			Response nextResponse = await next.ExecuteAsync<Request, Response>(request);
			if (nextResponse.HasError())
			{
				return nextResponse;
			}
			if (cacheRequestState == CacheRequestStateUtil.RequestState.UpdateFromWebDoNotStoreResults && !nextResponse.ShouldPurgeThreadCache)
			{
				return nextResponse;
			}
			await UpdateCache(nextResponse, request.RequestFromSyncProtocolService);
			Response webUpdatedCacheResponse = await QueryCache(actionId, maxTimestamp, folder, request.ThreadLimit);
			webUpdatedCacheResponse.ReceivedThreadsFromWebOrSync = true;
			return webUpdatedCacheResponse;
		}

		private static CacheRequestStateUtil.RequestState GetCacheRequestState(Request request, Response cacheResponse)
		{
			MailFolderCount counts = cacheResponse.Counts;
			bool isCacheStale = counts == null || CacheRequestStateUtil.IsCacheStale(request.RecentPushActionId, request.ForceToServer, counts.LargestSavedThreadActionId, counts.LastActionId);
			if (!request.RequestFromSyncProtocolService)
			{
				return CacheRequestStateUtil.GetRequestState(request.ActionId, request.MaxTimestamp, request.RecentPushActionId, cacheResponse.Counts.LargestSavedThreadActionId, isCacheStale, cacheResponse.Threads);
			}
			return CacheRequestStateUtil.RequestState.UpdateFromWeb;
		}

		private async Task<Response> QueryCache(long actionId, long maxTimestamp, string folder, int limit)
		{
			ThreadCache cache = Context.ThreadCache;
			List<MailThread> list = ((actionId > 0) ? (await cache.GetThreadsByActionId(actionId)) : (await cache.GetThreadsByMaxTimestamp(maxTimestamp, limit)));
			List<MailThread> threads = list;
			MailFolderCount counts = await cache.GetThreadCounts(folder);
			List<string> failedIds = await cache.GetThreadsIdsWithFailedSends();
			return new Response
			{
				Threads = threads,
				Counts = counts,
				ThreadIdsWithFailedSentMessages = failedIds,
				ReceivedThreadsFromWebOrSync = false
			};
		}

		private async Task UpdateCache(Response response, bool requestFromSyncProtocolService)
		{
			ThreadCache cache = Context.ThreadCache;
			List<MailThread> threads = response.Threads;
			MailFolderCount counts = response.Counts;
			if (response.ShouldPurgeThreadCache || requestFromSyncProtocolService)
			{
				await cache.ClearAllData();
			}
			foreach (MailThread thread in threads)
			{
				thread.LastMessageReadUserIds.Clear();
				List<ReadReceipt> readReceipts = thread.ReadReceipts;
				Func<ReadReceipt, bool> predicate = (ReadReceipt rr) => rr.Timestamp >= thread.Timestamp;
				if (readReceipts.Any(predicate))
				{
					thread.LastMessageState = MailMessage.MessageState.Seen;
					thread.LastMessageReadUserIds = (from readReceipt in thread.ReadReceipts
						where thread.Timestamp <= readReceipt.Timestamp
						select readReceipt.Id).ToList();
				}
				else if (thread.DeliveryReceipts.Any((DeliveryReceipt d) => d.Timestamp >= thread.Timestamp))
				{
					thread.LastMessageState = MailMessage.MessageState.Delivered;
				}
				await cache.AddThread(thread);
			}
			if (counts != null)
			{
				await cache.SetThreadCounts(counts);
			}
			else
			{
				Logger.Error("Failed to fetch threads - Mail Folder Counts are null", "UpdateCache", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Methods\\FetchThreads\\CacheFilter.cs", 145);
			}
		}
	}
}
