using System.Linq;
using Newtonsoft.Json.Linq;

namespace Facebook.Base;

public static class JObjectExtensions
{
	public static JToken GetNestedDescendant(this JObject jObject, params string[] properties)
	{
		JToken jToken = jObject;
		foreach (string text in properties)
		{
			if (!(jToken is JObject))
			{
				return null;
			}
			JObject jObject2 = (JObject)jToken;
			JProperty jProperty = ((text == null) ? jObject2.Properties().FirstOrDefault() : jObject2.Property(text));
			if (jProperty == null)
			{
				return null;
			}
			jToken = jProperty.Value;
		}
		return jToken;
	}
}
