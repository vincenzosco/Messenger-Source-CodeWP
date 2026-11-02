using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class Group
{
	[JsonProperty("count")]
	[DataMember]
	public int? Count { get; set; }
}
