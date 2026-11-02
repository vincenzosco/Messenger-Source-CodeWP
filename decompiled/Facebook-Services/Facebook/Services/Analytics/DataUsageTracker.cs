using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Services.Analytics.Logging;

namespace Facebook.Services.Analytics;

internal class DataUsageTracker
{
	private class Usage
	{
		public long DownloadedBytes { get; private set; }

		public long UploadedBytes { get; private set; }

		public void IncrementDownloadedBytes(long bytes)
		{
			DownloadedBytes += bytes;
		}

		public void IncrementUploadedBytes(long bytes)
		{
			DownloadedBytes += bytes;
		}

		public bool HasData()
		{
			if (DownloadedBytes == 0)
			{
				return UploadedBytes != 0;
			}
			return true;
		}

		public void Reset()
		{
			long downloadedBytes = (UploadedBytes = 0L);
			DownloadedBytes = downloadedBytes;
		}
	}

	private readonly Dictionary<DataType, Usage> _runningUsage;

	public DataUsageTracker()
	{
		_runningUsage = new Dictionary<DataType, Usage>();
		foreach (object value in Enum.GetValues(typeof(DataType)))
		{
			_runningUsage[(DataType)value] = new Usage();
		}
	}

	public void LogData(DataType type, long downloadedBytes, long uploadedBytes)
	{
		lock (_runningUsage)
		{
			Usage usage = _runningUsage[type];
			usage.IncrementDownloadedBytes(downloadedBytes);
			usage.IncrementUploadedBytes(uploadedBytes);
		}
	}

	public IEnumerable<AnalyticsEvent> CreateEventsAndReset()
	{
		lock (_runningUsage)
		{
			IEnumerable<KeyValuePair<DataType, Usage>> source = _runningUsage.Where((KeyValuePair<DataType, Usage> d) => d.Value.HasData());
			List<AnalyticsEvent> result = source.Select((KeyValuePair<DataType, Usage> d) => DataUsageEventCreator.CreateEvent(d.Key, d.Value.DownloadedBytes, d.Value.UploadedBytes)).ToList();
			ResetRunningUsage();
			return result;
		}
	}

	private void ResetRunningUsage()
	{
		foreach (Usage item in _runningUsage.Select((KeyValuePair<DataType, Usage> kv) => kv.Value))
		{
			item.Reset();
		}
	}
}
