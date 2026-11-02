using System.Collections.Generic;
using Facebook.Base.Logging;

namespace Facebook.Services.Mqtt;

public class NotificationDeduplicator
{
	public enum Uniqueness
	{
		Unique,
		Duplicate
	}

	private const int MaxMessageIdHistory = 20;

	private readonly Queue<string> _messageNotificationHistory = new Queue<string>();

	private readonly object _messageNotificationHistoryLock = new object();

	public Uniqueness Process(string messageId)
	{
		lock (_messageNotificationHistoryLock)
		{
			if (_messageNotificationHistory.Contains(messageId))
			{
				Logger.Info(this, $"Duplicate message ignored {messageId}", "Process", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\NotificationDeduplicator.cs", 25);
				return Uniqueness.Duplicate;
			}
			_messageNotificationHistory.Enqueue(messageId);
			while (_messageNotificationHistory.Count > 20)
			{
				_messageNotificationHistory.Dequeue();
			}
			return Uniqueness.Unique;
		}
	}
}
