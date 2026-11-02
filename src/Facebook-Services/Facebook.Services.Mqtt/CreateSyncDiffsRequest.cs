using Facebook.Services.Api;
using Facebook.Services.Api.Methods;

namespace Facebook.Services.Mqtt
{
	[Queue(QueueType.Web)]
	public class CreateSyncDiffsRequest : FacebookServiceRequest
	{
		public string SyncToken { get; private set; }

		public long LastSequenceId { get; private set; }

		public CreateSyncDiffsRequest(string syncToken, long lastSequenceId)
		{
			SyncToken = syncToken;
			LastSequenceId = lastSequenceId;
		}
	}
}
