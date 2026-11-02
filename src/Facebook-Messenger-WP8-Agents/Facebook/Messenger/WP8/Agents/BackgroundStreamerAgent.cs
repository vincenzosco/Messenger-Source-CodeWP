using System;
using System.Windows.Media;
using Facebook.Base.Logging;
using Facebook.Messenger.WP8.Agents.BackgroundInstance;
using Microsoft.Phone;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Agents
{
	public class BackgroundStreamerAgent : AudioStreamingAgent
	{
		private const int MaxRunTimeMs = 300000;

		private static volatile bool IsInitialized;

		private readonly IBackgroundInstance _backgroundSendingInstance;

		private StreamSource StreamSource;

		private object _lock = new object();

		public BackgroundStreamerAgent()
		{
			if (!IsInitialized)
			{
				IsInitialized = true;
				ServiceLocator.SetLocatorProvider(() => FacebookMessengerBackgroundLocator.Instance);
				_backgroundSendingInstance = ServiceLocator.Current.GetInstance<IBackgroundInstance>();
				IBackgroundInstance backgroundSendingInstance = _backgroundSendingInstance;
				backgroundSendingInstance.ExecutionComplete = (EventHandler)Delegate.Combine(backgroundSendingInstance.ExecutionComplete, new EventHandler(OnMessageSendingComplete));
			}
		}

		protected override void OnBeginStreaming(AudioTrack track, AudioStreamer streamer)
		{
			Logger.Info("[Audio Streamer Agent] Begin Streaming", "OnBeginStreaming", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundStreamerAgent.cs", 37);
			lock (_lock)
			{
				if (_backgroundSendingInstance.IsInstanceRunning)
				{
					return;
				}
			}
			StreamSource = new StreamSource();
			StreamSource.StreamComplete += OnStreamComplete;
			streamer.SetSource((MediaStreamSource)(object)StreamSource);
			if (!_backgroundSendingInstance.Execute().Wait(300000))
			{
				Logger.Info("Forced audio agent shutdown for exceeding maximum run time", "OnBeginStreaming", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundStreamerAgent.cs", 52);
				StreamSource.Shutdown();
			}
		}

		protected override void OnCancel()
		{
			Logger.Info("[Audio Streamer Agent] Cancelled", "OnCancel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundStreamerAgent.cs", 60);
			StreamSource.Shutdown();
		}

		private void OnStreamComplete(object sender, EventArgs e)
		{
			Logger.Info("[Audio Streamer Agent] Stream Complete", "OnStreamComplete", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundStreamerAgent.cs", 66);
			lock (_lock)
			{
				if (_backgroundSendingInstance.IsInstanceRunning)
				{
					_backgroundSendingInstance.Cancel();
					StreamSource.StreamComplete -= OnStreamComplete;
				}
			}
			NotifyComplete();
		}

		private void OnMessageSendingComplete(object sender, EventArgs e)
		{
			Logger.Info("[Audio Streamer Agent] Message Sending Complete", "OnMessageSendingComplete", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundStreamerAgent.cs", 80);
			StreamSource.Shutdown();
		}
	}
}
