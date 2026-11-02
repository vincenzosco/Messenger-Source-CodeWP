using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class SectionInfo
{
	[JsonProperty(PropertyName = "approximate_new_unit_count")]
	[DataMember]
	public int ApproxNewCount { get; set; }

	public string DisplayCount
	{
		get
		{
			if (ApproxNewCount == 0)
			{
				return string.Empty;
			}
			if (ApproxNewCount <= 20)
			{
				return ApproxNewCount.ToString();
			}
			return "20+";
		}
	}
}
