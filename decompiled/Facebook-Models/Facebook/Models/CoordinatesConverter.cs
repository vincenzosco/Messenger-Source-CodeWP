using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Facebook.Models;

public class CoordinatesConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		Dictionary<string, double> result = null;
		if (reader.TokenType == JsonToken.StartObject)
		{
			result = serializer.Deserialize<Dictionary<string, double>>(reader);
		}
		return result;
	}

	public override bool CanConvert(Type objectType)
	{
		return (object)objectType == typeof(List<string>);
	}
}
