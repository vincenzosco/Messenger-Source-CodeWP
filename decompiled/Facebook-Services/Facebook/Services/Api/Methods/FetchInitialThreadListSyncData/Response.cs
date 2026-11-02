using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchInitialThreadListSyncData
{
	[JsonConverter(typeof(InitialThreadListConverter))]
	public class Response : FacebookApiResponse
	{
		public int Count { get; set; }

		public int SyncSequenceId { get; set; }
	}
}
