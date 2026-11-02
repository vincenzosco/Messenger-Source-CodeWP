namespace Facebook.Services.Api.Methods.ReportCrash
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string Payload { get; set; }
	}
}
