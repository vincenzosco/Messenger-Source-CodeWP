using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.ScheduledTasksService.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.ScheduledTasksService
{
	public class Service : BackgroundService
	{
		private static readonly TimeSpan ScheduledTaskStartDelay = TimeSpan.FromMilliseconds(10000.0);

		private readonly IEnumerable<ScheduledTask> _scheduledTasks;

		private FacebookContext _context;

		private readonly Facebook.Services.Api.Service _apiService;

		private CancellationTokenSource _cancelTokenSource;

		public Service(IDispatcher ui, Facebook.Services.Api.Service service)
			: base(ui)
		{
			_apiService = service;
			_scheduledTasks = new List<ScheduledTask>
			{
				new GatekeeperSync()
			};
		}

		public override void StartService(FacebookContext context)
		{
			_context = context;
			_cancelTokenSource = new CancellationTokenSource();
			Messenger.Default.Register(this, (LoginSignal signal) =>
			{
				Task.Run((Func<Task>)CheckScheduledTasks);
			});
			Task.Run(() => CheckScheduledTasks());
		}

		public override void StopService()
		{
			if (_cancelTokenSource != null && !_cancelTokenSource.IsCancellationRequested)
			{
				_cancelTokenSource.Cancel();
			}
		}

		private async Task CheckScheduledTasks()
		{
			if (!_context.IsAuthenticated())
			{
				return;
			}
			await Task.Delay(ScheduledTaskStartDelay, _cancelTokenSource.Token);
			lock (_scheduledTasks)
			{
				IEnumerable<ScheduledTask> enumerable = _scheduledTasks.Where(ShouldRunTask);
				foreach (ScheduledTask item in enumerable)
				{
					RunTask(item);
				}
			}
		}

		private bool ShouldRunTask(ScheduledTask task)
		{
			DateTime lastScheduledTaskSuccessTime = _context.GetLastScheduledTaskSuccessTime(task.Name);
			return (DateTime.Now - lastScheduledTaskSuccessTime).TotalMilliseconds >= (double)task.ExecutionIntervalMs;
		}

		private async void RunTask(ScheduledTask task)
		{
			if (await task.Execute(_apiService, _context) == ScheduledTask.Result.Success)
			{
				_context.SetLastScheduledTaskSuccessTime(task.Name, DateTime.Now);
				Logger.Info(this, $"Task {task.Name} completed successfully", "RunTask", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\ScheduledTasksService\\Service.cs", 81);
			}
			else
			{
				Logger.Error(this, $"Task {task.Name} failed", "RunTask", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\ScheduledTasksService\\Service.cs", 85);
			}
		}
	}
}
