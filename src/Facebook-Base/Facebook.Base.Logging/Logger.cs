using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Facebook.Base.Logging
{
	public sealed class Logger
	{
		private static readonly Logger Instance = new Logger();

		private readonly ICollection<ILogListener> _listeners = new List<ILogListener>();

		private Logger()
		{
		}

		public static void AddListener(ILogListener listener)
		{
			lock (Instance._listeners)
			{
				Instance._listeners.Add(listener);
			}
		}

		public static void RemoveListener(ILogListener listener)
		{
			lock (Instance._listeners)
			{
				Instance._listeners.Remove(listener);
			}
		}

		public static void LogLocation(object obj, LogLevel level = LogLevel.Info, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(level, obj.GetType(), "", member, filePath, line);
		}

		public static void Critical(object obj, string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Critical, obj.GetType(), message, member, filePath, line);
		}

		public static void Critical(string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Critical, null, message, member, filePath, line);
		}

		public static void Debug(object obj, string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Debug, obj.GetType(), message, member, filePath, line);
		}

		public static void Debug(string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Debug, null, message, member, filePath, line);
		}

		public static void Error(object obj, string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Error, obj.GetType(), message, member, filePath, line);
		}

		public static void Error(string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Error, null, message, member, filePath, line);
		}

		public static void Info(object obj, string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Info, obj.GetType(), message, member, filePath, line);
		}

		public static void Info(string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Info, null, message, member, filePath, line);
		}

		public static void Warning(object obj, string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Warning, obj.GetType(), message, member, filePath, line);
		}

		public static void Warning(string message, [CallerMemberName] string member = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
		{
			Log(LogLevel.Warning, null, message, member, filePath, line);
		}

		private static void Log(LogLevel level, Type type, string message, string member, string filePath, int line)
		{
			lock (Instance._listeners)
			{
				ICollection<ILogListener> listeners = Instance._listeners;
				if (!listeners.Any())
				{
					return;
				}
				foreach (ILogListener item in listeners)
				{
					if ((object)type != null)
					{
						item.Log(level, type, message, member, filePath, line);
					}
					else
					{
						item.Log(level, message, member, filePath, line);
					}
				}
			}
		}
	}
}
