using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.LeaveThread;

[Bundle(BundleType.Web, typeof(Request), typeof(BoolResponse))]
public class WebFilter : FacebookApiFilter<Request, BoolResponse>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request graph = CreateGraphRequest(request, "participants").SetHttpMethod(Method.Delete).AddParam("id", request.ThreadId);
		return await ExecuteHttpRequestAndParseResponse<BoolResponse>(graph);
	}
}
