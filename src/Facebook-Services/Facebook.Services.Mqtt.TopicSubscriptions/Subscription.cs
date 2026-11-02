using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Mqtt;
using Facebook.Base.Mqtt.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Mqtt.TopicSubscriptions
{
	public abstract class Subscription
	{
		public readonly string Topic;

		public readonly QualityofService QoS;

		private readonly IDispatcher _uiDispatcher;

		protected Subscription(IDispatcher uiDispatcher, string topic, QualityofService qos)
		{
			_uiDispatcher = uiDispatcher;
			Topic = topic;
			QoS = qos;
		}

		public abstract Task HandleMessage(PublishMessage message);

		protected async Task PerformSignal<T>(T payload)
		{
			if (payload != null)
			{
				await _uiDispatcher.RunAsync(() =>
				{
					Messenger.Default.Send(payload);
				});
			}
		}
	}
}
