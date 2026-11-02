using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class MailFolderCount
	{
		[JsonProperty("largest_saved_action_id")]
		[DataMember]
		public long LargestSavedThreadActionId;

		[DataMember]
		[JsonProperty("folder")]
		public string Folder { get; set; }

		[DataMember]
		[JsonProperty("unseen_count")]
		public long UnseenCount { get; set; }

		[DataMember]
		[JsonProperty("unread_count")]
		public long UnreadCount { get; set; }

		[JsonProperty("last_action_id")]
		[DataMember]
		public long LastActionId { get; set; }

		[DataMember]
		[JsonProperty("refetch_action_id")]
		public long RefetchActionId { get; set; }

		[DataMember]
		[JsonProperty("last_seen_time")]
		public long LastSeenTime { get; set; }

		public static MailFolderCount CreateCopy(MailFolderCount m)
		{
			MailFolderCount mailFolderCount = new MailFolderCount();
			mailFolderCount.Folder = m.Folder;
			mailFolderCount.UnseenCount = m.UnseenCount;
			mailFolderCount.UnreadCount = m.UnreadCount;
			mailFolderCount.LastActionId = m.LastActionId;
			mailFolderCount.RefetchActionId = m.RefetchActionId;
			mailFolderCount.LastSeenTime = m.LastSeenTime;
			mailFolderCount.LargestSavedThreadActionId = m.LargestSavedThreadActionId;
			return mailFolderCount;
		}
	}
}
