namespace Facebook.Services.Api.Methods.SearchImages
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string SearchTerm { get; set; }

		public int Page { get; set; }
	}
}
