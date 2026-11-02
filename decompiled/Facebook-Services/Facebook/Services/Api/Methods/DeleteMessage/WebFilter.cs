using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.DeleteMessage;

[Bundle(BundleType.Web, typeof(Request), typeof(Facebook.Base.Response))]
public class WebFilter : FacebookApiFilter<Request, Facebook.Base.Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Facebook.Base.Response> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request http = CreateGraphRequest(request, request.MessageId).SetHttpMethod(Method.Delete);
		return await ExecuteHttpRequest(http);
	}
}
