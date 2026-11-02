using System;
using System.Threading.Tasks;
using Facebook.Base;

namespace Facebook.Messenger.WP8.Agents;

public class BackgroundDispatcher : IDispatcher
{
	public Task RunAsync(Action action, DispatcherPriority priorty = DispatcherPriority.Low)
	{
		return Task.Run(action);
	}

	public void Schedule(Action action, DispatcherPriority priorty = DispatcherPriority.Low)
	{
		action();
	}
}
