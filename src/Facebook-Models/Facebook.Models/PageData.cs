using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class PageData
	{
		[DataMember]
		[JsonProperty(PropertyName = "timeline_sections")]
		public TimelineSections TimelineSections { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "cover_photo")]
		public CoverPhoto CoverPhoto { get; set; }

		[JsonProperty(PropertyName = "profile_photo")]
		[DataMember]
		public FeedPhoto ProfilePhoto { get; set; }

		[JsonProperty("can_viewer_post")]
		[DataMember]
		public bool? CanViewerPost { get; set; }

		[JsonProperty("does_viewer_like")]
		[DataMember]
		public bool? DoesViewerLike { get; set; }

		[JsonProperty(PropertyName = "page_likers")]
		[DataMember]
		public ActionCount Likers { get; set; }

		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "is_verified")]
		public bool? IsVerified { get; set; }

		[JsonProperty(PropertyName = "about")]
		[DataMember]
		public StringEntity ShortDescription { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "long_description")]
		public StringEntity LongDescription { get; set; }

		[JsonProperty(PropertyName = "category_names")]
		[DataMember]
		public List<string> CategoryNames { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "short_category_names")]
		public List<string> ShortCategoryNames { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "page_visits")]
		public ActionCount PageVisits { get; set; }

		[JsonProperty(PropertyName = "viewer_visits")]
		[DataMember]
		public ActionCount ViewerVisits { get; set; }

		[JsonProperty(PropertyName = "address")]
		[DataMember]
		public Address Address { get; set; }
	}
}
