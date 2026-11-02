using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SetStickerTrayPacks;

[Bundle(BundleType.Web, typeof(Request), typeof(BoolResponse))]
public class WebFilter : FacebookApiFilter<Request, BoolResponse>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request httpRequest = CreateGraphRequest(request, "me/sticker_tray_packs");
		httpRequest.Method = Method.Post;
		httpRequest.AddParam("pack_ids", JsonConvert.SerializeObject(request.StickerPackIds));
		return await ExecuteHttpRequestAndParseResponse<BoolResponse>(httpRequest);
	}
}
