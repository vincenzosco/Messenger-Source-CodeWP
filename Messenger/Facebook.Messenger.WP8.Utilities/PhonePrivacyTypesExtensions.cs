using System;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class PhonePrivacyTypesExtensions
	{
		public static int GetPublicityRankDescending(this PhonePrivacyTypes type)
		{
			switch (type)
			{
			case PhonePrivacyTypes.Everyone:
				return 0;
			case PhonePrivacyTypes.FriendsOfFriends:
				return 1;
			case PhonePrivacyTypes.MutualFriends:
				return 2;
			case PhonePrivacyTypes.AllFriends:
				return 3;
			case PhonePrivacyTypes.Custom:
				return 4;
			case PhonePrivacyTypes.Self:
				return 5;
			case PhonePrivacyTypes.Unknown:
				return 6;
			default:
				throw new ArgumentOutOfRangeException("type");
			}
		}
	}
}
