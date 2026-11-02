using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api.Cache;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Cache
{
	public class Service : BackgroundService
	{
		private readonly IEnumerable<ICache> _caches;

		private FacebookContext _context;

		private readonly ApplicationState _applicationState;

		public Service(IDispatcher ui, IEnumerable<ICache> caches, ApplicationState applicationState)
			: base(ui)
		{
			_caches = caches;
			_applicationState = applicationState;
		}

		public override void StartService(FacebookContext context)
		{
			_context = context;
			if (_applicationState.CurrentRuntype == ApplicationState.Runtype.Foreground && context.HasPhoneLanguageChanged())
			{
				ClearState();
			}
			Messenger.Default.Register<LogoutSignal>(this, OnLogout);
		}

		public override void StopService()
		{
			Messenger.Default.Unregister(this);
			if (_context.IsAuthenticated())
			{
				SaveState();
			}
			else
			{
				ClearState();
			}
		}

		private void OnLogout(LogoutSignal obj)
		{
			ClearState();
		}

		private void SaveState()
		{
			IEnumerable<Task> source = _caches.Select((ICache cache) => cache.EnsureSave());
			Logger.Info(this, "Saving Cache States", "SaveState", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Cache\\Service.cs", 61);
			Task.WaitAll(source.ToArray());
			Logger.Info(this, "Saved Cache States", "SaveState", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Cache\\Service.cs", 63);
		}

		private void ClearState()
		{
			foreach (ICache cache in _caches)
			{
				cache.Purge();
			}
		}
	}
}
