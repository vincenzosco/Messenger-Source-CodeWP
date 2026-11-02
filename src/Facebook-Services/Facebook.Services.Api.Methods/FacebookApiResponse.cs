using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FacebookApiResponse : Response
	{
		[DataMember]
		[JsonProperty(PropertyName = "error_code")]
		public int? ErrorCode { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "error_msg")]
		public string ErrorMessage { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "error_data")]
		public string ErrorData { get; set; }

		[JsonProperty(PropertyName = "error")]
		[DataMember]
		public FacebookApiError ApiError { get; set; }

		public bool SessionExpired { get; set; }

		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			if (ApiError != null)
			{
				Error = ApiError;
			}
			else if (ErrorCode.HasValue)
			{
				FacebookApiError error = (ApiError = new FacebookApiError
				{
					ErrorCode = ErrorCode.Value,
					Message = ErrorMessage,
					Subcode = 0L,
					Type = ""
				});
				Error = error;
				if (ErrorData != null)
				{
					ErrorData errorData = JsonConvert.DeserializeObject<ErrorData>(ErrorData);
					Error error2 = Error;
					string message = (ApiError.DisplayMessage = errorData.ErrorMessage);
					error2.Message = message;
					Error error3 = Error;
					string title = (ApiError.DisplayTitle = errorData.ErrorTitle);
					error3.Title = title;
				}
			}
		}

		public static T FromApiResponse<T>(FacebookApiResponse apiResponse) where T : FacebookApiResponse, new()
		{
			if (apiResponse == null)
			{
				return new T();
			}
			return new T
			{
				Error = apiResponse.Error,
				ErrorCode = apiResponse.ErrorCode,
				ErrorData = apiResponse.ErrorData,
				ErrorMessage = apiResponse.ErrorMessage,
				ApiError = apiResponse.ApiError
			};
		}
	}
}
