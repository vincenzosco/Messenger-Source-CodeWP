namespace Facebook.Services.Api.Methods.DeleteThread;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string ThreadId { get; private set; }

	public Request(string threadId)
	{
		ThreadId = threadId;
	}
}
