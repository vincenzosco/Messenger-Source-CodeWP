using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models
{
	internal class SchoolClassesConverter : CollapseNodesConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JArray jArray = base.ReadJson(reader, objectType, existingValue, serializer) as JArray;
			if (jArray == null)
			{
				return null;
			}
			return (from token in jArray
				select token.ToObject<SchoolClass>() into schoolClass
				where schoolClass != null
				select schoolClass).ToList();
		}
	}
}
