using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class NamePart
{
	[JsonProperty(PropertyName = "length")]
	[DataMember]
	public int Length { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "offset")]
	public int Offset { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "part")]
	public string Text { get; set; }
}
