namespace Facebook.Services.Api.Methods.FetchFriendLastActive;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string Id { get; set; }

	public bool ForceToServer { get; set; }
}
