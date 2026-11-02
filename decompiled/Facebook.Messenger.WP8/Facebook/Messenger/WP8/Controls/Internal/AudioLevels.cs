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
	public class AudioLevels : UserControl
	{
		internal StackPanel AudioSettingsStackPanel;

		private bool _contentLoaded;

		public AudioLevels()
		{
			InitializeComponent();
		}

		private void AudioLevels_OnLoaded(object sender, RoutedEventArgs e)
		{
			AudioLevelsViewModel vm = ((FrameworkElement)this).DataContext as AudioLevelsViewModel;
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
			AudioLevelsViewModel audioLevelsViewModel = ((FrameworkElement)this).DataContext as AudioLevelsViewModel;
			if (audioLevelsViewModel != null)
			{
				audioLevelsViewModel.CopySoundInformationToClipboard();
				MessageBox.Show(InternalStringsLibrary.Internal_Settings_Audio_Copy_Completed);
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Internal/AudioLevels.xaml", UriKind.Relative));
				AudioSettingsStackPanel = (StackPanel)((FrameworkElement)this).FindName("AudioSettingsStackPanel");
			}
		}
	}
}
