using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class FeaturedObject
{
	[JsonConverter(typeof(FacebookSubTypeConverter))]
	[DataMember]
	[JsonProperty(PropertyName = "__type__")]
	public FacebookSubType? Type { get; set; }

	[JsonProperty(PropertyName = "id")]
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "name")]
	public string Name { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "profile_picture")]
	public FacebookImage ProfilePicture { get; set; }
}
