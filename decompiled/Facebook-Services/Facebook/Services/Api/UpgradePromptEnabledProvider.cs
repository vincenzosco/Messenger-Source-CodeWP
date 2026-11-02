using Facebook.Models;

namespace Facebook.Services.Api;

public class UpgradePromptEnabledProvider : GatekeeperProvider
{
	public UpgradePromptEnabledProvider(FacebookContext context)
		: base(context, "windows_phone_messenger_upgrade_prompt")
	{
	}
}
