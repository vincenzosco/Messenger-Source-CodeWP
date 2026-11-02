using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Friends;

namespace Facebook.Services.Api.Methods.FetchContactsById;

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
		List<FacebookUser> currentUsersList = await Context.ContactCache.GetContacts();
		List<FacebookUser> newUsers = GetNewUsers(Context.FBContext, currentUsersList, response.FetchedUsers);
		List<FacebookUser> deletedUsers = ((request.RequestType != Facebook.Services.Friends.Service.SyncContactTypes.Incremental) ? GetDeletedUsers(currentUsersList, response.FetchedUsers) : null);
		response.FriendsUpdated = await Context.ContactCache.Update(newUsers, deletedUsers);
		response.NewUsers = newUsers;
		response.DeletedUsers = deletedUsers;
		return response;
	}

	private static List<FacebookUser> GetNewUsers(FacebookContext fbContext, IEnumerable<FacebookUser> currentUsersList, IEnumerable<FacebookUser> contactSyncList)
	{
		return contactSyncList.Where((FacebookUser x) => !fbContext.Session.UserId.Equals(x.Id) && currentUsersList.All((FacebookUser u) => u.Id != x.Id)).ToList();
	}

	private static List<FacebookUser> GetDeletedUsers(IEnumerable<FacebookUser> currentUserList, IEnumerable<FacebookUser> contactSyncList)
	{
		return currentUserList.Where((FacebookUser u) => u.IsPhoneOnlyContact && contactSyncList.All((FacebookUser x) => u.Id != x.Id)).ToList();
	}
}
