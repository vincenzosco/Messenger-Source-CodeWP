namespace Facebook.Base.Mqtt;

public enum ConnectionFailureReason
{
	FailedSocketConnectError,
	FailedConnectMessage,
	FailedInvalidConnectAck,
	FailedDnsUnresolved,
	FailedCreateStream,
	FailedConnectRefused,
	FailedConnectRefusedNotAuthorized,
	FailedUnexpectedDisconnect
}
