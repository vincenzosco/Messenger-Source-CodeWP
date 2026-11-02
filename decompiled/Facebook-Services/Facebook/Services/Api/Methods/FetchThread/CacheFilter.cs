using System;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.FetchThread;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
internal class CacheFilter : Filter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		if (request.ForceToServer)
		{
			return await next.ExecuteAsync<Request, Response>(request);
		}
		ThreadCache cache = Context.ThreadCache;
		MailThread thread = request.KeyType switch
		{
			Request.RequestKeyType.Recipient => await cache.GetThreadBySingleRecipientId(request.RecipientId), 
			Request.RequestKeyType.ThreadFbid => await cache.GetThreadByThreadId(await cache.GetThreadIdByThreadFbid(request.ThreadFbid)), 
			Request.RequestKeyType.ThreadId => await cache.GetThreadByThreadId(request.ThreadId), 
			_ => throw new NotImplementedException(), 
		};
		if (thread != null || request.QueryDatabaseOnly)
		{
			Response response = new Response();
			response.Thread = thread;
			return response;
		}
		return await next.ExecuteAsync<Request, Response>(request);
	}
}
