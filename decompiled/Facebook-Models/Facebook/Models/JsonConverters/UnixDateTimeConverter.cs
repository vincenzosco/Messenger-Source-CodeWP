using System;
using Newtonsoft.Json;

namespace Facebook.Models.JsonConverters;

internal class UnixDateTimeConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		if (value is DateTime dateTime)
		{
			DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			writer.WriteRawValue((dateTime - dateTime2).TotalSeconds.ToString());
		}
		else
		{
			writer.WriteNull();
		}
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Integer)
		{
			try
			{
				if (double.TryParse(reader.Value.ToString(), out var result))
				{
					return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(result);
				}
			}
			catch (Exception)
			{
				return DateTime.Now;
			}
		}
		return DateTime.Now;
	}

	public override bool CanConvert(Type objectType)
	{
		return (object)objectType == typeof(string);
	}
}
