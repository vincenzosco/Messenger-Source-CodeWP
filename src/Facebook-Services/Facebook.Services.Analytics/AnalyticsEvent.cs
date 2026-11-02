using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Services.Analytics
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class AnalyticsEvent
	{
		[JsonProperty("log_type")]
		[DataMember]
		public string LogType = "client_event";

		public int Id { get; set; }

		[DataMember]
		[JsonProperty("time")]
		public long Time { get; set; }

		[DataMember]
		[JsonProperty("name")]
		public string Name { get; private set; }

		[JsonProperty("module")]
		[DataMember]
		public string Module { get; set; }

		[DataMember]
		[JsonProperty("session_id")]
		public string SessionId { get; set; }

		[DataMember]
		[JsonProperty("extra")]
		public IDictionary<string, string> ExtraData { get; set; }

		public AnalyticsEvent(string name, string module)
		{
			Name = name;
			Time = DateTime.Now.ToUnixTimeMilliseconds();
			Module = module;
			ExtraData = new Dictionary<string, string>();
		}

		public AnalyticsEvent AddData(string key, string value)
		{
			ExtraData.Add(key, value);
			return this;
		}
	}
}
