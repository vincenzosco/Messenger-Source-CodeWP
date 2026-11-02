namespace Facebook.Services.Api.Methods.FetchGroupSearchResults;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string Query { get; set; }

	public int Limit { get; set; }

	public bool IncludePinnedIds { get; set; }

	public bool ExcludeUnsubscribedThreads { get; set; }

	public Request(string query, int limit = 10)
	{
		Query = query;
		Limit = limit;
	}
}
