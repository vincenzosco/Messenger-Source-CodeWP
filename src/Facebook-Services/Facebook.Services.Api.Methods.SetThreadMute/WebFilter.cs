using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.SetThreadMute
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
			Facebook.Services.Http.Request http = CreateRestRequest(request, "messaging.mutethread").SetHttpMethod(Method.Post).AddParam("tid", request.ThreadId).AddParam("mute_until", request.MuteUntil.ToString());
			return await ExecuteHttpRequestAndParseResponse<BoolResponse>(http);
		}
	}
}
