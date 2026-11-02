namespace Facebook.Services.Api.Methods.LeaveThread
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }
	}
}
