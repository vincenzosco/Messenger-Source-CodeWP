using System;
using System.Runtime.Serialization;
using Facebook.Models.JsonConverters;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FacebookFriendRequest
	{
		[DataMember]
		[JsonProperty(PropertyName = "uid_from")]
		public string UidFrom { get; set; }

		[JsonConverter(typeof(UnixDateTimeConverter))]
		[DataMember]
		[JsonProperty(PropertyName = "time")]
		public DateTime Time { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "unread")]
		public bool? Unread { get; set; }

		[JsonProperty(PropertyName = "is_hidden")]
		[DataMember]
		public bool? IsHidden { get; set; }

		public bool? Confirmed { get; set; }

		public FacebookUser User { get; set; }
	}
}
