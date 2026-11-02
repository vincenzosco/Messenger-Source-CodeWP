using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Resources;
using Microsoft.Phone.BackgroundAudio;

namespace Facebook.Messenger.WP8.Agents
{
	internal static class EmptyAudioTrack
	{
		private const string VoidTrackFileName = "void.wav";

		private const string VoidTrackPath = "Assets/void.wav";

		private static readonly Uri VoidTrackUri = new Uri("void.wav", UriKind.Relative);

		private static readonly Lazy<AudioTrack> AudioTrack = new Lazy<AudioTrack>(CreateAudioTrack, true);

		public static AudioTrack Track
		{
			get
			{
				return AudioTrack.Value;
			}
		}

		private static AudioTrack CreateAudioTrack()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected Obj, but got Unknown
			EnsureAudioFileCopied();
			return new AudioTrack(VoidTrackUri, (string)null, (string)null, (string)null, (Uri)null, (string)null, (EnabledPlayerControls)0);
		}

		private static void EnsureAudioFileCopied()
		{
			using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (isolatedStorageFile.FileExists("void.wav"))
				{
					return;
				}
				StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("Assets/void.wav", UriKind.Relative));
				using (IsolatedStorageFileStream isolatedStorageFileStream = isolatedStorageFile.CreateFile("void.wav"))
				{
					byte[] buffer = new byte[4096];
					int count;
					while ((count = resourceStream.Stream.Read(buffer, 0, 4096)) > 0)
					{
						isolatedStorageFileStream.Write(buffer, 0, count);
					}
				}
			}
		}
	}
}
