using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Fql;

internal class MultiResponseConverter : JsonConverter
{
	private readonly Dictionary<string, Type> _typeMap;

	public override bool CanWrite => false;

	public MultiResponseConverter(Dictionary<string, Type> nameToTypes)
	{
		_typeMap = nameToTypes;
	}

	public override bool CanConvert(Type objectType)
	{
		return (object)objectType == typeof(MultiQueryData);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		MultiQueryData multiQueryData = new MultiQueryData();
		if (reader.TokenType == JsonToken.Null)
		{
			return multiQueryData;
		}
		JToken jToken = JToken.ReadFrom(reader);
		foreach (JToken item in (IEnumerable<JToken>)jToken)
		{
			string key = (string)item["name"];
			if (_typeMap.ContainsKey(key))
			{
				Type type = _typeMap[key];
				Type objectType2 = typeof(List<>).MakeGenericType(type);
				_ = item["fql_result_set"];
				object value = item["fql_result_set"].ToObject(objectType2, serializer);
				multiQueryData[key] = value;
			}
		}
		return multiQueryData;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
