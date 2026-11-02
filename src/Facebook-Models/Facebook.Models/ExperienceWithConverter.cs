using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models
{
	public class ExperienceWithConverter : CollapseNodesConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JArray jArray = base.ReadJson(reader, objectType, existingValue, serializer) as JArray;
			if (jArray == null)
			{
				return null;
			}
			List<FacebookProfile> list = new List<FacebookProfile>();
			foreach (JToken item in jArray)
			{
				FacebookProfile facebookProfile = new FacebookProfile();
				foreach (JToken item2 in (IEnumerable<JToken>)item)
				{
					if (!(item2 is JProperty))
					{
						continue;
					}
					JProperty jProperty = item2 as JProperty;
					if (jProperty.First != null && jProperty.First.Type != JTokenType.Null)
					{
						switch (jProperty.Name)
						{
						case "id":
							facebookProfile.Id = jProperty.First.ToString();
							break;
						case "name":
							facebookProfile.Name = jProperty.First.ToString();
							break;
						}
					}
				}
				list.Add(facebookProfile);
			}
			return list;
		}
	}
}
