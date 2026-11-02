using System;
using System.Text;
using Facebook.Base.Logging;
using Facebook.Messenger.Utilities;

namespace Facebook.Messenger.Logging;

public class FileLogger : ILogListener
{
	private readonly string _logFilePath;

	private readonly object _logLock = new object();

	protected FileLogger(string logFilePath)
	{
		_logFilePath = logFilePath;
	}

	protected static FileLogger StartNew(string logFilePath)
	{
		FileLogger fileLogger = new FileLogger(logFilePath);
		fileLogger.Start();
		return fileLogger;
	}

	protected static string ReadAndDeleteFileLog(string logFilePath)
	{
		byte[] array = ErrorIgnoringStorageUtils.ReadFile(logFilePath);
		if (array == null)
		{
			return null;
		}
		string result = Encoding.UTF8.GetString(array, 0, array.Length);
		ErrorIgnoringStorageUtils.DeleteFileInIsolateStorage(logFilePath);
		return result;
	}

	private void Start()
	{
		Facebook.Base.Logging.Logger.AddListener(this);
	}

	public void Stop()
	{
		Facebook.Base.Logging.Logger.RemoveListener(this);
		RemoveLogFile();
	}

	public void Log(LogLevel level, Type type, string message, string member, string filePath, int line)
	{
		string message2 = LogHelper.CreateEntryWithType(DateTime.UtcNow, level, type.Name, member, line, message);
		Log(message2);
	}

	public void Log(LogLevel level, string message, string member, string filePath, int line)
	{
		string message2 = LogHelper.CreateEntryWithoutType(DateTime.UtcNow, level, member, line, message);
		Log(message2);
	}

	private void Log(string message)
	{
		lock (_logLock)
		{
			byte[] bytes = Encoding.UTF8.GetBytes($"{message}{Environment.NewLine}");
			ErrorIgnoringStorageUtils.AppendToFile(_logFilePath, bytes);
		}
	}

	private void RemoveLogFile()
	{
		lock (_logLock)
		{
			ErrorIgnoringStorageUtils.DeleteFileInIsolateStorage(_logFilePath);
		}
	}
}
