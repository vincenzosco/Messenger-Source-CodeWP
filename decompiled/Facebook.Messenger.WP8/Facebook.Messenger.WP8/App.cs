using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Crash;
using Facebook.Messenger.Logging;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.Settings;
using Facebook.Messenger.Utilities;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Messenger.WP8.Utilities.Stickers;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Analytics;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api;
using Facebook.Services.Common;
using Facebook.Services.MessageSender;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Practices.ServiceLocation;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace Facebook.Messenger.WP8
{
	public class App : Application
	{
		public const string ProcessName = "app";

		private readonly FacebookContext _context;

		private readonly ServiceEngine _serviceEngine;

		private readonly Facebook.Services.MessageSender.Service _messageSenderService;

		private readonly Facebook.Services.Analytics.Service _analyticsService;

		private readonly INetworkStatus _networkStatus;

		private readonly ApplicationState _applicationState;

		private readonly BackgroundSendingAgentEnabledProvider _backgroundSendingAgentEnabledProvider;

		private readonly SingleExecutionRuntimeLock _runtimeLock = new SingleExecutionRuntimeLock();

		private Navigator _navigator;

		private bool _phoneApplicationInitialized;

		private bool _contentLoaded;

		public static PhoneApplicationFrame RootFrame { get; private set; }

		public static UiDispatcher Dispatcher { get; private set; }

		public static ExceptionHelper ExceptionHelper { get; private set; }

		public ShareOperation PendingShare { get; private set; }

		public App()
		{
			ExceptionHelper = new ExceptionHelper((Application)(object)this);
			FacebookMessengerUiLocator provider = FacebookMessengerUiLocator.Instance;
			ServiceLocator.SetLocatorProvider(() => provider);
			_serviceEngine = ServiceLocator.Current.GetInstance<ServiceEngine>();
			_messageSenderService = ServiceLocator.Current.GetInstance<Facebook.Services.MessageSender.Service>();
			_analyticsService = ServiceLocator.Current.GetInstance<Facebook.Services.Analytics.Service>();
			_networkStatus = ServiceLocator.Current.GetInstance<INetworkStatus>();
			_applicationState = ServiceLocator.Current.GetInstance<ApplicationState>();
			_backgroundSendingAgentEnabledProvider = ServiceLocator.Current.GetInstance<BackgroundSendingAgentEnabledProvider>();
			_context = provider.GetInstance<FacebookContext>();
			_context.LoggedIn += OnContextLoggedIn;
			_context.LoggedOut += OnContextLoggedOut;
			_context.GlobalMuteEvent += OnContextMuteChanged;
			ExceptionHelper.ResetRageShake(_context.Session.Profile);
			((Application)this).UnhandledException += Application_UnhandledException;
			InitializeComponent();
			((Application)this).Resources.Add("FacebookContext", (object)_context);
			InitializePhoneApplication();
			InitializeDispatcher();
			PhoneContactsManager instance = ServiceLocator.Current.GetInstance<PhoneContactsManager>();
			instance.Register();
			InitializeLanguage();
			if (Debugger.IsAttached)
			{
				Application.Current.Host.Settings.EnableFrameRateCounter = true;
				PhoneApplicationService.Current.UserIdleDetectionMode = (IdleDetectionMode)1;
			}
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<JewelType>(this, OnInboxSeen);
			ThemeManager.OverrideOptions = ThemeManagerOverrideOptions.SystemTrayColors;
			ThemeManager.ToLightTheme();
		}

		private static void OnInboxSeen(JewelType type)
		{
			TileUpdater.ResetCount();
		}

		private static void InitializeDispatcher()
		{
			Dispatcher = ServiceLocator.Current.GetInstance<IDispatcher>() as UiDispatcher;
			if (Dispatcher != null)
			{
				Dispatcher.Dispatcher = ((DependencyObject)RootFrame).Dispatcher;
			}
		}

		private void OnContextMuteChanged(object sender, GlobalMuteSignal e)
		{
			IMessenger msgr = GalaSoft.MvvmLight.Messaging.Messenger.Default;
			GlobalMuteSignal signal = new GlobalMuteSignal
			{
				MuteDuration = e.MuteDuration
			};
			Dispatcher.RunAsync(() =>
			{
				msgr.Send(signal);
			}, DispatcherPriority.Normal);
		}

		private void OnContextLoggedIn(object sender, FacebookSession session)
		{
			ExceptionHelper.ResetRageShake(session.Profile);
			IMessenger msgr = GalaSoft.MvvmLight.Messaging.Messenger.Default;
			LoginSignal signal = new LoginSignal(session);
			Dispatcher.RunAsync(() =>
			{
				msgr.Send(signal);
			}, DispatcherPriority.Normal);
			Dispatcher.RunAsync(PrefetchLargeProfilePicture);
		}

		public void PrefetchLargeProfilePicture()
		{
			if (_context.IsAuthenticated())
			{
				string url = ProfilePhotoConverter.CreatePhotoUrl(_context, _context.Session.UserId, 212);
				ImageCacheExtensions.PrefetchImage(url);
			}
		}

		private void OnContextLoggedOut(object sender, LogoutEventArgs e)
		{
			ExceptionHelper.ResetRageShake();
			IMessenger msgr = GalaSoft.MvvmLight.Messaging.Messenger.Default;
			LogoutSignal signal = new LogoutSignal(e.Session);
			StickerImageUrlHelper instance = ServiceLocator.Current.GetInstance<StickerImageUrlHelper>();
			instance.Reset().DoNotWait();
			Dispatcher.Schedule(() =>
			{
				_navigator.Reset(FacebookUri.GetFirstNavigationUri(e.SessionWasExpired));
				msgr.Send(signal);
			}, DispatcherPriority.Normal);
			TileUpdater.ResetCount();
		}

		private bool CanInvokeBackgroundAgent()
		{
			bool isAnotherApplicationPlayingAudio = BackgroundAudioHelper.IsAnotherApplicationPlayingAudio;
			if (_backgroundSendingAgentEnabledProvider.Enabled)
			{
				return !isAnotherApplicationPlayingAudio;
			}
			return false;
		}

		private Action CheckQueuedMessagesAndGetLaunchBackgroundAgentAction()
		{
			if (!_messageSenderService.HasQueuedRequests())
			{
				return null;
			}
			int numQueuedRequests = _messageSenderService.GetNumQueuedRequests();
			bool isAnotherApplicationPlayingAudio = BackgroundAudioHelper.IsAnotherApplicationPlayingAudio;
			bool enabled = _backgroundSendingAgentEnabledProvider.Enabled;
			BackgroundSendingEventLogger.LogFailedMessagesOnExit(numQueuedRequests, isAnotherApplicationPlayingAudio, enabled, _analyticsService);
			if (CanInvokeBackgroundAgent())
			{
				_context.SetBackgroundSendingAgentShouldRun();
				return BackgroundAudioHelper.Play;
			}
			return ToastHelper.ShowPendingSendsToast;
		}

		private void Application_Launching(object sender, LaunchingEventArgs e)
		{
			StartApplication(((object)e).ToString());
			_navigator.ApplicationStarted();
			ShareLaunchingEventArgs e2 = (ShareLaunchingEventArgs)(object)((e is ShareLaunchingEventArgs) ? e : null);
			if (e2 != null)
			{
				PendingShare = e2.ShareTargetActivatedEventArgs.ShareOperation;
			}
		}

		private void Application_Activated(object sender, ActivatedEventArgs e)
		{
			StartApplication(((object)e).ToString());
			_navigator.ApplicationResumed(e.IsApplicationInstancePreserved);
		}

		private void StartApplication(string eventString)
		{
			bool backgroundInstanceWasRun = BackgroundAnalyticsUtil.WasBackgroundSendingInstanceRun();
			CheckAndPostStartupLogs();
			FileLogger fileLogger = StartupLogger.StartNew();
			ShutDownBackgroundAgent(backgroundInstanceWasRun);
			_applicationState.AppIsStartingUp();
			TileUpdater.ResetCount();
			_networkStatus.ResetNetworkMap();
			ResumeApplicationFrame();
			try
			{
				_serviceEngine.StartServices(_context);
			}
			catch (Exception e)
			{
				Facebook.Base.Logging.Logger.Critical(this, "Fatal error starting services: " + eventString, "StartApplication", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\App.xaml.cs", 274);
				CrashSaver.SaveLog("app", e, true, ExceptionHelper.Log.LogMessages);
				ExceptionHelper.CheckForCrashLogs().Wait();
				throw;
			}
			CheckAndPostBackgroundErrors();
			CheckAndPostShutdownLogs();
			CheckAndLogBackgroundSendingAnalytics(backgroundInstanceWasRun);
			ExceptionHelper.CheckForCrashLogs().DoNotWait();
			_context.PurgeOldFolders().DoNotWait();
			fileLogger.Stop();
		}

		private void ShutDownBackgroundAgent(bool backgroundInstanceWasRun)
		{
			if (!BackgroundAudioHelper.IsAnotherApplicationPlayingAudio && backgroundInstanceWasRun)
			{
				BackgroundAudioHelper.Stop();
			}
			_runtimeLock.WaitForOwnerToRelease();
		}

		private void Application_Deactivated(object sender, DeactivatedEventArgs e)
		{
			Teardown();
		}

		private void Application_Closing(object sender, ClosingEventArgs e)
		{
			Teardown();
		}

		private void Teardown()
		{
			FileLogger fileLogger = ShutdownLogger.StartNew();
			TeardownApplicationFrame();
			_context.SavePhoneLanguage();
			_applicationState.AppIsShuttingDown();
			TileUpdater.ResetCount();
			Action action = CheckQueuedMessagesAndGetLaunchBackgroundAgentAction();
			_serviceEngine.StopServices();
			if (action != null)
			{
				_context.SetShutdownErrorStep(CrashSaver.ErrorLogType.ExecuteBackgroundSendingStep.ToString());
				action();
			}
			fileLogger.Stop();
			_context.ClearShutdownErrorStep();
		}

		public void Shutdown()
		{
			_applicationState.AppWasManuallyTerminated();
			Application_Closing(null, null);
			AppSettingsManager.Settings.Save();
			((Application)this).Terminate();
		}

		private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			Facebook.Base.Logging.Logger.Error(string.Format("Navigation to URI \"{0}\" failed with exception: {1}", e.Uri, e.Exception), "RootFrame_NavigationFailed", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\App.xaml.cs", 351);
			if (e.Exception.Message.Contains("Navigation is not allowed when the task is not in the foreground."))
			{
				e.Handled = true;
			}
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
		}

		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
		}

		private void CheckAndLogBackgroundSendingAnalytics(bool backgroundInstanceWasRun)
		{
			if (_context.GetBackgroundSendingAgentShouldRun())
			{
				if (!backgroundInstanceWasRun)
				{
					CrashSaver.CreateErrorLog("app", CrashSaver.ErrorLogType.BackgroundSendingError, "Background sending instance should have run, but it did not.", ExceptionHelper.Log.LogMessages);
				}
				_context.ResetBackgroundSendingAgentShouldRun();
			}
		}

		private void CheckAndPostShutdownLogs()
		{
			string fileLog = ShutdownLogger.ReadAndDeleteShutdownLog();
			CheckAndPostFileLogs(fileLog, CrashSaver.ErrorLogType.ShutdownError, "Error shutting down application");
		}

		private void CheckAndPostStartupLogs()
		{
			string fileLog = StartupLogger.ReadAndDeleteStartupLog();
			CheckAndPostFileLogs(fileLog, CrashSaver.ErrorLogType.StartupError, "Error starting application - possible deadlock");
		}

		private void CheckAndPostFileLogs(string fileLog, CrashSaver.ErrorLogType error, string message)
		{
			if (fileLog != null)
			{
				CrashSaver.ErrorLogType result = error;
				string[] logs = fileLog.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
				if (result == CrashSaver.ErrorLogType.ShutdownError)
				{
					string lastServiceFromLogs = GetLastServiceFromLogs(logs);
					message = string.Format("{0} - {1}", message, lastServiceFromLogs);
				}
				if (!string.IsNullOrEmpty(_context.GetShutdownErrorStep()))
				{
					Enum.TryParse<CrashSaver.ErrorLogType>(_context.GetShutdownErrorStep(), true, out result);
				}
				string result2 = CrashSaver.CreateErrorLog("app", result, message, logs);
				CrashSender.PostToServer(result2, _context.Session.Credentials.AccessToken).DoNotWait();
			}
		}

		private static string GetLastServiceFromLogs(string[] logs)
		{
			Regex regex = new Regex("(Stopping Services.*START )([a-zA-Z0-9\\.]+?)( .*)");
			string result = "";
			foreach (string item in logs.Reverse())
			{
				Match match = regex.Match(item);
				if (match.Success && match.Groups.Count > 3)
				{
					result = match.Groups[2].Value;
					break;
				}
			}
			return result;
		}

		private static void CheckAndPostBackgroundErrors()
		{
			Exception error = BackgroundAudioPlayer.Instance.Error;
			if (error != null)
			{
				CrashSender.PostToServer(string.Format("Error occurred in background agent: {0}", error)).DoNotWait();
			}
		}

		private static void ResumeApplicationFrame()
		{
			MessengerApplicationFrame messengerApplicationFrame = RootFrame as MessengerApplicationFrame;
			if (messengerApplicationFrame != null)
			{
				messengerApplicationFrame.ResumeFrame();
			}
		}

		private static void TeardownApplicationFrame()
		{
			MessengerApplicationFrame messengerApplicationFrame = RootFrame as MessengerApplicationFrame;
			if (messengerApplicationFrame != null)
			{
				messengerApplicationFrame.TearDownFrame();
			}
		}

		private void InitializePhoneApplication()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected Obj, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected Obj, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected Obj, but got Unknown
			if (!_phoneApplicationInitialized)
			{
				RootFrame = (PhoneApplicationFrame)(object)CreateApplicationFrame();
				((Frame)RootFrame).Navigated += CompleteInitializePhoneApplication;
				((Frame)RootFrame).NavigationFailed += RootFrame_NavigationFailed;
				((Frame)RootFrame).Navigated += (object sender, NavigationEventArgs args) =>
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					Facebook.Base.Logging.Logger.Info(this, string.Format("Navigated with mode {0} to {1}", args.NavigationMode, args.Uri), "InitializePhoneApplication", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\App.xaml.cs", 491);
				};
				InitializeNavigation();
				_phoneApplicationInitialized = true;
			}
		}

		private static MessengerApplicationFrame CreateApplicationFrame()
		{
			MessengerApplicationFrame messengerApplicationFrame = new MessengerApplicationFrame();
			((FrameworkElement)messengerApplicationFrame).DataContext = ServiceLocator.Current.GetInstance<MessengerApplicationViewModel>();
			return messengerApplicationFrame;
		}

		private void InitializeNavigation()
		{
			((Frame)RootFrame).UriMapper = (UriMapperBase)(object)new ExternalLaunchUriFilter();
			_navigator = ServiceLocator.Current.GetInstance<Navigator>();
			_navigator.SetFrame(RootFrame);
		}

		private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected Obj, but got Unknown
			((Application)this).RootVisual = (UIElement)(object)RootFrame;
			((Frame)RootFrame).Navigated -= CompleteInitializePhoneApplication;
		}

		private void InitializeLanguage()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				((FrameworkElement)RootFrame).FlowDirection = LocalizationUtils.GetFlowDirectionForCurrentLocale();
			}
			catch
			{
				if (Debugger.IsAttached)
				{
					Debugger.Break();
				}
				throw;
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/App.xaml", UriKind.Relative));
			}
		}
	}
}
