namespace Facebook.Services.Api.Methods.SetThreadMute
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }

		public long MuteUntil { get; set; }
	}
}
