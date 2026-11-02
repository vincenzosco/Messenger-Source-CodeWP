using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Mqtt;
using Facebook.Base.Mqtt.Messages;
using Facebook.Base.Mqtt.Serialization.Payloads;
using Newtonsoft.Json;

namespace Facebook.Services.Mqtt.TopicSubscriptions
{
	public class SendMessageResponse : Subscription
	{
		private class ResponseContext
		{
			private const int ResponseTimeout = 70000;

			private readonly SemaphoreSlim _semaphore;

			public SendResponse Response { get; private set; }

			public ResponseContext()
			{
				_semaphore = new SemaphoreSlim(0);
			}

			public void SetResponse(SendResponse response)
			{
				Response = response;
				_semaphore.Release();
			}

			public async Task WaitAsync(CancellationToken cancellationToken)
			{
				await _semaphore.WaitAsync(70000, cancellationToken);
			}
		}

		public const string SendMessageResponseTopic = "/send_message_response";

		private readonly IDictionary<string, ResponseContext> _outstandingSends = new Dictionary<string, ResponseContext>();

		public SendMessageResponse(IDispatcher uiDispatcher)
			: base(uiDispatcher, "/send_message_response", QualityofService.AcknowledgedDelivery)
		{
		}

		public async Task<SendResponse> WaitForResponse(string offlineThreadingId, CancellationToken cancellationToken)
		{
			ResponseContext context = new ResponseContext();
			lock (_outstandingSends)
			{
				_outstandingSends.Add(offlineThreadingId, context);
			}
			try
			{
				await context.WaitAsync(cancellationToken);
			}
			finally
			{
				lock (_outstandingSends)
				{
					_outstandingSends.Remove(offlineThreadingId);
				}
			}
			return context.Response;
		}

		public override Task HandleMessage(PublishMessage message)
		{
			return Task.Run(() =>
			{
				SendResponse sendResponse = ParseMessage(message.GetPayloadAsString());
				if (sendResponse != null)
				{
					HandleResponse(sendResponse);
				}
			});
		}

		private static SendResponse ParseMessage(string json)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			JsonSerializerSettings settings = jsonSerializerSettings;
			return JsonConvert.DeserializeObject<SendResponse>(json, settings);
		}

		private void HandleResponse(SendResponse response)
		{
			ResponseContext responseContext = null;
			lock (_outstandingSends)
			{
				if (_outstandingSends.ContainsKey(response.MessageId))
				{
					responseContext = _outstandingSends[response.MessageId];
				}
			}
			responseContext?.SetResponse(response);
		}
	}
}
