using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.FetchFriends
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
			ContactCache cache = Context.ContactCache;
			Response response = new Response();
			if (request.Uid != null)
			{
				response.Friend = await cache.GetUser(request.Uid);
			}
			else
			{
				response.Friends = await cache.GetUserFromFilter(request.Filter);
			}
			return response;
		}
	}
}
