using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Messages;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages
{
	public class ComposerBarPage : MessageComposerViewModelPage
	{
		private TextBox _searchTextBox;

		private Token _selectedToken;

		private NavigationMode _navigationMode;

		private bool _isSearchFocused;

		private bool _isInputFocused;

		internal Grid LayoutRoot;

		internal Grid SecondLayoutRoot;

		internal FBMessengerProgressBar SearchProgressBar;

		internal Border RecipientsBorder;

		internal ScrollViewer RecipientScrollViewer;

		internal TextBox TokenKeyboardTextBox;

		internal WrapPanel RecipientList;

		internal Border SearchListBorder;

		internal FacebookListView SearchList;

		internal Grid BubbleGrid;

		internal LocationNuxControl LocationWarning;

		internal Button LocationButton;

		internal Image LocationButtonDisabledImage;

		internal Image LocationButtonEnabledImage;

		internal MessageComposerBubble MessageComposerBubble;

		internal Grid DrawerPaddingElement;

		internal DrawerContainer DrawerContainer;

		private bool _contentLoaded;

		private bool IsSearchFocused
		{
			get
			{
				return _isSearchFocused;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				_isSearchFocused = value;
				((UIElement)SearchListBorder).Visibility = VisibilityUtil.FromBool(_isSearchFocused);
				RecipientsBorder.BorderThickness = (_isSearchFocused ? new Thickness(0.0) : new Thickness(0.0, 0.0, 0.0, 1.0));
			}
		}

		protected override Grid PhotoPopupParent => LayoutRoot;

		protected override ImageUploadAttachmentControl ImageUploadControl => MessageComposerBubble.UploadBar;

		public ComposerBarPage()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected Obj, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected Obj, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected Obj, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeSearchTextBox();
			InitializeAppBar();
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnloaded;
			MessageComposerBubble.HasTextChanged += MessageComposerBubbleOnHasTextChanged;
			((UIElement)MessageComposerBubble).GotFocus += OnMessageComposerBubbleGotFocus;
			((UIElement)MessageComposerBubble).LostFocus += OnMessageComposerBubbleLostFocus;
			MessageComposerBubble.EnterPressed += OnSendMessageEnter;
			RegisterInputElements(new List<UIElement>
			{
				(UIElement)(object)_searchTextBox,
				(UIElement)(object)MessageComposerBubble
			});
		}

		private void LocationButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			WithViewModel((MessageComposerViewModel vm) =>
			{
				vm.SetLocationSendingCommand.Execute(null);
			});
		}

		protected override void InitializeCustomApplicationBarItems(AppBarHelper appBarHelper)
		{
		}

		protected override void OnSendMessageEnter(object sender, EventArgs e)
		{
			WithViewModel((MessageComposerViewModel vm) =>
			{
				vm.SendComposedMessage(MessageComposerBubble.Text, MessageBaseViewModel.SendTrigger.Enter);
			});
		}

		protected override void OnSendMessageClick(object sender, EventArgs e)
		{
			WithViewModel((MessageComposerViewModel vm) =>
			{
				vm.SendComposedMessage(MessageComposerBubble.Text, MessageBaseViewModel.SendTrigger.Click);
			});
		}

		protected override void OnLikeClick(object sender, EventArgs e)
		{
		}

		protected override void OnTransferFocusToComposerClick(object sender, EventArgs e)
		{
			HideAllDrawers();
			((Control)MessageComposerBubble).Focus();
		}

		private void OnMessageComposerBubbleGotFocus(object sender, RoutedEventArgs args)
		{
			_isInputFocused = true;
		}

		private void OnMessageComposerBubbleLostFocus(object sender, RoutedEventArgs args)
		{
			object focusedElement = FocusManager.GetFocusedElement();
			if (focusedElement == LocationButton)
			{
				((Control)MessageComposerBubble).Focus();
			}
			else
			{
				_isInputFocused = false;
			}
		}

		protected override void SetRootFrameTransform(int translateY)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			base.SetRootFrameTransform(0);
			Thickness margin = ((FrameworkElement)LayoutRoot).Margin;
			margin.Bottom = Math.Abs(translateY);
			((FrameworkElement)LayoutRoot).Margin = margin;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
			case "DraftText":
				WithViewModel((MessageComposerViewModel vm) =>
				{
					if (!MessageComposerBubble.HasText)
					{
						MessageComposerBubble.Text = vm.DraftText;
					}
				});
				break;
			case "MessageComposerHasContent":
			case "HasRecipients":
				UpdateAppBarState();
				break;
			case "SendingMessage":
				WithViewModel((MessageComposerViewModel vm) =>
				{
					((PhoneApplicationPage)this).ApplicationBar.IsVisible = !vm.SendingMessage;
					if (vm.SendingMessage)
					{
						((Control)this).Focus();
					}
				});
				break;
			}
		}

		private void UpdateAppBarState()
		{
			WithViewModel((MessageComposerViewModel vm) =>
			{
				_sendOrLikeButton.State = (SendButtonState)(vm.MessageComposerHasContent ? SendButtonState.Send : SendButtonState.Like);
				_sendOrLikeButton.StateValues[SendButtonState.Like].IsEnabled = false;
				_sendOrLikeButton.ResetCurrentState();
				_cameraButton.IsEnabled = vm.HasRecipients;
				_galleryButton.IsEnabled = vm.HasRecipients;
				_stickerButton.IsEnabled = vm.HasRecipients;
				_recordVoiceMenuItem.StateValues[DrawerSelectedState.Default].IsEnabled = vm.HasRecipients;
				_recordVoiceMenuItem.ResetCurrentState();
				_imageSearchMenuItem.StateValues[DrawerSelectedState.Default].IsEnabled = vm.HasRecipients;
				_imageSearchMenuItem.ResetCurrentState();
				_locationMenuItem.StateValues[DrawerSelectedState.Default].IsEnabled = vm.HasRecipients;
				_locationMenuItem.ResetCurrentState();
			});
		}

		private void GroupedContactListOnFirstElementLoaded(object sender, EventArgs eventArgs)
		{
			SearchList.ScrollToTop();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((MessageComposerViewModel vm) =>
			{
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Expected Obj, but got Unknown
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Expected Obj, but got Unknown
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Expected Obj, but got Unknown
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Expected Obj, but got Unknown
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Invalid comparison between Unknown and I4
				vm.Recipients.CollectionChanged += OnRecipientsChanged;
				vm.PropertyChanged += OnPropertyChanged;
				vm.GroupedContactList.FirstElementLoaded += GroupedContactListOnFirstElementLoaded;
				UpdateAppBarState();
				((FrameworkElement)_searchTextBox).DataContext = vm;
				_searchTextBox.TextChanged += SearchTextBoxOnTextChanged;
				((UIElement)_searchTextBox).KeyDown += SearchTextBoxOnKeyDown;
				((UIElement)_searchTextBox).GotFocus += SearchTextBoxGotFocus;
				((UIElement)_searchTextBox).LostFocus += SearchTextBoxLostFocus;
				PopulateRecipientList();
				if ((int)_navigationMode != 1)
				{
					((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
					{
						vm.SearchFilter = "";
						((Control)_searchTextBox).Focus();
					}));
				}
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected Obj, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected Obj, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected Obj, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected Obj, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected Obj, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected Obj, but got Unknown
			WithViewModel((MessageComposerViewModel vm) =>
			{
				vm.Recipients.CollectionChanged -= OnRecipientsChanged;
				vm.PropertyChanged -= OnPropertyChanged;
				vm.GroupedContactList.FirstElementLoaded -= GroupedContactListOnFirstElementLoaded;
			});
			_searchTextBox.TextChanged -= SearchTextBoxOnTextChanged;
			((UIElement)_searchTextBox).KeyDown -= SearchTextBoxOnKeyDown;
			((UIElement)_searchTextBox).GotFocus -= SearchTextBoxGotFocus;
			((UIElement)_searchTextBox).LostFocus -= SearchTextBoxLostFocus;
			((UIElement)MessageComposerBubble).GotFocus -= OnMessageComposerBubbleGotFocus;
			((UIElement)MessageComposerBubble).LostFocus -= OnMessageComposerBubbleLostFocus;
			MessageComposerBubble.EnterPressed -= OnSendMessageEnter;
			MessageComposerBubble.HasTextChanged -= MessageComposerBubbleOnHasTextChanged;
		}

		private void InitializeSearchTextBox()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected Obj, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected Obj, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			TextBox val = new TextBox();
			((Control)val).Foreground = (Brush)new SolidColorBrush(Colors.Black);
			((Control)val).FontSize = (double)Application.Current.Resources[(object)"PhoneFontSizeMedium"];
			((FrameworkElement)val).Margin = new Thickness(5.0);
			((Control)val).Padding = new Thickness(0.0);
			((FrameworkElement)val).VerticalAlignment = (VerticalAlignment)1;
			((FrameworkElement)val).HorizontalAlignment = (HorizontalAlignment)3;
			object obj = Application.Current.Resources[(object)"NoBorderTextBoxStyle"];
			((FrameworkElement)val).Style = (Style)((obj is Style) ? obj : null);
			((FrameworkElement)val).MinWidth = 40.0;
			_searchTextBox = val;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			base.OnNavigatedTo(e);
			WithViewModel((MessageComposerViewModel vm) =>
			{
				MessageComposerBubble.Text = vm.DraftText;
			});
			_navigationMode = e.NavigationMode;
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			WithViewModel((MessageComposerViewModel vm) =>
			{
				vm.DraftText = MessageComposerBubble.Text;
			});
			base.OnNavigatedFrom(e);
		}

		private void SearchTextBoxOnKeyDown(object sender, KeyEventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Invalid comparison between Unknown and I4
			if ((int)e.Key == 1 && _searchTextBox.Text == string.Empty && ((PresentationFrameworkCollection<UIElement>)(object)((Panel)RecipientList).Children).Count > 1)
			{
				Token token = ((PresentationFrameworkCollection<UIElement>)(object)((Panel)RecipientList).Children)[((PresentationFrameworkCollection<UIElement>)(object)((Panel)RecipientList).Children).Count - 2] as Token;
				if (token != null)
				{
					token.IsFocused = true;
					_selectedToken = token;
					((Control)TokenKeyboardTextBox).Focus();
				}
			}
			else
			{
				if ((int)e.Key != 3)
				{
					return;
				}
				WithViewModel((MessageComposerViewModel vm) =>
				{
					if (string.IsNullOrEmpty(_searchTextBox.Text))
					{
						((Control)MessageComposerBubble).Focus();
					}
					else if (vm.GroupedContactList.Count == 1 && vm.GroupedContactList[0].Count == 1)
					{
						vm.ToggleRecipientCommand.Execute(new MessageArgs(vm.GroupedContactList[0][0]));
					}
				});
			}
		}

		private void SearchTextBoxGotFocus(object sender, RoutedEventArgs e)
		{
			IsSearchFocused = true;
			if (_selectedToken != null)
			{
				return;
			}
			RecipientScrollViewer.ScrollToVerticalOffset(RecipientScrollViewer.ScrollableHeight);
			if (!sender.Equals(_searchTextBox))
			{
				return;
			}
			foreach (UIElement item in (PresentationFrameworkCollection<UIElement>)(object)((Panel)RecipientList).Children)
			{
				Token token = item as Token;
				if (token != null)
				{
					token.IsFocused = false;
				}
			}
		}

		private void SearchTextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			IsSearchFocused = false;
		}

		private void MessageComposerBubbleOnHasTextChanged(object sender, EventArgs eventArgs)
		{
			WithViewModel((MessageComposerViewModel vm) =>
			{
				vm.MessageComposerHasText = MessageComposerBubble.HasText;
			});
		}

		private void OnRecipientsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			PopulateRecipientList();
		}

		private void OnItemTapped(object sender, GestureEventArgs e)
		{
			ContactUserControl contactUserControl = sender as ContactUserControl;
			if (contactUserControl == null)
			{
				return;
			}
			FacebookUser profile = ((FrameworkElement)contactUserControl).DataContext as FacebookUser;
			if (profile != null)
			{
				WithViewModel((MessageComposerViewModel vm) =>
				{
					vm.ToggleRecipientCommand.Execute(new MessageArgs(profile));
				});
			}
		}

		private void PopulateRecipientList()
		{
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)RecipientList).Children).Clear();
			WithViewModel((MessageComposerViewModel vm) =>
			{
				foreach (MessageArgs recipient in vm.Recipients)
				{
					Token token = new Token();
					((UIElement)token).Tap += OnTokenTap;
					((FrameworkElement)token).DataContext = recipient;
					((PresentationFrameworkCollection<UIElement>)(object)((Panel)RecipientList).Children).Add((UIElement)(object)token);
				}
			});
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)RecipientList).Children).Add((UIElement)(object)_searchTextBox);
			_searchTextBox.Text = string.Empty;
			if (IsSearchFocused)
			{
				((Control)_searchTextBox).Focus();
			}
			else if (_isInputFocused)
			{
				((Control)MessageComposerBubble).Focus();
			}
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)ScrollToLastToken);
		}

		private void ScrollToLastToken()
		{
			double scrollableHeight = RecipientScrollViewer.ScrollableHeight;
			RecipientScrollViewer.ScrollToVerticalOffset(scrollableHeight);
		}

		private void OnTokenTap(object sender, GestureEventArgs e)
		{
			_selectedToken = sender as Token;
			bool isFocused = _selectedToken.IsFocused;
			foreach (UIElement item in (PresentationFrameworkCollection<UIElement>)(object)((Panel)RecipientList).Children)
			{
				Token token = item as Token;
				if (token != null)
				{
					token.IsFocused = false;
				}
			}
			if (!isFocused)
			{
				_selectedToken.IsFocused = true;
				_searchTextBox.Text = string.Empty;
			}
			else
			{
				_selectedToken = null;
			}
			((Control)TokenKeyboardTextBox).Focus();
			e.Handled = true;
		}

		private void SearchTextBoxOnTextChanged(object sender, TextChangedEventArgs e)
		{
			WithViewModel((MessageComposerViewModel vm) =>
			{
				vm.SearchFilter = _searchTextBox.Text;
			});
		}

		private void OnRecipientBorderTapped(object sender, GestureEventArgs e)
		{
			((Control)_searchTextBox).Focus();
		}

		private void OnInvisibleTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)e.Key == 1 && _selectedToken != null)
			{
				MessageArgs messageArgs = ((FrameworkElement)_selectedToken).DataContext as MessageArgs;
				WithViewModel((MessageComposerViewModel vm) =>
				{
					if (messageArgs != null && messageArgs.Id != null)
					{
						vm.RemovedRecipientCommand.Execute(messageArgs);
					}
				});
			}
			e.Handled = true;
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			if (AttemptToDismissPopups(e))
			{
				return;
			}
			WithViewModel((MessageComposerViewModel vm) =>
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Invalid comparison between Unknown and I4
				if (!vm.MessageComposerHasContent)
				{
					base.OnBackKeyPress(e);
				}
				else
				{
					MessageBoxResult val = MessageBox.Show(StringLibrary.Composer_Exit_Warning, string.Empty, (MessageBoxButton)1);
					if ((int)val == 1)
					{
						base.OnBackKeyPress(e);
					}
					else
					{
						e.Cancel = true;
					}
				}
			});
		}

		protected override DrawerContainer GetDrawerContainer()
		{
			return DrawerContainer;
		}

		protected override FrameworkElement GetDrawerPaddingElement()
		{
			return (FrameworkElement)(object)DrawerPaddingElement;
		}

		protected override void DrawerStateChanged()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			base.DrawerStateChanged();
			DrawerType visibleDrawerType = GetVisibleDrawerType();
			bool flag = visibleDrawerType != DrawerType.None;
			((UIElement)BubbleGrid).Visibility = VisibilityUtil.FromBool(!flag);
			SendButtonState state = SendButtonState.FocusToTextBox;
			DrawerSelectedState state2 = DrawerSelectedState.Default;
			DrawerSelectedState state3 = DrawerSelectedState.Default;
			DrawerSelectedState state4 = DrawerSelectedState.Default;
			DrawerSelectedState state5 = DrawerSelectedState.Default;
			DrawerSelectedState state6 = DrawerSelectedState.Default;
			DrawerSelectedState state7 = DrawerSelectedState.Default;
			switch (visibleDrawerType)
			{
			case DrawerType.None:
				state = SendButtonState.Send;
				break;
			case DrawerType.Camera:
				state2 = DrawerSelectedState.Selected;
				break;
			case DrawerType.Gallery:
				state3 = DrawerSelectedState.Selected;
				break;
			case DrawerType.Sticker:
				state4 = DrawerSelectedState.Selected;
				break;
			case DrawerType.Voice:
				state5 = DrawerSelectedState.Selected;
				break;
			case DrawerType.ImageSearch:
				state6 = DrawerSelectedState.Selected;
				break;
			case DrawerType.Location:
				state7 = DrawerSelectedState.Selected;
				break;
			}
			_sendOrLikeButton.State = state;
			_cameraButton.State = state2;
			_galleryButton.State = state3;
			_stickerButton.State = state4;
			_recordVoiceMenuItem.State = state5;
			_imageSearchMenuItem.State = state6;
			_locationMenuItem.State = state7;
		}

		protected override UIElement GetLayoutRoot()
		{
			return (UIElement)(object)LayoutRoot;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected Obj, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected Obj, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected Obj, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected Obj, but got Unknown
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/ComposerBarPage.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				SecondLayoutRoot = (Grid)((FrameworkElement)this).FindName("SecondLayoutRoot");
				SearchProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("SearchProgressBar");
				RecipientsBorder = (Border)((FrameworkElement)this).FindName("RecipientsBorder");
				RecipientScrollViewer = (ScrollViewer)((FrameworkElement)this).FindName("RecipientScrollViewer");
				TokenKeyboardTextBox = (TextBox)((FrameworkElement)this).FindName("TokenKeyboardTextBox");
				RecipientList = (WrapPanel)((FrameworkElement)this).FindName("RecipientList");
				SearchListBorder = (Border)((FrameworkElement)this).FindName("SearchListBorder");
				SearchList = (FacebookListView)((FrameworkElement)this).FindName("SearchList");
				BubbleGrid = (Grid)((FrameworkElement)this).FindName("BubbleGrid");
				LocationWarning = (LocationNuxControl)((FrameworkElement)this).FindName("LocationWarning");
				LocationButton = (Button)((FrameworkElement)this).FindName("LocationButton");
				LocationButtonDisabledImage = (Image)((FrameworkElement)this).FindName("LocationButtonDisabledImage");
				LocationButtonEnabledImage = (Image)((FrameworkElement)this).FindName("LocationButtonEnabledImage");
				MessageComposerBubble = (MessageComposerBubble)((FrameworkElement)this).FindName("MessageComposerBubble");
				DrawerPaddingElement = (Grid)((FrameworkElement)this).FindName("DrawerPaddingElement");
				DrawerContainer = (DrawerContainer)((FrameworkElement)this).FindName("DrawerContainer");
			}
		}
	}
}
