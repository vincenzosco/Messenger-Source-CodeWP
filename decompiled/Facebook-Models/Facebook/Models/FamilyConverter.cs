using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models;

public class FamilyConverter : CollapseEdgesConverter
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (!(base.ReadJson(reader, objectType, existingValue, serializer) is JArray source))
		{
			return null;
		}
		return source.Select((JToken token) => token.ToObject<FamilyMember>()).ToList();
	}
}
