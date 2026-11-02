using System.Collections.Generic;
using System.Linq;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchFriendsOfFriends
{
	[JsonConverter(typeof(FriendsOfFriendsArrayConverter))]
	public class Response : FacebookApiResponse
	{
		public List<FacebookUberSearchResult> UberSearchResults { private get; set; }

		public List<FacebookUser> Users
		{
			get
			{
				if (UberSearchResults == null || !UberSearchResults.Any())
				{
					return new List<FacebookUser>();
				}
				return UberSearchResults.Where((FacebookUberSearchResult usl) => !string.IsNullOrWhiteSpace(usl.Subtext)).Select(ToFacebookUser).ToList();
			}
		}

		public void MarkAsSelected(FacebookUser user)
		{
			if (UberSearchResults != null && UberSearchResults.Any())
			{
				FacebookUberSearchResult facebookUberSearchResult = UberSearchResults.FirstOrDefault((FacebookUberSearchResult u) => u.Uid == user.Id);
				if (facebookUberSearchResult != null)
				{
					facebookUberSearchResult.IsSelected = true;
				}
			}
		}

		private FacebookUser ToFacebookUser(FacebookUberSearchResult usl)
		{
			FacebookUser facebookUser = new FacebookUser();
			facebookUser.Name = usl.Text;
			facebookUser.IsMessengerUser = usl.IsMessengerUser;
			facebookUser.IsSelected = usl.IsSelected;
			facebookUser.Id = usl.Uid;
			facebookUser.IsFriendOfFriend = true;
			return facebookUser;
		}
	}
}
