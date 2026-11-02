using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.RemovePhoneContacts
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
			List<FacebookUser> deletedUsers = (await Context.ContactCache.GetContacts()).Where((FacebookUser x) => x.IsPhoneOnlyContact).ToList();
			await Context.ContactCache.Update(null, deletedUsers);
			return new Response
			{
				FriendsUpdated = deletedUsers.Any()
			};
		}
	}
}
