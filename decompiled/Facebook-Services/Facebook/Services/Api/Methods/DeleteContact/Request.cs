namespace Facebook.Services.Api.Methods.DeleteContact;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string ContactWriteId;

	public string ContactId;
}
