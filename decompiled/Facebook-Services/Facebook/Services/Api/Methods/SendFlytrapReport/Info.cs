using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SendFlytrapReport;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class Info
{
	[DataMember]
	[JsonProperty("Git_Hash")]
	public string GitHash { get; set; }

	[DataMember]
	[JsonProperty("Git_Branch")]
	public string GitBranch { get; set; }

	[DataMember]
	[JsonProperty("Build_Time")]
	public string BuildTime { get; set; }

	[DataMember]
	[JsonProperty("Build_Num")]
	public string BuildNumber { get; set; }

	[DataMember]
	[JsonProperty("OS_Version")]
	public string OSVersion { get; set; }

	[JsonProperty("Manufacturer")]
	[DataMember]
	public string Manufacturer { get; set; }

	[DataMember]
	[JsonProperty("Model")]
	public string Model { get; set; }

	[DataMember]
	[JsonProperty("Locale")]
	public string Locale { get; set; }

	[JsonProperty("Zombie(s)")]
	[DataMember]
	public string Zombies { get; set; }
}
