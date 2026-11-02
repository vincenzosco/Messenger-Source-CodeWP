using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Friend
	{
		[DataMember]
		[JsonProperty("uid2")]
		public string Uid2 { get; set; }

		[JsonProperty("communication_rank")]
		[DataMember]
		public double CommunicationRank { get; set; }

		[JsonProperty("is_removed")]
		[DataMember]
		public bool IsRemoved { get; set; }
	}
}
