using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models
{
	internal class StringPropertyConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer, string propertyName)
		{
			string result = null;
			if (reader.TokenType == JsonToken.StartObject)
			{
				JObject jObject = JObject.Load(reader);
				JToken value;
				if (jObject.TryGetValue(propertyName, out value))
				{
					result = value.ToString();
				}
			}
			return result;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return ReadJson(reader, objectType, existingValue, serializer, "text");
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}
