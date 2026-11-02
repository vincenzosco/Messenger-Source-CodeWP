using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.ViewModels.Internal;

namespace Facebook.Messenger.WP8.Controls.Internal
{
	public partial class AudioLevels : UserControl
	{


		public AudioLevels()
		{
			InitializeComponent();
		}

		private void AudioLevels_OnLoaded(object sender, RoutedEventArgs e)
		{
			AudioLevelsViewModel vm = base.DataContext as AudioLevelsViewModel;
			if (vm == null)
			{
				return;
			}
			foreach (Sounds.Sound sound in vm.Sounds)
			{
				InternalSettingsAudioControl internalSettingsAudioControl = new InternalSettingsAudioControl();
				((FrameworkElement)internalSettingsAudioControl).DataContext = sound;
				InternalSettingsAudioControl internalSettingsAudioControl2 = internalSettingsAudioControl;
				internalSettingsAudioControl2.PlayRequested += (object o, Sounds.Sound s) =>
				{
					vm.PlaySound(s);
				};
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)AudioSettingsStackPanel).Children).Add((UIElement)(object)internalSettingsAudioControl2);
			}
		}

		private void CopyAudioData_OnTap(object sender, GestureEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			AudioLevelsViewModel audioLevelsViewModel = base.DataContext as AudioLevelsViewModel;
			if (audioLevelsViewModel != null)
			{
				audioLevelsViewModel.CopySoundInformationToClipboard();
				MessageBox.Show(InternalStringsLibrary.Internal_Settings_Audio_Copy_Completed);
			}
		}

	}
}
