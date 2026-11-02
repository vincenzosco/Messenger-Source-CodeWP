using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models
{
	public class CollapseNodesConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartObject)
			{
				JObject jObject = JObject.Load(reader);
				JToken value;
				if (jObject.TryGetValue("nodes", out value))
				{
					return value as JArray;
				}
			}
			return null;
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}
