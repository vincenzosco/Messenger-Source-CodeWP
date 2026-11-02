using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class GroupStoryNodes
{
	[JsonProperty("node")]
	[DataMember]
	public Story Story { get; set; }
}
