using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.PhoneNumber
{
	[Bundle(BundleType.Web, typeof(Request), typeof(BoolResponse))]
	public class WebFilter : FacebookApiFilter<Request, BoolResponse>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request http = CreateRestRequest(request, "user.sendMessengerPhoneConfirmationCode").SetHttpMethod(Method.Post).AddParam("phone_number", request.PhoneNumber).AddParam("country", request.CountryCode ?? "ZZ");
			AddRestSignature(http);
			return await ExecuteHttpRequestAndParseResponse<BoolResponse>(http);
		}
	}
}
