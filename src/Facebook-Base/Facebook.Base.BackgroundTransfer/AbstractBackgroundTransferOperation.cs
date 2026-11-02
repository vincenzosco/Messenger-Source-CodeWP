using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Facebook.Base.BackgroundTransfer
{
	public abstract class AbstractBackgroundTransferOperation
	{
		public delegate void TransferProgressCallback(AbstractBackgroundTransferOperation operation);

		public TransferOperation TransferOperation { get; protected set; }

		public TransferPriority TransferPriority { get; protected set; }

		public BackgroundTransferProgress Progress { get; set; }

		public Uri RequestUri { get; protected set; }

		public List<FileDataParameter> Files { get; protected set; }

		public Guid Guid { get; protected set; }

		public TransferProgressCallback ProgressCallback { get; set; }

		protected AbstractBackgroundTransferOperation(TransferOperation operation, Uri targetUri, List<FileDataParameter> files, Guid guid)
		{
			TransferOperation = operation;
			Files = files;
			RequestUri = targetUri;
			Guid = guid;
		}

		public abstract IResponseInformation GetResponseInformation();

		public abstract Task StartTransfer(CancellationToken token);

		public abstract Task<byte[]> GetBytesAt(ulong index);

		public async Task<string> DataAsString()
		{
			byte[] bytes = await GetBytesAt(0uL);
			return (bytes != null) ? Encoding.UTF8.GetString(bytes, 0, bytes.Length) : null;
		}
	}
}
