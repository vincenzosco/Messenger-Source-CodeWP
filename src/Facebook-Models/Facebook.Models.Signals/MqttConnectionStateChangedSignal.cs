using Facebook.Base.Mqtt;

namespace Facebook.Models.Signals
{
	public class MqttConnectionStateChangedSignal
	{
		public readonly ClientState State;

		public MqttConnectionStateChangedSignal(ClientState state)
		{
			State = state;
		}
	}
}
