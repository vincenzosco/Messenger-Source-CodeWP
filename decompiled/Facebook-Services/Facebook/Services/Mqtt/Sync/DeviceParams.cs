using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Mqtt.Sync;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class DeviceParams
{
	[JsonProperty("image_sizes")]
	[DataMember]
	public Dictionary<string, string> ImageSizes
	{
		get
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("1", "960x960");
			dictionary.Add("2", "206x206");
			return dictionary;
		}
	}
}
