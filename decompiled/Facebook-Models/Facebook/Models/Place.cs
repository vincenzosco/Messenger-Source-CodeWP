using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class Place
{
	[JsonConverter(typeof(FacebookSubTypeConverter))]
	[DataMember]
	[JsonProperty(PropertyName = "__type__")]
	public FacebookSubType Type { get; set; }

	[JsonProperty(PropertyName = "address")]
	[DataMember]
	public Address Address { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "category_names")]
	public List<string> CategoryNames { get; set; }

	[JsonProperty(PropertyName = "contextual_name")]
	[DataMember]
	public string ContextualName { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "description")]
	public string Description { get; set; }

	[JsonProperty(PropertyName = "does_viewer_like")]
	[DataMember]
	public bool? DoesViewerLike { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "expressed_as_place")]
	public bool? ExpressedAsPlace { get; set; }

	[JsonProperty(PropertyName = "is_owned")]
	[DataMember]
	public bool? IsOwned { get; set; }

	[JsonProperty(PropertyName = "place_type")]
	[DataMember]
	public string PlaceType { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "profile_picture")]
	public FacebookImage ProfilePicture { get; set; }

	[JsonProperty(PropertyName = "profile_picture_is_silhouette")]
	[DataMember]
	public bool? ProfilePictureIsSilhouette { get; set; }

	[JsonProperty(PropertyName = "super_category_type")]
	[DataMember]
	public string SuperCategoryType { get; set; }

	[JsonProperty(PropertyName = "id")]
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "url")]
	public string Url { get; set; }

	[JsonProperty(PropertyName = "name")]
	[DataMember]
	public string Name { get; set; }
}
