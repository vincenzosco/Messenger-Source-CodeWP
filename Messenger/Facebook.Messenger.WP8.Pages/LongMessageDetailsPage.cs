using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Common;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class LongMessageDetailsPage : MessageDetailsViewModelPage
	{
		private SolidColorBrush _fromMe;

		private SolidColorBrush _fromFriend;











		public SolidColorBrush FromMeBrush
		{
			get
			{
				SolidColorBrush val = _fromMe;
				if (val == null)
				{
					object obj = Application.Current.Resources[(object)"SelfMessageBubbleBrush"];
					val = (_fromMe = (SolidColorBrush)((obj is SolidColorBrush) ? obj : null));
				}
				return val;
			}
		}

		public SolidColorBrush FromFriendBrush
		{
			get
			{
				SolidColorBrush val = _fromFriend;
				if (val == null)
				{
					object obj = Application.Current.Resources[(object)"MessageBubbleBrush"];
					val = (_fromFriend = (SolidColorBrush)((obj is SolidColorBrush) ? obj : null));
				}
				return val;
			}
		}

		public LongMessageDetailsPage()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAudioPlayback((Panel)(object)LayoutRoot);
			base.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((MessageDetailsViewModel vm) =>
			{
				if (vm.MessageViewModel != null)
				{
					ImageAttachmentsPanel.UpdateView(vm.MessageViewModel);
					GenericFileAttachmentsPanel.UpdateView(vm.MessageViewModel);
				}
			});
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			InitializeAudioPlayback((Panel)(object)LayoutRoot);
			WithViewModel((MessageDetailsViewModel vm) =>
			{
				if (vm.MessageViewModel != null)
				{
					FacebookContext instance = ServiceLocator.Current.GetInstance<FacebookContext>();
					FacebookProfile profile = instance.Session.Profile;
					if (vm.MessageViewModel.Sender != null && profile.Id == vm.MessageViewModel.Sender.Id)
					{
						DisplayMessageFromLoggedInUser();
					}
					else
					{
						DisplayMessageFromOtherUser();
					}
				}
			});
		}

		private void DisplayMessageFromLoggedInUser()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected Obj, but got Unknown
			SetMessageBackground((Brush)(object)FromMeBrush);
			((Control)Body).Foreground = (Brush)new SolidColorBrush(Colors.White);
		}

		private void DisplayMessageFromOtherUser()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected Obj, but got Unknown
			SetMessageBackground((Brush)(object)FromFriendBrush);
			((Control)Body).Foreground = (Brush)new SolidColorBrush(Colors.Black);
		}

		private void SetMessageBackground(Brush brush)
		{
			ScrollableTextBlock body = Body;
			Border paddingBorder = PaddingBorder;
			Border imageAttachmentBorder = ImageAttachmentBorder;
			Brush val = (GenericFileAttachmentBorder.Background = brush);
			Brush val3 = (imageAttachmentBorder.Background = val);
			Brush background = (paddingBorder.Background = val3);
			((Control)body).Background = background;
		}

	}
}
