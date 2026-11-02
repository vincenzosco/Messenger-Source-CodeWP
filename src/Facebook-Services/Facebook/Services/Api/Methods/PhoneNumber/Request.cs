namespace Facebook.Services.Api.Methods.PhoneNumber
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string PhoneNumber { get; set; }

		public string CountryCode { get; set; }
	}
}
