using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SetGlobalMute
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
			Facebook.Services.Http.Request http = CreateRestRequest(request, "messaging.setsettings").SetHttpMethod(Method.Post).AddParam("mute_until", request.MuteUntil.ToString());
			Response httpResponse = await ExecuteHttpRequest(http);
			if (httpResponse.HasError())
			{
				return new BoolResponse
				{
					Error = httpResponse.Error
				};
			}
			string responseString = httpResponse.DataAsString();
			if (responseString != null && responseString.Equals("null"))
			{
				BoolResponse boolResponse = new BoolResponse();
				boolResponse.Succeeded = true;
				return boolResponse;
			}
			return JsonConvert.DeserializeObject<BoolResponse>(httpResponse.DataAsString());
		}
	}
}
