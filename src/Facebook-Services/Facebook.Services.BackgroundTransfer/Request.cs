using Facebook.Base.BackgroundTransfer;
using Facebook.Services.Http;

namespace Facebook.Services.BackgroundTransfer
{
	public class Request : Facebook.Services.Http.Request
	{
		public TransferOperation TransferOperation { get; set; }

		public AbstractBackgroundTransferOperation.TransferProgressCallback ProgressCallback { get; set; }

		public string FailureMessage { get; set; }

		public string SuccessMessage { get; set; }

		public Request(string url, TransferOperation operation = TransferOperation.Upload)
			: base(url, Method.Post)
		{
			if (operation == TransferOperation.Download)
			{
				Method = Method.Get;
			}
			TransferOperation = operation;
		}

		public Request SetTransferOperation(TransferOperation operation)
		{
			TransferOperation = operation;
			return this;
		}
	}
}
