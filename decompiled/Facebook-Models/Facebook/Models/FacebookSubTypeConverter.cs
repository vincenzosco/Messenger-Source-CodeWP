using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models;

public class FacebookSubTypeConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		if (value is FacebookSubType facebookSubType)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("name");
			writer.WriteValue(facebookSubType.ToString());
			writer.WriteEndObject();
		}
		else
		{
			writer.WriteNull();
		}
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.StartObject)
		{
			JObject jObject = JObject.Load(reader);
			if (jObject.HasValues && jObject.First.Type == JTokenType.Property)
			{
				JToken first = jObject.First;
				if (first.HasValues && first.First.Type == JTokenType.String)
				{
					first = first.First;
					string text = first.ToString();
					if (text != null)
					{
						Enum.TryParse<FacebookSubType>(text, ignoreCase: true, out var result);
						return result;
					}
				}
			}
		}
		return FacebookSubType.Unknown;
	}

	public override bool CanConvert(Type objectType)
	{
		return (object)objectType == typeof(FacebookSubType);
	}
}
