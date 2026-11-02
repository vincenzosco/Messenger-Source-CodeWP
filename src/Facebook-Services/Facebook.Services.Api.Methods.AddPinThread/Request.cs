namespace Facebook.Services.Api.Methods.AddPinThread
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }

		public string NewName { get; set; }

		public string ImagePath { get; set; }

		public string OfflineThreadingId { get; set; }

		public bool RefetchThread { get; set; }

		public Request(string threadId)
		{
			ThreadId = threadId;
		}
	}
}
