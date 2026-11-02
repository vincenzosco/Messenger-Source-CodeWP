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
	public class LongMessageDetailsPage : MessageDetailsViewModelPage
	{
		private SolidColorBrush _fromMe;

		private SolidColorBrush _fromFriend;

		internal MessageDetailsViewModelPage LongMessageDetailsViewPage;

		internal Grid LayoutRoot;

		internal MessageTopBar MessageTopBar;

		internal ScrollableTextBlock Body;

		internal Border PaddingBorder;

		internal Border ImageAttachmentBorder;

		internal ImageAttachmentsControl ImageAttachmentsPanel;

		internal Border GenericFileAttachmentBorder;

		internal GenericFileAttachmentsControl GenericFileAttachmentsPanel;

		private bool _contentLoaded;

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
			((FrameworkElement)this).Loaded += OnLoaded;
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

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/LongMessageDetailsPage.xaml", UriKind.Relative));
				LongMessageDetailsViewPage = (MessageDetailsViewModelPage)((FrameworkElement)this).FindName("LongMessageDetailsViewPage");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				MessageTopBar = (MessageTopBar)((FrameworkElement)this).FindName("MessageTopBar");
				Body = (ScrollableTextBlock)((FrameworkElement)this).FindName("Body");
				PaddingBorder = (Border)((FrameworkElement)this).FindName("PaddingBorder");
				ImageAttachmentBorder = (Border)((FrameworkElement)this).FindName("ImageAttachmentBorder");
				ImageAttachmentsPanel = (ImageAttachmentsControl)((FrameworkElement)this).FindName("ImageAttachmentsPanel");
				GenericFileAttachmentBorder = (Border)((FrameworkElement)this).FindName("GenericFileAttachmentBorder");
				GenericFileAttachmentsPanel = (GenericFileAttachmentsControl)((FrameworkElement)this).FindName("GenericFileAttachmentsPanel");
			}
		}
	}
}
