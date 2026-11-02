using System;

namespace Facebook.Base.Mqtt
{
	public class ConnectionFailedEventArgs : StateChangedEventArgs
	{
		public readonly ConnectionFailureReason FailureReason;

		public readonly Exception FailureException;

		public ConnectionFailedEventArgs(ConnectionFailureReason failureReason, Exception failureException)
			: base(ClientState.Disconnected)
		{
			FailureReason = failureReason;
			FailureException = failureException;
		}
	}
}
