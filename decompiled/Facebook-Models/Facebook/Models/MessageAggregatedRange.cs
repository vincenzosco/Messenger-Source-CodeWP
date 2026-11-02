using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

public class MessageAggregatedRange : BaseRange
{
	[DataMember]
	[JsonProperty(PropertyName = "sample_entities")]
	public List<Actor> SampleEntities { get; set; }
}
