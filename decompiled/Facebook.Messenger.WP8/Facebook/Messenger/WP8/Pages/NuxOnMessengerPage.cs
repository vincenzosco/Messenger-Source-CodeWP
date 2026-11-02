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
	public class NuxOnMessengerPage : NuxOnMessengerViewModelPage
	{
		private readonly Uri _nextUriIcon = new Uri("/Assets/AppBar/Next.png", UriKind.Relative);

		private ApplicationBarIconButton _nextButton;

		internal Grid LayoutRoot;

		internal Border TitleTopMargin;

		internal Image ProfilePhoto;

		internal Image Logo;

		internal TextBlock WelcomeTitle;

		internal MediaElement NuxSound;

		private bool _contentLoaded;

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
			((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_nextButton);
		}

		private void NextButton_OnClick(object sender, EventArgs e)
		{
			(((FrameworkElement)this).DataContext as NuxOnMessengerViewModel)?.ContinueCommand.Execute(null);
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

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/NuxOnMessengerPage.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				TitleTopMargin = (Border)((FrameworkElement)this).FindName("TitleTopMargin");
				ProfilePhoto = (Image)((FrameworkElement)this).FindName("ProfilePhoto");
				Logo = (Image)((FrameworkElement)this).FindName("Logo");
				WelcomeTitle = (TextBlock)((FrameworkElement)this).FindName("WelcomeTitle");
				NuxSound = (MediaElement)((FrameworkElement)this).FindName("NuxSound");
			}
		}
	}
}
