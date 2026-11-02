using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class ErrorData
{
	[DataMember]
	[JsonProperty(PropertyName = "error_title")]
	public string ErrorTitle;

	[JsonProperty(PropertyName = "error_message")]
	[DataMember]
	public string ErrorMessage;
}
