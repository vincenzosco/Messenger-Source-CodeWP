using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchFriendLastActive
{
	public class Response : FacebookApiResponse
	{
		[DataMember]
		[JsonProperty(PropertyName = "data")]
		public List<LastActiveParams> LastActiveInfo;

		public static Response FromLastActive(LastActiveParams lastActive)
		{
			Response response = new Response();
			response.LastActiveInfo = new List<LastActiveParams> { lastActive };
			return response;
		}

		public bool HasValidLastActive()
		{
			if (!HasError() && LastActiveInfo != null && LastActiveInfo.Count > 0)
			{
				return LastActiveInfo[0].LastActive != null;
			}
			return false;
		}
	}
}
