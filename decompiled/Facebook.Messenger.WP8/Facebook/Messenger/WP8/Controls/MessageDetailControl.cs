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

namespace Facebook.Messenger.WP8.Controls;

public class MessageDetailControl : MessageListItemControl
{
	internal Grid RootGrid;

	internal ColumnDefinition ProfilePhotoColumn;

	internal ColumnDefinition ContentColumn;

	internal ColumnDefinition RightProfilePhotoColumn;

	internal TextBlock SenderName;

	internal UserTile ProfilePhoto;

	internal MessageBubbleControl Bubble;

	internal StackPanel MessageDetailsStatusWrapper;

	internal TextBlock TimeStampTextBlock;

	internal TextBlock OriginTextBlock;

	internal TextBlock ReadReceiptLabel;

	internal StackPanel ReadReceiptDisplay;

	private bool _contentLoaded;

	public event EventHandler<NavigationEventArgs> NavigateEvent;

	public event EventHandler<MessageActionArgs> MessageActionEvent;

	public MessageDetailControl()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected Obj, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
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
		((UIElement)ProfilePhoto).Visibility = (Visibility)1;
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
		UpdatePresentation(ViewModel, recycled: true);
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
		((UIElement)ProfilePhoto).Visibility = (Visibility)0;
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
		((UIElement)ProfilePhoto).Visibility = (Visibility)0;
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
			((UIElement)ReadReceiptLabel).Visibility = (Visibility)0;
		}
	}

	protected override void Recycled(MessageViewModel oldViewModel)
	{
		ResetView(recycled: true);
		oldViewModel.PropertyChanged -= OnMessagePropertyChanged;
	}

	private void OnMessagePropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		UpdatePresentation(ViewModel, recycled: false);
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
		if (((FrameworkElement)this).DataContext is MappedMessageViewModel message && MessageActionEvent != null)
		{
			MessageActionEvent(this, new MessageActionArgs(message, action));
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
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected Obj, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected Obj, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected Obj, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected Obj, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/MessageDetailControl.xaml", UriKind.Relative));
			RootGrid = (Grid)((FrameworkElement)this).FindName("RootGrid");
			ProfilePhotoColumn = (ColumnDefinition)((FrameworkElement)this).FindName("ProfilePhotoColumn");
			ContentColumn = (ColumnDefinition)((FrameworkElement)this).FindName("ContentColumn");
			RightProfilePhotoColumn = (ColumnDefinition)((FrameworkElement)this).FindName("RightProfilePhotoColumn");
			SenderName = (TextBlock)((FrameworkElement)this).FindName("SenderName");
			ProfilePhoto = (UserTile)((FrameworkElement)this).FindName("ProfilePhoto");
			Bubble = (MessageBubbleControl)((FrameworkElement)this).FindName("Bubble");
			MessageDetailsStatusWrapper = (StackPanel)((FrameworkElement)this).FindName("MessageDetailsStatusWrapper");
			TimeStampTextBlock = (TextBlock)((FrameworkElement)this).FindName("TimeStampTextBlock");
			OriginTextBlock = (TextBlock)((FrameworkElement)this).FindName("OriginTextBlock");
			ReadReceiptLabel = (TextBlock)((FrameworkElement)this).FindName("ReadReceiptLabel");
			ReadReceiptDisplay = (StackPanel)((FrameworkElement)this).FindName("ReadReceiptDisplay");
		}
	}
}
