using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.DeleteContact
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
			if (!response.HasError())
			{
				List<FacebookUser> deletedUser = (await Context.ContactCache.GetContacts()).Where((FacebookUser x) => x.Id == request.ContactId).ToList();
				await Context.ContactCache.Update(null, deletedUser);
			}
			return response;
		}
	}
}
