using Facebook.Models;

namespace Facebook.Services.Api
{
	public class ContactSyncFeatureEnabledProvider : GatekeeperProvider
	{
		public ContactSyncFeatureEnabledProvider(FacebookContext context)
			: base(context, "windows_phone_messenger_contact_sync")
		{
		}
	}
}
