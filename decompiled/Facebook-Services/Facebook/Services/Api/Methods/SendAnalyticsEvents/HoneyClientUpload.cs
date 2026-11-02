using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Facebook.Base;
using Facebook.Services.Analytics;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SendAnalyticsEvents;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class HoneyClientUpload
{
	[JsonProperty("time")]
	[DataMember]
	public string GeneratedTime { get; private set; }

	[JsonProperty("app_ver")]
	[DataMember]
	public string ApplicationVersion { get; private set; }

	[JsonProperty("build_num")]
	[DataMember]
	public string BuildNumber { get; private set; }

	[JsonProperty("session_id")]
	[DataMember]
	public string SessionId { get; private set; }

	[JsonProperty("seq")]
	[DataMember]
	public string SequenceNumber { get; private set; }

	[DataMember]
	[JsonProperty("uid")]
	public string UserId { get; private set; }

	[DataMember]
	[JsonProperty("app_id")]
	public string ApplicationId { get; private set; }

	[DataMember]
	[JsonProperty("device_id")]
	public string DeviceId { get; private set; }

	[JsonProperty("carrier")]
	[DataMember]
	public string Carrier { get; private set; }

	[DataMember]
	[JsonProperty("device")]
	public string DeviceModelName { get; private set; }

	[DataMember]
	[JsonProperty("os_ver")]
	public string OperatingSystemVersion { get; private set; }

	[DataMember]
	[JsonProperty("conn")]
	public string ConnectionType { get; private set; }

	[JsonProperty("sent_time")]
	[DataMember]
	public string SentTime { get; private set; }

	[JsonProperty("data")]
	[DataMember]
	public IEnumerable<AnalyticsEvent> Data { get; private set; }

	public int MaxEventLogId { get; private set; }

	public HoneyClientUpload(string sessionId, Request request, int sequence, IEnumerable<AnalyticsEvent> data)
	{
		GeneratedTime = request.GeneratedTime.ToString();
		ApplicationVersion = request.ApplicationVersion;
		BuildNumber = request.BuildNumber;
		SessionId = sessionId;
		SequenceNumber = sequence.ToString();
		UserId = request.UserId;
		ApplicationId = request.ApplicationId;
		DeviceId = request.DeviceId;
		Carrier = request.Carrier;
		DeviceModelName = request.DeviceModelName;
		OperatingSystemVersion = request.OperatingSystemVersion;
		ConnectionType = request.ConnectionType;
		SentTime = DateTime.Now.ToUnixTimeMilliseconds().ToString();
		Data = data;
		MaxEventLogId = Data.Last().Id;
	}
}
