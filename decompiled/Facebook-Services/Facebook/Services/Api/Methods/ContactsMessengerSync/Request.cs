namespace Facebook.Services.Api.Methods.ContactsMessengerSync
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public bool Enabled { get; set; }
	}
}
