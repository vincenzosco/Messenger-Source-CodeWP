using System.Collections.Generic;
using System.Linq;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class PhoneNumberUtility
	{
		public static PhonePrivacyTypes PhonePrivacyFromString(string privacy)
		{
			privacy = privacy.ToUpper();
			switch (privacy)
			{
			case "EVERYONE":
				return PhonePrivacyTypes.Everyone;
			case "FRIENDS_OF_FRIENDS":
				return PhonePrivacyTypes.FriendsOfFriends;
			case "MUTUAL_FRIENDS":
				return PhonePrivacyTypes.MutualFriends;
			case "ALL_FRIENDS":
				return PhonePrivacyTypes.AllFriends;
			case "CUSTOM":
				return PhonePrivacyTypes.Custom;
			case "SELF":
				return PhonePrivacyTypes.Self;
			default:
				return PhonePrivacyTypes.Unknown;
			}
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
}
