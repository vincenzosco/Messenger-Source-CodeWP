using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.ThreadQuery;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
internal class CacheFilter : Filter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		string threadId = request.ThreadFbId;
		long actionId = request.ActionId;
		long maxTimestamp = request.MaxTimestamp;
		Response cacheResponse = await QueryCache(threadId, actionId, maxTimestamp);
		ActionIdResult actionIds = await Context.ThreadCache.GetActionIdsForThreadByThreadId(threadId);
		CacheRequestStateUtil.RequestState cacheRequestState = CacheRequestStateUtil.GetRequestState(isCacheStale: actionIds.HasStaleMessages || CacheRequestStateUtil.IsCacheStale(request.RecentPushActionId, request.ForceToServer, actionIds.MaxMessageActionId, actionIds.ThreadLastAddActionId), requestActionId: actionId, requestMaxTimestamp: maxTimestamp, requestRecentPushActionId: request.RecentPushActionId, maxSavedActionId: actionIds.MaxMessageActionId, cachedData: cacheResponse.Messages);
		if (cacheResponse.Messages != null && cacheResponse.Messages.Any())
		{
			return cacheResponse;
		}
		switch (cacheRequestState)
		{
		case CacheRequestStateUtil.RequestState.UseCache:
			return cacheResponse;
		case CacheRequestStateUtil.RequestState.UpdateFromWebUseCachedActionId:
			request.ActionId = actionIds.MaxMessageActionId;
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
		if (cacheRequestState == CacheRequestStateUtil.RequestState.UpdateFromWebDoNotStoreResults)
		{
			return nextResponse;
		}
		await UpdateCache(nextResponse);
		return nextResponse;
	}

	private async Task<Response> QueryCache(string threadId, long actionId, long maxTimestamp)
	{
		ThreadCache cache = Context.ThreadCache;
		List<MailMessage> list = ((actionId > 0) ? (await cache.GetMessagesByActionIdAndThreadId(threadId, actionId)) : (await cache.GetMessagesByMaxtimestampAndThreadId(threadId, maxTimestamp, 10)));
		List<MailMessage> messages = list;
		MailThread thread = await cache.GetThreadByThreadId(threadId);
		return new Response
		{
			Messages = messages,
			Thread = thread
		};
	}

	private async Task UpdateCache(Response nextResponse)
	{
		if (nextResponse.Thread == null)
		{
			return;
		}
		ThreadCache cache = Context.ThreadCache;
		MailThread thread = await cache.GetThreadByThreadId(await cache.GetThreadIdByThreadFbid(nextResponse.Thread.Fbid.Value));
		foreach (MailMessage message in nextResponse.Messages)
		{
			List<DeliveryReceipt> deliveryReceipts = thread.DeliveryReceipts;
			Func<DeliveryReceipt, bool> predicate = (DeliveryReceipt d) => d.Timestamp >= message.Timestamp;
			if (deliveryReceipts.Any(predicate))
			{
				message.State = MailMessage.MessageState.Delivered;
			}
			await cache.AddMessage(message, shouldNotify: false);
		}
	}
}
