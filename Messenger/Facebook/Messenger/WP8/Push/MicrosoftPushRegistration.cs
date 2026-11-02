using System;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.Crash;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Microsoft.Phone.Notification;

namespace Facebook.Messenger.WP8.Push
{
	public class MicrosoftPushRegistration
	{
		public enum ChannelState
		{
			Valid,
			ChannelIsNull,
			ChannelNotConnected,
			ChannelConnectedButUriInvalid
		}

		public class PushRegistrationException : Exception
		{
			public PushRegistrationException(string message, Exception innerException)
				: base(message, innerException)
			{
			}
		}

		private const string MessengerPushChannelName = "Messenger.PushChannel";

		private const string MessengerPushServiceName = "push.delivery.facebook.com";

		private const int WaitForValidChannelTimeout = 20000;

		private const int WaitForValidChannelInterval = 1000;

		private static readonly Version MigrationVersion = new Version("8.0.4");

		public static ChannelState GetChannelState(HttpNotificationChannel channel)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			if (channel == null)
			{
				return ChannelState.ChannelIsNull;
			}
			if ((int)channel.ConnectionStatus != 0)
			{
				return ChannelState.ChannelNotConnected;
			}
			if (channel.ChannelUri == null)
			{
				return ChannelState.ChannelConnectedButUriInvalid;
			}
			if (string.IsNullOrWhiteSpace(channel.ChannelUri.AbsoluteUri))
			{
				return ChannelState.ChannelConnectedButUriInvalid;
			}
			return ChannelState.Valid;
		}

		public static async Task<HttpNotificationChannel> GetOrRegisterChannel(string pushRegistrationVersion, EventHandler<NotificationChannelUriEventArgs> channelUriUpdatedHandler, EventHandler<NotificationChannelErrorEventArgs> errorOccurredHandler, FacebookContext context)
		{
			EnsureUpgrade(pushRegistrationVersion, channelUriUpdatedHandler, errorOccurredHandler);
			Logger.Info("Attempting to find channel", "GetOrRegisterChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 53);
			HttpNotificationChannel channel = HttpNotificationChannel.Find("Messenger.PushChannel");
			ChannelState channelState = GetChannelState(channel);
			Logger.Info($"Channel state is: {channelState}", "GetOrRegisterChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 57);
			if (channelState != ChannelState.Valid && channelState != ChannelState.ChannelNotConnected)
			{
				if (channelState == ChannelState.ChannelConnectedButUriInvalid)
				{
					string message = $"Channel state was bad - destroying and starting again. State: {channelState}, Uri: {channel.ChannelUri}";
					CrashLogHelper.SaveError(CrashSaver.ErrorLogType.BadChannelState, message, "GetOrRegisterChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 66);
					DestroyChannel(channel, channelUriUpdatedHandler, errorOccurredHandler);
				}
				channel = new HttpNotificationChannel("Messenger.PushChannel", "push.delivery.facebook.com");
				Logger.Info("Created new channel, opening", "GetOrRegisterChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 71);
				try
				{
					channel.Open();
				}
				catch (Exception ex)
				{
					if (IsFatalOpenException(ex))
					{
						CrashLogHelper.SaveObservedException("Fatal open exception, destroying channel", ex, "GetOrRegisterChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 80);
						DestroyChannel(channel, channelUriUpdatedHandler, errorOccurredHandler);
					}
					else
					{
						PushRegistrationException e = new PushRegistrationException("Exception occurred while trying to open a push registration channel", ex);
						CrashLogHelper.SaveObservedException("Failed to open channel during create", e, "GetOrRegisterChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 86);
					}
					return null;
				}
			}
			DateTime startTime = DateTime.Now;
			while (startTime.TotalMillisecondsSinceNow() < 20000.0)
			{
				ChannelState state = GetChannelState(channel);
				if (state == ChannelState.Valid)
				{
					break;
				}
				Logger.Info($"Waiting for channel, current state is {state} after {startTime.TotalMillisecondsSinceNow()}ms", "GetOrRegisterChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 99);
				await Task.Delay(1000);
			}
			RegisterChannelListeners(channel, channelUriUpdatedHandler, errorOccurredHandler);
			return channel;
		}

		public static void UnregisterChannelListeners(HttpNotificationChannel channel, EventHandler<NotificationChannelUriEventArgs> channelUriUpdatedHandler, EventHandler<NotificationChannelErrorEventArgs> errorOccurredHandler)
		{
			if (channel != null)
			{
				channel.ChannelUriUpdated -= channelUriUpdatedHandler;
				channel.ErrorOccurred -= errorOccurredHandler;
			}
		}

		public static void DestroyChannel(HttpNotificationChannel channel, EventHandler<NotificationChannelUriEventArgs> channelUriUpdatedHandler, EventHandler<NotificationChannelErrorEventArgs> errorOccurredHandler)
		{
			if (channel == null)
			{
				return;
			}
			Logger.Info("Destroying Microsoft push channel", "DestroyChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 123);
			UnregisterChannelListeners(channel, channelUriUpdatedHandler, errorOccurredHandler);
			try
			{
				channel.Open();
			}
			catch (ArgumentException ex)
			{
				Logger.Warning("Exception opening channel to destroy it - good chance it's already open so ignoring: " + ex, "DestroyChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 134);
			}
			catch (Exception e)
			{
				CrashLogHelper.SaveObservedException("Could not open channel to destory", e, "DestroyChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 138);
				return;
			}
			try
			{
				if (channel.IsShellToastBound)
				{
					channel.UnbindToShellToast();
				}
			}
			catch (Exception e2)
			{
				CrashLogHelper.SaveObservedException("Error unbinding Microsoft push channel from toast", e2, "DestroyChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 148);
			}
			try
			{
				if (channel.IsShellTileBound)
				{
					channel.UnbindToShellTile();
				}
			}
			catch (Exception e3)
			{
				CrashLogHelper.SaveObservedException("Error unbinding Microsoft push channel from tile", e3, "DestroyChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 157);
			}
			try
			{
				Logger.Info("Closing and disposing Microsoft channel", "DestroyChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 162);
				channel.Close();
				channel.Dispose();
			}
			catch (Exception e4)
			{
				CrashLogHelper.SaveObservedException("Failed to close Microsoft push channel", e4, "DestroyChannel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 168);
			}
		}

		private static void EnsureUpgrade(string pushRegistrationVersion, EventHandler<NotificationChannelUriEventArgs> channelUriUpdatedHandler, EventHandler<NotificationChannelErrorEventArgs> errorOccurredHandler)
		{
			Version result;
			if (Version.TryParse(pushRegistrationVersion, out result) && result >= MigrationVersion)
			{
				Logger.Info("No channel upgrade required", "EnsureUpgrade", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 181);
				return;
			}
			HttpNotificationChannel val = HttpNotificationChannel.Find("Messenger.PushChannel");
			if (val == null)
			{
				Logger.Info("Upgrade was required but no channel found so skipping", "EnsureUpgrade", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 188);
				return;
			}
			Logger.Warning("Performing Microsoft push channel upgrade", "EnsureUpgrade", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 192);
			DestroyChannel(val, channelUriUpdatedHandler, errorOccurredHandler);
		}

		private static bool IsFatalOpenException(Exception e)
		{
			return e.Message.Contains("The maximum number");
		}

		private static void RegisterChannelListeners(HttpNotificationChannel channel, EventHandler<NotificationChannelUriEventArgs> channelUriUpdatedHandler, EventHandler<NotificationChannelErrorEventArgs> errorOccurredHandler)
		{
			channel.ChannelUriUpdated += channelUriUpdatedHandler;
			channel.ErrorOccurred += errorOccurredHandler;
			try
			{
				if (!channel.IsShellToastBound)
				{
					channel.BindToShellToast();
				}
			}
			catch (Exception e)
			{
				CrashLogHelper.SaveObservedException("Error binding Microsoft push channel from toast", e, "RegisterChannelListeners", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 220);
			}
			try
			{
				if (!channel.IsShellTileBound)
				{
					channel.BindToShellTile();
				}
			}
			catch (Exception e2)
			{
				CrashLogHelper.SaveObservedException("Error binding Microsoft push channel from tile", e2, "RegisterChannelListeners", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\MicrosoftPushRegistration.cs", 229);
			}
		}
	}
}
