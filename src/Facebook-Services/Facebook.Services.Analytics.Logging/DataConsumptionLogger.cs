using Facebook.Base.Extensions;
using Facebook.Services.Common;

namespace Facebook.Services.Analytics.Logging
{
	public class DataConsumptionLogger
	{
		public static void LogDataEvent(Service service, DataType type, long downloadedBytes, long uploadedBytes)
		{
			if (downloadedBytes != 0 || uploadedBytes != 0)
			{
				DataEventRequest request = new DataEventRequest(type, downloadedBytes, uploadedBytes);
				service.ExecuteAsync<DataEventRequest, BoolResponse>(request).DoNotWait();
			}
		}
	}
}
