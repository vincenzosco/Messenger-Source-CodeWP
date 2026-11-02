using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Facebook.Base;
using Facebook.Base.Logging;

namespace Facebook.Messenger
{
	public class UiDispatcher : IDispatcher
	{
		public Dispatcher Dispatcher { get; set; }

		public Task RunAsync(Action action, DispatcherPriority priority = DispatcherPriority.Low)
		{
			Schedule(action, priority);
			return Task.FromResult(true);
		}

		public void Schedule(Action action, DispatcherPriority priorty = DispatcherPriority.Low)
		{
			if (Dispatcher == null)
			{
				Logger.Warning(this, "Calling RunAsync with no window dispatcher", "Schedule", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\UiDispatcher.cs", 23);
			}
			else
			{
				Dispatcher.BeginInvoke(action);
			}
		}
	}
}
