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
			Logger.Error($"Error: {error}", member, filePath, line);
			string message2 = $"Error: {message}, at {member}:{line}";
			CrashSaver.SaveErrorLog("app", error, message2, App.ExceptionHelper.Log.LogMessages);
			Task.Run((Func<Task>)ExceptionHelper.CheckForCrashLogs);
		}

		public static void SaveObservedException(string errorMessage, Exception e, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Logger.Error($"Exception: {errorMessage}\r\n{e}", member, filePath, line);
			CrashSaver.SaveLog("app", e, true, App.ExceptionHelper.Log.LogMessages);
			Task.Run((Func<Task>)ExceptionHelper.CheckForCrashLogs);
		}
	}
}
