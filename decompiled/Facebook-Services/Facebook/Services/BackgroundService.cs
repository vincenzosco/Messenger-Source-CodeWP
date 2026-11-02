using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services;

public abstract class BackgroundService : Service
{
	protected readonly IDispatcher UiDispatcher;

	protected BackgroundService(IDispatcher ui)
	{
		UiDispatcher = ui;
	}

	protected Task Signal<T>(T payload, DispatcherPriority priority = DispatcherPriority.Low)
	{
		return UiDispatcher.RunAsync(() =>
		{
			Messenger.Default.Send(payload);
		}, priority);
	}

	protected Task Signal<T>(string token, T payload, DispatcherPriority priority = DispatcherPriority.Low)
	{
		return UiDispatcher.RunAsync(() =>
		{
			Messenger.Default.Send(payload, token);
		}, priority);
	}

	protected void PerformSignal<T>(T payload) where T : class
	{
		if (payload != null)
		{
			Signal(payload);
		}
	}

	public abstract void StartService(FacebookContext context);

	public virtual void PrepareStopService()
	{
	}

	public abstract void StopService();

	public virtual void PrepareStopServiceUiThreadMustNotUseDispatcher()
	{
	}
}
