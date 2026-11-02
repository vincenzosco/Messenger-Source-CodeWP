namespace Facebook.Services.Api.Methods.SendFlytrapReport
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string Description { get; set; }

		public string BuildNumber { get; set; }

		public string Logs { get; set; }

		public Info Info { get; set; }
	}
}
