using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.RemoveThreadParticipants
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
			Facebook.Services.Http.Request graph = CreateGraphRequest(request, "participants");
			graph = graph.SetHttpMethod(Method.Delete);
			graph = graph.AddParam("id", request.ThreadId);
			graph = graph.AddParam("to", JsonConvert.SerializeObject(request.UserIds));
			return await ExecuteHttpRequestAndParseResponse<BoolResponse>(graph);
		}
	}
}
