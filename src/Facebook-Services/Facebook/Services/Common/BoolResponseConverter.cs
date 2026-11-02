using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Common
{
	public class BoolResponseConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			BoolResponse boolResponse = new BoolResponse();
			if (reader.TokenType == JsonToken.Boolean)
			{
				JToken jToken = JToken.Load(reader);
				if (jToken.Type == JTokenType.Boolean)
				{
					boolResponse.Succeeded = jToken.ToObject<bool>();
				}
			}
			else
			{
				serializer.Populate(reader, boolResponse);
			}
			return boolResponse;
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}
