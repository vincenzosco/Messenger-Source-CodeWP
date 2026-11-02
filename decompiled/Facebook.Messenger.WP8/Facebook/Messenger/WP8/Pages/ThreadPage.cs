using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Common;
using Facebook.Messenger.WP8.Controls.Messages;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Services.Api.Methods.SendMessage;
using Microsoft.Phone.Tasks;

namespace Facebook.Messenger.WP8.Pages;

public class ThreadPage : MessagesViewModelPage
{
	private bool ListHeaderFixApplied;

	private FrameworkElement ElementToToggle;

	private MenuBarEntry<bool> _detailsMenuItem;

	internal MessagesViewModelPage ThreadViewPage;

	internal Grid LayoutRoot;

	internal Grid SecondLayoutRoot;

	internal MessageTopBar TopBar;

	internal FacebookListView MessageList;

	internal Grid ListHeaderGrid;

	internal Grid NestedListHeaderGrid;

	internal TypingIndicator TypingIndicator;

	internal Grid ComposerBubbleComponents;

	internal Button FocusButton;

	internal Button LocationButton;

	internal Image LocationButtonDisabledImage;

	internal Image LocationButtonEnabledImage;

	internal MessageComposerBubble MessageComposerBubble;

	internal Grid DrawerPaddingElement;

	internal DrawerContainer Container;

	internal FBMessengerProgressBar BusyIndicator;

	private bool _contentLoaded;

	protected override Grid PhotoPopupParent => LayoutRoot;

	protected override ImageUploadAttachmentControl ImageUploadControl => MessageComposerBubble.UploadBar;

	public void ApplyListHeaderFix(FrameworkElement elementToToggle)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected Obj, but got Unknown
		Version version = Environment.OSVersion.Version;
		if (!ListHeaderFixApplied && ListHeaderGrid != null && version.Major == 8 && version.Minor == 0)
		{
			ElementToToggle = elementToToggle;
			((FrameworkElement)ListHeaderGrid).SizeChanged += OnListHeaderSizeChanged;
		}
	}

	private void OnListHeaderSizeChanged(object sender, SizeChangedEventArgs args)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected Obj, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected Obj, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		((FrameworkElement)ListHeaderGrid).SizeChanged -= OnListHeaderSizeChanged;
		((FrameworkElement)ListHeaderGrid).SizeChanged += OnListHeaderSizeChangedSecondTry;
		((UIElement)ElementToToggle).Visibility = (Visibility)((int)((UIElement)ElementToToggle).Visibility != 1);
	}

	private void OnListHeaderSizeChangedSecondTry(object sender, SizeChangedEventArgs args)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected Obj, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		((FrameworkElement)ListHeaderGrid).SizeChanged -= OnListHeaderSizeChangedSecondTry;
		((UIElement)ElementToToggle).Visibility = (Visibility)((int)((UIElement)ElementToToggle).Visibility != 1);
		ListHeaderFixApplied = true;
	}

	private void OnListHeaderGridLoaded(object sender, RoutedEventArgs e)
	{
		UpdateComposerVisibility();
	}

	public ThreadPage()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected Obj, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected Obj, but got Unknown
		InitializeComponent();
		InitializeAudioPlayback((Panel)(object)LayoutRoot);
		RegisterInputElements(new List<UIElement> { (UIElement)(object)MessageComposerBubble });
		MessageList.ViewportChanged += MessageListOnViewportChanged;
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
		InitializeAppBar();
	}

	public bool IsComposerVisible()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		GeneralTransform val = null;
		GeneralTransform val2 = null;
		try
		{
			val = ((UIElement)MessageComposerBubble).TransformToVisual((UIElement)(object)this);
			val2 = ((UIElement)MessageList).TransformToVisual((UIElement)(object)this);
		}
		catch (ArgumentException)
		{
		}
		if (val == null || val2 == null)
		{
			return false;
		}
		Point val3 = val.Transform(new Point(0.0, 0.0));
		Point val4 = val2.Transform(new Point(0.0, 0.0));
		if (val3.Y <= val4.Y)
		{
			return !IsAnyDrawerVisible();
		}
		return false;
	}

	private void MessageListOnViewportChanged(object sender, ViewportChangedEventArgs e)
	{
		bool composerVisible = IsComposerVisible();
		WithViewModel((MessagesViewModel vm) =>
		{
			if (!composerVisible && vm.CanSendToThread)
			{
				_sendOrLikeButton.State = SendButtonState.FocusToTextBox;
			}
			else if (_sendOrLikeButton.State == SendButtonState.FocusToTextBox)
			{
				UpdateAppBarButtonStates(resetToSendOrLike: true);
			}
		});
	}

	protected override void InitializeCustomApplicationBarItems(AppBarHelper appBarHelper)
	{
		_detailsMenuItem = appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(state: true, StringLibrary.Thread_AppBar_Details, OnDetailsClicked));
		appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(state: true, StringLibrary.FlyTrap_MenuItem_ReportProblem, ReportProblemBarButtonOnClick));
	}

	private void OnDetailsClicked(object sender, EventArgs e)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.NavigateToDetailsCommand.Execute(null);
		});
	}

	protected override void AttachPhoto(PhotoResult e)
	{
		base.AttachPhoto(e);
		MessageList.ScrollToTop();
	}

	protected override UIElement GetLayoutRoot()
	{
		return (UIElement)(object)LayoutRoot;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected Obj, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected Obj, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected Obj, but got Unknown
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.PropertyChanged += OnPropertyChanged;
			vm.SendMessageStarted += OnSendMessageStarted;
			vm.Messages.MessagesAddedToCollection += OnMessagesAddedToCollection;
			vm.MarkThreadRead();
		});
		UpdateTopBar();
		UpdateAppBarMenuItems();
		UpdateAppBarButtonStates();
		UpdateComposerVisibility();
		((UIElement)MessageComposerBubble).GotFocus += OnMessageComposerGotFocus;
		((UIElement)MessageComposerBubble).LostFocus += OnMessageComposerLostFocus;
		MessageComposerBubble.ComposerTextBox.TextKeyUp += OnComposerKeyUp;
		MessageComposerBubble.EnterPressed += OnSendMessageEnter;
		MessageComposerBubble.HasTextChanged += MessageComposerBubbleOnHasTextChanged;
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected Obj, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected Obj, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected Obj, but got Unknown
		((UIElement)MessageComposerBubble).GotFocus -= OnMessageComposerGotFocus;
		((UIElement)MessageComposerBubble).LostFocus -= OnMessageComposerLostFocus;
		MessageComposerBubble.ComposerTextBox.TextKeyUp -= OnComposerKeyUp;
		MessageComposerBubble.EnterPressed -= OnSendMessageEnter;
		MessageComposerBubble.HasTextChanged -= MessageComposerBubbleOnHasTextChanged;
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.PropertyChanged -= OnPropertyChanged;
			vm.SendMessageStarted -= OnSendMessageStarted;
			vm.Messages.MessagesAddedToCollection -= OnMessagesAddedToCollection;
		});
	}

	private void OnMessagesAddedToCollection(object sender, EventArgs e)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.MarkThreadRead();
		});
	}

	private void LocationButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.SetLocationSendingCommand.Execute(null);
		});
	}

	private void OnComposerKeyUp(object sender, KeyEventArgs e)
	{
		((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
		{
			MessageList.ScrollToTop();
		}));
	}

	protected override void OnSendMessageEnter(object sender, EventArgs e)
	{
		SendMessage(MessageBaseViewModel.SendTrigger.Enter);
	}

	protected override void OnSendMessageClick(object sender, EventArgs e)
	{
		SendMessage(MessageBaseViewModel.SendTrigger.Click);
	}

	private void SendMessage(MessageBaseViewModel.SendTrigger trigger)
	{
		((Control)FocusButton).Focus();
		WithViewModel((MessagesViewModel vm) =>
		{
			if (vm.SendComposedMessage(MessageComposerBubble.Text, trigger))
			{
				MessageComposerBubble.ClearText();
			}
		});
	}

	private void MessageComposerBubbleOnHasTextChanged(object sender, EventArgs eventArgs)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.MessageComposerHasText = MessageComposerBubble.HasText;
		});
	}

	private void OnSendMessageStarted(object sender, EventArgs eventArgs)
	{
		((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
		{
			MessageList.ScrollToTop();
		}));
	}

	protected override void OnLikeClick(object sender, EventArgs e)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.SendLikeCommand.Execute(null);
		});
	}

	protected override void OnTransferFocusToComposerClick(object sender, EventArgs e)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.PlayTextDrawerSelectedSound();
		});
		HideAllDrawers();
		UpdateAppBarButtonStates(resetToSendOrLike: true);
		MessageList.ScrollToTop(animate: true);
		((Control)MessageComposerBubble).Focus();
	}

	private void UpdateAppBarButtonStates(bool resetToSendOrLike = false)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			_sendOrLikeButton.StateValues[SendButtonState.Like].IsEnabled = vm.CanLike;
			_sendOrLikeButton.StateValues[SendButtonState.Send].IsEnabled = vm.MessageComposerHasContent;
			if (_sendOrLikeButton.State != SendButtonState.FocusToTextBox || resetToSendOrLike)
			{
				_sendOrLikeButton.State = (SendButtonState)(vm.MessageComposerHasContent ? SendButtonState.Send : SendButtonState.Like);
			}
			_sendOrLikeButton.ResetCurrentState();
			bool canSendToThread = vm.CanSendToThread;
			_cameraButton.IsEnabled = canSendToThread && vm.CanAttachMoreFiles;
			_stickerButton.IsEnabled = canSendToThread;
			_galleryButton.IsEnabled = canSendToThread;
			_recordVoiceMenuItem.StateValues[DrawerSelectedState.Default].IsEnabled = canSendToThread;
			_imageSearchMenuItem.StateValues[DrawerSelectedState.Default].IsEnabled = canSendToThread;
			_locationMenuItem.StateValues[DrawerSelectedState.Default].IsEnabled = canSendToThread;
			_recordVoiceMenuItem.ResetCurrentState();
			_imageSearchMenuItem.ResetCurrentState();
			_locationMenuItem.ResetCurrentState();
		});
	}

	private void OnMessageComposerLostFocus(object sender, RoutedEventArgs args)
	{
		object focusedElement = FocusManager.GetFocusedElement();
		FrameworkElement val = (FrameworkElement)((focusedElement is FrameworkElement) ? focusedElement : null);
		if ((object)val != MessageComposerBubble.ComposerTextBox.TextBox)
		{
			if ((object)val == LocationButton || (object)val == FocusButton)
			{
				((Control)MessageComposerBubble).Focus();
			}
			if (!IsAnyDrawerVisible())
			{
				SystemTrayManager.RemoveOperationFromStack(((object)this).GetType());
			}
		}
	}

	private void OnMessageComposerGotFocus(object sender, RoutedEventArgs args)
	{
		HideAllDrawers();
		SystemTrayManager.AddOperationToStack(((object)this).GetType());
	}

	protected override void SetRootFrameTransform(int translateY)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected Obj, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		base.SetRootFrameTransform(translateY);
		FBMessengerProgressBar busyIndicator = BusyIndicator;
		CompositeTransform val = new CompositeTransform();
		val.TranslateY = -1 * translateY;
		((UIElement)busyIndicator).RenderTransform = (Transform)(object)val;
		double num = (double)(-1 * translateY) - ((FrameworkElement)TopBar).ActualHeight;
		Thickness margin = ((FrameworkElement)TopBar).Margin;
		double num2 = num - margin.Top;
		Thickness margin2 = ((FrameworkElement)TopBar).Margin;
		double num3 = num2 - margin2.Bottom;
		if (translateY == 0)
		{
			num3 = 0.0;
		}
		((FrameworkElement)MessageList).Margin = new Thickness(0.0, num3, 0.0, 0.0);
	}

	private void ReportProblemBarButtonOnClick(object sender, EventArgs eventArgs)
	{
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.ReportProblem));
	}

	private void UpdateAppBarMenuItems()
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			_detailsMenuItem.IsVisible = vm.CanSendToThread;
		});
	}

	private void UpdateTopBar()
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			if (vm.Messages.MessagesFetched)
			{
				TopBar.IsCanonical = vm.Messages.Thread.IsCanonical();
			}
			TopBar.ThreadName = vm.Messages.Thread.DisplayName;
		});
	}

	public void UpdateComposerVisibility()
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (NestedListHeaderGrid != null)
			{
				ApplyListHeaderFix((FrameworkElement)(object)NestedListHeaderGrid);
				((UIElement)NestedListHeaderGrid).Visibility = VisibilityUtil.FromBool(vm.CanSendToThread);
			}
		});
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			switch (e.PropertyName)
			{
			case "MessageComposerHasContent":
			case "CanLike":
				UpdateAppBarButtonStates(IsComposerVisible());
				break;
			case "CanSendToThread":
				UpdateComposerVisibility();
				UpdateAppBarButtonStates();
				UpdateAppBarMenuItems();
				if (!vm.CanSendToThread)
				{
					((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
					{
						((Control)this).Focus();
					}));
				}
				break;
			case "ThreadUpdated":
				UpdateAppBarMenuItems();
				UpdateTopBar();
				break;
			case "DisplayTypingIndicator":
				ApplyListHeaderFix((FrameworkElement)(object)TypingIndicator);
				if (vm.DisplayTypingIndicator)
				{
					TypingIndicator.Show();
				}
				else
				{
					TypingIndicator.Hide();
				}
				break;
			}
		});
	}

	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		InitializeAudioPlayback((Panel)(object)LayoutRoot);
		WithViewModel((MessagesViewModel vm) =>
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			if ((int)e.NavigationMode != 4)
			{
				vm.FetchUpdatedMessagesCommand.Execute(null);
			}
		});
	}

	protected override void OnNavigatedFrom(NavigationEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		base.OnNavigatedFrom(e);
		if ((int)e.NavigationMode != 1)
		{
			return;
		}
		WithViewModel((MessagesViewModel vm) =>
		{
			if (vm.HasAttachments)
			{
				DeleteFiles(vm.AttachedFiles);
			}
		});
	}

	private static void DeleteFiles(IEnumerable<Attachment> attachments)
	{
		Task.Run(async () =>
		{
			FileAccess fileAccess = new FileAccess();
			foreach (Attachment file in attachments)
			{
				await fileAccess.DeleteFileAtPath(file.Path);
			}
		});
	}

	protected override DrawerContainer GetDrawerContainer()
	{
		return Container;
	}

	protected override FrameworkElement GetDrawerPaddingElement()
	{
		return (FrameworkElement)(object)DrawerPaddingElement;
	}

	protected override void DrawerStateChanged()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		base.DrawerStateChanged();
		DrawerType visibleDrawerType = GetVisibleDrawerType();
		Visibility visibility = VisibilityUtil.FromBool(visibleDrawerType == DrawerType.None);
		ApplyListHeaderFix((FrameworkElement)(object)ComposerBubbleComponents);
		((UIElement)TopBar).Visibility = visibility;
		((UIElement)ComposerBubbleComponents).Visibility = visibility;
		DrawerSelectedState state = DrawerSelectedState.Default;
		DrawerSelectedState state2 = DrawerSelectedState.Default;
		DrawerSelectedState state3 = DrawerSelectedState.Default;
		DrawerSelectedState state4 = DrawerSelectedState.Default;
		DrawerSelectedState state5 = DrawerSelectedState.Default;
		DrawerSelectedState state6 = DrawerSelectedState.Default;
		switch (visibleDrawerType)
		{
		case DrawerType.Camera:
			state = DrawerSelectedState.Selected;
			break;
		case DrawerType.Gallery:
			state2 = DrawerSelectedState.Selected;
			break;
		case DrawerType.Sticker:
			state3 = DrawerSelectedState.Selected;
			break;
		case DrawerType.Voice:
			state4 = DrawerSelectedState.Selected;
			break;
		case DrawerType.ImageSearch:
			state5 = DrawerSelectedState.Selected;
			break;
		case DrawerType.Location:
			state6 = DrawerSelectedState.Selected;
			break;
		}
		_cameraButton.State = state;
		_galleryButton.State = state2;
		_stickerButton.State = state3;
		_recordVoiceMenuItem.State = state4;
		_imageSearchMenuItem.State = state5;
		_locationMenuItem.State = state6;
	}

	private void TopBar_OnTap(object sender, GestureEventArgs e)
	{
		WithViewModel((MessagesViewModel vm) =>
		{
			vm.NavigateToDetailsCommand.Execute(null);
		});
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected Obj, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected Obj, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected Obj, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected Obj, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected Obj, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected Obj, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/ThreadPage.xaml", UriKind.Relative));
			ThreadViewPage = (MessagesViewModelPage)((FrameworkElement)this).FindName("ThreadViewPage");
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			SecondLayoutRoot = (Grid)((FrameworkElement)this).FindName("SecondLayoutRoot");
			TopBar = (MessageTopBar)((FrameworkElement)this).FindName("TopBar");
			MessageList = (FacebookListView)((FrameworkElement)this).FindName("MessageList");
			ListHeaderGrid = (Grid)((FrameworkElement)this).FindName("ListHeaderGrid");
			NestedListHeaderGrid = (Grid)((FrameworkElement)this).FindName("NestedListHeaderGrid");
			TypingIndicator = (TypingIndicator)((FrameworkElement)this).FindName("TypingIndicator");
			ComposerBubbleComponents = (Grid)((FrameworkElement)this).FindName("ComposerBubbleComponents");
			FocusButton = (Button)((FrameworkElement)this).FindName("FocusButton");
			LocationButton = (Button)((FrameworkElement)this).FindName("LocationButton");
			LocationButtonDisabledImage = (Image)((FrameworkElement)this).FindName("LocationButtonDisabledImage");
			LocationButtonEnabledImage = (Image)((FrameworkElement)this).FindName("LocationButtonEnabledImage");
			MessageComposerBubble = (MessageComposerBubble)((FrameworkElement)this).FindName("MessageComposerBubble");
			DrawerPaddingElement = (Grid)((FrameworkElement)this).FindName("DrawerPaddingElement");
			Container = (DrawerContainer)((FrameworkElement)this).FindName("Container");
			BusyIndicator = (FBMessengerProgressBar)((FrameworkElement)this).FindName("BusyIndicator");
		}
	}
}
