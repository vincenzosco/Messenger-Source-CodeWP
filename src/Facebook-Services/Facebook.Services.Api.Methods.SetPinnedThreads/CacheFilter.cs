using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.SetPinnedThreads
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
			ThreadCache cache = Context.ThreadCache;
			if (request.RemovedThreadIds != null)
			{
				await cache.RemovePinnedThreadsByThreadId(request.RemovedThreadIds);
			}
			Response response = await next.ExecuteAsync<Request, Response>(request);
			if (!response.HasError())
			{
				await cache.UpdatePinnedThreadsOrder(request.OrderedThreadIds);
			}
			return response;
		}
	}
}
