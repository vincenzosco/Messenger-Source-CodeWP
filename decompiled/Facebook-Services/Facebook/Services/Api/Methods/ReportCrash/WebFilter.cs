using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.ReportCrash
{
	[Bundle(BundleType.Web, typeof(Request), typeof(BoolResponse))]
	internal class WebFilter : FacebookApiFilter<Request, BoolResponse>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request http = new Facebook.Services.Http.Request("https://www.facebook.com/mobile/winphone_messenger_crash_logs");
			http.SetHttpMethod(Method.Post);
			http.AddParam("values", request.Payload);
			http.AddParam("access_token", request.AccessToken);
			Response response = await ExecuteHttpRequest(http);
			return new BoolResponse
			{
				Succeeded = !response.HasError()
			};
		}
	}
}
