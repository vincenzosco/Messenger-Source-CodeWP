using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Mqtt.Sync
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class CreateSyncDiffsMessage : SyncMessageBase
	{
		[JsonProperty("sync_token")]
		[DataMember]
		public string SyncToken { get; private set; }

		[JsonProperty("last_seq_id")]
		[DataMember]
		public long LastSequenceId { get; private set; }

		public CreateSyncDiffsMessage(string syncToken, long lastSequenceId)
		{
			SyncToken = syncToken;
			LastSequenceId = lastSequenceId;
		}
	}
}
