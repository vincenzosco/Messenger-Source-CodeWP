using Facebook.Base.Mqtt;

namespace Facebook.Models.Signals
{
	public class SyncConnectionStateChangedSignal
	{
		public readonly SyncConnectionState State;

		public SyncConnectionStateChangedSignal(SyncConnectionState state)
		{
			State = state;
		}
	}
}
