using System;
using System.Collections.Generic;
using System.Diagnostics;
using Facebook.Base.Logging;

namespace Facebook.Messenger.Logging
{
	public class Logger : ILogListener
	{
		private const int MaxLogHistory = 200;

		private readonly object _queueLock = new object();

		private readonly Queue<string> _logMessages;

		public List<string> LogMessages
		{
			get
			{
				lock (_queueLock)
				{
					return new List<string>(_logMessages);
				}
			}
		}

		public Logger()
		{
			_logMessages = new Queue<string>(200);
		}

		public void Log(LogLevel level, Type type, string message, string member, string filePath, int line)
		{
			string message2 = LogHelper.CreateEntryWithType(DateTime.UtcNow, level, type.Name, member, line, message);
			Log(level, message2);
		}

		public void Log(LogLevel level, string message, string member, string filePath, int line)
		{
			string message2 = LogHelper.CreateEntryWithoutType(DateTime.UtcNow, level, member, line, message);
			Log(level, message2);
		}

		private void Log(LogLevel level, string message)
		{
			if (Debugger.IsAttached && (level == LogLevel.Error || level == LogLevel.Critical))
			{
				Debugger.Break();
			}
			lock (_queueLock)
			{
				if (_logMessages.Count > 199)
				{
					_logMessages.Dequeue();
				}
				if (message != null)
				{
					_logMessages.Enqueue(message);
				}
			}
		}
	}
}
