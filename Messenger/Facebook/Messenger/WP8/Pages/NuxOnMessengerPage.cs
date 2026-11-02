using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class NuxOnMessengerPage : NuxOnMessengerViewModelPage
	{
		private readonly Uri _nextUriIcon = new Uri("/Assets/AppBar/Next.png", UriKind.Relative);

		private ApplicationBarIconButton _nextButton;








		public NuxOnMessengerPage()
		{
			InitializeComponent();
			InitializeAppBar();
		}

		private void InitializeAppBar()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected Obj, but got Unknown
			_nextButton = new ApplicationBarIconButton(_nextUriIcon);
			_nextButton.Text = StringLibrary.Button_Continue.ToLower();
			_nextButton.Click += NextButton_OnClick;
			base.ApplicationBar.Buttons.Add(_nextButton);
		}

		private void NextButton_OnClick(object sender, EventArgs e)
		{
			(base.DataContext as NuxOnMessengerViewModel)?.ContinueCommand.Execute(null);
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((NuxOnMessengerViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
			});
			if (!BackgroundAudioHelper.IsAnotherApplicationPlayingAudio)
			{
				NuxSound.Source = new Uri("Assets/Sounds/receiveMessageNeueOutOfApp.wma", UriKind.Relative);
				NuxSound.Play();
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("IsLoading"))
			{
				WithViewModel((NuxOnMessengerViewModel vm) =>
				{
					_nextButton.IsEnabled = !vm.IsLoading;
				});
			}
		}

	}
}
