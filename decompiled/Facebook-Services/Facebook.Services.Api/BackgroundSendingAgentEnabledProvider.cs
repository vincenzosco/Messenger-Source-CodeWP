using Facebook.Models;

namespace Facebook.Services.Api
{
	public class BackgroundSendingAgentEnabledProvider : GatekeeperProvider
	{
		public BackgroundSendingAgentEnabledProvider(FacebookContext context)
			: base(context, "windows_phone_messenger_background_sending_agent")
		{
		}
	}
}
