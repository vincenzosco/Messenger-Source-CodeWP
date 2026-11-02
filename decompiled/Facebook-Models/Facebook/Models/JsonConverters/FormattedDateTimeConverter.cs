using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Facebook.Models.JsonConverters
{
	public class FormattedDateTimeConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is DateTime)
			{
				string value2 = ((DateTime)value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK", DateTimeFormatInfo.InvariantInfo);
				writer.WriteValue(value2);
			}
			else
			{
				writer.WriteNull();
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Date)
			{
				string s = reader.Value.ToString();
				DateTime result;
				if (DateTime.TryParse(s, out result))
				{
					return result;
				}
			}
			return DateTime.Now;
		}

		public override bool CanConvert(Type objectType)
		{
			return (object)objectType == typeof(string);
		}
	}
}
