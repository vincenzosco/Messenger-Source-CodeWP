using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class WorkExperience : IAboutType
	{
		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "time_range")]
		[DataMember]
		public Dictionary<string, string> TimeRange { get; set; }

		[JsonProperty(PropertyName = "message")]
		[JsonConverter(typeof(StringPropertyConverter))]
		[DataMember]
		public string Message { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "location_page")]
		[JsonConverter(typeof(NameStringPropertyConverter))]
		public string Location { get; set; }

		[JsonProperty(PropertyName = "work_projects")]
		[DataMember]
		[JsonConverter(typeof(WorkProjectsConverter))]
		public List<WorkProject> WorkProjects { get; set; }

		public AboutType Type
		{
			get
			{
				return AboutType.WorkExperience;
			}
		}
	}
}
