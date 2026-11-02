using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Services.Api.Fql;

public class UnifiedThread
{
	private static readonly string[] ThreadFields = new string[24]
	{
		"thread_id", "thread_fbid", "sync_sequence_id", "name", "folder", "pic_hash", "mute", "snippet", "snippet_sender", "snippet_message_id",
		"admin_snippet", "senders", "participants", "former_participants", "timestamp", "action_id", "last_visible_add_action_id", "unread", "single_recipient", "can_reply",
		"is_subscribed", "archived", "read_receipts", "delivery_receipts"
	};

	private static readonly string[] ThreadSyncFields = new string[25]
	{
		"thread_id", "thread_fbid", "name", "folder", "pic_hash", "mute", "snippet", "snippet_sender", "snippet_message_id", "admin_snippet",
		"senders", "participants", "former_participants", "timestamp", "action_id", "last_visible_add_action_id", "unread", "single_recipient", "can_reply", "is_subscribed",
		"archived", "refetch_action_id", "sync_change_type", "read_receipts", "delivery_receipts"
	};

	private static readonly string[] GroupSearchThreadFields = new string[8] { "thread_id", "name", "pic_hash", "timestamp", "senders", "participants", "single_recipient", "former_participants" };

	private static readonly string[] UserFields = new string[9] { "uid", "name", "first_name", "middle_name", "last_name", "is_pushable", "is_messenger_user", "last_active", "is_partial" };

	private static readonly string[] GroupSearchUserFields = new string[5] { "uid", "name", "first_name", "middle_name", "last_name" };

	private static readonly string[] ProfileFields = new string[3] { "name", "id", "type" };

	private static readonly string[] FolderCountFields = new string[6] { "folder", "unseen_count", "unread_count", "last_action_id", "refetch_action_id", "last_seen_time" };

	private static readonly string[] SettingsFields = new string[2] { "setting", "value" };

	private static string GetThread(string whereClause)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("threads", Common.Create(ThreadFields, "unified_thread", whereClause));
		dictionary.Add("users", Common.Create(UserFields, "user", string.Join(" OR ", GetUserWhereClauses())));
		dictionary.Add("me", "select id from profile where id = me()");
		Dictionary<string, string> queryMap = dictionary;
		return Common.CreateMultiQuery(queryMap);
	}

	public static string GetUserById(IEnumerable<string> userIds)
	{
		return Common.Create(UserFields, "user", $"uid IN ({Common.CreateCsv(userIds)})");
	}

	public static string GetUsers(string whereClause)
	{
		return Common.Create(UserFields, "user", whereClause);
	}

	public static string GetThreadById(string threadId)
	{
		string whereClause = $"thread_id = '{threadId}'";
		return GetThread(whereClause);
	}

	public static string GetThreadByRecipient(string recipientId)
	{
		string whereClause = $"single_recipient = '{recipientId}'";
		return GetThread(whereClause);
	}

	public static string GetThreadByFbid(long threadFbid)
	{
		string whereClause = $"thread_fbid = {threadFbid}";
		return GetThread(whereClause);
	}

	public static string GetGroupThreads(long timestamp, int limit = 3)
	{
		string text = $"folder = 'inbox' AND is_group_conversation = 1 AND not archived AND timestamp > {timestamp}";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("threads", Common.Create(ThreadFields, "unified_thread", text, limit));
		dictionary.Add("users", Common.Create(UserFields, "user", string.Join(" OR ", GetUserWhereClauses())));
		Dictionary<string, string> queryMap = dictionary;
		return Common.CreateMultiQuery(queryMap);
	}

	public static string GetGroupSearchResults(string searchQuery, int limit = 3, bool includePinnedIds = false, bool excludeUnsubscribed = false)
	{
		string text = (excludeUnsubscribed ? "AND is_subscribed" : string.Empty);
		string text2 = string.Empty;
		if (!string.IsNullOrEmpty(searchQuery))
		{
			text2 = $"CONTAINS ('{searchQuery}', thread_and_participants_name) AND";
		}
		string text3 = string.Format("{0} folder = 'inbox' AND is_group_conversation = 1 {1} order by timestamp desc", new object[2] { text2, text });
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("threads", Common.Create(GroupSearchThreadFields, "unified_thread", text3, limit));
		dictionary.Add("users", Common.Create(GroupSearchUserFields, "user", string.Join(" OR ", GetUserWhereClauses())));
		Dictionary<string, string> dictionary2 = dictionary;
		if (includePinnedIds)
		{
			dictionary2["pinned"] = "SELECT thread_id from pinned_thread WHERE update_time > 0";
		}
		return Common.CreateMultiQuery(dictionary2);
	}

	public static string Get(string folder, long maxTimestamp = 0L, int limit = 10)
	{
		string text = CreateWhereClause(folder, maxTimestamp);
		return CreateFql(folder, text, sync: false, limit);
	}

	public static string Sync(string folder, long actionId = 0L, int limit = 10)
	{
		string text = CreateWhereClause(folder, actionId, isActionId: true);
		return CreateFql(folder, text, sync: true, limit);
	}

	private static void AddTimeOrActionClause(StringBuilder b, long value, bool isActionId)
	{
		if (value > 0)
		{
			if (isActionId)
			{
				b.AppendFormat(" AND action_id > {0}", new object[1] { value });
			}
			else
			{
				b.AppendFormat(" AND timestamp < {0}", new object[1] { value });
			}
		}
		b.Append(" ORDER BY timestamp DESC");
	}

	private static string CreateWhereClause(string folder, long timestamp = 0L, bool isActionId = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("folder = '{0}'", new object[1] { folder });
		if (!isActionId)
		{
			stringBuilder.Append(" AND archived = 0");
		}
		AddTimeOrActionClause(stringBuilder, timestamp, isActionId);
		return stringBuilder.ToString();
	}

	private static string CreateFql(string folder, string where, bool sync, int limit = 0)
	{
		string table = (sync ? "unified_thread_sync" : "unified_thread");
		string[] fields = (sync ? ThreadSyncFields : ThreadFields);
		string text = $"folder = '{folder}' ";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("threads", Common.Create(fields, table, where, limit));
		dictionary.Add("users", Common.Create(UserFields, "user", string.Join(" OR ", GetUserWhereClauses())));
		dictionary.Add("counts", Common.Create(FolderCountFields, "unified_thread_count", text));
		dictionary.Add("me", "select id from profile where id = me()");
		dictionary.Add("settings", Common.Create(SettingsFields, "user_settings", "project='messaging'"));
		Dictionary<string, string> queryMap = dictionary;
		return Common.CreateMultiQuery(queryMap);
	}

	public static string GetPinnedGroups()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("pinned", "SELECT thread_id, auto_pinned, update_time FROM pinned_thread WHERE update_time > 0 LIMIT 60");
		dictionary.Add("threads", "SELECT thread_id, thread_fbid, name, pic_hash, participants, senders, timestamp, read_receipts FROM unified_thread WHERE thread_id IN (select thread_id from #pinned) AND is_subscribed LIMIT 60");
		dictionary.Add("users", Common.Create(UserFields, "user", "uid in (SELECT participants.user_id FROM #threads)"));
		Dictionary<string, string> queryMap = dictionary;
		return Common.CreateMultiQuery(queryMap);
	}

	public static Dictionary<string, string> CreateThreadsQueryMap(IEnumerable<long> threadFbIds, IEnumerable<long> threadRecipientIds)
	{
		List<string> list = new List<string>();
		if (threadFbIds.Any())
		{
			list.Add($"thread_fbid IN ({Common.CreateCsv(threadFbIds)})");
		}
		if (threadRecipientIds.Any())
		{
			list.Add($"single_recipient IN ({Common.CreateCsv(threadRecipientIds)})");
		}
		string text = string.Join(" OR ", list);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("threads", Common.Create(ThreadFields, "unified_thread", text));
		dictionary.Add("me", "select id from profile where id = me()");
		return dictionary;
	}

	public static List<string> GetUserWhereClauses()
	{
		List<string> list = new List<string>();
		list.Add("uid in (SELECT participants.user_id FROM #threads)");
		list.Add("uid in (SELECT former_participants.user_id FROM #threads)");
		return list;
	}
}
