using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Base;
using Facebook.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Methods.SyncStickerPacks;

public class ResponseConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		StickerPacksResponse stickerPacksResponse3;
		if (reader.TokenType == JsonToken.StartObject)
		{
			JObject jObject = JObject.Load(reader);
			List<StickerPack> stickers = GetStickers(jObject);
			StickerPacksResponse stickerPacksResponse;
			if (stickers == null)
			{
				stickerPacksResponse = FacebookApiResponse.FromApiResponse<StickerPacksResponse>(jObject.ToObject<FacebookApiResponse>());
			}
			else
			{
				StickerPacksResponse stickerPacksResponse2 = new StickerPacksResponse();
				stickerPacksResponse2.StickerPacks = stickers;
				stickerPacksResponse = stickerPacksResponse2;
			}
			stickerPacksResponse3 = stickerPacksResponse;
		}
		else
		{
			stickerPacksResponse3 = new StickerPacksResponse();
			serializer.Populate(reader, stickerPacksResponse3);
		}
		return stickerPacksResponse3;
	}

	private List<StickerPack> GetStickers(JObject jObject)
	{
		JToken nestedDescendant = jObject.GetNestedDescendant("viewer", "sticker_store", null, "nodes");
		IJEnumerable<JToken> source = (IJEnumerable<JToken>)((nestedDescendant != null) ? ((object)nestedDescendant.Children<JToken>()) : jObject.Values());
		return source.Select((JToken token) => token.ToObject<StickerPack>()).ToList();
	}

	public override bool CanConvert(Type objectType)
	{
		return true;
	}
}
