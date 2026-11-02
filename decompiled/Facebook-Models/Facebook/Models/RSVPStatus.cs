using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public enum RSVPStatus
	{
		Going,
		NotGoing,
		Maybe,
		Invited
	}
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class RsvpStatus
	{
		[DataMember]
		[JsonProperty("eid")]
		public string EventId { get; set; }

		[JsonProperty("inviter")]
		[DataMember]
		public string InviterId { get; set; }

		[JsonProperty("inviter_type")]
		[DataMember]
		public string InviterType { get; set; }

		[DataMember]
		[JsonProperty("rsvp_status")]
		public string Status { get; set; }

		[JsonProperty("uid")]
		[DataMember]
		public string Uid { get; set; }
	}
}
