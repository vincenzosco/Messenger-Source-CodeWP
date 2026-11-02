using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Details;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class CanonicalThreadDetailsPage : CanonicalThreadDetailsViewModelPage
	{
		private IconBarEntry<MuteButtonState> _muteButton;

		private IconBarEntry<bool> _timelineButton;

		private IconBarEntry<bool> _addToGroupButton;







		public CanonicalThreadDetailsPage()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected Obj, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBar();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((CanonicalThreadDetailsViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
				if (vm.Thread == null)
				{
					vm.FetchThreadCommand.Execute(null);
				}
				UpdateView(vm);
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			WithViewModel((CanonicalThreadDetailsViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
			});
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			WithViewModel((CanonicalThreadDetailsViewModel vm) =>
			{
				switch (e.PropertyName)
				{
				case "Thread":
					UpdateView(vm);
					break;
				case "IsMuteInProgress":
					_muteButton.IsEnabled = !vm.IsMuteInProgress;
					break;
				}
			});
		}

		private void InitializeAppBar()
		{
			AppBarHelper appBarHelper = new AppBarHelper(base.ApplicationBar);
			_muteButton = appBarHelper.CreateBarButtonEntry<MuteButtonState>(new EntryState<MuteButtonState>(MuteButtonState.Mute, StringLibrary.Mute_Conversation_Menu_Item, OnMuteButtonClicked, new Uri("/Assets/AppBar/Mute.png", UriKind.Relative)), new EntryState<MuteButtonState>(MuteButtonState.Unmute, StringLibrary.UnMute_Conversation_Menu_Item, OnUnmuteButtonClicked, new Uri("/Assets/AppBar/Unmute.png", UriKind.Relative)));
			_timelineButton = appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.View_Timeline_Menu_Item, OnViewTimelineButtonClicked, new Uri("/Assets/AppBar/Profile.png", UriKind.Relative)));
			_addToGroupButton = appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.CanonicalThreadDetailsPage_Start_Group, OnStartGroupConversationClicked, new Uri("/Assets/AppBar/AddToGroup.png", UriKind.Relative)));
		}

		private void UpdateView(CanonicalThreadDetailsViewModel vm)
		{
			if (vm.Thread != null)
			{
				_muteButton.State = (vm.Thread.IsMuted ? MuteButtonState.Unmute : MuteButtonState.Mute);
				_muteButton.ResetCurrentState();
			}
			SetMenuItemsEnabled(vm);
		}

		private void SetMenuItemsEnabled(CanonicalThreadDetailsViewModel vm)
		{
			bool flag = vm.Thread != null;
			_muteButton.IsEnabled = flag && !vm.IsMuteInProgress;
			_timelineButton.IsEnabled = flag;
			_addToGroupButton.IsEnabled = flag;
		}

		private void OnMuteButtonClicked(object sender, EventArgs e)
		{
			WithViewModel((CanonicalThreadDetailsViewModel vm) =>
			{
				vm.MuteCommand.Execute(null);
			});
		}

		private void OnUnmuteButtonClicked(object sender, EventArgs e)
		{
			WithViewModel((CanonicalThreadDetailsViewModel vm) =>
			{
				vm.UnmuteCommand.Execute(null);
			});
		}

		private void OnViewTimelineButtonClicked(object sender, EventArgs e)
		{
			WithViewModel((CanonicalThreadDetailsViewModel vm) =>
			{
				vm.LaunchTimelineCommand.Execute(null);
			});
		}

		private void OnStartGroupConversationClicked(object sender, EventArgs e)
		{
			WithViewModel((CanonicalThreadDetailsViewModel vm) =>
			{
				vm.StartGroupConversationWithRecipientCommand.Execute(null);
			});
		}

	}
}
