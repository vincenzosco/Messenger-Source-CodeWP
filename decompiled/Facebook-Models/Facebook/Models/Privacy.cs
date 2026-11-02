using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Privacy
	{
		[JsonProperty("allow")]
		[DataMember]
		public string Allow { get; set; }

		[DataMember]
		[JsonProperty("deny")]
		public string Deny { get; set; }

		[DataMember]
		[JsonProperty("description")]
		public string Description { get; set; }

		[DataMember]
		[JsonProperty("friends")]
		public string Friends { get; set; }

		[DataMember]
		[JsonProperty("id")]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty("networks")]
		public string Networks { get; set; }

		[DataMember]
		[JsonProperty("object_id")]
		public string ObjectId { get; set; }

		[DataMember]
		[JsonProperty("owner_id")]
		public string OwnerId { get; set; }

		[JsonProperty("value")]
		[DataMember]
		public string Value { get; set; }

		public string OwnId { get; set; }
	}
}
