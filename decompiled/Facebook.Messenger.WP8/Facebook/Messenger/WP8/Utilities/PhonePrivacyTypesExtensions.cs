using System;

namespace Facebook.Messenger.WP8.Utilities;

public static class PhonePrivacyTypesExtensions
{
	public static int GetPublicityRankDescending(this PhonePrivacyTypes type)
	{
		return type switch
		{
			PhonePrivacyTypes.Everyone => 0, 
			PhonePrivacyTypes.FriendsOfFriends => 1, 
			PhonePrivacyTypes.MutualFriends => 2, 
			PhonePrivacyTypes.AllFriends => 3, 
			PhonePrivacyTypes.Custom => 4, 
			PhonePrivacyTypes.Self => 5, 
			PhonePrivacyTypes.Unknown => 6, 
			_ => throw new ArgumentOutOfRangeException("type"), 
		};
	}
}
