using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Models.Signals;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Presence;

public class TypingHelper
{
	private const int TypingExpiredIntervalMs = 30000;

	private readonly Dictionary<string, long> _typingIdsToLastTypingTime = new Dictionary<string, long>();

	private readonly AutoRenewCancellationTokenSource _typingTimerCancelSource = new AutoRenewCancellationTokenSource();

	private readonly IDispatcher _uiDispatcher;

	public TypingHelper(IDispatcher uiDispatcher)
	{
		_uiDispatcher = uiDispatcher;
	}

	public void Stop()
	{
		StopTypingTimer();
	}

	public void Cleanup()
	{
		StopTypingTimer();
		_typingIdsToLastTypingTime.Clear();
	}

	public bool IsUserTyping(string id)
	{
		lock (_typingIdsToLastTypingTime)
		{
			if (string.IsNullOrWhiteSpace(id) || !_typingIdsToLastTypingTime.ContainsKey(id))
			{
				return false;
			}
			bool flag = HasTypingExpired(_typingIdsToLastTypingTime[id]);
			if (flag)
			{
				_typingIdsToLastTypingTime.Remove(id);
			}
			return !flag;
		}
	}

	public void UpdateTyping(string senderId, bool isTyping)
	{
		if (isTyping)
		{
			AddUserTyping(senderId);
		}
		else
		{
			RemoveUserTyping(senderId);
		}
		SignalTypingChanged(senderId, isTyping);
	}

	private static bool HasTypingExpired(long lastTypingTime)
	{
		long num = DateTime.UtcNow.ToUnixTimeMilliseconds() - lastTypingTime;
		return num > 30000;
	}

	private void RemoveUserTyping(string id)
	{
		lock (_typingIdsToLastTypingTime)
		{
			if (!string.IsNullOrWhiteSpace(id) && _typingIdsToLastTypingTime.ContainsKey(id))
			{
				_typingIdsToLastTypingTime.Remove(id);
				if (!_typingIdsToLastTypingTime.Any())
				{
					StopTypingTimer();
				}
			}
		}
	}

	private void AddUserTyping(string id)
	{
		lock (_typingIdsToLastTypingTime)
		{
			bool flag = !_typingIdsToLastTypingTime.Any();
			long value = DateTime.UtcNow.ToUnixTimeMilliseconds();
			if (!_typingIdsToLastTypingTime.ContainsKey(id))
			{
				_typingIdsToLastTypingTime.Add(id, value);
			}
			else
			{
				_typingIdsToLastTypingTime[id] = value;
			}
			if (flag)
			{
				ResetTypingTimer();
			}
		}
	}

	private void SignalTypingChanged(string id, bool isTyping)
	{
		_uiDispatcher.RunAsync(() =>
		{
			Messenger.Default.Send(new MessageTypingReceivedSignal
			{
				SenderId = id,
				State = isTyping
			});
		});
	}

	private void StopTypingTimer()
	{
		_typingTimerCancelSource.CancelAndRenew();
	}

	private void ResetTypingTimer()
	{
		StopTypingTimer();
		LoopTypingTimer().DoNotWait();
	}

	private async Task LoopTypingTimer()
	{
		while (!_typingTimerCancelSource.IsCancellationRequested && !(await Task.Delay(30000, _typingTimerCancelSource.Token).RunAndDetermineIfCancelled()))
		{
			lock (_typingIdsToLastTypingTime)
			{
				Dictionary<string, long> dictionary = new Dictionary<string, long>(_typingIdsToLastTypingTime);
				foreach (KeyValuePair<string, long> item in dictionary)
				{
					string key = item.Key;
					long value = item.Value;
					if (HasTypingExpired(value))
					{
						_typingIdsToLastTypingTime.Remove(key);
						SignalTypingChanged(key, isTyping: false);
					}
				}
				if (!_typingIdsToLastTypingTime.Any())
				{
					break;
				}
			}
		}
	}
}
