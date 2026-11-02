using System;
using Facebook.Base.Logging;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Xna.Framework.Media;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class BackgroundAudioHelper
	{
		public static bool IsAnotherApplicationPlayingAudio
		{
			get
			{
				bool gameHasControl = MediaPlayer.GameHasControl;
				return !gameHasControl;
			}
		}

		public static void Stop()
		{
			try
			{
				BackgroundAudioPlayer.Instance.Stop();
			}
			catch (InvalidOperationException arg)
			{
				Logger.Warning(string.Format("Calling rewind on BackgroundAudioPlayer threw an Exception: {0}", arg), "Stop", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\BackgroundAudioHelper.cs", 26);
			}
		}

		public static void Play()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected Obj, but got Unknown
			try
			{
				BackgroundAudioPlayer.Instance.Close();
				BackgroundAudioPlayer.Instance.Track = new AudioTrack();
				BackgroundAudioPlayer.Instance.Play();
			}
			catch (InvalidOperationException arg)
			{
				Logger.Error(string.Format("Calling play on BackgroundAudioPlayer threw an Exception: {0}", arg), "Play", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\BackgroundAudioHelper.cs", 40);
			}
		}
	}
}
