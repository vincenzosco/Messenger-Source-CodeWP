using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class BodyConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.String)
			{
				return null;
			}
			if (object.Equals(reader.Value, "true"))
			{
				Body body = new Body();
				body.Success = true;
				return body;
			}
			if (object.Equals(reader.Value, "false"))
			{
				Body body2 = new Body();
				body2.Success = false;
				return body2;
			}
			Body body3 = new Body();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			object value = reader.Value;
			JsonConvert.PopulateObject(value.ToString(), dictionary);
			if (dictionary.ContainsKey("id"))
			{
				body3.Id = dictionary["id"].ToString();
			}
			if (dictionary.ContainsKey("post_id"))
			{
				body3.PostId = dictionary["post_id"].ToString();
			}
			if (dictionary.ContainsKey("privacy"))
			{
				body3.Privacy = JsonConvert.DeserializeObject<Privacy>(dictionary["privacy"].ToString());
			}
			if (dictionary.ContainsKey("data"))
			{
				List<Dictionary<string, string>> list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(dictionary["data"].ToString());
				if (list.Any() && list[0] != null && list[0].ContainsKey("id"))
				{
					body3.Id = list[0]["id"];
				}
			}
			return body3;
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}
