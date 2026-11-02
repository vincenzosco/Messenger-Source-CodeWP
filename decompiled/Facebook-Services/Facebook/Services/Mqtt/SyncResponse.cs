using Facebook.Base;

namespace Facebook.Services.Mqtt
{
	public class SyncResponse : Response
	{
		public enum FailureReason
		{
			None,
			NotConnected,
			Exception,
			NoPubAckReceived
		}

		public readonly FailureReason Reason;

		public SyncResponse()
			: this(FailureReason.None, null)
		{
		}

		public SyncResponse(FailureReason reason, Error error)
		{
			Reason = reason;
			Error = error;
		}
	}
}
