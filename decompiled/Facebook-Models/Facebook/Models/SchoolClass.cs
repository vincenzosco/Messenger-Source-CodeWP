using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

public class SchoolClass
{
	[JsonConverter(typeof(ExperienceWithConverter))]
	[JsonProperty(PropertyName = "experience_with")]
	[DataMember]
	public List<FacebookProfile> ExperienceWith { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "school_class")]
	[JsonConverter(typeof(NameStringPropertyConverter))]
	public string Name { get; set; }

	[JsonConverter(typeof(StringPropertyConverter))]
	[DataMember]
	[JsonProperty(PropertyName = "message")]
	public string Message { get; set; }
}
