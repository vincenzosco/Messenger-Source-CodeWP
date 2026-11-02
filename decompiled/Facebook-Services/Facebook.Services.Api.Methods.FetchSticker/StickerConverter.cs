using System;
using Facebook.Base;
using Facebook.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Methods.FetchSticker
{
	public class StickerConverter : JsonConverter
	{
		private readonly string _stickerId;

		public StickerConverter(string id)
		{
			_stickerId = id;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Response response = new Response();
			if (reader.TokenType == JsonToken.StartObject)
			{
				JObject jObject = JObject.Load(reader);
				JToken nestedDescendant = jObject.GetNestedDescendant(_stickerId);
				if (nestedDescendant != null)
				{
					response.Sticker = nestedDescendant.ToObject<Sticker>();
				}
				else
				{
					response = FacebookApiResponse.FromApiResponse<Response>(jObject.ToObject<FacebookApiResponse>());
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
