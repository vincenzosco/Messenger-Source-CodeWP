using Facebook.Services.Api;
using Facebook.Services.Api.Methods;

namespace Facebook.Services.Mqtt
{
	[Queue(QueueType.Web)]
	public class CreateSyncQueueRequest : FacebookServiceRequest
	{
		public long LargestSequenceId { get; private set; }

		public int SyncApiVersion { get; private set; }

		public CreateSyncQueueRequest(long largestSequenceId, int syncApiVersion)
		{
			LargestSequenceId = largestSequenceId;
			SyncApiVersion = syncApiVersion;
		}
	}
}
