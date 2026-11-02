namespace Facebook.Services.Api.Methods.ThreadQuery;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string ThreadFbId { get; set; }

	public int MessageCount { get; set; }

	public long ActionId { get; set; }

	public long MaxTimestamp { get; set; }

	public bool ForceToServer { get; set; }

	public long RecentPushActionId { get; set; }
}
