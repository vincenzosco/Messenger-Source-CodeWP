using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Facebook.Base.Logging;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Cache
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class ThreadCachedData
	{
		[DataMember]
		[JsonProperty("threads")]
		public IDictionary<string, CachedThreadInfo> Threads { get; set; }

		[DataMember]
		[JsonProperty("pinned_groups")]
		public List<PinnedThread> PinnedThreads { get; set; }

		[DataMember]
		[JsonProperty("counts")]
		public MailFolderCount Counts { get; set; }

		[DataMember]
		[JsonProperty("largest_sequence_id")]
		public long LargestSyncSequenceId { get; set; }

		public ThreadCachedData()
		{
			Clear();
		}

		public void Clear()
		{
			ClearInboxData();
			PinnedThreads = new List<PinnedThread>();
		}

		public void ClearInboxData()
		{
			Logger.Info(this, "ThreadCache Inbox data cleared.", "ClearInboxData", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCachedData.cs", 40);
			Threads = new Dictionary<string, CachedThreadInfo>();
			Counts = new MailFolderCount();
			ClearLargestSequenceId();
		}

		public void ClearAllMessages()
		{
			foreach (CachedThreadInfo value in Threads.Values)
			{
				value.ClearMessages();
			}
		}

		public void ClearLargestSequenceId()
		{
			LargestSyncSequenceId = 0L;
		}

		public void TrimThreadsAndMessages()
		{
			Threads = Threads.Values.OrderByDescending((CachedThreadInfo i) => i.Thread.Timestamp).Take(20).ToDictionary((CachedThreadInfo i) => i.Thread.Id);
			foreach (CachedThreadInfo value in Threads.Values)
			{
				value.TrimToMostRecentAndUnsentMessages(50, 20);
			}
			Logger.Info(this, string.Format("Thread Cache finished trimming. Number of threads to save: {0}", new object[1] { Threads.Count }), "TrimThreadsAndMessages", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCachedData.cs", 74);
		}
	}
}
