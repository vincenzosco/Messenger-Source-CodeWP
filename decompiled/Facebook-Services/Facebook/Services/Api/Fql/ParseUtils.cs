using System.Collections.Generic;
using System.Linq;
using Facebook.Models;

namespace Facebook.Services.Api.Fql;

public static class ParseUtils
{
	public static Dictionary<string, FacebookUser> CreateIdMap(IEnumerable<FacebookUser> profiles)
	{
		if (profiles == null)
		{
			return new Dictionary<string, FacebookUser>();
		}
		return profiles.ToDictionary((FacebookUser p) => p.Id, (FacebookUser p) => p);
	}

	public static void PatchUsers(IList<MailUser> users, Dictionary<string, FacebookUser> userMap)
	{
		if (users == null || userMap == null)
		{
			return;
		}
		foreach (MailUser user in users)
		{
			PatchUser(user, userMap);
		}
	}

	public static void PatchUser(MailUser u, Dictionary<string, FacebookUser> details)
	{
		if (u != null && details != null && details.ContainsKey(u.Id))
		{
			FacebookUser facebookUser = details[u.Id];
			u.Name = facebookUser.Name;
			u.FirstName = facebookUser.FirstName;
			u.MiddleName = facebookUser.MiddleName;
			u.LastName = facebookUser.LastName;
			u.Pushable = facebookUser.Pushable;
			u.IsPartial = facebookUser.IsPartial;
			u.IsMessengerUser = facebookUser.IsMessengerUser;
			u.LastActive = facebookUser.LastActive;
		}
	}

	public static void UpdateMuteUntil(MailThread thread, List<FacebookProfile> profiles)
	{
		if (profiles != null && profiles.Any())
		{
			string id = profiles[0].Id;
			if (thread.MuteMap.ContainsKey(id))
			{
				thread.MuteUntil = thread.MuteMap[id];
			}
		}
	}
}
