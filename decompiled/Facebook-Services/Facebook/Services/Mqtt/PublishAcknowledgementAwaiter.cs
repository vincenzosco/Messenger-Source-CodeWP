using System;
using System.Collections.Generic;
using System.Threading;
using Facebook.Base.Mqtt.Messages;

namespace Facebook.Services.Mqtt
{
	public class PublishAcknowledgementAwaiter
	{
		public const string NoPubAckReceivedError = "Failed to receive a PubAck";

		private const long TimeoutMs = 15000L;

		private readonly HashSet<int> _receivedMessageIds = new HashSet<int>();

		private AutoResetEvent _waitEvent = new AutoResetEvent(false);

		private readonly object _pubAckLock = new object();

		public void Reset()
		{
			lock (_pubAckLock)
			{
				_receivedMessageIds.Clear();
				_waitEvent.Set();
			}
		}

		public void Received(PublishAckMessage message)
		{
			lock (_pubAckLock)
			{
				_receivedMessageIds.Add(message.MessageId);
				_waitEvent.Set();
			}
		}

		public bool Await(int messageId)
		{
			DateTime dateTime = DateTime.UtcNow.AddMilliseconds(15000.0);
			while (dateTime > DateTime.UtcNow)
			{
				if (CheckAndRemoveId(messageId))
				{
					return true;
				}
				_waitEvent.WaitOne(dateTime - DateTime.UtcNow);
			}
			return CheckAndRemoveId(messageId);
		}

		private bool CheckAndRemoveId(int id)
		{
			lock (_pubAckLock)
			{
				return _receivedMessageIds.Remove(id);
			}
		}
	}
}
