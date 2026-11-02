using System;
using System.Threading.Tasks;

namespace Facebook.Base
{
	public class DisposableMutexInit : DisposableMutex
	{
		private Func<Task> _init;

		private bool _initialized;

		public DisposableMutexInit(Func<Task> init)
		{
			_init = init;
		}

		public new async Task<IDisposable> WaitAsync()
		{
			await base.WaitAsync();
			if (!_initialized)
			{
				await _init();
				_init = null;
				_initialized = true;
			}
			return this;
		}

		public async Task<bool> IsInitialized()
		{
			using (await base.WaitAsync())
			{
				return _initialized;
			}
		}
	}
}
