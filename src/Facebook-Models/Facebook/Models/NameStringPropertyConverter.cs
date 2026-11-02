using System;
using Newtonsoft.Json;

namespace Facebook.Models
{
	internal class NameStringPropertyConverter : StringPropertyConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return ReadJson(reader, objectType, existingValue, serializer, "name");
		}
	}
}
