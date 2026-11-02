using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Actor
	{
		[JsonProperty(PropertyName = "__type__")]
		[JsonConverter(typeof(FacebookSubTypeConverter))]
		[DataMember]
		public FacebookSubType Type { get; set; }

		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "url")]
		[DataMember]
		public string Url { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "cover_photo")]
		public CoverPhoto CoverPhoto { get; set; }

		[JsonProperty(PropertyName = "group_icon")]
		[DataMember]
		public GroupIcon GroupIcon { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "does_viewer_like")]
		public bool? DoesViewerLike { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "expressed_as_place")]
		public bool? ExpressedAsPlace { get; set; }

		[JsonProperty(PropertyName = "friendship_status")]
		[JsonConverter(typeof(StringEnumConverter))]
		[DataMember]
		public FriendRequestStatus? FriendshipStatus { get; set; }

		[JsonProperty(PropertyName = "is_owned")]
		[DataMember]
		public bool? IsOwned { get; set; }

		[JsonProperty(PropertyName = "structured_name")]
		[DataMember]
		public StructuredName StructuredName { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }

		[JsonProperty(PropertyName = "mutual_friends")]
		[DataMember]
		public ActionCount MutualFriends { get; set; }
	}
}
