using System;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Settings;
using Facebook.Messenger.Crash;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Notification;

namespace Facebook.Messenger.WP8.Push
{
	public class Service : BackgroundService
	{
		private const string PushAppVersionKey = "PushAppVersion";

		private const int RegistrationIdle = 0;

		private const int RegistrationInProgress = 1;

		private readonly IEnvironmentInformation _environmentInformation;

		private FacebookContext _context;

		private CancellationTokenSource _cancellationTokenSource;

		private HttpNotificationChannel _channel;

		private int _registrationInProgress;

		public Service(IDispatcher ui, IEnvironmentInformation environmentInformation)
			: base(ui)
		{
			_environmentInformation = environmentInformation;
		}

		public override void StartService(FacebookContext context)
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<LoginSignal>(this, OnLogin);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<LogoutSignal>(this, OnLogout);
			_context = context;
			if (context.IsAuthenticated())
			{
				Task.Run((Func<Task>)EnsurePushRegistered);
			}
		}

		public override void StopService()
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
			_context = null;
			if (_channel != null)
			{
				MicrosoftPushRegistration.UnregisterChannelListeners(_channel, OnPushChannelUriUpdated, OnPushChannelErrorOccurred);
				_channel = null;
			}
		}

		private void CancelAndCreateNewToken()
		{
			if (_cancellationTokenSource != null)
			{
				try
				{
					_cancellationTokenSource.Cancel();
				}
				catch (Exception e)
				{
					CrashLogHelper.SaveObservedException("Error calling cancel on token", e, "CancelAndCreateNewToken", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 78);
				}
			}
			_cancellationTokenSource = new CancellationTokenSource();
		}

		private void OnLogin(LoginSignal signal)
		{
			if (_context.IsAuthenticated())
			{
				Task.Run((Func<Task>)EnsurePushRegistered);
			}
		}

		private void OnLogout(LogoutSignal signal)
		{
			Task.Run((Func<Task>)DestroyRegistration);
		}

		private async Task EnsurePushRegistered()
		{
			if (1 == Interlocked.Exchange(ref _registrationInProgress, 1))
			{
				return;
			}
			try
			{
				CancelAndCreateNewToken();
				CancellationToken token = _cancellationTokenSource.Token;
				FacebookContext context = _context;
				IUserSettings settings = context.Settings;
				string currentPushVersion = context.Settings.GetSetting<string>("PushAppVersion");
				_channel = await MicrosoftPushRegistration.GetOrRegisterChannel(currentPushVersion, OnPushChannelUriUpdated, OnPushChannelErrorOccurred, _context);
				MicrosoftPushRegistration.ChannelState channelState = MicrosoftPushRegistration.GetChannelState(_channel);
				if (channelState != MicrosoftPushRegistration.ChannelState.Valid)
				{
					Logger.Error(this, string.Format("Microsoft push registration failed, got channel state of {0}, giving up", channelState), "EnsurePushRegistered", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 115);
				}
				else if (!token.IsCancellationRequested)
				{
					string appVersion = _environmentInformation.GetApplicationVersionAsThreePartString();
					settings.SaveSetting("PushAppVersion", appVersion);
					string uri = _channel.ChannelUri.AbsoluteUri;
					bool facebookRegistrationSuccess = await FacebookPushRegistration.EnsureFacebookRegistration(context, uri, appVersion, token);
					if (token.IsCancellationRequested)
					{
						Logger.Warning(this, "Registration was cancelled", "EnsurePushRegistered", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 129);
					}
					else if (!facebookRegistrationSuccess)
					{
						CrashLogHelper.SaveError(CrashSaver.ErrorLogType.PushRegistrationError, "Error registering push channel with Facebook", "EnsurePushRegistered", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 135);
					}
				}
			}
			catch (Exception e)
			{
				CrashLogHelper.SaveObservedException("Error registering Microsoft and Facebook push", e, "EnsurePushRegistered", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 140);
			}
			finally
			{
				Interlocked.Exchange(ref _registrationInProgress, 0);
			}
		}

		private async Task DestroyRegistration()
		{
			Logger.Info(this, "Destroying Push Registration", "DestroyRegistration", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 150);
			CancelAndCreateNewToken();
			MicrosoftPushRegistration.DestroyChannel(_channel, OnPushChannelUriUpdated, OnPushChannelErrorOccurred);
			await FacebookPushRegistration.UnregisterWithFacebook(_context, _cancellationTokenSource.Token);
		}

		private void OnPushChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
		{
			Logger.Info(this, "Push Uri Updated", "OnPushChannelUriUpdated", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 159);
			if (_context != null && _context.IsAuthenticated())
			{
				CancelAndCreateNewToken();
				EnsurePushRegistered().DoNotWait();
			}
		}

		private void OnPushChannelErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Invalid comparison between Unknown and I4
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Invalid comparison between Unknown and I4
			Logger.Info(this, string.Format("Push channel error. Error Type: {0}, Error Code: {1}, Error Additional Data: {2}, Error Message: {3}", e.ErrorType, e.ErrorCode, e.ErrorAdditionalData, e.Message), "OnPushChannelErrorOccurred", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 169);
			if (_context != null && _context.IsAuthenticated() && ((int)e.ErrorType == 0 || (int)e.ErrorType == 1))
			{
				if ((int)e.ErrorType == 1)
				{
					MicrosoftPushRegistration.DestroyChannel(_channel, OnPushChannelUriUpdated, OnPushChannelErrorOccurred);
					CrashLogHelper.SaveError(CrashSaver.ErrorLogType.PayloadFormatError, "Received payload format error from Microsoft push channel", "OnPushChannelErrorOccurred", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\Service.cs", 179);
				}
				MicrosoftPushRegistration.UnregisterChannelListeners(_channel, OnPushChannelUriUpdated, OnPushChannelErrorOccurred);
				EnsurePushRegistered().DoNotWait();
			}
		}
	}
}
