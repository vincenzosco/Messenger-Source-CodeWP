namespace Facebook.Services.Api.Methods.SetThreadPicture
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }

		public string ImagePath { get; set; }
	}
}
