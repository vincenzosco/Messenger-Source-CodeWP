using System;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods
{
	internal class GraphResponseConverter : JsonConverter
	{
		private readonly Type[] _typeOrder;

		private int _currentIndex;

		public GraphResponseConverter(Type[] typeOrder)
		{
			_typeOrder = typeOrder;
			_currentIndex = 0;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartObject)
			{
				GraphResponse graphResponse = new GraphResponse();
				serializer.Populate(reader, graphResponse);
				if (_typeOrder.Length > _currentIndex && !string.IsNullOrWhiteSpace(graphResponse.RawBody))
				{
					Type type = _typeOrder[_currentIndex];
					if ((object)type != null)
					{
						graphResponse.Body = JsonConvert.DeserializeObject(graphResponse.RawBody, type);
					}
				}
				_currentIndex++;
				return graphResponse;
			}
			return null;
		}

		public override bool CanConvert(Type objectType)
		{
			return (object)objectType == typeof(GraphResponse);
		}
	}
}
