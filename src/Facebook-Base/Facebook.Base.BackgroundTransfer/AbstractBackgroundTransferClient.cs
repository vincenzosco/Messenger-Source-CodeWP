using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facebook.Base.BackgroundTransfer
{
	public abstract class AbstractBackgroundTransferClient
	{
		public TransferOperation TransferOperation { get; protected set; }

		public abstract void InitializeBackgroundTransfer(TransferOperation operation);

		public abstract void SetRequestHeaders(Dictionary<string, string> headers);

		public abstract void SetRequestHeader(string key, string value);

		public abstract Task<AbstractBackgroundTransferOperation> CreateTransferOperationAsync(TransferOperation operation, Uri targetUri, List<FileDataParameter> files, List<FormDataParameter> formDataParameters = null, string failureMessage = null, string successMessage = null);
	}
}
