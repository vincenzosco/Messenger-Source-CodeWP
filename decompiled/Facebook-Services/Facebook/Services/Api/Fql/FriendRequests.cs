using System.Collections.Generic;
using System.Text;

namespace Facebook.Services.Api.Fql;

public class FriendRequests
{
	private enum RequestType
	{
		Get,
		Sync,
		Delete,
		Tail
	}

	private const string FriendRequestTable = "friend_request";

	private static string[] FriendRequestFields = new string[4] { "uid_from", "time", "unread", "is_hidden" };

	private static string[] UserFields = new string[3] { "uid", "name", "affiliations.name" };

	private static string[] FriendRequestIds = new string[1] { "uid_from" };

	public static string Get(long? startTime, int limit = 0)
	{
		string text = CreateWhereClause(RequestType.Get, startTime);
		Dictionary<string, string> query = GetQuery(RequestType.Get, text, limit);
		return Common.CreateMultiQuery(query);
	}

	public static string Sync(bool fullSync, long? minTime, long? maxTime, int limit = 0)
	{
		string text = CreateWhereClause(RequestType.Sync, maxTime);
		Dictionary<string, string> query = GetQuery(RequestType.Sync, text, limit);
		if (fullSync)
		{
			string text2 = CreateWhereClause(RequestType.Delete, minTime, maxTime);
			Dictionary<string, string> query2 = GetQuery(RequestType.Delete, text2, limit);
			query.Add("ids", query2["ids"]);
		}
		return Common.CreateMultiQuery(query);
	}

	public static string Delete(long? startTime, long? endTime, int limit = 0)
	{
		string text = CreateWhereClause(RequestType.Delete, startTime, endTime);
		Dictionary<string, string> query = GetQuery(RequestType.Delete, text, limit);
		return Common.CreateMultiQuery(query);
	}

	public static string Tail(long? maxTime, int limit = 0)
	{
		string text = CreateWhereClause(RequestType.Tail, null, maxTime);
		Dictionary<string, string> query = GetQuery(RequestType.Tail, text, limit);
		return Common.CreateMultiQuery(query);
	}

	private static string CreateWhereClause(RequestType type, long? startTime = null, long? endTime = null)
	{
		StringBuilder stringBuilder = new StringBuilder("uid_to = me()");
		switch (type)
		{
		case RequestType.Get:
			if (startTime.HasValue && startTime > 0)
			{
				stringBuilder.Append($" AND (time > {startTime})");
			}
			break;
		case RequestType.Sync:
			if (startTime.HasValue && startTime > 0)
			{
				stringBuilder.Append($" AND (unread = 1 OR time > {startTime})");
			}
			break;
		case RequestType.Delete:
			if (startTime.HasValue && endTime.HasValue)
			{
				stringBuilder.Append($" AND time <= {endTime}");
				stringBuilder.Append($" AND time >= {startTime}");
			}
			break;
		case RequestType.Tail:
			if (endTime.HasValue && endTime > 0)
			{
				stringBuilder.Append($" AND (time < {endTime})");
			}
			break;
		}
		stringBuilder.Append($" AND is_hidden = 0");
		return stringBuilder.ToString();
	}

	private static Dictionary<string, string> GetQuery(RequestType type, string where, int limit = 0)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		switch (type)
		{
		case RequestType.Get:
		case RequestType.Tail:
			dictionary.Add("requests", Common.Create(FriendRequestFields, "friend_request", where, "time DESC", limit));
			dictionary.Add("users", Common.Create(UserFields, "user", "uid in (SELECT uid_from FROM #requests)"));
			break;
		case RequestType.Sync:
			dictionary.Add("requests", Common.Create(FriendRequestFields, "friend_request", where, "time DESC", limit));
			dictionary.Add("users", Common.Create(UserFields, "user", "uid in (SELECT uid_from FROM #requests)"));
			break;
		case RequestType.Delete:
			dictionary.Add("ids", Common.Create(FriendRequestIds, "friend_request", where, "time DESC", limit));
			break;
		}
		return dictionary;
	}
}
