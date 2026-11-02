using System;
using System.Collections.Generic;
using System.Threading;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Analytics;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class FacebookMessengerViewModel : ViewModelBase
	{
		private Facebook.Services.Analytics.Service _analytics;

		protected CancellationTokenSource CancelTokenSource;

		private bool _loading;

		private string _title;

		private RelayCommand<NavigationEventArgs> _navigateEventCommand;

		protected Facebook.Services.Api.Service ApiService { get; private set; }

		protected Facebook.Services.Analytics.Service AnalyticsService => _analytics ?? (_analytics = ServiceLocator.Current.GetInstance<Facebook.Services.Analytics.Service>());

		protected INetworkStatus NetworkStatus { get; private set; }

		protected IDispatcher Dispatcher { get; private set; }

		protected Navigator Navigator { get; private set; }

		protected ITranslator Translator { get; private set; }

		public FacebookUri PathUri { get; private set; }

		public FacebookContext Context { get; private set; }

		public bool Foregrounded { get; set; }

		public bool IsLoading
		{
			get
			{
				return _loading;
			}
			set
			{
				if (_loading != value)
				{
					Set(ref _loading, value, false, "IsLoading");
				}
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				Set(ref _title, value, false, "Title");
			}
		}

		public bool HasConnection => NetworkStatus.IsConnected();

		public string LogPageUrl { get; set; }

		public bool IsPartialUser => Context.IsPartialUser();

		public RelayCommand NavigateBackCommand => new RelayCommand(() =>
		{
			Navigator.GoBack();
		});

		public RelayCommand<NavigationEventArgs> NavigateCommand => _navigateEventCommand ?? (_navigateEventCommand = new RelayCommand<NavigationEventArgs>(OnNavigation));

		public event EventHandler<Error> ErrorOccurred;

		public string T(string resourceName)
		{
			return resourceName;
		}

		public void OnError(Error error)
		{
			if (ErrorOccurred != null && error != null)
			{
				ErrorOccurred(this, error);
			}
		}

		private void OnNavigation(NavigationEventArgs obj)
		{
			Navigator.Navigate(obj.Uri, obj.NavigationData);
		}

		public FacebookMessengerViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
		{
			CancelTokenSource = new CancellationTokenSource();
			ApiService = apiService;
			Dispatcher = dispatcher;
			Context = context;
			Navigator = navigator;
			NetworkStatus = networkStatus;
		}

		public virtual void SaveState(Dictionary<string, object> state)
		{
		}

		public virtual void LoadState(Dictionary<string, object> state)
		{
		}

		protected T GetStateValue<T>(Dictionary<string, object> state, string key, T defaultValue = default(T))
		{
			object value;
			if (state.TryGetValue(key, out value) && value is T)
			{
				return (T)value;
			}
			return defaultValue;
		}

		public virtual bool CanUpdate(Uri uri)
		{
			return false;
		}

		public virtual void SetPath(Uri uri)
		{
			PathUri = (uri as FacebookUri) ?? FacebookUri.UnknownUri;
		}

		public bool UriPathMatches(Uri uri)
		{
			if (PathUri == null || uri == null)
			{
				return false;
			}
			if (uri.Host == PathUri.Host)
			{
				return uri.AbsolutePath == PathUri.AbsolutePath;
			}
			return false;
		}

		public override void Cleanup()
		{
			CancelTokenSource.Cancel();
			base.Cleanup();
		}

		public virtual void OnApplicationResumed()
		{
		}

		protected void LogPageReady()
		{
			PageNavigationLogger.LogNavigationFinish(LogPageUrl, AnalyticsService);
		}

		public virtual void LogNavigatedTo()
		{
		}
	}
}
