using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class MessageGroupInfo
	{
		[JsonProperty("participant_ids")]
		[DataMember]
		public List<string> ParticipantIds { get; set; }

		[JsonProperty("participant_names")]
		[DataMember]
		public List<string> ParticipantNames { get; set; }

		[JsonProperty("participant_total_count")]
		[DataMember]
		public long ParticipantTotalCount { get; set; }

		[DataMember]
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("pic_hash")]
		[DataMember]
		public string PicHash { get; set; }
	}
}
