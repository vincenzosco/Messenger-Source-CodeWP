using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models.PhoneContacts;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.FetchChangedPhoneContacts
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
			PhoneContactCache phoneContactsCache = Context.PhoneContactCache;
			List<FBPhoneContact> addedOrModifiedContacts = await phoneContactsCache.GetAddedOrModifiedContacts(request.PhoneContacts);
			await phoneContactsCache.AddContactsToCache(addedOrModifiedContacts);
			return new Response
			{
				ChangedContacts = addedOrModifiedContacts
			};
		}
	}
}
