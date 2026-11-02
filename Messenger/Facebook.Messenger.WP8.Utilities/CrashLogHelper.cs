using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Messenger.Crash;

namespace Facebook.Messenger.WP8.Utilities
{
	public class CrashLogHelper
	{
		public static void SaveError(CrashSaver.ErrorLogType error, string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Logger.Error(string.Format("Error: {0}", error), member, filePath, line);
			string message2 = string.Format("Error: {0}, at {1}:{2}", message, member, line);
			CrashSaver.SaveErrorLog("app", error, message2, App.ExceptionHelper.Log.LogMessages);
			Task.Run((Func<Task>)ExceptionHelper.CheckForCrashLogs);
		}

		public static void SaveObservedException(string errorMessage, Exception e, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Logger.Error(string.Format("Exception: {0}\r\n{1}", errorMessage, e), member, filePath, line);
			CrashSaver.SaveLog("app", e, true, App.ExceptionHelper.Log.LogMessages);
			Task.Run((Func<Task>)ExceptionHelper.CheckForCrashLogs);
		}
	}
}
