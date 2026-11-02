using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Facebook.Models;

public class DeliveryReceiptConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		List<DeliveryReceipt> source = value as List<DeliveryReceipt>;
		if (source.Any())
		{
			Dictionary<string, TimestampObject> value2 = source.ToDictionary((DeliveryReceipt deliveryReceipt) => deliveryReceipt.Id, (DeliveryReceipt deliveryReceipt) => new TimestampObject
			{
				Timestamp = deliveryReceipt.Timestamp
			});
			serializer.Serialize(writer, value2);
		}
		else
		{
			writer.WriteStartArray();
			writer.WriteEndArray();
		}
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		List<DeliveryReceipt> list = new List<DeliveryReceipt>();
		if (reader.TokenType == JsonToken.Null)
		{
			return new List<DeliveryReceipt>();
		}
		if (reader.TokenType == JsonToken.StartArray)
		{
			reader.Read();
			return new List<DeliveryReceipt>();
		}
		if (reader.TokenType == JsonToken.StartObject)
		{
			reader.Read();
		}
		while (reader.TokenType != JsonToken.EndObject)
		{
			DeliveryReceipt deliveryReceipt = new DeliveryReceipt();
			if (reader.TokenType == JsonToken.PropertyName)
			{
				deliveryReceipt.Id = reader.Value as string;
				reader.Read();
			}
			if (reader.TokenType == JsonToken.StartObject)
			{
				reader.Read();
				if (reader.TokenType == JsonToken.PropertyName)
				{
					reader.Read();
				}
				if (reader.TokenType == JsonToken.Integer)
				{
					deliveryReceipt.Timestamp = (long)reader.Value;
					reader.Read();
				}
				if (reader.TokenType == JsonToken.EndObject)
				{
					reader.Read();
				}
			}
			list.Add(deliveryReceipt);
		}
		return list;
	}

	public override bool CanConvert(Type objectType)
	{
		return (object)objectType == typeof(List<DeliveryReceipt>);
	}
}
