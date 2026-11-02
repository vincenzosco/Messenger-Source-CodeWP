namespace Facebook.Base.BackgroundTransfer;

public enum BackgroundTransferStatus
{
	Idle,
	Running,
	PausedByApplication,
	PausedCostedNetwork,
	PausedNoNetwork,
	Completed,
	Canceled,
	Error
}
