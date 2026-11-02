using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Services.Api.Fql
{
	public class UnifiedMessage
	{
		private static readonly string[] ActionFields = new string[8] { "thread_id", "type", "actor", "users.user_id", "body", "timestamp", "action_id", "internal_message_id" };

		private static readonly string[] MessageFields = new string[13]
		{
			"thread_id", "message_id", "action_id", "body", "sender", "timestamp", "attachment_map", "share_map", "shares", "coordinates",
			"log_message", "offline_threading_id", "tags"
		};

		private static readonly string[] MessageSyncFields = new string[14]
		{
			"thread_id", "message_id", "action_id", "body", "sender", "timestamp", "attachment_map", "share_map", "shares", "coordinates",
			"log_message", "offline_threading_id", "tags", "sync_change_type"
		};

		private static readonly string[] ThreadReceipts = new string[2] { "thread_id", "read_receipts" };

		private static readonly string[] ThreadInfo = new string[21]
		{
			"thread_id", "read_receipts", "name", "folder", "pic_hash", "mute", "snippet", "snippet_sender", "snippet_message_id", "admin_snippet",
			"senders", "participants", "former_participants", "timestamp", "action_id", "last_visible_add_action_id", "unread", "single_recipient", "can_reply", "is_subscribed",
			"archived"
		};

		private static readonly string[] UserFields = new string[9] { "uid", "name", "first_name", "middle_name", "last_name", "is_pushable", "is_messenger_user", "is_partial", "last_active" };

		public static string GetIndividual(string messageId, string threadId, bool fetchAllThreadInfo = false)
		{
			string text = string.Format("message_id = '{0}'", new object[1] { messageId });
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("messages", Common.Create(MessageFields, "unified_message", text, 0));
			dictionary.Add("users", Common.Create(UserFields, "user", GetUserWhereClause()));
			Dictionary<string, string> queryMap = dictionary;
			return Common.CreateMultiQuery(queryMap);
		}

		public static string Get(string threadId, long sinceTimestamp = 0L, int limit = 10, bool fetchAllThreadInfo = false)
		{
			sinceTimestamp = ((sinceTimestamp < 0) ? 0 : sinceTimestamp);
			string text = CreateWhereClause(threadId, sinceTimestamp);
			return CreateFql(threadId, text, false, limit, fetchAllThreadInfo);
		}

		public static string Sync(string threadId, long actionId, int limit, bool fetchAllThreadInfo = false)
		{
			actionId = ((actionId < 0) ? 0 : actionId);
			string text = CreateWhereClause(threadId, actionId, true);
			return CreateFql(threadId, text, true, limit, fetchAllThreadInfo);
		}

		private static void AddTimeOrActionClause(StringBuilder b, long value, bool isActionId)
		{
			if (value != 0)
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

		private static string CreateWhereClause(string threadId, long timestamp = 0L, bool isActionId = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("thread_id = '{0}'", new object[1] { threadId });
			AddTimeOrActionClause(stringBuilder, timestamp, isActionId);
			return stringBuilder.ToString();
		}

		private static string CreateFql(string threadId, string where, bool sync, int limit = 0, bool fetchAllThreadInfo = false)
		{
			string table = (sync ? "unified_message_sync" : "unified_message");
			string[] fields = (sync ? MessageSyncFields : MessageFields);
			string text = string.Format("thread_id = '{0}'", new object[1] { threadId });
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("messages", Common.Create(fields, table, where, limit));
			dictionary.Add("actions", Common.Create(ActionFields, "unified_thread_action", where, limit));
			dictionary.Add("thread", Common.Create(fetchAllThreadInfo ? ThreadInfo : ThreadReceipts, "unified_thread", text));
			dictionary.Add("users", Common.Create(UserFields, "user", GetUserWhereClause() + " " + ((!fetchAllThreadInfo) ? string.Empty : "OR uid in (SELECT participants.user_id FROM #thread) OR uid in (SELECT former_participants.user_id FROM #thread)")));
			Dictionary<string, string> queryMap = dictionary;
			return Common.CreateMultiQuery(queryMap);
		}

		public static Dictionary<string, string> CreateMessagesQueryMap(IEnumerable<string> messageIds)
		{
			string text = string.Format("message_id IN ({0})", new object[1] { string.Join(",", messageIds.Select((string i) => string.Format("'{0}'", new object[1] { i }))) });
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("messages", Common.Create(MessageFields, "unified_message", text));
			return dictionary;
		}

		public static string GetUserWhereClause()
		{
			return "uid in (SELECT sender.user_id FROM #messages)";
		}
	}
}
