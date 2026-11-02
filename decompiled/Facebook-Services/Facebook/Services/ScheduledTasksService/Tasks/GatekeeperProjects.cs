using System.Collections.Generic;

namespace Facebook.Services.ScheduledTasksService.Tasks
{
	public class GatekeeperProjects
	{
		public const string DeltaSentMessage = "windows_phone_messenger_delta_sent_message";

		public const string DebugLogDisplay = "windows_phone_messenger_debug_log_display";

		public const string ContactSync = "windows_phone_messenger_contact_sync";

		public const string BackgroundSendingAgent = "windows_phone_messenger_background_sending_agent";

		public const string UpgradePrompt = "windows_phone_messenger_upgrade_prompt";

		public const string StickerDrawerBadge = "windows_phone_messenger_sticker_drawer_badge";

		public static readonly IEnumerable<string> Set = new List<string> { "windows_phone_messenger_debug_log_display", "windows_phone_messenger_contact_sync", "windows_phone_messenger_background_sending_agent", "windows_phone_messenger_upgrade_prompt", "windows_phone_messenger_sticker_drawer_badge" };
	}
}
