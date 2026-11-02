using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Target
	{
		[JsonProperty(PropertyName = "__type__")]
		[DataMember]
		[JsonConverter(typeof(FacebookSubTypeConverter))]
		public FacebookSubType Type { get; set; }

		[JsonProperty(PropertyName = "article_author_name")]
		[DataMember]
		public string ArticleAuthorName { get; set; }

		[JsonProperty(PropertyName = "can_viewer_join")]
		[DataMember]
		public bool? CanViewerJoin { get; set; }

		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "canvas_url")]
		public string CanvasUrl { get; set; }

		[JsonProperty(PropertyName = "category_names")]
		[DataMember]
		public List<string> CategoryNames { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "cover_photo")]
		public CoverPhoto CoverPhoto { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "does_viewer_like")]
		public bool? DoesViewerLike { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "domain_icon")]
		public FacebookImage DomainFacebook { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "event_description")]
		public EventDescription EventDescription { get; set; }

		[JsonProperty(PropertyName = "event_place")]
		[DataMember]
		public Place EventPlace { get; set; }

		[JsonProperty(PropertyName = "expressed_as_place")]
		[DataMember]
		public bool? ExpressedAsPlace { get; set; }

		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(PropertyName = "friendship_status")]
		public FriendRequestStatus? FriendshipStatus { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "group_icon")]
		public FacebookImage GroupFacebook { get; set; }

		[JsonProperty(PropertyName = "icon_image")]
		[DataMember]
		public FacebookImage FacebookImage { get; set; }

		[JsonProperty(PropertyName = "is_owned")]
		[DataMember]
		public bool? IsOwned { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "page_likers")]
		[DataMember]
		public ActionCount PageLikers { get; set; }

		[JsonProperty(PropertyName = "place_type")]
		[DataMember]
		public string PlaceType { get; set; }

		[JsonProperty(PropertyName = "primary_object_node")]
		[DataMember]
		public Story PrimaryObjectNode { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "profile_picture")]
		public FacebookImage ProfilePicture { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "privacy_scope")]
		public PrivacyScope PrivacyScope { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "structured_name")]
		public StructuredName StructuredName { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "time_range")]
		public EventTimeRange TimeRange { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }
	}
}
