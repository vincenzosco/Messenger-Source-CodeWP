namespace Facebook.Base.Mqtt
{
	public class StateChangedEventArgs
	{
		public readonly ClientState State;

		public StateChangedEventArgs(ClientState state)
		{
			State = state;
		}
	}
}
