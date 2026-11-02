using System;
using System.Collections.Generic;
using Facebook.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Methods.FetchStickerPackIds;

public class StickerPacksIdConverter : JsonConverter
{
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
			List<string> stickerPackIds = GetStickerPackIds(jObject);
			if (stickerPackIds != null)
			{
				response.StickerPackIds = stickerPackIds;
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

	private List<string> GetStickerPackIds(JObject jObject)
	{
		JToken nestedDescendant = jObject.GetNestedDescendant("viewer", "sticker_store", null, "nodes");
		if (nestedDescendant == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		foreach (JObject item2 in nestedDescendant.Children<JObject>())
		{
			string item = item2.GetNestedDescendant("id").ToObject<string>();
			list.Add(item);
		}
		return list;
	}

	public override bool CanConvert(Type objectType)
	{
		return true;
	}
}
