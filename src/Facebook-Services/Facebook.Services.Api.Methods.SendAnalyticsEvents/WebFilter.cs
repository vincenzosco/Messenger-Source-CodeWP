using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Services.Analytics;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SendAnalyticsEvents
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		public const int MaximumBatchSize = 50;

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			int lastUploadedEventLogId = -1;
			try
			{
				IEnumerable<HoneyClientUpload> batches = CreateBatches(request);
				foreach (HoneyClientUpload batch in batches)
				{
					Facebook.Services.Http.Request http = CreateRestRequest(request, "logging.clientevent").SetHttpMethod(Method.Post).AddParam("compressed", "0").AddParam("message", JsonConvert.SerializeObject(batch));
					AddRestSignature(http, SecretType.AppSecret);
					Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(http);
					if (!httpResponse.HasError() && !httpResponse.HasError())
					{
						lastUploadedEventLogId = batch.MaxEventLogId;
						continue;
					}
					break;
				}
			}
			catch (Exception ex)
			{
				Logger.Error(this, ex.ToString(), "ExecuteAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Methods\\SendAnalyticsEvents\\WebFilter.cs", 56);
			}
			return new Response
			{
				LastUploadedEventLogId = lastUploadedEventLogId
			};
		}

		private static IEnumerable<HoneyClientUpload> CreateBatches(Request request)
		{
			IEnumerable<string> enumerable = request.Data.Select((AnalyticsEvent e) => e.SessionId).Distinct();
			List<HoneyClientUpload> list = new List<HoneyClientUpload>();
			foreach (string item in enumerable)
			{
				string session = item;
				List<AnalyticsEvent> list2 = request.Data.Where((AnalyticsEvent e) => e.SessionId == session).ToList();
				int num = list2.Count / 50 + 1;
				for (int num2 = 0; num2 < num; num2++)
				{
					List<AnalyticsEvent> list3 = list2.Skip(num2 * 50).Take(50).ToList();
					if (list3.Any())
					{
						list.Add(new HoneyClientUpload(item, request, num2, list3));
					}
				}
			}
			return list;
		}
	}
}
