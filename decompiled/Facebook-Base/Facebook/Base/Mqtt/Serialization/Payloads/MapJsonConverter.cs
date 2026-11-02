using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Base.Mqtt.Serialization.Payloads;

internal class MapJsonConverter<T> : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return true;
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		List<T> list = new List<T>();
		if (reader.TokenType == JsonToken.StartObject)
		{
			JObject jObject = JObject.Load(reader);
			foreach (JToken item2 in jObject.PropertyValues())
			{
				T item = item2.ToObject<T>(serializer);
				list.Add(item);
			}
		}
		else if (reader.TokenType == JsonToken.StartArray)
		{
			JArray.Load(reader);
		}
		return list;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
