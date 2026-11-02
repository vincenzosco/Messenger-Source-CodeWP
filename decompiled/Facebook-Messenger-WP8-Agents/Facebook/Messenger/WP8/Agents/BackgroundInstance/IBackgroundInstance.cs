using System;
using System.Threading.Tasks;

namespace Facebook.Messenger.WP8.Agents.BackgroundInstance
{
	public interface IBackgroundInstance
	{
		bool IsInstanceRunning { get; }

		EventHandler ExecutionComplete { get; set; }

		Task Execute();

		void Cancel();
	}
}
