using System;
using System.Diagnostics;
using System.Windows;
using Facebook.Base.Logging;
using Facebook.Messenger.Crash;
using Facebook.Messenger.Logging;
using Microsoft.Phone;
using Microsoft.Phone.BackgroundAudio;

namespace Facebook.Messenger.WP8.Agents
{
	public class BackgroundSendingAgent : AudioPlayerAgent
	{
		public const string ProcessName = "background_sender";

		private static volatile bool IsInitialized;

		private readonly Facebook.Messenger.Logging.Logger _logger = new Facebook.Messenger.Logging.Logger();

		public BackgroundSendingAgent()
		{
			if (!IsInitialized)
			{
				IsInitialized = true;
				((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(() =>
				{
					Application.Current.UnhandledException += AudioPlayer_UnhandledException;
				}));
				Facebook.Base.Logging.Logger.AddListener(_logger);
			}
		}

		protected override void OnUserAction(BackgroundAudioPlayer player, AudioTrack track, UserAction action, object param)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Invalid comparison between Unknown and I4
			Facebook.Base.Logging.Logger.Info($"[Audio Agent User Action] {action}", "OnUserAction", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundSendingAgent.cs", 37);
			if ((int)action == 3)
			{
				player.Play();
			}
			if ((int)action == 1)
			{
				player.Close();
			}
			((BackgroundAgent)this).NotifyComplete();
		}

		protected override void OnPlayStateChanged(BackgroundAudioPlayer player, AudioTrack track, PlayState playState)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			Facebook.Base.Logging.Logger.Info($"[Audio Agent State Changed] {playState}", "OnPlayStateChanged", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundSendingAgent.cs", 51);
			((BackgroundAgent)this).NotifyComplete();
		}

		protected override void OnError(BackgroundAudioPlayer player, AudioTrack track, Exception error, bool isFatal)
		{
			Facebook.Base.Logging.Logger.Info($"[Audio Agent Error] {error}", "OnError", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundSendingAgent.cs", 57);
			if (isFatal)
			{
				CrashSaver.SaveLog("background_sender", error, true, _logger.LogMessages);
				((BackgroundAgent)this).Abort();
				player.Close();
			}
			else
			{
				((BackgroundAgent)this).NotifyComplete();
			}
		}

		private void AudioPlayer_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			Facebook.Base.Logging.Logger.Error(this, $"[Audio Agent Unhandled Exception] {e}", "AudioPlayer_UnhandledException", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundSendingAgent.cs", 73);
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
			else
			{
				CrashSaver.SaveLog("background_sender", e.ExceptionObject, true, _logger.LogMessages);
			}
		}
	}
}
