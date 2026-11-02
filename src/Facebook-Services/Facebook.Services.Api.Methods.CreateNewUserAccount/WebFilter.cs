using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Methods.Login;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.CreateNewUserAccount
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Facebook.Services.Api.Methods.Login.Response))]
	public class WebFilter : AuthFilter<Request>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Facebook.Services.Api.Methods.Login.Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request http = CreateRestRequest(request, "user.createMessengerOnlyAccount").SetHttpMethod(Method.Post).AddParam("device_id", request.DeviceId).AddParam("phone_number", request.PhoneNumber)
				.AddParam("first_name", request.FirstName)
				.AddParam("last_name", request.LastName);
			if (request.MachineId != null)
			{
				http.AddParam("machine_id", request.MachineId);
			}
			else
			{
				http.AddParam("generate_machine_id", "1");
			}
			AddRestSignature(http, SecretType.AppSecret);
			return await ParseAuthResponse(await ExecuteHttpRequestAndParseResponse<AuthResponse>(http));
		}
	}
}
