using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models
{
	public class FamilyConverter : CollapseEdgesConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JArray jArray = base.ReadJson(reader, objectType, existingValue, serializer) as JArray;
			if (jArray == null)
			{
				return null;
			}
			return jArray.Select((JToken token) => token.ToObject<FamilyMember>()).ToList();
		}
	}
}
