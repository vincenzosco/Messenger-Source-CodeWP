namespace Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string DeviceId { get; private set; }

	public string ConfirmationCode { get; private set; }

	public string PhoneNumber { get; private set; }

	public Request(string deviceId, string confirmationCode, string phoneNumber)
	{
		DeviceId = deviceId;
		ConfirmationCode = confirmationCode;
		PhoneNumber = phoneNumber;
	}
}
