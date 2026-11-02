namespace Facebook.Base.Mqtt
{
	public class ConnectionLostEventArgs : StateChangedEventArgs
	{
		public readonly string Message;

		public ConnectionLostEventArgs(string message)
			: base(ClientState.Disconnected)
		{
			Message = message;
		}
	}
}
