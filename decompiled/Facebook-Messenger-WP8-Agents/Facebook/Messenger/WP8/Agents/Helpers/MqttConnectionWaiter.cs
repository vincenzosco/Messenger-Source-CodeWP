using System;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Extensions;
using Facebook.Base.Mqtt;
using Facebook.Models.Signals;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.Agents.Helpers
{
	public class MqttConnectionWaiter
	{
		public static async Task WaitAsync(int delay)
		{
			CancellationTokenSource watchCancellation = new CancellationTokenSource();
			Action<MqttConnectionStateChangedSignal> watcher = (MqttConnectionStateChangedSignal signal) =>
			{
				if (signal.State == ClientState.Connected)
				{
					watchCancellation.Cancel();
				}
			};
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register(watcher, watcher);
			await Task.Delay(delay, watchCancellation.Token).RunAndDetermineIfCancelled();
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(watcher, watcher);
		}
	}
}
