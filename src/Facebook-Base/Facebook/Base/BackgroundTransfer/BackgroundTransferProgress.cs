namespace Facebook.Base.BackgroundTransfer
{
	public struct BackgroundTransferProgress
	{
		public ulong BytesReceived { get; set; }

		public ulong BytesSent { get; set; }

		public ulong TotalBytesToReceive { get; set; }

		public ulong TotalBytesToSend { get; set; }

		public bool HasRestarted { get; set; }

		public bool HasResponseChanged { get; set; }

		public BackgroundTransferStatus Status { get; set; }
	}
}
