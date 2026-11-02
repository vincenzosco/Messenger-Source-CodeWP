using System.Collections.Generic;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads
{
	[JsonObject(MemberSerialization.OptIn)]
	public class MessagingEventMessage
	{
		[JsonProperty("event")]
		public string Event { get; set; }

		[JsonProperty("tid")]
		public string ThreadId { get; set; }

		[JsonProperty("tids")]
		public List<string> ThreadIds { get; set; }

		[JsonProperty("reader_fbid")]
		public string ReaderId { get; set; }

		[JsonProperty("timestamp")]
		public long Timestamp { get; set; }

		[JsonProperty("time")]
		internal long Time
		{
			get
			{
				return Timestamp;
			}
			set
			{
				Timestamp = value;
			}
		}

		[JsonProperty("from_fbid")]
		public string FromId { get; set; }

		[JsonProperty("mid")]
		public string MessageId { get; set; }
	}
}
