using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.RegisterPush
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string DeviceId { get; set; }

		public string ChannelUri { get; set; }

		public string AppVersion { get; set; }

		public Dictionary<string, string> ExtraParams { get; set; }
	}
}
