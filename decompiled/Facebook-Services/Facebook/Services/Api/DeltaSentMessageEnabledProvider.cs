using Facebook.Models;

namespace Facebook.Services.Api;

public class DeltaSentMessageEnabledProvider : GatekeeperProvider
{
	public DeltaSentMessageEnabledProvider(FacebookContext context)
		: base(context, "windows_phone_messenger_delta_sent_message")
	{
	}
}
