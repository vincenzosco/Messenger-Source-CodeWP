using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

public class EducationExperience : IAboutType
{
	[JsonConverter(typeof(StringPropertyConverter))]
	[DataMember]
	[JsonProperty(PropertyName = "message")]
	public string Message { get; set; }

	[JsonConverter(typeof(SchoolClassesConverter))]
	[JsonProperty(PropertyName = "school_classes")]
	[DataMember]
	public List<SchoolClass> SchoolClasses { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "time_range")]
	public Dictionary<string, string> TimeRange { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "has_graduated")]
	public bool HasGraduated { get; set; }

	public AboutType Type => AboutType.EducationExperience;
}
