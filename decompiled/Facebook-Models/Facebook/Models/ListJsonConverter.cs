using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models;

internal class ListJsonConverter<T> : JsonConverter
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
			JArray source = JArray.Load(reader);
			list.AddRange(source.Select((JToken i) => i.ToObject<T>()));
		}
		return list;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		IList<T> value2 = value as IList<T>;
		serializer.Serialize(writer, value2);
	}
}
