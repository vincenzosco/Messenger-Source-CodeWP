using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchUserById
{
	public class Response : FacebookApiResponse
	{
		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "first_name")]
		[DataMember]
		public string FirstName { get; set; }

		[JsonProperty(PropertyName = "last_name")]
		[DataMember]
		public string LastName { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "username")]
		[DataMember]
		public string Username { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "gender")]
		public string Gender { get; set; }

		[JsonProperty(PropertyName = "locale")]
		[DataMember]
		public string Locale { get; set; }
	}
}
