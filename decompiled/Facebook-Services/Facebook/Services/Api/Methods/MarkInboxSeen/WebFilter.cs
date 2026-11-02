using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.MarkInboxSeen;

[Bundle(BundleType.Web, typeof(Request), typeof(BoolResponse))]
internal class WebFilter : FacebookApiFilter<Request, BoolResponse>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request http = CreateGraphRequest(request, "/me/threads/inbox").SetHttpMethod(Method.Post).AddParam("viewed", "true");
		return ParseResponse<BoolResponse>(await ExecuteHttpRequest(http));
	}
}
