using System;
using System.Text;
using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchStickerPackIds;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		string query = CreateFetchQuery(request.Type);
		return await FetchStickers(request, query);
	}

	private static string CreateFetchQuery(Request.SourceType requestType)
	{
		StringBuilder stringBuilder = new StringBuilder("viewer(){sticker_store{");
		stringBuilder.Append(requestType switch
		{
			Request.SourceType.StorePacks => "available_packs", 
			Request.SourceType.DownloadedPacks => "tray_packs", 
			Request.SourceType.OwnedPacks => "owned_packs", 
			_ => throw new InvalidOperationException(), 
		});
		stringBuilder.Append("{nodes{id}}}}");
		return stringBuilder.ToString();
	}

	private async Task<Response> FetchStickers(Request request, string query)
	{
		Facebook.Services.Http.Request httpRequest = CreateGraphQLRequest(request, query);
		return await ExecuteHttpRequestAndParseResponse<Response>(httpRequest);
	}
}
