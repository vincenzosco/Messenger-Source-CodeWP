using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api;

namespace Facebook.Services.ScheduledTasksService
{
	public abstract class ScheduledTask
	{
		public enum Result
		{
			Success,
			Failure
		}

		public string Name { get; private set; }

		public long ExecutionIntervalMs { get; private set; }

		protected ScheduledTask(string name, long executionIntervalMs)
		{
			Name = name;
			ExecutionIntervalMs = executionIntervalMs;
		}

		public abstract Task<Result> Execute(Facebook.Services.Api.Service apiService, FacebookContext context);
	}
}
