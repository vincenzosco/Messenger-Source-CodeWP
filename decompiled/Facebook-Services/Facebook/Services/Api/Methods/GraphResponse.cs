using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class GraphResponse : FacebookApiResponse
	{
		[JsonObject(MemberSerialization.OptIn)]
		[DataContract]
		public class Header
		{
			[JsonProperty("name")]
			[DataMember]
			public string Name { get; set; }

			[DataMember]
			[JsonProperty("value")]
			public string Value { get; set; }
		}

		[JsonProperty("code")]
		[DataMember]
		public int ResponseCode { get; set; }

		[JsonProperty("headers")]
		[DataMember]
		public List<Header> Headers { get; set; }

		[DataMember]
		[JsonProperty("body")]
		public string RawBody { get; set; }

		public object Body { get; set; }
	}
}
