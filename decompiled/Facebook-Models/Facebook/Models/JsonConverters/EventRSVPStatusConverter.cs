using System;
using Newtonsoft.Json;

namespace Facebook.Models.JsonConverters;

internal class EventRSVPStatusConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType != JsonToken.String)
		{
			return null;
		}
		string text = reader.Value.ToString().ToLower();
		if (text.Equals("going") || text.Equals("attending"))
		{
			return RSVPStatus.Going;
		}
		if (text.Equals("not_going") || text.Equals("declined"))
		{
			return RSVPStatus.NotGoing;
		}
		if (text.Equals("maybe") || text.Equals("unsure"))
		{
			return RSVPStatus.Maybe;
		}
		return RSVPStatus.Invited;
	}

	public override bool CanConvert(Type objectType)
	{
		throw new NotImplementedException();
	}
}
