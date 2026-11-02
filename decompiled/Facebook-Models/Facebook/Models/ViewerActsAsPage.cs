using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class ViewerActsAsPage
{
	[JsonProperty(PropertyName = "id")]
	[DataMember]
	public string Id;

	[DataMember]
	[JsonProperty(PropertyName = "name")]
	public string Name;
}
