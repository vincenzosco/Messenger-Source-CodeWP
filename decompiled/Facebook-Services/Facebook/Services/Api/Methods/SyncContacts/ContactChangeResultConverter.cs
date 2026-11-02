using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Methods.SyncContacts;

public class ContactChangeResultConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		Dictionary<string, ContactChangeResult> dictionary = new Dictionary<string, ContactChangeResult>();
		if (reader.TokenType == JsonToken.StartObject)
		{
			JObject jObject = JObject.Load(reader);
			foreach (KeyValuePair<string, JToken> item in jObject)
			{
				string text = item.Value["update_type"].ToString();
				if (!(text != "add"))
				{
					string remoteContactId = item.Value["contact"]["id"].ToString();
					string matchConfidence = item.Value["match_confidence"].ToString();
					dictionary.Add(item.Key, new ContactChangeResult
					{
						UpdateType = text,
						RemoteContactId = remoteContactId,
						MatchConfidence = matchConfidence
					});
				}
			}
		}
		return dictionary;
	}

	public override bool CanConvert(Type objectType)
	{
		return true;
	}
}
