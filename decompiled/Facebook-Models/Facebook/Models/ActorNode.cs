using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class ActorNode
{
	[JsonProperty(PropertyName = "node")]
	[DataMember]
	public Actor Actor { get; set; }
}
