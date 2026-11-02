namespace Facebook.Services.Api.Methods.FetchUserById
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string Id { get; set; }

		public Request(string id)
		{
			Id = id;
		}
	}
}
