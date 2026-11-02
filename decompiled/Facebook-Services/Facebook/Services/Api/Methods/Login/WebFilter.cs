using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.Login
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : AuthFilter<Request>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request http = CreateRestRequest(request, "auth.login").SetHttpMethod(Method.Post).AddParam("email", request.UserName).AddParam("password", request.Password);
			if (request.MachineId != null)
			{
				http.AddParam("machine_id", request.MachineId);
			}
			else
			{
				http.AddParam("generate_machine_id", "1");
			}
			AddRestSignature(http);
			return await ParseAuthResponse(await ExecuteHttpRequestAndParseResponse<AuthResponse>(http));
		}
	}
}
