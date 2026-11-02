using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class GroupVisibility
{
	[JsonProperty("text")]
	[DataMember]
	public string Text { get; set; }
}
