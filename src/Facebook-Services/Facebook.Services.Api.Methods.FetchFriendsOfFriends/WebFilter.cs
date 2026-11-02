using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchFriendsOfFriends
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	internal class WebFilter : FacebookApiFilter<Request, Response>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			if (string.IsNullOrWhiteSpace(request.SearchQuery) || request.SearchQuery.Length <= 1)
			{
				Response response = new Response();
				response.UberSearchResults = new List<FacebookUberSearchResult>();
				return response;
			}
			Facebook.Services.Http.Request uberHttp = CreateRestRequest(request, "ubersearch.get").SetHttpMethod(Method.Get).AddParam("query", request.SearchQuery).AddParam("limit", request.Limit)
				.AddParam("context", "messenger_composer")
				.AddParam("filter", "['user']")
				.AddParam("include_is_messenger_user", "1");
			return await ExecuteHttpRequestAndParseResponse<Response>(uberHttp);
		}
	}
}
