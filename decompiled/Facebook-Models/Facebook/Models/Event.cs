using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Models.JsonConverters;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Event
	{
		[JsonProperty("id")]
		[DataMember]
		public string EventId { get; set; }

		[JsonProperty("name")]
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		[JsonProperty("can_view_members")]
		public bool CanViewMembers { get; set; }

		[JsonProperty("can_viewer_join")]
		[DataMember]
		public bool CanViewerJoin { get; set; }

		[DataMember]
		[JsonProperty("can_viewer_invite")]
		public bool CanViewerInvite { get; set; }

		[JsonProperty("event_description")]
		[DataMember]
		public StringEntity EventDescription { get; set; }

		[DataMember]
		[JsonProperty("privacy_scope")]
		public PrivacyScope PrivacyScope { get; set; }

		[JsonProperty("cover_photo")]
		[DataMember]
		public CoverPhoto CoverPhoto { get; set; }

		[JsonProperty("event_creator")]
		[DataMember]
		public Actor EventCreator { get; set; }

		[DataMember]
		[JsonProperty("event_hosts")]
		public EventUserNode EventHosts { get; set; }

		[JsonProperty("event_invitees")]
		[DataMember]
		public EventUserNode EventInvitees { get; set; }

		[JsonProperty("event_maybes")]
		[DataMember]
		public EventUserNode EventMaybes { get; set; }

		[JsonProperty("event_members")]
		[DataMember]
		public EventUserNode EventMembers { get; set; }

		[DataMember]
		[JsonProperty("time_range")]
		public EventTimeRange TimeRange { get; set; }

		[DataMember]
		[JsonProperty("event_place")]
		public Place EventPlace { get; set; }

		[JsonProperty("viewer_inviters")]
		[DataMember]
		public List<Actor> Inviters { get; set; }

		[JsonProperty("viewer_guest_status")]
		[JsonConverter(typeof(EventRSVPStatusConverter))]
		[DataMember]
		public RSVPStatus? RsvpStatus { get; set; }

		public bool IsDateOnly { get; set; }

		public DateTime? StartTime { get; set; }

		public DateTime? EndTime { get; set; }
	}
}
