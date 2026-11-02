using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation
{
	[Bundle(BundleType.Web, typeof(Request), typeof(ExistingUserResponse))]
	public class WebFilter : FacebookApiFilter<Request, ExistingUserResponse>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<ExistingUserResponse> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request http = CreateRestRequest(request, "user.confirmMessengerOnlyPhone").SetHttpMethod(Method.Post).AddParam("device_id", request.DeviceId).AddParam("code", request.ConfirmationCode)
				.AddParam("phone_number", request.PhoneNumber);
			AddRestSignature(http, SecretType.AppSecret);
			return await ExecuteHttpRequestAndParseResponse<ExistingUserResponse>(http);
		}
	}
}
