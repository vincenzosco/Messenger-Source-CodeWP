using System.Collections.Generic;
using System.Linq;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Utilities;

public static class PhoneNumberUtility
{
	public static PhonePrivacyTypes PhonePrivacyFromString(string privacy)
	{
		privacy = privacy.ToUpper();
		return privacy switch
		{
			"EVERYONE" => PhonePrivacyTypes.Everyone, 
			"FRIENDS_OF_FRIENDS" => PhonePrivacyTypes.FriendsOfFriends, 
			"MUTUAL_FRIENDS" => PhonePrivacyTypes.MutualFriends, 
			"ALL_FRIENDS" => PhonePrivacyTypes.AllFriends, 
			"CUSTOM" => PhonePrivacyTypes.Custom, 
			"SELF" => PhonePrivacyTypes.Self, 
			_ => PhonePrivacyTypes.Unknown, 
		};
	}

	public static PhoneInfo GetBroadestPrivacyPhoneInfo(List<PhoneInfo> phoneInfos)
	{
		if (!phoneInfos.IsNullOrEmpty())
		{
			return phoneInfos.OrderBy((PhoneInfo i) => PhonePrivacyFromString(i.Privacy).GetPublicityRankDescending()).FirstOrDefault();
		}
		return null;
	}
}
