namespace Facebook.Services.Api.Methods.PhoneNumberRegistration
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string DeviceId { get; private set; }

		public string CountryCode { get; private set; }

		public string PhoneNumber { get; private set; }

		public Request(string deviceId, string countryCode, string phoneNumber)
		{
			DeviceId = deviceId;
			CountryCode = countryCode;
			PhoneNumber = phoneNumber;
		}
	}
}
