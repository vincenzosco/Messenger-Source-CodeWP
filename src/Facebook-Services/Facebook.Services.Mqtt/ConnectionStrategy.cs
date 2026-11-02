using System;

namespace Facebook.Services.Mqtt
{
	public class ConnectionStrategy
	{
		private static readonly TimeSpan[] RetryIntervals = new TimeSpan[8]
		{
			TimeSpan.Zero,
			TimeSpan.Zero,
			TimeSpan.Zero,
			TimeSpan.FromSeconds(2.0),
			TimeSpan.FromSeconds(4.0),
			TimeSpan.FromSeconds(8.0),
			TimeSpan.FromSeconds(15.0),
			TimeSpan.FromSeconds(30.0)
		};

		private int _retryAttempt;

		public TimeSpan CurrentRetryDelay
		{
			get
			{
				return RetryIntervals[_retryAttempt];
			}
		}

		public void Increment()
		{
			_retryAttempt = Math.Min(_retryAttempt + 1, RetryIntervals.Length - 1);
		}

		public void Reset()
		{
			_retryAttempt = 0;
		}
	}
}
