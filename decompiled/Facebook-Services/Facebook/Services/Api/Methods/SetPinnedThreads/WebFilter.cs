using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SetPinnedThreads;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request httpRequest = CreateGraphRequest(request, "me/pinned_threads").SetHttpMethod(Method.Post);
		if (request.OrderedThreadIds != null)
		{
			httpRequest.AddParam("tids", JsonConvert.SerializeObject(request.OrderedThreadIds));
		}
		if (request.RemovedThreadIds != null)
		{
			httpRequest.AddParam("removed_tids", JsonConvert.SerializeObject(request.RemovedThreadIds));
		}
		return await ExecuteHttpRequestAndParseResponse<Response>(httpRequest);
	}
}
