namespace Facebook.Services.Api.Methods.SetThreadName;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string ThreadId { get; set; }

	public string Name { get; set; }

	public string OfflineThreadingId { get; set; }
}
