namespace Facebook.Services.Api.Methods.FetchPinnedThreads
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public bool ForceToServer { get; set; }
	}
}
