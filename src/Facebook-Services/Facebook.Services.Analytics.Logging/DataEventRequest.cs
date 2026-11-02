using Facebook.Base;

namespace Facebook.Services.Analytics.Logging
{
	public class DataEventRequest : Request
	{
		public DataType DataType { get; private set; }

		public long DownloadedBytes { get; private set; }

		public long UploadedBytes { get; private set; }

		public DataEventRequest(DataType dataType, long downloadedBytes, long uploadedBytes)
		{
			DataType = dataType;
			DownloadedBytes = downloadedBytes;
			UploadedBytes = uploadedBytes;
		}
	}
}
