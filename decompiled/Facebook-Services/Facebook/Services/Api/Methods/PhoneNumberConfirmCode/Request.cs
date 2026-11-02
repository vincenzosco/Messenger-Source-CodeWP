namespace Facebook.Services.Api.Methods.PhoneNumberConfirmCode;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string ConfirmCode { get; set; }
}
