using Facebook.Models;

namespace Facebook.Services.Api
{
	public class StickerDrawerBadgeEnabledProvider : GatekeeperProvider
	{
		public StickerDrawerBadgeEnabledProvider(FacebookContext context)
			: base(context, "windows_phone_messenger_sticker_drawer_badge")
		{
		}
	}
}
