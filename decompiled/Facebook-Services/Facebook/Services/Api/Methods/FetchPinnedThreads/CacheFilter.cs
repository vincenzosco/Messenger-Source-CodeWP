using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.FetchPinnedThreads;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
internal class CacheFilter : Filter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		Response cacheResponse = await QueryCache();
		if (!request.ForceToServer && cacheResponse.PinnedThreads.Any())
		{
			return cacheResponse;
		}
		Response nextResponse = await next.ExecuteAsync<Request, Response>(request);
		if (nextResponse.HasError())
		{
			return nextResponse;
		}
		await UpdateCache(nextResponse);
		Response response = await QueryCache();
		response.ResultWasPersisted = true;
		return response;
	}

	private async Task<Response> QueryCache()
	{
		return new Response
		{
			PinnedThreads = await Context.ThreadCache.GetPinnedThreads()
		};
	}

	private async Task UpdateCache(Response nextResponse)
	{
		ThreadCache cache = Context.ThreadCache;
		await cache.ClearPinnedThreads();
		foreach (PinnedThread pinnedThread in nextResponse.PinnedThreads)
		{
			await cache.AddPinnedThread(pinnedThread);
		}
	}
}
