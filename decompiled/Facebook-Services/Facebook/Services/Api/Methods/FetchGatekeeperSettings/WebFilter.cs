using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchGatekeeperSettings;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		IOrderedEnumerable<string> orderedGatekeepers = request.ProjectNames.OrderBy((string gk) => gk);
		string query = string.Join(",", orderedGatekeepers);
		string queryHash = CreateHash(query);
		Facebook.Services.Http.Request http = CreateRestRequest(request, "mobile.gatekeepers").AddParam("query_hash", queryHash);
		if (!(queryHash == request.LastQueryHash))
		{
			http.AddParam("query", query);
		}
		Response response = await ExecuteHttpRequestAndParseResponse<Response>(http);
		if (!response.HasError())
		{
			response.QueryHash = queryHash;
			response.GatekeeperBitmaskClientHash = CreateHash(response.GatekeeperBitmask);
			response.GatekeeperQueryOrder = orderedGatekeepers;
		}
		return response;
	}

	private string CreateHash(string query)
	{
		byte[] source = Context.Crypto.ToSHA1(query);
		IEnumerable<string> values = source.Select((byte b) => b.ToString("X2"));
		return string.Join(string.Empty, values);
	}
}
