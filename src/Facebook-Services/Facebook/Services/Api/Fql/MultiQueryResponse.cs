using System.Collections.Generic;
using Facebook.Services.Api.Methods;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Fql
{
	public class MultiQueryResponse : FacebookApiResponse
	{
		[JsonProperty("data")]
		public MultiQueryData Data { get; set; }

		public MultiQueryResponse()
		{
			Data = new MultiQueryData();
		}

		public List<T> Get<T>(string key)
		{
			if (!Data.ContainsKey(key))
			{
				Data[key] = new List<T>();
			}
			object obj = Data[key];
			List<T> list = obj as List<T>;
			return list ?? new List<T>();
		}
	}
}
