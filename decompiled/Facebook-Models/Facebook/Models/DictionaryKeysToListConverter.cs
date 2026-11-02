using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class DictionaryKeysToListConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			List<string> result = null;
			if (reader.TokenType == JsonToken.StartObject)
			{
				Dictionary<string, string> dictionary = serializer.Deserialize<Dictionary<string, string>>(reader);
				result = dictionary.Keys.ToList();
			}
			return result;
		}

		public override bool CanConvert(Type objectType)
		{
			return (object)objectType == typeof(List<string>);
		}
	}
}
