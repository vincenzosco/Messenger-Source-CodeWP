using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.UnregisterPush;

[Bundle(BundleType.Web, typeof(Request), typeof(BoolResponse))]
internal class WebFilter : FacebookApiFilter<Request, BoolResponse>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request http = CreateRestRequest(request, "user.unregisterPushCallback").SetHttpMethod(Method.Post).AddParam("token", request.ChannelUri);
		return await ExecuteHttpRequestAndParseResponse<BoolResponse>(http);
	}
}
