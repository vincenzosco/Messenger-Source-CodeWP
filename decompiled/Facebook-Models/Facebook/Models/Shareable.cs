using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class Shareable
{
	[DataMember]
	[JsonConverter(typeof(FacebookSubTypeConverter))]
	[JsonProperty(PropertyName = "__type__")]
	public FacebookSubType Type { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "id")]
	public string Id { get; set; }
}
