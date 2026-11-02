using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchSticker;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		string query = $"node({request.StickerId}){{id, animated_image, thread_image}}";
		Facebook.Services.Http.Request fbRequest = CreateGraphQLRequest(request, query);
		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			Converters = new JsonConverter[1]
			{
				new StickerConverter(request.StickerId)
			}
		};
		return await ExecuteHttpRequestAndParseResponse<Response>(fbRequest, settings);
	}
}
