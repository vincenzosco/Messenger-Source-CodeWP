using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods;

internal class BatchGraphResponseConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		BatchGraphResponse batchGraphResponse = new BatchGraphResponse();
		if (reader.TokenType == JsonToken.StartArray)
		{
			batchGraphResponse.Responses = serializer.Deserialize<List<GraphResponse>>(reader);
		}
		else if (reader.TokenType == JsonToken.StartObject)
		{
			serializer.Populate(reader, batchGraphResponse);
		}
		return batchGraphResponse;
	}

	public override bool CanConvert(Type objectType)
	{
		return true;
	}
}
