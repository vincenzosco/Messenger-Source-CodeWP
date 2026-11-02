using Newtonsoft.Json.Linq;

namespace Facebook.Models
{
	public static class ConverterHelper
	{
		public static string GetTextProperty(JProperty property)
		{
			return GetPropertyString(property, "text");
		}

		public static string GetPropertyString(JProperty property, string propertyName)
		{
			JObject jObject = property.First as JObject;
			if (jObject != null)
			{
				JProperty jProperty = jObject.Property(propertyName);
				if (jProperty != null && jProperty.First != null && jProperty.First.Type == JTokenType.String)
				{
					return jProperty.First.ToString();
				}
			}
			return null;
		}
	}
}
