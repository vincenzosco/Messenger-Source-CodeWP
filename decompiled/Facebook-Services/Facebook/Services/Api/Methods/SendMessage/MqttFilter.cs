using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Mqtt;

namespace Facebook.Services.Api.Methods.SendMessage;

[Bundle(BundleType.Mqtt, typeof(Request), typeof(Response))]
internal class MqttFilter : Filter<Request, Response>
{
	public MqttFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		if (!request.HasServerThreadId() || request.HasFiles())
		{
			return await next.ExecuteAsync<Request, Response>(request);
		}
		Facebook.Services.Mqtt.Service mqtt = Context.MqttService;
		long startMs = DateExtensions.MSecsSinceEpoch();
		Response response = await mqtt.ExecuteAsync<Request, Response>(request);
		SendEventLogger.LogSendState(extraFailureReason: request.GetExtraFailureReason(startMs, Context.ApplicationState), request: request, service: Context.AnalyticsService, method: SendEventLogger.SendMethod.MQTT, response: response);
		if (!response.HasError())
		{
			return response;
		}
		return await next.ExecuteAsync<Request, Response>(request);
	}
}
