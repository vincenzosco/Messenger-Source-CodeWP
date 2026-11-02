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

namespace Facebook.Messenger.WP8.Controls;

public class MessageControl : ContextualMessageListItemControl
{
	private static readonly Uri PlaceholderImage = new Uri("/Assets/Messages/AdminMessagePlaceholder.png", UriKind.Relative);

	private FacebookContext _context;

	public static readonly DependencyProperty CanRetryProperty = DependencyProperty.Register("CanRetry", typeof(bool), typeof(MessageControl), new PropertyMetadata((object)true, (PropertyChangedCallback)OnRetryChangedCallback));

	internal Grid RootGrid;

	internal ContentPresenter AdminMessagePresenter;

	internal ColumnDefinition ProfilePhotoColumn;

	internal ColumnDefinition ContentColumn;

	internal TextBlock SeparatorTimestamp;

	internal TextBlock SenderName;

	internal ContentPresenter UserTilePresenter;

	internal ContentPresenter BubblePresenter;

	internal Grid MessageStatusWrapper;

	internal Image DeliveryStatusImage;

	internal SeenHeadsControl SeenHeadsControl;

	internal SeenHeadsControl GroupSeenHeadsControl;

	private bool _contentLoaded;

	private FacebookContext Context => _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);

	public bool CanRetry
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(CanRetryProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(CanRetryProperty, (object)value);
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
		if (BubblePresenter.Content is MessageBubbleControl messageBubbleControl)
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
		((UIElement)AdminMessagePresenter).Visibility = (Visibility)1;
		((UIElement)SenderName).Visibility = (Visibility)1;
		((UIElement)SeparatorTimestamp).Visibility = (Visibility)1;
	}

	protected override void OnItemValueAssigned()
	{
		ViewModel.PropertyChanged += OnMessagePropertyChanged;
		UpdatePresentation(recycled: true);
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
			((UIElement)SeparatorTimestamp).Visibility = (Visibility)0;
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
			((UIElement)DeliveryStatusImage).Visibility = (Visibility)1;
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
		((UIElement)SenderName).Visibility = (Visibility)1;
		((UIElement)UserTilePresenter).Visibility = (Visibility)1;
	}

	private void UpdateMessageStateVisibility()
	{
		if (ViewModel.MessageType != MailMessage.Type.Regular || !IsMessageFromLoggedInUser())
		{
			((UIElement)DeliveryStatusImage).Visibility = (Visibility)1;
			return;
		}
		switch (ViewModel.State)
		{
		case MailMessage.MessageState.Sent:
		case MailMessage.MessageState.Queued:
		case MailMessage.MessageState.Failed:
		case MailMessage.MessageState.Delivered:
			((UIElement)DeliveryStatusImage).Visibility = (Visibility)0;
			break;
		default:
			((UIElement)DeliveryStatusImage).Visibility = (Visibility)1;
			break;
		}
		DeliveryStatusImage.Source = (ImageSource)(object)MessageDetailsUtils.MessageStateImage(ViewModel.State);
		UpdateTouchOnMessage(ViewModel);
	}

	private void UpdateTouchOnMessage(MessageViewModel m)
	{
		if (BubblePresenter.Content is MessageBubbleControl messageBubbleControl)
		{
			((UIElement)messageBubbleControl).IsHitTestVisible = m.State != MailMessage.MessageState.Failed;
		}
		if (m.State == MailMessage.MessageState.Failed)
		{
			((UIElement)this).Tap += DeliveryStatusImage_ShowContextMenu;
			((UIElement)this).Hold += DeliveryStatusImage_ShowContextMenu;
		}
		else
		{
			((UIElement)this).Tap -= DeliveryStatusImage_ShowContextMenu;
			((UIElement)this).Hold -= DeliveryStatusImage_ShowContextMenu;
		}
	}

	private void DeliveryStatusImage_ShowContextMenu(object sender, GestureEventArgs e)
	{
		e.Handled = true;
		if (BubblePresenter.Content is MessageBubbleControl messageBubbleControl)
		{
			messageBubbleControl.ShowContextMenu();
		}
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
		((UIElement)UserTilePresenter).Visibility = (Visibility)0;
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
			UpdatePresentation(recycled: false);
			break;
		}
	}

	protected override void Recycled(ContextualMessageViewModel oldViewModel)
	{
		ResetView(recycled: true);
		oldViewModel.PropertyChanged -= OnMessagePropertyChanged;
	}

	private void DisplayAdminMessage()
	{
		((UIElement)UserTilePresenter).Visibility = (Visibility)1;
		((UIElement)AdminMessagePresenter).Visibility = (Visibility)0;
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
			if (Uri.TryCreate(uriString, UriKind.Absolute, out var result))
			{
				ImageCacheExtensions.SetPlaceholderUri((FrameworkElement)(object)adminMessageControl.AdminImage, PlaceholderImage);
				ImageCacheExtensions.SetImageUri((FrameworkElement)(object)adminMessageControl.AdminImage, result);
				((UIElement)adminMessageControl.AdminImageBlock).Visibility = (Visibility)0;
			}
			else
			{
				((UIElement)adminMessageControl.AdminImageBlock).Visibility = (Visibility)1;
			}
		}
		else
		{
			((UIElement)adminMessageControl.AdminImageBlock).Visibility = (Visibility)1;
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
		ContextualMessageViewModel message = ((FrameworkElement)this).DataContext as ContextualMessageViewModel;
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
		if (((FrameworkElement)this).DataContext is ContextualMessageViewModel { IsCancellable: not false })
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
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected Obj, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected Obj, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/MessageControl.xaml", UriKind.Relative));
			RootGrid = (Grid)((FrameworkElement)this).FindName("RootGrid");
			AdminMessagePresenter = (ContentPresenter)((FrameworkElement)this).FindName("AdminMessagePresenter");
			ProfilePhotoColumn = (ColumnDefinition)((FrameworkElement)this).FindName("ProfilePhotoColumn");
			ContentColumn = (ColumnDefinition)((FrameworkElement)this).FindName("ContentColumn");
			SeparatorTimestamp = (TextBlock)((FrameworkElement)this).FindName("SeparatorTimestamp");
			SenderName = (TextBlock)((FrameworkElement)this).FindName("SenderName");
			UserTilePresenter = (ContentPresenter)((FrameworkElement)this).FindName("UserTilePresenter");
			BubblePresenter = (ContentPresenter)((FrameworkElement)this).FindName("BubblePresenter");
			MessageStatusWrapper = (Grid)((FrameworkElement)this).FindName("MessageStatusWrapper");
			DeliveryStatusImage = (Image)((FrameworkElement)this).FindName("DeliveryStatusImage");
			SeenHeadsControl = (SeenHeadsControl)((FrameworkElement)this).FindName("SeenHeadsControl");
			GroupSeenHeadsControl = (SeenHeadsControl)((FrameworkElement)this).FindName("GroupSeenHeadsControl");
		}
	}

	static MessageControl()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected Obj, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected Obj, but got Unknown
	}
}
