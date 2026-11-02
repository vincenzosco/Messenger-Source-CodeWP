using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.CreateGroup
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
			Response response = await next.ExecuteAsync<Request, Response>(request);
			if (!response.HasError() && response.Thread != null)
			{
				await UpdateCache(response.Thread);
			}
			return response;
		}

		private async Task UpdateCache(MailThread thread)
		{
			ThreadCache cache = Context.ThreadCache;
			await cache.AddThread(thread);
			await cache.AddPinnedThreadAtEnd(thread);
		}
	}
}
