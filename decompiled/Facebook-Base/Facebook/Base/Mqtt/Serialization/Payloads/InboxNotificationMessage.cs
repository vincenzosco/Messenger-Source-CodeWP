using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class InboxNotificationMessage
{
	[JsonProperty("type")]
	[DataMember]
	public string Type { get; set; }

	[DataMember]
	[JsonProperty("unread")]
	public long Unread { get; set; }

	[DataMember]
	[JsonProperty("unseen")]
	public long Unseen { get; set; }

	[JsonProperty("other_unread")]
	[DataMember]
	public long OtherUnread { get; set; }

	[DataMember]
	[JsonProperty("other_unseen")]
	public long OtherUnseen { get; set; }

	[DataMember]
	[JsonProperty("seen_timestamp")]
	public long SeenTimestamp { get; set; }
}
