using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.RegisterPush
{
	[Bundle(BundleType.Web, typeof(Request), typeof(BoolResponse))]
	internal class WebFilter : FacebookApiFilter<Request, BoolResponse>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
		{
			Dictionary<string, string> dict = new Dictionary<string, string>
			{
				{ "device_id", request.DeviceId },
				{ "token", request.ChannelUri },
				{ "app_version", request.AppVersion }
			};
			if (request.ExtraParams != null)
			{
				dict.Add("extra_data", JsonConvert.SerializeObject(request.ExtraParams));
			}
			Facebook.Services.Http.Request http = CreateRestRequest(request, "user.registerPushCallback").SetHttpMethod(Method.Post).AddParam("protocol_params", JsonConvert.SerializeObject(dict));
			return await ExecuteHttpRequestAndParseResponse<BoolResponse>(http);
		}
	}
}
