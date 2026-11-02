using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Models
{
	public class WorkProjectsConverter : CollapseNodesConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JArray jArray = base.ReadJson(reader, objectType, existingValue, serializer) as JArray;
			if (jArray == null)
			{
				return null;
			}
			List<WorkProject> list = new List<WorkProject>();
			foreach (JToken item in jArray)
			{
				WorkProject workProject = new WorkProject();
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
						case "message":
							workProject.Message = ConverterHelper.GetTextProperty(jProperty);
							break;
						case "experience_with":
							workProject.ExperienceWith = JsonConvert.DeserializeObject<List<FacebookProfile>>(jProperty.First.ToString(), new JsonConverter[1]
							{
								new ExperienceWithConverter()
							});
							break;
						case "work_project":
							workProject.ProjectName = ConverterHelper.GetPropertyString(jProperty, "name");
							break;
						}
					}
				}
				list.Add(workProject);
			}
			return list;
		}
	}
}
