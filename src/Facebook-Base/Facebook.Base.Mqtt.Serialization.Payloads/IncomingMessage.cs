using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class IncomingMessage
	{
		[JsonProperty("type")]
		[DataMember]
		public string Type { get; set; }

		[JsonProperty("tid")]
		[DataMember]
		public string ThreadId { get; set; }

		[DataMember]
		[JsonProperty("mid")]
		public string MessageId { get; set; }

		[DataMember]
		[JsonProperty("sender_fbid")]
		public string SenderId { get; set; }

		[JsonProperty("sender_name")]
		[DataMember]
		public string SenderName { get; set; }

		[DataMember]
		[JsonProperty("offline_threading_id")]
		public string OfflineThreadingId { get; set; }

		[DataMember]
		[JsonProperty("body")]
		public string Body { get; set; }

		[JsonProperty("action_id")]
		[DataMember]
		public long ActionId { get; set; }

		[JsonProperty("prev_last_visible_action_id")]
		[DataMember]
		public string LastVisibleActionId { get; set; }

		[DataMember]
		[JsonProperty("timestamp")]
		public long Timestamp { get; set; }

		[JsonProperty("coordinates")]
		[DataMember]
		public MessageCoordinates Coordinates { get; set; }

		[JsonProperty("short_source")]
		[DataMember]
		public long ShortSource { get; set; }

		[JsonProperty("api_tags")]
		[DataMember]
		public List<string> ApiTags { get; set; }

		[JsonProperty("group_thread_info")]
		[DataMember]
		public MessageGroupInfo GroupThreadInfo { get; set; }

		[JsonProperty("share_map")]
		[DataMember]
		[JsonConverter(typeof(MapJsonConverter<Share>))]
		public List<Share> Shares { get; set; }

		[JsonProperty("admin_snippet")]
		[DataMember]
		public string AdminSnippet { get; set; }

		[DataMember]
		[JsonProperty("has_attachment")]
		public bool HasAttachment { get; set; }

		[DataMember]
		[JsonConverter(typeof(MapJsonConverter<Attachment>))]
		[JsonProperty("attachment_map")]
		public List<Attachment> Attachments { get; set; }
	}
}
