namespace Facebook.Services.Api.Methods.AddContact
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string PhoneNumber { get; set; }

		public string CountryCode { get; set; }

		public Request(string phoneNumber, string countryCode)
		{
			PhoneNumber = phoneNumber;
			CountryCode = countryCode;
		}
	}
}
