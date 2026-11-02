using System;
using System.Globalization;
using Facebook.Base.Logging;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class FacebookDateConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value != null)
			{
				try
				{
					string value2 = ((DateTime)value).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
					serializer.Serialize(writer, value2);
				}
				catch
				{
				}
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.String)
			{
				string text = reader.Value as string;
				try
				{
					string text2 = string.Empty;
					DateTime? dateTime = null;
					if (text.Length == 5)
					{
						text2 = "MM/dd";
					}
					else if (text.Length == 8)
					{
						text2 = "MM/dd/yy";
					}
					else if (text.Length > 8)
					{
						text2 = "MM/dd/yyyy";
					}
					if (!string.IsNullOrWhiteSpace(text2))
					{
						DateTime result;
						if (DateTime.TryParseExact(text, text2, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
						{
							dateTime = result;
						}
						else
						{
							if (!DateTime.TryParseExact(text, text2, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
							{
								throw new FormatException("Unable to parse date");
							}
							dateTime = result;
						}
					}
					if (text.Length == 5)
					{
						DateTime dateTime2 = new DateTime(1, 1, 1);
						dateTime = new DateTime(1, (dateTime ?? dateTime2).Month, (dateTime ?? dateTime2).Day);
					}
					return dateTime;
				}
				catch (FormatException)
				{
					Logger.Error(this, "Unable to parse date string: " + text, "ReadJson", "c:\\Open\\messenger\\Facebook\\Facebook-Models\\FacebookUser.cs", 270);
				}
			}
			return new DateTime(1, 1, 1);
		}

		public override bool CanConvert(Type objectType)
		{
			return (object)objectType == typeof(string);
		}
	}
}
