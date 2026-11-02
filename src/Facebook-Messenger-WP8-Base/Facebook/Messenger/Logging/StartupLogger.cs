namespace Facebook.Messenger.Logging
{
	public class StartupLogger : FileLogger
	{
		private const string LogFilePath = "startup_crashlog";

		private StartupLogger(string logFilePath)
			: base(logFilePath)
		{
		}

		public static FileLogger StartNew()
		{
			return FileLogger.StartNew("startup_crashlog");
		}

		public static string ReadAndDeleteStartupLog()
		{
			return FileLogger.ReadAndDeleteFileLog("startup_crashlog");
		}
	}
}
