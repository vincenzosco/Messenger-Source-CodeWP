using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class MessageDetailControl : MessageListItemControl
	{













		public event EventHandler<NavigationEventArgs> NavigateEvent;

		public event EventHandler<MessageActionArgs> MessageActionEvent;

		public MessageDetailControl()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected Obj, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
			MessageBubbleControl bubble = Bubble;
			EventHandler<bool> value = (object sender, bool b) =>
			{
				TiltEffect.SetIsTiltEnabled((DependencyObject)(object)this, !b);
			};
			bubble.VideoTouched += value;
			Bubble.LayoutGenerator = MessageListItemControl.LayoutGenerator;
		}

		private void ResetView(bool recycled)
		{
			Bubble.ResetView(recycled);
			((UIElement)ProfilePhoto).Visibility = Visibility.Collapsed;
			((DependencyObject)ProfilePhoto).SetValue(Grid.ColumnProperty, (object)0);
			ProfilePhoto.ShowUserTile();
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			Bubble.NavigateEvent += BubbleOnNavigate;
		}

		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			Bubble.NavigateEvent -= BubbleOnNavigate;
		}

		protected override void OnItemValueAssigned()
		{
			ViewModel.PropertyChanged += OnMessagePropertyChanged;
			UpdatePresentation(ViewModel, true);
		}

		private void UpdatePresentation(MessageViewModel m, bool recycled)
		{
			ResetView(recycled);
			FacebookContext instance = ServiceLocator.Current.GetInstance<FacebookContext>();
			FacebookProfile profile = instance.Session.Profile;
			bool flag = m.Sender != null && profile.Id == m.Sender.Id;
			Bubble.UpdatePresentation(m, flag);
			DisplayMessageOrigin(m);
			if (!flag)
			{
				DisplayMessageFromOtherUser();
			}
			else
			{
				DisplayMessageFromSelfForDetailsPage();
			}
			DisplayReadReceipts(m, (TextAlignment)1);
		}

		private void DisplayMessageOrigin(MessageViewModel m)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			string messageOriginText = MessageDetailsUtils.GetMessageOriginText(m);
			OriginTextBlock.Text = messageOriginText;
			((UIElement)OriginTextBlock).Visibility = VisibilityUtil.FromBool(!string.IsNullOrEmpty(messageOriginText));
		}

		private void DisplayMessageFromOtherUser()
		{
			((DependencyObject)ProfilePhoto).SetValue(Grid.ColumnProperty, (object)0);
			((UIElement)ProfilePhoto).Visibility = Visibility.Visible;
			((FrameworkElement)TimeStampTextBlock).HorizontalAlignment = (HorizontalAlignment)0;
			((FrameworkElement)OriginTextBlock).HorizontalAlignment = (HorizontalAlignment)0;
		}

		private void DisplayMessageFromSelfForDetailsPage()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			((DependencyObject)SenderName).SetValue(Grid.ColumnSpanProperty, (object)1);
			((FrameworkElement)SenderName).Margin = new Thickness(0.0, 0.0, 0.0, 10.0);
			((FrameworkElement)SenderName).HorizontalAlignment = (HorizontalAlignment)2;
			((DependencyObject)ProfilePhoto).SetValue(Grid.ColumnProperty, (object)2);
			((FrameworkElement)ProfilePhoto).Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
			((UIElement)ProfilePhoto).Visibility = Visibility.Visible;
			((FrameworkElement)TimeStampTextBlock).HorizontalAlignment = (HorizontalAlignment)2;
			((FrameworkElement)OriginTextBlock).HorizontalAlignment = (HorizontalAlignment)2;
		}

		private void DisplayReadReceipts(MessageViewModel m, TextAlignment alignment)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected Obj, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)ReadReceiptDisplay).Children).Clear();
			foreach (string readIds in m.ReadByUserIds)
			{
				if (m.Recipients != null)
				{
					MailUser mailUser = m.Recipients.FirstOrDefault((MailUser x) => x.Id.Equals(readIds));
					if (mailUser != null)
					{
						TextBlock val = new TextBlock();
						val.Text = mailUser.Name;
						((FrameworkElement)val).HorizontalAlignment = (HorizontalAlignment)3;
						val.TextAlignment = alignment;
						object obj = Application.Current.Resources[(object)"MessengerTextLightGreyBrush"];
						val.Foreground = (Brush)((obj is SolidColorBrush) ? obj : null);
						val.FontSize = (double)Application.Current.Resources[(object)"PhoneFontSizeSmall"];
						TextBlock val2 = val;
						((PresentationFrameworkCollection<UIElement>)(object)((Panel)ReadReceiptDisplay).Children).Add((UIElement)(object)val2);
					}
					continue;
				}
				break;
			}
			if (((PresentationFrameworkCollection<UIElement>)(object)((Panel)ReadReceiptDisplay).Children).Count > 0)
			{
				((UIElement)ReadReceiptLabel).Visibility = Visibility.Visible;
			}
		}

		protected override void Recycled(MessageViewModel oldViewModel)
		{
			ResetView(true);
			oldViewModel.PropertyChanged -= OnMessagePropertyChanged;
		}

		private void OnMessagePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			UpdatePresentation(ViewModel, false);
		}

		private void BubbleOnNavigate(object sender, NavigationEventArgs args)
		{
			if (NavigateEvent != null)
			{
				NavigateEvent(this, args);
			}
		}

		private void SignalMessageAction(MessageAction action)
		{
			MappedMessageViewModel mappedMessageViewModel = base.DataContext as MappedMessageViewModel;
			if (mappedMessageViewModel != null && MessageActionEvent != null)
			{
				MessageActionEvent(this, new MessageActionArgs(mappedMessageViewModel, action));
			}
		}

		private void Bubble_OnViewMoreEvent(object sender, EventArgs e)
		{
			SignalMessageAction(MessageAction.LongMessageDetails);
		}

		private void Bubble_OnMessageActionEvent(object sender, MessageActionArgs e)
		{
			if (e.Action == MessageAction.Forward)
			{
				SignalMessageAction(MessageAction.Forward);
			}
		}

	}
}
