using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SendFlytrapReport
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
			Facebook.Services.Http.Request http = CreateRestRequest(request, "bug.create").SetHttpMethod(Method.Post);
			http.AddParam("desc", request.Description);
			http.AddParam("info", JsonConvert.SerializeObject(request.Info));
			http.AddParam("build_num", request.BuildNumber);
			http.AddParam("log", request.Logs);
			http.AddParam("expect_more_logs", false.ToString());
			http.AddParam("category_id", Context.Config.FlytrapCategoryId);
			Response response = await ExecuteHttpRequest(http);
			return new BoolResponse
			{
				Succeeded = !response.HasError()
			};
		}
	}
}
