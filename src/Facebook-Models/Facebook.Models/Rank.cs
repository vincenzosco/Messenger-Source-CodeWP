using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Rank
	{
		[JsonProperty("uid2")]
		[DataMember]
		public string Uid2 { get; set; }

		[JsonProperty("communication_rank")]
		[DataMember]
		public double CommunicationRank { get; set; }
	}
}
