using System.Collections.Generic;
using System.Text;

namespace Facebook.Services.Api.Fql;

public class Notifications
{
	private enum RequestType
	{
		Get,
		Sync,
		Delete,
		Tail
	}

	private const string NotificationsTable = "notification";

	private static string[] NotificationsFields = new string[12]
	{
		"notification_id", "sender_id", "object_id", "object_type", "title_text", "title_html", "icon_url", "href", "created_time", "updated_time",
		"is_unread", "join_data"
	};

	private static string[] ProfileFields = new string[2] { "id", "name" };

	private static string[] NotificationsIdsFields = new string[2] { "notification_id", "is_unread" };

	public static string Get(long? startTime, int limit = 0)
	{
		string text = CreateWhereClause(RequestType.Get, startTime);
		Dictionary<string, string> query = GetQuery(RequestType.Get, text, limit);
		return CreateFql(query);
	}

	public static string Sync(bool fullSync, long? startTime, string notificationIds, int limit = 0)
	{
		string text = CreateWhereClause(RequestType.Sync, startTime);
		Dictionary<string, string> query = GetQuery(RequestType.Sync, text, limit);
		if (fullSync)
		{
			string text2 = CreateWhereClause(RequestType.Delete, null, null, notificationIds);
			Dictionary<string, string> query2 = GetQuery(RequestType.Delete, text2, limit);
			query.Add("ids", query2["ids"]);
		}
		return CreateFql(query);
	}

	public static string Delete(string notificationIds, int limit = 0)
	{
		string text = CreateWhereClause(RequestType.Delete, null, null, notificationIds);
		Dictionary<string, string> query = GetQuery(RequestType.Delete, text, limit);
		return CreateFql(query);
	}

	public static string Tail(long? endTime, int limit = 0)
	{
		string text = CreateWhereClause(RequestType.Tail, null, endTime);
		Dictionary<string, string> query = GetQuery(RequestType.Tail, text, limit);
		return CreateFql(query);
	}

	private static string CreateWhereClause(RequestType type, long? startTime = null, long? endTime = null, string notificationIds = null)
	{
		StringBuilder stringBuilder = new StringBuilder("recipient_id = me() AND object_type != '' AND is_mobile_ready ");
		switch (type)
		{
		case RequestType.Get:
			if (startTime.HasValue)
			{
				stringBuilder.Append($"AND (updated_time > {startTime})");
			}
			break;
		case RequestType.Sync:
			if (startTime.HasValue)
			{
				stringBuilder.Append($"AND (updated_time > {startTime})");
			}
			break;
		case RequestType.Delete:
			if (!string.IsNullOrWhiteSpace(notificationIds))
			{
				stringBuilder.Append($"AND (notification_id IN ({notificationIds}) )");
			}
			break;
		case RequestType.Tail:
			if (endTime.HasValue)
			{
				stringBuilder.Append($"AND (updated_time < {endTime})");
			}
			break;
		}
		return stringBuilder.ToString();
	}

	private static string CreateFql(Dictionary<string, string> query)
	{
		return Common.CreateMultiQuery(query);
	}

	private static Dictionary<string, string> GetQuery(RequestType type, string where, int limit)
	{
		switch (type)
		{
		case RequestType.Get:
		case RequestType.Tail:
		{
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			dictionary3.Add("notifications", Common.Create(NotificationsFields, "notification", where, "updated_time DESC", limit));
			dictionary3.Add("profiles", Common.Create(ProfileFields, "profile", "id in (select sender_id from #notifications)"));
			return dictionary3;
		}
		case RequestType.Sync:
		{
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2.Add("notifications", Common.Create(NotificationsFields, "notification", where, "updated_time DESC", limit));
			dictionary2.Add("profiles", Common.Create(ProfileFields, "profile", "id in (select sender_id from #notifications)"));
			return dictionary2;
		}
		case RequestType.Delete:
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("ids", Common.Create(NotificationsIdsFields, "notification", where, "updated_time DESC", limit));
			return dictionary;
		}
		default:
			return new Dictionary<string, string>();
		}
	}
}
