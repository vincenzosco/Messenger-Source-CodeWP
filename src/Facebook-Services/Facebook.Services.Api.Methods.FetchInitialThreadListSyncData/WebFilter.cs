using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchInitialThreadListSyncData
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		private const string InitialThreadListQuery = "viewer(){message_threads{count,sync_sequence_id}}";

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request graphQlRequest = CreateGraphQLRequest(request, "viewer(){message_threads{count,sync_sequence_id}}");
			Response response = await ExecuteHttpRequestAndParseResponse<Response>(graphQlRequest);
			if (response.HasError())
			{
				Response response2 = new Response();
				response2.Error = response.Error;
				return response2;
			}
			return response;
		}
	}
}
