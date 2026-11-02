using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SyncContacts
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class ContactChangeResult
	{
		[DataMember]
		[JsonProperty("contact")]
		public string RemoteContactId;

		[JsonProperty("match_confidence")]
		[DataMember]
		public string MatchConfidence;

		[JsonProperty("update_type")]
		[DataMember]
		public string UpdateType { get; set; }
	}
}
