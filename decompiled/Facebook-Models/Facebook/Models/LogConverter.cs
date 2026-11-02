using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models;

internal class LogConverter : JsonConverter
{
	private const string TypeKey = "type";

	public override bool CanConvert(Type objectType)
	{
		return true;
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		MailMessage.Type result = MailMessage.Type.Regular;
		if (JsonToken.StartObject != reader.TokenType)
		{
			JArray.Load(reader);
			return result;
		}
		JObject jObject = JObject.Load(reader);
		JToken jToken = jObject["type"];
		if (jToken != null)
		{
			string value = jToken.Value<string>();
			if (Enum.TryParse<MailMessage.Type>(value, out result))
			{
				return result;
			}
		}
		JToken jToken2 = jObject["threadPic"];
		JToken jToken3 = jObject["threadName"];
		JToken jToken4 = jObject["videoCallLog"];
		JToken jToken5 = jObject["callLog"];
		if (jToken2 != null)
		{
			result = ((jToken2["deleted"] != null) ? MailMessage.Type.ThreadImageRemoved : MailMessage.Type.ThreadImageSet);
		}
		else if (jToken3 != null)
		{
			result = (string.IsNullOrWhiteSpace(jToken3.Value<string>()) ? MailMessage.Type.ThreadNameRemoved : MailMessage.Type.ThreadNameSet);
		}
		else if (jToken4 != null)
		{
			result = MailMessage.Type.VideoCall;
		}
		else if (jToken5 != null)
		{
			result = (jToken5.HasValues ? jToken5["type"].Value<string>() : null) switch
			{
				"missed" => MailMessage.Type.VoiceCallMissed, 
				_ => MailMessage.Type.VoiceCall, 
			};
		}
		return result;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("type");
		writer.WriteValue(value);
		writer.WriteEndObject();
	}
}
