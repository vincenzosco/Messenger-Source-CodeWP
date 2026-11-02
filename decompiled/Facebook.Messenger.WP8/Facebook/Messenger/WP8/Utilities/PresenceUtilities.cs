using System;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class PresenceUtilities
	{
		public static string GenerateLastActiveText(long timestamp)
		{
			if (timestamp == 0)
			{
				return string.Empty;
			}
			long num = DateTime.UtcNow.ToUnixTimeSeconds() - timestamp;
			long num2 = num / 60;
			long num3 = num2 / 60;
			long num4 = num3 / 24;
			if (num < 60)
			{
				return StringLibrary.Messages_Active_1m_Ago;
			}
			if (num2 < 60)
			{
				return string.Format(StringLibrary.Messages_Active_Xm_Ago, num2);
			}
			if (num3 < 24)
			{
				return string.Format(StringLibrary.Messages_Active_Xh_Ago, num3);
			}
			return string.Format(StringLibrary.Messages_Active_Xd_Ago, num4);
		}
	}
}
