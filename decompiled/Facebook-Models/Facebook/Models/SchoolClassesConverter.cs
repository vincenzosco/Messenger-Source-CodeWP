using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models;

internal class SchoolClassesConverter : CollapseNodesConverter
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (!(base.ReadJson(reader, objectType, existingValue, serializer) is JArray source))
		{
			return null;
		}
		return (from token in source
			select token.ToObject<SchoolClass>() into schoolClass
			where schoolClass != null
			select schoolClass).ToList();
	}
}
