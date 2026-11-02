using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models
{
	public class MuteJsonConvertor : JsonConverter
	{
		private const string FacebookUserEmailPostfix = "@facebook.com";

		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Dictionary<string, long> dictionary = new Dictionary<string, long>();
			switch (reader.TokenType)
			{
			case JsonToken.Null:
				return null;
			case JsonToken.StartArray:
				JArray.Load(reader);
				break;
			case JsonToken.StartObject:
			{
				JObject jObject = JObject.Load(reader);
				foreach (KeyValuePair<string, JToken> item in jObject)
				{
					int num = item.Key.IndexOf("@facebook.com", StringComparison.Ordinal);
					if (num >= 0)
					{
						string key = item.Key.Substring(0, num);
						dictionary[key] = (long)item.Value;
					}
				}
				break;
			}
			}
			return dictionary;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Dictionary<string, long> dictionary = new Dictionary<string, long>();
			Dictionary<string, long> dictionary2 = value as Dictionary<string, long>;
			if (dictionary2 != null)
			{
				foreach (KeyValuePair<string, long> item in dictionary2)
				{
					dictionary[item.Key + "@facebook.com"] = item.Value;
				}
			}
			serializer.Serialize(writer, dictionary);
		}
	}
}
