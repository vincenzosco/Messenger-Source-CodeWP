using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Inviter
	{
		[DataMember]
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("uid")]
		[DataMember]
		public string Uid { get; set; }
	}
}
