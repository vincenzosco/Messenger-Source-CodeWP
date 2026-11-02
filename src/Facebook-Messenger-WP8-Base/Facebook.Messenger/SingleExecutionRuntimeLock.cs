using System.Threading;
using Facebook.Base.Logging;

namespace Facebook.Messenger
{
	public class SingleExecutionRuntimeLock
	{
		private const string SemaphoreName = "FACEBOOK_MESSENGER_SEMAPHORE";

		private readonly Semaphore _semaphore;

		public SingleExecutionRuntimeLock()
		{
			_semaphore = new Semaphore(1, 1, "FACEBOOK_MESSENGER_SEMAPHORE");
		}

		public void Acquire()
		{
			try
			{
				Logger.Info(this, "Attempting to acquire semaphore", "Acquire", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\SingleExecutionRuntimeLock.cs", 21);
				_semaphore.WaitOne();
				Logger.Info(this, "Semaphore successfully acquired", "Acquire", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\SingleExecutionRuntimeLock.cs", 23);
			}
			catch (AbandonedMutexException arg)
			{
				Logger.Warning(this, string.Format("Semaphore was abandoned: {0}", arg), "Acquire", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\SingleExecutionRuntimeLock.cs", 27);
			}
		}

		public void TryRelease()
		{
			Logger.Info(this, "Attempting to release semaphore", "TryRelease", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\SingleExecutionRuntimeLock.cs", 33);
			_semaphore.Release();
			Logger.Info(this, "Semaphore successfully released", "TryRelease", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\SingleExecutionRuntimeLock.cs", 35);
		}

		public void WaitForOwnerToRelease()
		{
			Acquire();
			TryRelease();
		}
	}
}
