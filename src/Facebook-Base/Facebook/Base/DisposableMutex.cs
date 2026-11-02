using System;
using System.Threading;
using System.Threading.Tasks;

namespace Facebook.Base
{
	public class DisposableMutex : IDisposable
	{
		private readonly SemaphoreSlim _semaphore;

		public DisposableMutex()
		{
			_semaphore = new SemaphoreSlim(1, 1);
		}

		public async Task<IDisposable> WaitAsync()
		{
			await _semaphore.WaitAsync();
			return this;
		}

		public void Dispose()
		{
			_semaphore.Release();
		}
	}
}
