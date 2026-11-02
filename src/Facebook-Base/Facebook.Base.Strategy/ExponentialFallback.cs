using System;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Extensions;

namespace Facebook.Base.Strategy
{
	public class ExponentialFallback<TParameter, TResult>
	{
		public delegate Task<StrategyResponse<TResult>> ActionDelegate(int attempt, CancellationToken token, TParameter paramters);

		private const int DefaultInitialDelayMilliseconds = 1000;

		private const int DefaultMaxDelayMilliseconds = 10000;

		private readonly ActionDelegate _action;

		private readonly Func<TResult> _defaultCreator;

		public ExponentialFallback(ActionDelegate action, Func<TResult> defaultCreator = null)
		{
			_action = action;
			_defaultCreator = defaultCreator ?? ((Func<TResult>)(() => default(TResult)));
		}

		public async Task<StrategyResponse<TResult>> Execute(CancellationToken token, TParameter parameters, int initialDelay = 1000, int maxDelayMs = 10000)
		{
			int attempt = 0;
			while (!token.IsCancellationRequested)
			{
				StrategyResponse<TResult> response = await _action(attempt, token, parameters);
				if (response.ExecutionResult != ExecutionResult.Failed)
				{
					return response;
				}
				if (token.IsCancellationRequested)
				{
					break;
				}
				int delayMs = (int)Math.Min(maxDelayMs, Math.Pow(2.0, attempt) * (double)initialDelay);
				if (await Task.Delay(delayMs, token).RunAndDetermineIfCancelled())
				{
					break;
				}
				attempt++;
			}
			return StrategyResponse.Create(ExecutionResult.Cancelled, _defaultCreator());
		}
	}
}
