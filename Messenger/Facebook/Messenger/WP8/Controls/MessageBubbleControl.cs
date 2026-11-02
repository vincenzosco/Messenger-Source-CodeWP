using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Common;
using Facebook.Messenger.WP8.Controls.Messages;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Text;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class MessageBubbleControl : UserControl, IResetableView
	{
		private const int MaxBodyCharsForDisplay = 400;

		private const int MaxBodyCharsForDisplayWithImages = 300;

		private const int MaxLinesForDisplay = 6;

		private const int MaxImagePreviewsForDisplay = 6;

		private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Colors.Transparent);

		public static readonly DependencyProperty ShouldShowContextMenuProperty = DependencyProperty.Register("ShouldShowContextMenu", typeof(bool), typeof(MessageBubbleControl), new PropertyMetadata((object)true));

		private SolidColorBrush _fromMe;

		private SolidColorBrush _fromMeText;

		private SolidColorBrush _fromFriend;

		private SolidColorBrush _fromFriendText;

		private RelayCommand<string> _launchUrlCommand;

		private MenuItem _copyMenuItem;

		private MenuItem _deleteMenuItem;

		private MenuItem _failedMenuItem;

		private MenuItem _sentMenuItem;

		private MenuItem _forwardMenuItem;

		private MessageViewModel _message;

		public LayoutGenerator LayoutGenerator;

		public bool CanRetry;



















		public bool ShouldShowContextMenu
		{
			get
			{
				return (bool)base.GetValue(ShouldShowContextMenuProperty);
			}
			set
			{
				base.SetValue(ShouldShowContextMenuProperty, (object)value);
			}
		}

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

		public SolidColorBrush FromMeTextBrush
		{
			get
			{
				SolidColorBrush val = _fromMeText;
				if (val == null)
				{
					object obj = Application.Current.Resources[(object)"MessengerWhiteBrush"];
					val = (_fromMeText = (SolidColorBrush)((obj is SolidColorBrush) ? obj : null));
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

		public SolidColorBrush FromFriendTextBrush
		{
			get
			{
				SolidColorBrush val = _fromFriendText;
				if (val == null)
				{
					object obj = Application.Current.Resources[(object)"MessengerBlackBrush"];
					val = (_fromFriendText = (SolidColorBrush)((obj is SolidColorBrush) ? obj : null));
				}
				return val;
			}
		}

		public event EventHandler<MessageActionArgs> MessageActionEvent;

		public event EventHandler<NavigationEventArgs> NavigateEvent;

		public event EventHandler<bool> VideoTouched;

		public event EventHandler ViewMoreEvent;

		public event EventHandler CancelEvent;

		public MessageBubbleControl()
		{
			InitializeComponent();
			BubbleContainer.TopNubVisibility = Visibility.Collapsed;
			BubbleContainer.BottomNubVisibility = Visibility.Collapsed;
			_launchUrlCommand = new RelayCommand<string>(LaunchUrl);
			ContextMenu contextMenu = new ContextMenu();
			object obj = Application.Current.Resources[(object)"ContextMenuStyle"];
			((FrameworkElement)contextMenu).Style = (Style)((obj is Style) ? obj : null);
			ContextMenuService.SetContextMenu((DependencyObject)(object)this, contextMenu);
		}

		public void ResetView(bool recycled)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			if (recycled)
			{
				_message = null;
			}
			base.Visibility = Visibility.Collapsed;
			((FrameworkElement)BubbleContainer).Margin = new Thickness(0.0);
			((UIElement)BubbleContainer).Visibility = Visibility.Collapsed;
			BubbleContainer.TopNubVisibility = Visibility.Collapsed;
			BubbleContainer.BottomNubVisibility = Visibility.Collapsed;
			LayoutGenerator.Reset(recycled, VideoUploadOverlayPresenter, (VideoUploadOverlay c) =>
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Expected Obj, but got Unknown
				c.CancelEvent -= CancelButton_OnClick;
			});
			LayoutGenerator.Reset<VoiceClipAttachment>(recycled, AudioPresenter);
			LayoutGenerator.Reset(recycled, ImageAttachmentPresenter, (ImageAttachmentsControl c) =>
			{
				c.NavigateEvent -= AttachmentPanelOnNavigate;
			});
			LayoutGenerator.Reset<GenericFileAttachmentsControl>(recycled, GenericFileAttachmentPresenter);
			LayoutGenerator.Reset<StickerControl>(recycled, StickerPresenter);
			LayoutGenerator.Reset<MessageShareControl>(recycled, SharePresenter);
			LayoutGenerator.Reset(recycled, VideoPresenter, (VideoAttachmentControl c) =>
			{
				c.OnVideoTouch -= OnVideoTouched;
			});
			Thickness margin = ((FrameworkElement)Body).Margin;
			margin.Bottom = 0.0;
			((FrameworkElement)Body).Margin = margin;
			((PresentationFrameworkCollection<Inline>)(object)MessageTextParagraph.Inlines).Clear();
		}

		private void UpdateOpacity()
		{
			bool flag = _message.State == MailMessage.MessageState.Failed;
			((UIElement)BubbleContainer).Opacity = (flag ? 0.5 : 1.0);
		}

		public void UpdatePresentation(MessageViewModel m, bool fromLoggedInUser)
		{
			_message = m;
			base.Visibility = Visibility.Visible;
			((UIElement)BubbleContainer).Visibility = Visibility.Visible;
			UpdateOpacity();
			if (m.HasNonStickerImageAttachments())
			{
				LayoutGenerator.Load(ImageAttachmentPresenter, (ImageAttachmentsControl c) =>
				{
					c.UpdateView(m);
				}, (ImageAttachmentsControl c) =>
				{
					c.NavigateEvent += AttachmentPanelOnNavigate;
				});
			}
			if (m.HasGenericFileAttachments())
			{
				LayoutGenerator.Load(GenericFileAttachmentPresenter, (GenericFileAttachmentsControl c) =>
				{
					c.UpdateView(m);
				});
			}
			if (m.HasAudioAttachments())
			{
				LayoutGenerator.Load(AudioPresenter, (VoiceClipAttachment c) =>
				{
					c.UpdateView(m, fromLoggedInUser);
				});
			}
			if (m.HasVideoAttachments())
			{
				if (_message.State == MailMessage.MessageState.Queued)
				{
					LayoutGenerator.Load(VideoUploadOverlayPresenter, (VideoUploadOverlay c) =>
					{
						c.UpdateView(_message);
					}, (VideoUploadOverlay c) =>
					{
						//IL_0008: Unknown result type (might be due to invalid IL or missing references)
						//IL_0012: Expected Obj, but got Unknown
						c.CancelEvent += CancelButton_OnClick;
					});
				}
				LayoutGenerator.Load(VideoPresenter, (VideoAttachmentControl c) =>
				{
					c.UpdateView(m);
				}, (VideoAttachmentControl c) =>
				{
					c.OnVideoTouch += OnVideoTouched;
				});
			}
			if (fromLoggedInUser)
			{
				DisplayMessageFromLoggedInUser();
			}
			else
			{
				DisplayMessageFromOtherUser();
			}
			if (m.HasSticker())
			{
				DisplaySticker();
			}
			else if (m.HasShares() || m.HasSharesHiddenByPrivacy())
			{
				SolidColorBrush brush = (fromLoggedInUser ? FromMeTextBrush : FromFriendTextBrush);
				LayoutGenerator.Load(SharePresenter, (MessageShareControl c) =>
				{
					c.UpdateView(m, brush);
				});
			}
			DisplayBody(ClipBody(m), fromLoggedInUser, m.HasAttachments());
			ShowViewMore(fromLoggedInUser);
		}

		private void OnVideoTouched(object sender, bool args)
		{
			if (VideoTouched != null)
			{
				VideoTouched(sender, args);
			}
		}

		private bool IsEarlierInGroup()
		{
			if (_message is ContextualMessageViewModel)
			{
				return ((ContextualMessageViewModel)_message).IsEarlierInGroup();
			}
			return false;
		}

		private bool IsLaterInGroup()
		{
			if (_message is ContextualMessageViewModel)
			{
				return ((ContextualMessageViewModel)_message).IsLaterInGroup();
			}
			return false;
		}

		private bool AnimateSticker()
		{
			if (_message is ContextualMessageViewModel)
			{
				return ((ContextualMessageViewModel)_message).ShouldAutoAnimateSticker;
			}
			return false;
		}

		private void DisplayMessageFromLoggedInUser()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			((FrameworkElement)ContentGrid).HorizontalAlignment = (HorizontalAlignment)2;
			Grid.SetColumn((FrameworkElement)(object)ForwardButton, 0);
			((UIElement)ForwardButton).Visibility = VisibilityUtil.FromBool(_message.IsForwardable);
			((FrameworkElement)ForwardButton).Margin = new Thickness(0.0, 0.0, 10.0, (double)((!IsEarlierInGroup()) ? 10 : 0));
			BubbleContainer bubbleContainer = BubbleContainer;
			double num;
			if (!_message.IsForwardable)
			{
				double width = ((FrameworkElement)ForwardButton).Width;
				Thickness margin = ((FrameworkElement)ForwardButton).Margin;
				num = width + margin.Right;
			}
			else
			{
				num = 0.0;
			}
			((FrameworkElement)bubbleContainer).Margin = new Thickness(num, 0.0, 0.0, 0.0);
			BubbleContainer.BottomNubVisibility = Visibility.Visible;
			BubbleContainer bubbleContainer2 = BubbleContainer;
			SolidColorBrush background = (BubbleContainer.BubbleFill = FromMeBrush);
			((Control)bubbleContainer2).Background = (Brush)(object)background;
			BubbleContainer bubbleContainer3 = BubbleContainer;
			ContentPresenter videoUploadOverlayPresenter = VideoUploadOverlayPresenter;
			HorizontalAlignment bubbleAlignment = (HorizontalAlignment)2;
			((FrameworkElement)videoUploadOverlayPresenter).HorizontalAlignment = (HorizontalAlignment)2;
			bubbleContainer3.BubbleAlignment = bubbleAlignment;
			((Control)Body).Foreground = (Brush)(object)FromMeTextBrush;
			base.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
		}

		private void DisplayMessageFromOtherUser()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			((FrameworkElement)ContentGrid).HorizontalAlignment = (HorizontalAlignment)0;
			Grid.SetColumn((FrameworkElement)(object)ForwardButton, 2);
			((UIElement)ForwardButton).Visibility = VisibilityUtil.FromBool(_message.IsForwardable);
			((FrameworkElement)ForwardButton).Margin = new Thickness(10.0, (double)((!IsLaterInGroup()) ? 10 : 0), 0.0, 0.0);
			BubbleContainer bubbleContainer = BubbleContainer;
			double num;
			if (!_message.IsForwardable)
			{
				double width = ((FrameworkElement)ForwardButton).Width;
				Thickness margin = ((FrameworkElement)ForwardButton).Margin;
				num = width + margin.Left;
			}
			else
			{
				num = 0.0;
			}
			((FrameworkElement)bubbleContainer).Margin = new Thickness(0.0, 0.0, num, 0.0);
			BubbleContainer.TopNubVisibility = Visibility.Visible;
			BubbleContainer bubbleContainer2 = BubbleContainer;
			SolidColorBrush background = (BubbleContainer.BubbleFill = FromFriendBrush);
			((Control)bubbleContainer2).Background = (Brush)(object)background;
			BubbleContainer bubbleContainer3 = BubbleContainer;
			ContentPresenter videoUploadOverlayPresenter = VideoUploadOverlayPresenter;
			HorizontalAlignment bubbleAlignment = (HorizontalAlignment)0;
			((FrameworkElement)videoUploadOverlayPresenter).HorizontalAlignment = (HorizontalAlignment)0;
			bubbleContainer3.BubbleAlignment = bubbleAlignment;
			((Control)Body).Foreground = (Brush)(object)FromFriendTextBrush;
			base.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		}

		private void DisplaySticker()
		{
			BubbleContainer.TopNubVisibility = Visibility.Collapsed;
			BubbleContainer.BottomNubVisibility = Visibility.Collapsed;
			BubbleContainer bubbleContainer = BubbleContainer;
			SolidColorBrush background = (BubbleContainer.BubbleFill = TransparentBrush);
			((Control)bubbleContainer).Background = (Brush)(object)background;
			LayoutGenerator.Load(StickerPresenter, (StickerControl c) =>
			{
				c.UpdateShare(_message.Shares[0], AnimateSticker()).DoNotWait();
			});
		}

		private void AttachmentPanelOnNavigate(object sender, NavigationEventArgs args)
		{
			if (NavigateEvent != null)
			{
				NavigateEvent(this, args);
			}
		}

		public void ShowViewMore(bool fromLoggedInUser)
		{
			((Control)ViewMoreButton).Foreground = (Brush)(object)(fromLoggedInUser ? FromMeTextBrush : FromFriendTextBrush);
			if (_message.ImageAttachmentsCount > 6 || _message.Body.Length > 400 || (_message.Body.Length > 300 && _message.HasImageAttachments()) || GetNthIndexOfNewLine(_message.Body, 6) >= 0)
			{
				((UIElement)ViewMoreButton).Visibility = Visibility.Visible;
			}
			else
			{
				((UIElement)ViewMoreButton).Visibility = Visibility.Collapsed;
			}
		}

		private int GetNthIndexOfNewLine(string str, int n)
		{
			if (string.IsNullOrEmpty(str))
			{
				return -1;
			}
			string pattern = $"(.*?[\r|\n]){{{n}}}";
			Match match = Regex.Match(str, pattern);
			if (match.Success)
			{
				return match.Groups[0].Length;
			}
			return -1;
		}

		private string ClipBody(MessageViewModel message)
		{
			if (message == null || string.IsNullOrWhiteSpace(message.Body))
			{
				return string.Empty;
			}
			if (message.HasImageAttachments() && message.Body.Length > 300)
			{
				return message.Body.Substring(0, 300) + " ...";
			}
			if (message.Body.Length > 400)
			{
				return message.Body.Substring(0, 400) + " ...";
			}
			int nthIndexOfNewLine = GetNthIndexOfNewLine(message.Body, 6);
			if (nthIndexOfNewLine >= 0 && message.Body.Length > nthIndexOfNewLine)
			{
				return message.Body.Substring(0, nthIndexOfNewLine - 1);
			}
			return message.Body;
		}

		private void DisplayBody(string text, bool fromLoggedInUser, bool hasAttachments)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(text))
			{
				((UIElement)Body).Visibility = Visibility.Collapsed;
				return;
			}
			if (hasAttachments)
			{
				Thickness margin = ((FrameworkElement)Body).Margin;
				margin.Bottom += 5.0;
				((FrameworkElement)Body).Margin = margin;
			}
			List<ParsedText> list = ParseTextHelper.ParseText(text);
			foreach (ParsedText item in list)
			{
				if (item.type == TextType.Url)
				{
					((PresentationFrameworkCollection<Inline>)(object)MessageTextParagraph.Inlines).Add((Inline)(object)GetHyperlink(fromLoggedInUser, item));
					continue;
				}
				if (item.type == TextType.Text)
				{
					MessageTextParagraph.Inlines.Add(item.text);
					continue;
				}
				InlineUIContainer emoticonImage = ParseEmoticonHelper.GetEmoticonImage(item.text);
				if (emoticonImage != null)
				{
					((PresentationFrameworkCollection<Inline>)(object)MessageTextParagraph.Inlines).Add((Inline)(object)emoticonImage);
				}
				else
				{
					MessageTextParagraph.Inlines.Add(item.text);
				}
			}
			((UIElement)Body).Visibility = Visibility.Visible;
		}

		private Hyperlink GetHyperlink(bool fromLoggedInUser, ParsedText p)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected Obj, but got Unknown
			Hyperlink val = new Hyperlink();
			val.Command = _launchUrlCommand;
			val.CommandParameter = p.text;
			Hyperlink val2 = val;
			InlineCollection inlines = ((Span)val2).Inlines;
			Run val3 = new Run();
			val3.Text = p.text;
			((PresentationFrameworkCollection<Inline>)(object)inlines).Add((Inline)(object)val3);
			((TextElement)val2).Foreground = (Brush)(object)(fromLoggedInUser ? FromMeTextBrush : FromFriendTextBrush);
			return val2;
		}

		private void LaunchUrl(string url)
		{
			if (!string.IsNullOrEmpty(url) && NavigateEvent != null)
			{
				if (!url.ToLower().StartsWith("http") || !url.Contains("://"))
				{
					url = url.Insert(0, "http://");
				}
				Uri result;
				if (Uri.TryCreate(url, UriKind.Absolute, out result))
				{
					NavigateEvent(this, new NavigationEventArgs(result));
				}
				else
				{
					Logger.Error($"Unable to launch uri {url}", "LaunchUrl", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\MessageBubbleControl.xaml.cs", 434);
				}
			}
		}

		private void ViewMoreButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (ViewMoreEvent != null)
			{
				ViewMoreEvent(sender, (EventArgs)(object)e);
			}
		}

		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (CancelEvent != null)
			{
				CancelEvent(sender, (EventArgs)(object)e);
			}
		}

		private void RootGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			ContentPresenter videoUploadOverlayPresenter = VideoUploadOverlayPresenter;
			Size newSize = e.NewSize;
			((FrameworkElement)videoUploadOverlayPresenter).Width = newSize.Width;
		}

		private void OnBubbleHold(object sender, GestureEventArgs e)
		{
			UpdateContextMenuState();
		}

		public void UpdateContextMenuState()
		{
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected Obj, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Expected Obj, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected Obj, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected Obj, but got Unknown
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Expected Obj, but got Unknown
			if (_message != null)
			{
				MessageViewModel message = _message;
				bool flag = message.State != MailMessage.MessageState.Queued;
				bool flag2 = !string.IsNullOrWhiteSpace(message.Body);
				bool flag3 = message.State == MailMessage.MessageState.Failed && CanRetry;
				bool flag4 = message.IsMessageSent();
				bool enabled = flag4;
				bool flag5 = message.MessageType == MailMessage.Type.Regular && ShouldShowContextMenu && (flag || flag2 || flag3 || flag4);
				ContextMenu contextMenu = base.GetValue(ContextMenuService.ContextMenuProperty) as ContextMenu;
				((UIElement)contextMenu).Visibility = VisibilityUtil.FromBool(flag5);
				if (flag5)
				{
					AddMenuItem(flag2, 0, contextMenu, ref _copyMenuItem, StringLibrary.Messages_ContextMenu_Copy, CopyMessage_OnClick);
					AddMenuItem(flag, 1, contextMenu, ref _deleteMenuItem, StringLibrary.Messages_ContextMenu_Delete, DeleteMessage_OnClick);
					AddMenuItem(flag3, 2, contextMenu, ref _failedMenuItem, StringLibrary.Messages_ContextMenu_Retry, RetryMessage_OnClick);
					AddMenuItem(flag4, 3, contextMenu, ref _sentMenuItem, StringLibrary.Messages_Context_Menu_View_Details, ViewDetails_OnClick);
					AddMenuItem(enabled, 4, contextMenu, ref _forwardMenuItem, StringLibrary.Messages_ContextMenu_Forward, Forward_OnClick);
				}
			}
		}

		private void AddMenuItem(bool enabled, int desiredIndex, ContextMenu menu, ref MenuItem menuItem, string header, RoutedEventHandler handler)
		{
			if (!enabled)
			{
				if (menuItem != null)
				{
					((UIElement)menuItem).Visibility = Visibility.Collapsed;
				}
				return;
			}
			if (menuItem == null)
			{
				menuItem = new MenuItem();
				((FrameworkElement)menuItem).Tag = desiredIndex;
				menuItem.Header = header;
				menuItem.Click += handler;
				int? num = null;
				foreach (object item in (PresentationFrameworkCollection<object>)(object)((ItemsControl)menu).Items)
				{
					MenuItem menuItem2 = item as MenuItem;
					if ((int)((FrameworkElement)menuItem2).Tag > desiredIndex)
					{
						num = ((PresentationFrameworkCollection<object>)(object)((ItemsControl)menu).Items).IndexOf((object)menuItem2);
						break;
					}
				}
				if (num.HasValue)
				{
					((PresentationFrameworkCollection<object>)(object)((ItemsControl)menu).Items).Insert(num.Value, (object)menuItem);
				}
				else
				{
					((PresentationFrameworkCollection<object>)(object)((ItemsControl)menu).Items).Add((object)menuItem);
				}
			}
			((UIElement)menuItem).Visibility = Visibility.Visible;
		}

		private void ViewDetails_OnClick(object sender, RoutedEventArgs e)
		{
			TriggerMessageAction(MessageAction.ViewDetails);
		}

		private void DeleteMessage_OnClick(object sender, RoutedEventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			if ((int)MessageBox.Show(StringLibrary.Message_Delete_Text, StringLibrary.Message_Delete_Caption, (MessageBoxButton)1) == 1)
			{
				TriggerMessageAction(MessageAction.Delete);
			}
		}

		private void RetryMessage_OnClick(object sender, RoutedEventArgs e)
		{
			TriggerMessageAction(MessageAction.Retry);
		}

		private void TriggerMessageAction(MessageAction action)
		{
			if (_message != null && MessageActionEvent != null)
			{
				MessageActionEvent(this, new MessageActionArgs(_message, action));
			}
		}

		private void CopyMessage_OnClick(object sender, RoutedEventArgs e)
		{
			MessageViewModel messageViewModel = base.DataContext as MessageViewModel;
			if (messageViewModel != null)
			{
				Clipboard.SetText(messageViewModel.Body);
			}
		}

		private void Forward_OnClick(object sender, RoutedEventArgs e)
		{
			TriggerMessageAction(MessageAction.Forward);
		}

		public void ShowContextMenu()
		{
			ContextMenu contextMenu = ContextMenuService.GetContextMenu((DependencyObject)(object)this);
			if (contextMenu != null && !contextMenu.IsOpen)
			{
				UpdateContextMenuState();
				contextMenu.IsOpen = true;
			}
		}

		private void ForwardButton_OnTap(object sender, GestureEventArgs e)
		{
			TriggerMessageAction(MessageAction.Forward);
		}


		static MessageBubbleControl()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected Obj, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected Obj, but got Unknown
		}
	}
}
