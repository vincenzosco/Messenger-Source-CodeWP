using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Methods.MediaItemUpload
{
	public class FbidResponseConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Response response = new Response();
			if (reader.TokenType == JsonToken.String)
			{
				JToken jToken = JToken.Load(reader);
				if (jToken.Type == JTokenType.String)
				{
					response.Fbid = jToken.ToObject<string>();
				}
			}
			else
			{
				serializer.Populate(reader, response);
			}
			return response;
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}
