using Facebook.Services.Analytics.Logging;

namespace Facebook.Services.Analytics;

internal class DataUsageEventCreator
{
	public static AnalyticsEvent CreateEvent(DataType dataType, long downloadedBytes, long uploadedBytes)
	{
		AnalyticsEvent analyticsEvent = new AnalyticsEvent(GetEventName(dataType), "data_consumption_events").AddData("total_downloaded_bytes", downloadedBytes.ToString()).AddData("total_uploaded_bytes", uploadedBytes.ToString());
		AddLegacyMethodData(analyticsEvent, dataType);
		return analyticsEvent;
	}

	private static string GetEventName(DataType dataType)
	{
		switch (dataType)
		{
		case DataType.Api:
		case DataType.Mqtt:
			return "api_data_consumed";
		case DataType.Image:
			return "image_data_consumed";
		case DataType.Sticker:
			return "sticker_data_consumed";
		default:
			return "unknown_data_consumed";
		}
	}

	private static void AddLegacyMethodData(AnalyticsEvent e, DataType dataType)
	{
		switch (dataType)
		{
		case DataType.Api:
			e.AddData("method", "Graph");
			break;
		case DataType.Mqtt:
			e.AddData("method", "MQTT");
			break;
		}
	}
}
