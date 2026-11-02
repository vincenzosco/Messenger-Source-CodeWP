using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class ReportInfo
	{
		[JsonProperty(PropertyName = "action")]
		[DataMember]
		public string Action { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "action_uri")]
		public string ActionUrl { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "undo_action")]
		public string UndoAction { get; set; }
	}
}
