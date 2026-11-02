using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.SyncFriends
{
	[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
	public class CacheFilter : Filter<Request, Response>
	{
		public CacheFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Response response = await next.ExecuteAsync<Request, Response>(request);
			if (request.CancelToken.IsCancellationRequested)
			{
				return new Response
				{
					Error = Error.FromException(new TaskCanceledException())
				};
			}
			if (response.HasError())
			{
				return response;
			}
			if (request.ShouldPurgeBeforeSaving())
			{
				await Context.ContactCache.Purge();
			}
			await Context.ContactCache.Update(response.NewFriends, response.DeletedFriends);
			return response;
		}
	}
}
