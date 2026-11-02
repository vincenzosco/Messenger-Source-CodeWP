using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation;

public class ExistingUserResponse : FacebookApiResponse
{
	[DataMember]
	[JsonProperty(PropertyName = "account_id")]
	public string AccountId { get; set; }

	[JsonProperty(PropertyName = "firstname")]
	[DataMember]
	public string FirstName { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "lastname")]
	public string LastName { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "profilepic")]
	public string ProfilePicUrl { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "is_partial")]
	public bool PartialUser { get; set; }
}
