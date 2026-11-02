namespace Facebook.Messenger.Logging
{
	public class ShutdownLogger : FileLogger
	{
		private const string LogFilePath = "shutdown_crashlog";

		private ShutdownLogger(string logFilePath)
			: base(logFilePath)
		{
		}

		public static FileLogger StartNew()
		{
			return FileLogger.StartNew("shutdown_crashlog");
		}

		public static string ReadAndDeleteShutdownLog()
		{
			return FileLogger.ReadAndDeleteFileLog("shutdown_crashlog");
		}
	}
}
