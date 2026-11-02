using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.AudioController;

namespace Facebook.Messenger.WP8.Controls
{
	public class InternalSettingsAudioControl : UserControl
	{
		internal TextBox VolumeTextBox;

		private bool _contentLoaded;

		public event EventHandler<Sounds.Sound> PlayRequested;

		public InternalSettingsAudioControl()
		{
			InitializeComponent();
		}

		private void VolumeControl_OnKeyUp(object sender, KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)e.Key == 3)
			{
				UpdateVolumeAndPlay();
			}
		}

		private void VolumeControl_OnLostFocus(object sender, RoutedEventArgs e)
		{
			UpdateVolume();
		}

		private void PlayButton_OnTap(object sender, GestureEventArgs e)
		{
			UpdateVolumeAndPlay();
		}

		private void UpdateVolumeAndPlay()
		{
			if (UpdateVolume() && PlayRequested != null)
			{
				PlayRequested(this, ((FrameworkElement)this).DataContext as Sounds.Sound);
			}
		}

		private bool UpdateVolume()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			Sounds.Sound sound = ((FrameworkElement)this).DataContext as Sounds.Sound;
			if (sound == null)
			{
				return false;
			}
			float result;
			if (!float.TryParse(VolumeTextBox.Text, out result))
			{
				MessageBox.Show(InternalStringsLibrary.Internal_Settings_Audio_Volume_Error);
				return false;
			}
			sound.Volume = result;
			return true;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/InternalSettingsAudioControl.xaml", UriKind.Relative));
				VolumeTextBox = (TextBox)((FrameworkElement)this).FindName("VolumeTextBox");
			}
		}
	}
}
