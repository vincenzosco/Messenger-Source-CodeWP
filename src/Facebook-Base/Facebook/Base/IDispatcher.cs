using System;
using System.Threading.Tasks;

namespace Facebook.Base
{
	public interface IDispatcher
	{
		Task RunAsync(Action action, DispatcherPriority priorty = DispatcherPriority.Low);

		void Schedule(Action action, DispatcherPriority priorty = DispatcherPriority.Low);
	}
}
