using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.FetchLocalThreadData
{
	[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
	public class CacheFilter : FacebookApiFilter<Request, Response>
	{
		public CacheFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Response response = new Response();
			ThreadCache cache = Context.ThreadCache;
			List<string> threadsWithFailedSends = await cache.GetThreadsIdsWithFailedSends();
			response.ThreadIdsWithFailedUnsentMessages.AddRange(threadsWithFailedSends);
			return response;
		}
	}
}
