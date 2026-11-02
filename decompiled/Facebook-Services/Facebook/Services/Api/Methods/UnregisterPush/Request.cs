namespace Facebook.Services.Api.Methods.UnregisterPush
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ChannelUri { get; set; }
	}
}
