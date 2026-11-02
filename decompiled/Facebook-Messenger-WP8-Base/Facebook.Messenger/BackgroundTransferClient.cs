using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base.BackgroundTransfer;

namespace Facebook.Messenger
{
	public class BackgroundTransferClient : AbstractBackgroundTransferClient
	{
		public override void InitializeBackgroundTransfer(TransferOperation operation)
		{
			throw new NotImplementedException();
		}

		public override void SetRequestHeaders(Dictionary<string, string> headers)
		{
			throw new NotImplementedException();
		}

		public override void SetRequestHeader(string key, string value)
		{
			throw new NotImplementedException();
		}

		public override Task<AbstractBackgroundTransferOperation> CreateTransferOperationAsync(TransferOperation operation, Uri targetUri, List<FileDataParameter> files, List<FormDataParameter> formDataParameters = null, string failureMessage = null, string successMessage = null)
		{
			throw new NotImplementedException();
		}
	}
}
