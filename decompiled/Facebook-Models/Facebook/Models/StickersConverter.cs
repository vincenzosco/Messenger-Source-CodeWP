using System;
using System.Collections.Generic;
using Facebook.Base.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models;

public class StickersConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		List<Sticker> o = value as List<Sticker>;
		JObject jObject = new JObject();
		jObject.Add("nodes", JToken.FromObject(o));
		JObject jObject2 = jObject;
		jObject2.WriteTo(writer);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			Logger.Error(this, "Attempted to deserialize Json and received Null token for list of stickers", "ReadJson", "c:\\Open\\messenger\\Facebook\\Facebook-Models\\StickerPack.cs", 91);
			return new List<Sticker>();
		}
		JObject jObject = JObject.Load(reader);
		if (jObject.TryGetValue("nodes", out var value))
		{
			return value.ToObject<List<Sticker>>();
		}
		Logger.Error(this, "Could not read the nodes json array for list of stickers", "ReadJson", "c:\\Open\\messenger\\Facebook\\Facebook-Models\\StickerPack.cs", 105);
		return new List<Sticker>();
	}

	public override bool CanConvert(Type objectType)
	{
		return true;
	}
}
