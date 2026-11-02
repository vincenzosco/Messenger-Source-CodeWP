using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Mqtt;
using Facebook.Base.Mqtt.Messages;
using Facebook.Models.Signals;
using Facebook.Models.SyncProtocol;
using Thrift.Protocol;
using Thrift.Transport;

namespace Facebook.Services.Mqtt.TopicSubscriptions
{
	public class Sync : Subscription
	{
		private const string SyncTopic = "/t_ms";

		public Sync(IDispatcher uiDispatcher)
			: base(uiDispatcher, "/t_ms", QualityofService.FireAndForget)
		{
		}

		public override async Task HandleMessage(PublishMessage message)
		{
			ClientPayload payload = ParseMessage(message.Payload);
			await PerformSignal(new SyncMessageSignal(payload));
		}

		private static ClientPayload ParseMessage(byte[] bytePayload)
		{
			TCompactProtocol iprot = new TCompactProtocol(new TMemoryBuffer(bytePayload));
			MqttThriftHeader mqttThriftHeader = new MqttThriftHeader();
			mqttThriftHeader.Read(iprot);
			ClientPayload clientPayload = new ClientPayload();
			clientPayload.Read(iprot);
			return clientPayload;
		}
	}
}
