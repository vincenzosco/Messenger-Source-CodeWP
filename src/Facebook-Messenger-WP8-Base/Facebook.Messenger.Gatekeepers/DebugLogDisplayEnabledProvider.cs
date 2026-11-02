using Facebook.Models;
using Facebook.Services.Api;

namespace Facebook.Messenger.Gatekeepers
{
	public class DebugLogDisplayEnabledProvider : GatekeeperProvider
	{
		public override bool Enabled
		{
			get
			{
				if (!DebuggingUtility.IsTestBuild)
				{
					return base.Enabled;
				}
				return true;
			}
		}

		public DebugLogDisplayEnabledProvider(FacebookContext context)
			: base(context, "windows_phone_messenger_debug_log_display")
		{
		}
	}
}
