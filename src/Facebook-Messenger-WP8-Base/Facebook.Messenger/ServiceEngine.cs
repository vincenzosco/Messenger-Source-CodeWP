using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.Crash;
using Facebook.Models;
using Facebook.Services;

namespace Facebook.Messenger
{
	public class ServiceEngine
	{
		private readonly IList<BackgroundService> _services;

		private readonly object _lock = new object();

		private FacebookContext _context;

		private bool _servicesStarted;

		public ServiceEngine(IList<BackgroundService> services)
		{
			_services = services;
		}

		public void StartServices(FacebookContext context)
		{
			lock (_lock)
			{
				StopwatchLogger stopwatchLogger = StopwatchLogger.StartNew("Starting Services");
				_context = context;
				ExecuteServiceStep("Starting", stopwatchLogger, _services, (BackgroundService service) =>
				{
					service.StartService(context);
				});
				_servicesStarted = true;
				stopwatchLogger.End();
			}
		}

		public void StopServices()
		{
			lock (_lock)
			{
				if (!_servicesStarted)
				{
					return;
				}
				StopwatchLogger stopwatch = StopwatchLogger.StartNew("Stopping Services");
				BackgroundService[] servicesInReverse = _services.Reverse().ToArray();
				_context.SetShutdownErrorStep(CrashSaver.ErrorLogType.BeginStoppingServicesStep.ToString());
				ExecuteServiceStep("Prepare Stop (UI thread)", stopwatch, servicesInReverse, (BackgroundService service) =>
				{
					service.PrepareStopServiceUiThreadMustNotUseDispatcher();
				});
				Logger.Info(this, "Finished Prepare Stop (UI thread), spinning off thread", "StopServices", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\ServiceEngine.cs", 60);
				Task.Run(() =>
				{
					Logger.Info(this, "Thread spun off, now starting Prepare Stop", "StopServices", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\ServiceEngine.cs", 64);
					_context.SetShutdownErrorStep(CrashSaver.ErrorLogType.ExecutePrepareStoppingServicesStep.ToString());
					ExecuteServiceStep("Prepare Stop", stopwatch, servicesInReverse, (BackgroundService service) =>
					{
						service.PrepareStopService();
					});
					_context.SetShutdownErrorStep(CrashSaver.ErrorLogType.ExecuteStoppingServicesStep.ToString());
					ExecuteServiceStep("Stop", stopwatch, servicesInReverse, (BackgroundService service) =>
					{
						service.StopService();
					});
				}).Wait();
				_servicesStarted = false;
				stopwatch.End();
			}
		}

		private static void ExecuteServiceStep(string step, StopwatchLogger stopwatch, IEnumerable<BackgroundService> services, Action<BackgroundService> action)
		{
			stopwatch.LogSubtaskStart(step);
			foreach (BackgroundService service in services)
			{
				stopwatch.LogSubtaskStart(service.GetType().FullName);
				action(service);
				stopwatch.LogSubtaskEnd();
			}
			stopwatch.LogSubtaskEnd();
		}
	}
}
