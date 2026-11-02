namespace Facebook.Services.Api.Methods.FetchMessage
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string MessageId { get; set; }

		public string ThreadId { get; set; }

		public string OfflineThreadingId { get; set; }
	}
}
