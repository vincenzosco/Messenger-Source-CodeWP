using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Common;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class ForwardingPage : ForwardingViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private IconBarEntry<bool> _sendButton;












		public event EventHandler OnClosed;

		public ForwardingPage()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected Obj, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBarItems();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				if (vm.SearchViewModel.IsSearching)
				{
					e.Cancel = true;
					Reset();
				}
			});
			base.OnBackKeyPress(e);
		}

		private void InitializeAppBarItems()
		{
			_appBarHelper = new AppBarHelper(base.ApplicationBar);
			_sendButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.Conversation_AppBar_Send, SendButton_Clicked, new Uri("/Assets/AppBar/SendMessage.png", UriKind.Relative)));
			_sendButton.IsEnabled = false;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
			});
			SetUpBindings();
		}

		private void SetUpBindings()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected Obj, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected Obj, but got Unknown
			Border searchResultsBorder = SearchResultsBorder;
			DependencyProperty visibilityProperty = UIElement.VisibilityProperty;
			Binding val = new Binding();
			val.Path = new PropertyPath("Text", new object[0]);
			val.Source = SearchTextBox;
			val.Converter = (IValueConverter)(object)new StringLengthToVisibilityConverter();
			((FrameworkElement)searchResultsBorder).SetBinding(visibilityProperty, val);
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
			});
			((DependencyObject)SearchResultsBorder).ClearValue(UIElement.VisibilityProperty);
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName;
			if ((propertyName = e.PropertyName) != null && propertyName == "IsLoading")
			{
				WithViewModel((ForwardingViewModel vm) =>
				{
					base.ApplicationBar.IsVisible = !vm.IsLoading;
				});
			}
		}

		private void ToggleMembership(ThreadViewModel thread)
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				if (!vm.IsLoading)
				{
					vm.ToggleMembership(thread);
					_sendButton.IsEnabled = vm.SelectedRecipients.Any();
				}
			});
		}

		private void OnThreadTapped(object sender, GestureEventArgs args)
		{
			ThreadListItemControl threadListItemControl = sender as ThreadListItemControl;
			if (threadListItemControl == null)
			{
				return;
			}
			ThreadViewModel viewModel = threadListItemControl.ViewModel;
			if (viewModel == null)
			{
				return;
			}
			ToggleMembership(viewModel);
			if (viewModel.IsSelected)
			{
				base.Dispatcher.BeginInvoke((Action)(() =>
				{
					RecipientScrollViewer.ScrollToHorizontalOffset(RecipientScrollViewer.ExtentWidth);
				}));
			}
		}

		private void OnSelectionTapped(object sender, GestureEventArgs args)
		{
			FaceMash faceMash = sender as FaceMash;
			if (faceMash != null)
			{
				ThreadViewModel threadViewModel = ((FrameworkElement)faceMash).DataContext as ThreadViewModel;
				if (threadViewModel != null)
				{
					ToggleMembership(threadViewModel);
				}
			}
		}

		private void SendButton_Clicked(object sender, EventArgs e)
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				if (!vm.HasConnection)
				{
					OnErrorOccurred(this, new Error
					{
						Message = StringLibrary.Error_No_Connection
					});
				}
				else if (!vm.IsLoading)
				{
					((UIElement)FullscreenProgressIndicator).Visibility = Visibility.Visible;
					vm.EnqueueAllMessages();
				}
			});
		}

		private void SearchFilterChanged(object sender, TextChangedEventArgs e)
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				vm.SearchViewModel.SearchFilter = SearchTextBox.Text;
			});
		}

		private void SearchTextBox_OnGotFocus(object sender, RoutedEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			double actualHeight = ((FrameworkElement)SearchTextBox).ActualHeight;
			Thickness margin = ((FrameworkElement)SearchTextBox).Margin;
			double num = actualHeight + margin.Top;
			Thickness margin2 = ((FrameworkElement)SearchTextBox).Margin;
			double num2 = num + margin2.Bottom + ((FrameworkElement)TitleTextBlock).ActualHeight;
			Thickness margin3 = ((FrameworkElement)TitleTextBlock).Margin;
			double num3 = num2 + margin3.Top;
			Thickness margin4 = ((FrameworkElement)TitleTextBlock).Margin;
			double spaceRequired = num3 + margin4.Bottom + 70.0;
			double remainingSpace = InputHelper.GetRemainingSpace(spaceRequired);
			WithViewModel((ForwardingViewModel vm) =>
			{
				vm.SearchViewModel.IsSearching = true;
			});
			((FrameworkElement)ContactsSearchResults).MaxHeight = remainingSpace;
		}

		private void SearchTextBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				ContactSearchViewModel searchViewModel = vm.SearchViewModel;
				HintTextBox hintTextBox = sender as HintTextBox;
				if (hintTextBox != null && hintTextBox.Text == string.Empty)
				{
					Reset();
				}
				ObservableCollection<FacebookUser> allUsers = searchViewModel.SearchResults.AllUsers;
				if (allUsers.Count > 3 && allUsers.Count < 8)
				{
					ContactsSearchResults.ScrollToTop();
				}
				((FrameworkElement)ContactsSearchResults).MaxHeight = Application.Current.Host.Content.ActualHeight;
			});
		}

		private void OnOverlayGridTapped(object sender, GestureEventArgs e)
		{
			Reset();
		}

		private void OnGroupItemTapped(object sender, ThreadViewModel thread)
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				ThreadViewModel threadViewModel = vm.Threads.FirstOrDefault((ThreadViewModel t) => t.Id == thread.Id);
				ToggleMembership(threadViewModel ?? thread);
				Reset();
			});
		}

		private void OnContactItemTapped(object sender, FacebookProfile profile)
		{
			MailUser contact = new MailUser
			{
				Id = profile.Id,
				Name = profile.Name
			};
			WithViewModel((ForwardingViewModel vm) =>
			{
				ThreadViewModel threadViewModel = vm.Threads.FirstOrDefault((ThreadViewModel t) => t.IsCanonicalWithParticipant(contact));
				if (threadViewModel != null)
				{
					ToggleMembership(threadViewModel);
				}
				else
				{
					ThreadViewModel thread = new ThreadViewModel(contact);
					ToggleMembership(thread);
				}
				Reset();
			});
		}

		public void Reset()
		{
			WithViewModel((ForwardingViewModel vm) =>
			{
				SearchTextBox.Text = string.Empty;
				vm.SearchViewModel.Reset();
			});
			if (OnClosed != null)
			{
				OnClosed(this, null);
			}
		}

	}
}
