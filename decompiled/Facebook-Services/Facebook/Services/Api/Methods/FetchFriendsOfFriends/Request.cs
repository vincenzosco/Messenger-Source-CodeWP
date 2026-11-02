namespace Facebook.Services.Api.Methods.FetchFriendsOfFriends;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string SearchQuery { get; set; }

	public string Limit { get; set; }

	public Request(string query, string limit)
	{
		SearchQuery = query;
		Limit = limit;
	}
}
