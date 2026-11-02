using System;
using System.Runtime.Serialization;
using Facebook.Models.JsonConverters;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class EventListItemModel : IEventType
	{
		public EventType EventType { get; set; }

		[DataMember]
		[JsonProperty("eid")]
		public string EventId { get; set; }

		[JsonProperty("name")]
		[DataMember]
		public string EventName { get; set; }

		[JsonProperty("description")]
		[DataMember]
		public string Description { get; set; }

		[JsonProperty("creator")]
		[DataMember]
		public string HostId { get; set; }

		[JsonProperty("host")]
		[DataMember]
		public string HostName { get; set; }

		[DataMember]
		[JsonProperty("pic")]
		public string Picture { get; set; }

		[DataMember]
		[JsonProperty("pic_big")]
		public string PictureBig { get; set; }

		[DataMember]
		[JsonProperty("inviter")]
		public string InviterId { get; set; }

		public string InviterName { get; set; }

		[JsonProperty("inviter_type")]
		[DataMember]
		public string InviterType { get; set; }

		[DataMember]
		[JsonProperty("has_profile_pic")]
		public bool HasProfilePic { get; set; }

		[JsonProperty("can_invite_friends")]
		[DataMember]
		public bool CanInviteFriends { get; set; }

		[DataMember]
		[JsonProperty("declined_count")]
		public int DeclinedCount { get; set; }

		[JsonProperty("attending_count")]
		[DataMember]
		public int AttendingCount { get; set; }

		[JsonProperty("unsure_count")]
		[DataMember]
		public int UnsureCount { get; set; }

		[DataMember]
		[JsonProperty("not_replied_count")]
		public int NotRepliedCount { get; set; }

		[JsonProperty("all_members_count")]
		[DataMember]
		public int AllMembersCount { get; set; }

		[JsonProperty("location")]
		[DataMember]
		public string Location { get; set; }

		[JsonProperty("privacy")]
		[DataMember]
		public string Privacy { get; set; }

		[DataMember]
		[JsonProperty("is_date_only")]
		public bool IsDateOnly { get; set; }

		[JsonConverter(typeof(FormattedDateTimeConverter))]
		[DataMember]
		[JsonProperty("start_time")]
		public DateTime StartTime { get; set; }

		[JsonProperty("end_time")]
		[JsonConverter(typeof(FormattedDateTimeConverter))]
		[DataMember]
		public DateTime EndTime { get; set; }

		[DataMember]
		[JsonProperty("update_time")]
		[JsonConverter(typeof(FormattedDateTimeConverter))]
		public DateTime UpdateTime { get; set; }

		[JsonProperty("rsvp_status")]
		[DataMember]
		public string RsvpStatus { get; set; }
	}
}
