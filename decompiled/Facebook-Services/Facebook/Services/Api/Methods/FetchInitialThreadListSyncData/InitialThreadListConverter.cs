using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Methods.FetchInitialThreadListSyncData;

public class InitialThreadListConverter : JsonConverter
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
			JObject source = JObject.Load(reader);
			JToken jToken = source.Values().FirstOrDefault();
			if (jToken == null)
			{
				return null;
			}
			JToken jToken2 = jToken.Values().FirstOrDefault();
			if (jToken2 == null)
			{
				return null;
			}
			int count = jToken2.ToObject<JObject>().Value<int>("count");
			int syncSequenceId = jToken2.ToObject<JObject>().Value<int>("sync_sequence_id");
			response.Count = count;
			response.SyncSequenceId = syncSequenceId;
		}
		return response;
	}

	public override bool CanConvert(Type objectType)
	{
		return true;
	}
}
