using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class StructuredName
{
	[JsonProperty(PropertyName = "parts")]
	[DataMember]
	public List<NamePart> Parts { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "text")]
	public string Text { get; set; }
}
