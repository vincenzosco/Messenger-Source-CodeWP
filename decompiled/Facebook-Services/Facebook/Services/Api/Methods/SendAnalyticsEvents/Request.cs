using System.Collections.Generic;
using Facebook.Services.Analytics;

namespace Facebook.Services.Api.Methods.SendAnalyticsEvents;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public long GeneratedTime { get; set; }

	public string ApplicationVersion { get; set; }

	public string BuildNumber { get; set; }

	public string UserId { get; set; }

	public string ApplicationId { get; set; }

	public string DeviceId { get; set; }

	public string Carrier { get; set; }

	public string DeviceModelName { get; set; }

	public string OperatingSystemVersion { get; set; }

	public string ConnectionType { get; set; }

	public IEnumerable<AnalyticsEvent> Data { get; set; }
}
