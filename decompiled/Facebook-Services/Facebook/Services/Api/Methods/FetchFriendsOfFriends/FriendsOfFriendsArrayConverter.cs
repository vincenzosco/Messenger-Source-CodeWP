using System;
using System.Collections.Generic;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchFriendsOfFriends
{
	public class FriendsOfFriendsArrayConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Response response = new Response();
			if (reader.TokenType == JsonToken.StartArray)
			{
				response.UberSearchResults = serializer.Deserialize<List<FacebookUberSearchResult>>(reader);
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
