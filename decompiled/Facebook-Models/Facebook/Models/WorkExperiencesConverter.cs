using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models;

internal class WorkExperiencesConverter : CollapseNodesConverter
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (!(base.ReadJson(reader, objectType, existingValue, serializer) is JArray source))
		{
			return null;
		}
		return (from token in source
			select token.ToObject<WorkExperience>() into workExperience
			where workExperience != null
			select workExperience).ToList();
	}
}
