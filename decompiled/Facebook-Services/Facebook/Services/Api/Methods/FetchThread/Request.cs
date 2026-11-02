namespace Facebook.Services.Api.Methods.FetchThread;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public enum RequestKeyType
	{
		Recipient,
		ThreadId,
		ThreadFbid
	}

	public string RecipientId { get; private set; }

	public string ThreadId { get; private set; }

	public long ThreadFbid { get; private set; }

	public RequestKeyType KeyType { get; private set; }

	public bool QueryDatabaseOnly { get; set; }

	public bool ForceToServer { get; set; }

	private Request()
	{
	}

	public static Request ByRecipient(string recipientId)
	{
		Request request = new Request();
		request.RecipientId = recipientId;
		request.KeyType = RequestKeyType.Recipient;
		return request;
	}

	public static Request ByThreadId(string threadId)
	{
		Request request = new Request();
		request.ThreadId = threadId;
		request.KeyType = RequestKeyType.ThreadId;
		return request;
	}

	public static Request ByThreadFbid(long threadFbid)
	{
		Request request = new Request();
		request.ThreadFbid = threadFbid;
		request.KeyType = RequestKeyType.ThreadFbid;
		return request;
	}
}
