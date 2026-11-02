using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class MessageControl : ContextualMessageListItemControl
	{
		private static readonly Uri PlaceholderImage = new Uri("/Assets/Messages/AdminMessagePlaceholder.png", UriKind.Relative);

		private FacebookContext _context;

		public static readonly DependencyProperty CanRetryProperty = DependencyProperty.Register("CanRetry", typeof(bool), typeof(MessageControl), new PropertyMetadata((object)true, (PropertyChangedCallback)OnRetryChangedCallback));














		private FacebookContext Context => _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);

		public bool CanRetry
		{
			get
			{
				return (bool)base.GetValue(CanRetryProperty);
			}
			set
			{
				base.SetValue(CanRetryProperty, (object)value);
			}
		}

		public event EventHandler<NavigationEventArgs> NavigateEvent;

		public event EventHandler<MessageActionArgs> MessageActionEvent;

		private static void OnRetryChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			MessageControl messageControl = o as MessageControl;
			messageControl.UpdateRetryState();
		}

		private void UpdateRetryState()
		{
			MessageBubbleControl messageBubbleControl = BubblePresenter.Content as MessageBubbleControl;
			if (messageBubbleControl != null)
			{
				messageBubbleControl.CanRetry = CanRetry;
			}
		}

		public MessageControl()
		{
			InitializeComponent();
		}

		private void ResetView(bool recycled)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			ContextualMessageListItemControl.LayoutGenerator.Reset<MessageBubbleControl>(recycled, BubblePresenter, OnBubbleDetach);
			Thickness margin = ((FrameworkElement)RootGrid).Margin;
			margin.Top = 5.0;
			((FrameworkElement)RootGrid).Margin = margin;
			((UIElement)AdminMessagePresenter).Visibility = Visibility.Collapsed;
			((UIElement)SenderName).Visibility = Visibility.Collapsed;
			((UIElement)SeparatorTimestamp).Visibility = Visibility.Collapsed;
		}

		protected override void OnItemValueAssigned()
		{
			ViewModel.PropertyChanged += OnMessagePropertyChanged;
			UpdatePresentation(true);
			UpdateMessageStateVisibility();
			UpdateSeenHeads();
		}

		private void UpdatePresentation(bool recycled)
		{
			ResetView(recycled);
			if (!ViewModel.IsRegularMessage())
			{
				DisplayAdminMessage();
				return;
			}
			UpdateMessageBubble();
			if (ViewModel.ShowTimestamp)
			{
				((UIElement)SeparatorTimestamp).Visibility = Visibility.Visible;
				SeparatorTimestamp.Text = ThreadDateConverter.FormatMessageDateAsString(ViewModel.Timestamp);
			}
		}

		private void UpdateMessageBubble()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			bool flag = IsMessageFromLoggedInUser();
			MessageBubbleControl bubbleControl = ContextualMessageListItemControl.LayoutGenerator.Load(BubblePresenter, (MessageBubbleControl c) =>
			{
				c.UpdatePresentation(ViewModel, IsMessageFromLoggedInUser());
				c.CanRetry = CanRetry;
			}, OnBubbleAttach);
			if (flag)
			{
				DisplayMessageFromSelf(bubbleControl);
			}
			else
			{
				DisplayMessageFromOtherUser(bubbleControl);
			}
			if (ViewModel.IsLaterInGroup())
			{
				Thickness margin = ((FrameworkElement)RootGrid).Margin;
				margin.Top = 2.0;
				((FrameworkElement)RootGrid).Margin = margin;
			}
		}

		private bool IsMessageFromLoggedInUser()
		{
			FacebookProfile profile = Context.Session.Profile;
			if (ViewModel.Sender != null)
			{
				return profile.Id == ViewModel.Sender.Id;
			}
			return false;
		}

		private void UpdateSeenHeads()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			bool flag = ViewModel.MostRecentMessageReadByUserIds.Any();
			SeenHeadsControl seenHeadsControl = GetSeenHeadsControl();
			((UIElement)seenHeadsControl).Visibility = VisibilityUtil.FromBool(flag);
			seenHeadsControl.UpdateSeenHeads(ViewModel.MostRecentMessageReadByUserIds);
			if (flag)
			{
				((UIElement)DeliveryStatusImage).Visibility = Visibility.Collapsed;
			}
		}

		private SeenHeadsControl GetSeenHeadsControl()
		{
			if (!ViewModel.IsSingleRecipient)
			{
				return GroupSeenHeadsControl;
			}
			return SeenHeadsControl;
		}

		private void DisplayMessageFromSelf(MessageBubbleControl bubbleControl)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			bubbleControl.BubbleContainer.BottomNubVisibility = VisibilityUtil.FromBool(!ViewModel.IsEarlierInGroup());
			((UIElement)SenderName).Visibility = Visibility.Collapsed;
			((UIElement)UserTilePresenter).Visibility = Visibility.Collapsed;
		}

		private void UpdateMessageStateVisibility()
		{
			if (ViewModel.MessageType != MailMessage.Type.Regular || !IsMessageFromLoggedInUser())
			{
				((UIElement)DeliveryStatusImage).Visibility = Visibility.Collapsed;
				return;
			}
			switch (ViewModel.State)
			{
			case MailMessage.MessageState.Sent:
			case MailMessage.MessageState.Queued:
			case MailMessage.MessageState.Failed:
			case MailMessage.MessageState.Delivered:
				((UIElement)DeliveryStatusImage).Visibility = Visibility.Visible;
				break;
			default:
				((UIElement)DeliveryStatusImage).Visibility = Visibility.Collapsed;
				break;
			}
			DeliveryStatusImage.Source = (ImageSource)(object)MessageDetailsUtils.MessageStateImage(ViewModel.State);
			UpdateTouchOnMessage(ViewModel);
		}

		private void UpdateTouchOnMessage(MessageViewModel m)
		{
			MessageBubbleControl messageBubbleControl = BubblePresenter.Content as MessageBubbleControl;
			if (messageBubbleControl != null)
			{
				((UIElement)messageBubbleControl).IsHitTestVisible = m.State != MailMessage.MessageState.Failed;
			}
			if (m.State == MailMessage.MessageState.Failed)
			{
				base.Tap += DeliveryStatusImage_ShowContextMenu;
				base.Hold += DeliveryStatusImage_ShowContextMenu;
			}
			else
			{
				base.Tap -= DeliveryStatusImage_ShowContextMenu;
				base.Hold -= DeliveryStatusImage_ShowContextMenu;
			}
		}

		private void DeliveryStatusImage_ShowContextMenu(object sender, GestureEventArgs e)
		{
			e.Handled = true;
			(BubblePresenter.Content as MessageBubbleControl)?.ShowContextMenu();
		}

		private void DisplayMessageFromOtherUser(MessageBubbleControl bubbleControl)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			SenderName.Text = ((ViewModel.Sender != null) ? ViewModel.Sender.FirstName : string.Empty);
			bool flag = ViewModel.IsLaterInGroup();
			bool isSingleRecipient = ViewModel.IsSingleRecipient;
			((UIElement)SenderName).Visibility = VisibilityUtil.FromBool(!isSingleRecipient && !flag);
			bubbleControl.BubbleContainer.TopNubVisibility = VisibilityUtil.FromBool(!flag);
			((UIElement)UserTilePresenter).Visibility = Visibility.Visible;
			UserTile userTile = UserTilePresenter.Content as UserTile;
			if (ViewModel.IsEarlierInGroup())
			{
				userTile?.HideUserTile();
			}
			if (!ViewModel.IsEarlierInGroup())
			{
				if (userTile == null)
				{
					double num = (double)Application.Current.Resources[(object)"MessagesProfilePictureSize"];
					UserTile userTile2 = new UserTile();
					userTile2.IndicatorDiameter = 20.0;
					((FrameworkElement)userTile2).Width = num;
					((FrameworkElement)userTile2).Height = num;
					userTile = userTile2;
					UserTilePresenter.Content = userTile;
				}
				((FrameworkElement)userTile).DataContext = ViewModel.Sender;
				userTile.ShowUserTile();
			}
		}

		private void OnMessagePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
			case "State":
				UpdateMessageStateVisibility();
				break;
			case "MostRecentMessageReadByUserIds":
				UpdateSeenHeads();
				break;
			case "Attachments":
			case "Shares":
			case "MessageGroupState":
				UpdatePresentation(false);
				break;
			}
		}

		protected override void Recycled(ContextualMessageViewModel oldViewModel)
		{
			ResetView(true);
			oldViewModel.PropertyChanged -= OnMessagePropertyChanged;
		}

		private void DisplayAdminMessage()
		{
			((UIElement)UserTilePresenter).Visibility = Visibility.Collapsed;
			((UIElement)AdminMessagePresenter).Visibility = Visibility.Visible;
			AdminMessageControl adminMessageControl;
			if (AdminMessagePresenter.Content == null)
			{
				adminMessageControl = new AdminMessageControl();
				adminMessageControl.NavigateEvent += OnNavigate;
				AdminMessagePresenter.Content = adminMessageControl;
			}
			else
			{
				adminMessageControl = AdminMessagePresenter.Content as AdminMessageControl;
			}
			adminMessageControl.AdminMessageBlock.Text = ViewModel.Body;
			if (ViewModel.MessageType == MailMessage.Type.ThreadImageSet)
			{
				int size = ((((FrameworkElement)adminMessageControl.AdminImage).Width > 0.0) ? ((int)((FrameworkElement)adminMessageControl.AdminImage).Width) : 100);
				string uriString = AttachmentUriGenerator.UrlForMessage(ViewModel.Id, size);
				Uri result;
				if (Uri.TryCreate(uriString, UriKind.Absolute, out result))
				{
					ImageCacheExtensions.SetPlaceholderUri((FrameworkElement)(object)adminMessageControl.AdminImage, PlaceholderImage);
					ImageCacheExtensions.SetImageUri((FrameworkElement)(object)adminMessageControl.AdminImage, result);
					((UIElement)adminMessageControl.AdminImageBlock).Visibility = Visibility.Visible;
				}
				else
				{
					((UIElement)adminMessageControl.AdminImageBlock).Visibility = Visibility.Collapsed;
				}
			}
			else
			{
				((UIElement)adminMessageControl.AdminImageBlock).Visibility = Visibility.Collapsed;
			}
		}

		private void OnNavigate(object sender, NavigationEventArgs args)
		{
			if (NavigateEvent != null)
			{
				NavigateEvent(this, args);
			}
		}

		private void TriggerMessageAction(MessageAction action)
		{
			ContextualMessageViewModel message = base.DataContext as ContextualMessageViewModel;
			OnBubbleMessageActionEvent(this, new MessageActionArgs(message, action));
		}

		private void OnBubbleMessageActionEvent(object sender, MessageActionArgs e)
		{
			if (MessageActionEvent != null)
			{
				MessageActionEvent(sender, e);
			}
		}

		private void Bubble_OnViewMoreEvent(object sender, EventArgs e)
		{
			TriggerMessageAction(MessageAction.LongMessageDetails);
		}

		private void Bubble_OnCancelEvent(object sender, EventArgs e)
		{
			ContextualMessageViewModel contextualMessageViewModel = base.DataContext as ContextualMessageViewModel;
			if (contextualMessageViewModel != null && contextualMessageViewModel.IsCancellable)
			{
				TriggerMessageAction(MessageAction.Cancel);
			}
		}

		private void OnBubbleAttach(MessageBubbleControl c)
		{
			c.LayoutGenerator = ContextualMessageListItemControl.LayoutGenerator;
			c.CancelEvent += Bubble_OnCancelEvent;
			c.ViewMoreEvent += Bubble_OnViewMoreEvent;
			c.NavigateEvent += OnNavigate;
			c.MessageActionEvent += OnBubbleMessageActionEvent;
		}

		private void OnBubbleDetach(MessageBubbleControl c)
		{
			c.CancelEvent -= Bubble_OnCancelEvent;
			c.ViewMoreEvent -= Bubble_OnViewMoreEvent;
			c.NavigateEvent -= OnNavigate;
			c.MessageActionEvent -= OnBubbleMessageActionEvent;
		}


		static MessageControl()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected Obj, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected Obj, but got Unknown
		}
	}
}
