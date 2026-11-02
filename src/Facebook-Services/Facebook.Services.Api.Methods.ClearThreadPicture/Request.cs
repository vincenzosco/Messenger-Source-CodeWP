namespace Facebook.Services.Api.Methods.ClearThreadPicture
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }
	}
}
