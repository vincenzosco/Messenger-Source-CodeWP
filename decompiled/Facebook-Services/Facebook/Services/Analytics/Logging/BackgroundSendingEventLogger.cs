using Facebook.Base.Extensions;
using Facebook.Services.Common;

namespace Facebook.Services.Analytics.Logging;

public class BackgroundSendingEventLogger
{
	public const string BackgroundSendingModule = "wp_background_sending";

	public static void LogFailedMessagesOnExit(int count, bool otherAppIsPlaying, bool backgroundAgentEnabled, Service analyticsService)
	{
		AddEventRequest request = new AddEventRequest("message_toast_showing", "wp_background_sending").AddData("count", count.ToString()).AddData("other_app_is_playing", otherAppIsPlaying.ToString()).AddData("background_agent_enabled", backgroundAgentEnabled.ToString());
		analyticsService.ExecuteAsync<AddEventRequest, BoolResponse>(request).DoNotWait();
	}

	public static void LogAppStartedFromMessageFailedToast(Service analyticsService)
	{
		AddEventRequest request = new AddEventRequest("message_toast_started_app", "wp_background_sending");
		analyticsService.ExecuteAsync<AddEventRequest, BoolResponse>(request).DoNotWait();
	}
}
