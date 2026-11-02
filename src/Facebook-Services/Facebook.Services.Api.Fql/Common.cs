using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Fql
{
	internal class Common
	{
		public class SafeLongConverter : JsonConverter
		{
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				throw new NotImplementedException();
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				long result = 0L;
				try
				{
					if (reader.TokenType == JsonToken.Integer)
					{
						result = (long)reader.Value;
					}
					if (reader.TokenType == JsonToken.String)
					{
						string s = (string)reader.Value;
						long.TryParse(s, out result);
					}
				}
				catch (Exception)
				{
				}
				return result;
			}

			public override bool CanConvert(Type objectType)
			{
				return (object)objectType == typeof(long);
			}
		}

		public static MultiQueryResponse ParseMulti(Dictionary<string, Type> mapping, string json)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.Converters.Add(new MultiResponseConverter(mapping));
			jsonSerializerSettings.Converters.Add(new SafeLongConverter());
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			JsonSerializerSettings settings = jsonSerializerSettings;
			return JsonConvert.DeserializeObject<MultiQueryResponse>(json, settings);
		}

		public static string CreateMultiQuery(Dictionary<string, string> queryMap)
		{
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteStartObject();
			foreach (KeyValuePair<string, string> item in queryMap)
			{
				jsonTextWriter.WritePropertyName(item.Key);
				jsonTextWriter.WriteValue(item.Value);
			}
			jsonTextWriter.WriteEndObject();
			return stringWriter.ToString();
		}

		public static string CreateCsv<T>(IEnumerable<T> values)
		{
			return string.Join(",", values.Select((T i) => string.Format("'{0}'", new object[1] { i })));
		}

		public static string EscapeString(string fqlString)
		{
			return fqlString.Replace("'", "\\'");
		}

		public static string Create(string[] fields, string table, string where, int limit, int offset = 0)
		{
			return Create(fields, table, where, "", limit, offset);
		}

		public static string Create(string[] fields, string table, string where = "", string orderBy = "", int limit = 0, int offset = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ");
			stringBuilder.Append(string.Join(", ", fields));
			stringBuilder.AppendFormat(" FROM {0}", new object[1] { table });
			if (!string.IsNullOrWhiteSpace(where))
			{
				stringBuilder.AppendFormat(" WHERE {0}", new object[1] { where });
			}
			if (!string.IsNullOrWhiteSpace(orderBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0}", new object[1] { orderBy });
			}
			if (limit > 0)
			{
				stringBuilder.AppendFormat(" LIMIT {0}", new object[1] { limit });
			}
			if (offset > 0)
			{
				stringBuilder.AppendFormat(" OFFSET {0}", new object[1] { offset });
			}
			return stringBuilder.ToString();
		}
	}
}
