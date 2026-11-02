using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.AddPinThread
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
				await Context.ThreadCache.UpdateExistingThread(response.Thread);
				await Context.ThreadCache.AddPinnedThreadAtEnd(response.Thread);
			}
			return response;
		}
	}
}
