using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.SearchImages
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	internal class WebFilter : FacebookApiFilter<Request, Response>
	{
		private const int ImageSearchCount = 20;

		private const string ImageOffsetParameter = "Image.Offset";

		private const string QueryParameter = "Query";

		private readonly KeyValuePair<string, string> AppParameter = new KeyValuePair<string, string>("AppId", "D41D8CD98F00B204E9800998ECF8427E465C0750");

		private readonly KeyValuePair<string, string> SourceParameter = new KeyValuePair<string, string>("Sources", "Image");

		private readonly KeyValuePair<string, string> VersionParameter = new KeyValuePair<string, string>("Version", "2.0");

		private readonly KeyValuePair<string, string> MarketParameter = new KeyValuePair<string, string>("Market", "en-us");

		private readonly KeyValuePair<string, string> AdultParamter = new KeyValuePair<string, string>("Adult", "Strict");

		private readonly KeyValuePair<string, string> ImageCountParameter = new KeyValuePair<string, string>("Image.Count", 20.ToString());

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request http = new Facebook.Services.Http.Request("http://api.search.live.net/json.aspx");
			http.SetHttpMethod(Method.Get);
			http.AddParam(AppParameter.Key, AppParameter.Value);
			http.AddParam(SourceParameter.Key, SourceParameter.Value);
			http.AddParam(VersionParameter.Key, VersionParameter.Value);
			http.AddParam(MarketParameter.Key, MarketParameter.Value);
			http.AddParam(AdultParamter.Key, AdultParamter.Value);
			http.AddParam(ImageCountParameter.Key, ImageCountParameter.Value);
			http.AddParam("Image.Offset", (20 * request.Page).ToString());
			http.AddParam("Query", request.SearchTerm);
			return await ExecuteHttpRequestAndParseResponse<Response>(http);
		}
	}
}
