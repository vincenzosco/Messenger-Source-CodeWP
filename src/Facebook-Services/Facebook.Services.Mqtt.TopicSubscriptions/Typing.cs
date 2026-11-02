using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Mqtt;
using Facebook.Base.Mqtt.Messages;
using Facebook.Base.Mqtt.Serialization.Payloads;
using Facebook.Models.Signals;
using Newtonsoft.Json;

namespace Facebook.Services.Mqtt.TopicSubscriptions
{
	public class Typing : Subscription
	{
		private const string TypingTopic = "/orca_typing_notifications";

		public Typing(IDispatcher uiDispatcher)
			: base(uiDispatcher, "/orca_typing_notifications", QualityofService.AcknowledgedDelivery)
		{
		}

		public override async Task HandleMessage(PublishMessage message)
		{
			MqttTypingReceivedSignal signal = ParseMessage(message.GetPayloadAsString());
			await PerformSignal(signal);
		}

		private static MqttTypingReceivedSignal ParseMessage(string json)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			JsonSerializerSettings settings = jsonSerializerSettings;
			TypingMessage typingMessage = JsonConvert.DeserializeObject<TypingMessage>(json, settings);
			MqttTypingReceivedSignal mqttTypingReceivedSignal = new MqttTypingReceivedSignal();
			mqttTypingReceivedSignal.SenderId = typingMessage.SenderId;
			mqttTypingReceivedSignal.State = typingMessage.State;
			return mqttTypingReceivedSignal;
		}
	}
}
