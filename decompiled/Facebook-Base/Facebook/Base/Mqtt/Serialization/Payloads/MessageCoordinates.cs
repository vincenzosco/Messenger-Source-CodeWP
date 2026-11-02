using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class MessageCoordinates
{
	[JsonProperty("latitude")]
	[DataMember]
	public double Latitude { get; set; }

	[JsonProperty("longitude")]
	[DataMember]
	public double Longitude { get; set; }

	[DataMember]
	[JsonProperty("accuracy")]
	public double Accuracy { get; set; }
}
