using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FacebookApiError : Error
	{
		[DataMember]
		[JsonProperty(PropertyName = "message")]
		public string ErrorMessage { get; set; }

		[JsonProperty(PropertyName = "type")]
		[DataMember]
		public string Type { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "code")]
		public long ErrorCode { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "error_subcode")]
		public long Subcode { get; set; }

		public string DisplayTitle { get; set; }

		public string DisplayMessage { get; set; }

		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			Message = ErrorMessage;
			Code = ErrorCode;
		}
	}
}
