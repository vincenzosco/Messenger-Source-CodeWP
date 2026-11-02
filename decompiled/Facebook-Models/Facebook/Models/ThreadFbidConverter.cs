using System;
using Newtonsoft.Json;

namespace Facebook.Models;

public class ThreadFbidConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return (object)objectType == typeof(string);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.Value == null)
		{
			return null;
		}
		return new ThreadFbid(long.Parse((string)reader.Value));
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		if (value == null)
		{
			writer.WriteNull();
			return;
		}
		ThreadFbid threadFbid = (ThreadFbid)value;
		writer.WriteValue(threadFbid.Value.ToString());
	}
}
