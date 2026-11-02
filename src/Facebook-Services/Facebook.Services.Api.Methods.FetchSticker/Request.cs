namespace Facebook.Services.Api.Methods.FetchSticker
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string StickerId { get; set; }
	}
}
