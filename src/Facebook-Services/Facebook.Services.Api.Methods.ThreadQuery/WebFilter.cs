using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.GraphQL;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.ThreadQuery
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			string query = Facebook.Services.Api.GraphQL.ThreadQuery.CreateThreadQuery();
			Dictionary<string, string> queryParams = new Dictionary<string, string>
			{
				{ "thread_ids", request.ThreadFbId },
				{
					"msg_count",
					request.MessageCount.ToString()
				}
			};
			Facebook.Services.Http.Request graphQlRequest = CreateGraphQLRequest(request, query, queryParams);
			graphQlRequest.SetHttpMethod(Method.Post);
			return await ExecuteHttpRequestAndParseResponse<Response>(graphQlRequest);
		}
	}
}
