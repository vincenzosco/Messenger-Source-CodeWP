using System.Collections.Generic;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchContactsById
{
	[JsonConverter(typeof(ContactSyncConverter))]
	public class Response : FacebookApiResponse
	{
		public List<FacebookUser> FetchedUsers { get; set; }

		public List<FacebookUser> NewUsers { get; set; }

		public List<FacebookUser> DeletedUsers { get; set; }

		public bool FriendsUpdated { get; set; }
	}
}
