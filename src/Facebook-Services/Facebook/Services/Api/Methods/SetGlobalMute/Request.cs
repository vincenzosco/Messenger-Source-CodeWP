namespace Facebook.Services.Api.Methods.SetGlobalMute
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public long MuteUntil { get; set; }
	}
}
