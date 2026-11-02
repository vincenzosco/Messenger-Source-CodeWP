using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class SelectableFeedSectionsViewer
{
	[JsonProperty(PropertyName = "selectable_feed_sections")]
	[DataMember]
	public SelectableFeedSections SelectableSections { get; set; }
}
