using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class AffiliationsInfo
{
	public static long FacebookAffiliationId = 50431648L;

	[DataMember]
	[JsonProperty(PropertyName = "name")]
	public string Name { get; set; }

	[JsonProperty(PropertyName = "nid")]
	[DataMember]
	public long Id { get; set; }
}
