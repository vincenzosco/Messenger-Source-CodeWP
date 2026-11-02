using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.Utilities.SystemTrayManager;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls
{
	public class SearchOverlay : Grid
	{
		private readonly SystemTrayManager _systemTrayManager;

		internal Storyboard AnimateContactSearchIn;

		internal Storyboard AnimateContactSearchOut;

		internal Border ContactSearch;

		internal TextBox SearchTextBox;

		internal Grid SearchOverlayGrid;

		internal Border SearchResultsBorder;

		internal FacebookListView ContactsSearchResults;

		internal TextBlock EmptyStateTextBlock;

		private bool _contentLoaded;

		public event EventHandler OnClosed;

		public event EventHandler<ThreadViewModel> OnGroupThreadTapped;

		public event EventHandler<FacebookProfile> OnContactTapped;

		public SearchOverlay(SystemTrayManager systemTrayManager)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected Obj, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			InitializeComponent();
			_systemTrayManager = systemTrayManager;
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnloaded;
		}

		public void Reset()
		{
			_systemTrayManager.RemoveOperationFromStack(((object)this).GetType());
			((FrameworkElement)(object)this).CallWithViewModel((ContactSearchViewModel vm) =>
			{
				vm.Reset();
			});
			((UIElement)SearchOverlayGrid).Visibility = (Visibility)1;
			((UIElement)SearchResultsBorder).Visibility = (Visibility)1;
			((UIElement)EmptyStateTextBlock).Visibility = (Visibility)1;
			((DependencyObject)SearchTextBox).ClearValue(TextBox.TextProperty);
			if (OnClosed != null)
			{
				OnClosed(this, null);
			}
			AnimateContactSearchOut.Begin();
		}

		public void OnLoaded(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)(object)this).CallWithViewModel((ContactSearchViewModel vm) =>
			{
				vm.SearchResults.FirstElementLoaded += SearchResultsOnFirstElementLoaded;
			});
		}

		public void OnUnloaded(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)(object)this).CallWithViewModel((ContactSearchViewModel vm) =>
			{
				vm.SearchResults.FirstElementLoaded -= SearchResultsOnFirstElementLoaded;
			});
		}

		public void Show()
		{
			_systemTrayManager.AddOperationToStack(((object)this).GetType());
			((FrameworkElement)(object)this).CallWithViewModel((ContactSearchViewModel vm) =>
			{
				vm.IsSearching = true;
			});
			SetupBindings();
			AnimateContactSearchIn.Begin();
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				((Control)SearchTextBox).Focus();
			}));
		}

		private void SetupBindings()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected Obj, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected Obj, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected Obj, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected Obj, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			Border searchResultsBorder = SearchResultsBorder;
			DependencyProperty visibilityProperty = UIElement.VisibilityProperty;
			Binding val = new Binding();
			val.Path = new PropertyPath("Text", new object[0]);
			val.Source = SearchTextBox;
			val.Converter = (IValueConverter)(object)new StringLengthToVisibilityConverter();
			((FrameworkElement)searchResultsBorder).SetBinding(visibilityProperty, val);
			Grid searchOverlayGrid = SearchOverlayGrid;
			DependencyProperty visibilityProperty2 = UIElement.VisibilityProperty;
			Binding val2 = new Binding();
			val2.Path = new PropertyPath("IsSearching", new object[0]);
			val2.Converter = (IValueConverter)(object)new BoolToVisibilityConverter();
			((FrameworkElement)searchOverlayGrid).SetBinding(visibilityProperty2, val2);
			TextBlock emptyStateTextBlock = EmptyStateTextBlock;
			DependencyProperty visibilityProperty3 = UIElement.VisibilityProperty;
			Binding val3 = new Binding();
			val3.Path = new PropertyPath("ContactSearchState", new object[0]);
			val3.Converter = (IValueConverter)(object)new SearchStateToVisibilityConverter();
			((FrameworkElement)emptyStateTextBlock).SetBinding(visibilityProperty3, val3);
		}

		private void ContactSearchFilterChanged(object sender, TextChangedEventArgs e)
		{
			((FrameworkElement)(object)this).CallWithViewModel((ContactSearchViewModel vm) =>
			{
				vm.SearchFilter = SearchTextBox.Text;
			});
		}

		private void SearchTextBox_OnGotFocus(object sender, RoutedEventArgs e)
		{
			double remainingSpace = InputHelper.GetRemainingSpace(((FrameworkElement)ContactSearch).ActualHeight);
			((FrameworkElement)ContactsSearchResults).MaxHeight = remainingSpace;
		}

		private void SearchTextBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)(object)this).CallWithViewModel((ContactSearchViewModel vm) =>
			{
				object obj = sender;
				TextBox val = (TextBox)((obj is TextBox) ? obj : null);
				if (val != null && val.Text == string.Empty)
				{
					Reset();
				}
				ObservableCollection<FacebookUser> allUsers = vm.SearchResults.AllUsers;
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
			OnGroupThreadTapped(sender, thread);
			Reset();
		}

		private void OnItemTapped(object sender, FacebookProfile profile)
		{
			OnContactTapped(sender, profile);
			Reset();
		}

		private void SearchResultsOnFirstElementLoaded(object sender, EventArgs eventArgs)
		{
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				ContactsSearchResults.ScrollToTop();
			}));
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
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/SearchOverlay.xaml", UriKind.Relative));
				AnimateContactSearchIn = (Storyboard)((FrameworkElement)this).FindName("AnimateContactSearchIn");
				AnimateContactSearchOut = (Storyboard)((FrameworkElement)this).FindName("AnimateContactSearchOut");
				ContactSearch = (Border)((FrameworkElement)this).FindName("ContactSearch");
				SearchTextBox = (TextBox)((FrameworkElement)this).FindName("SearchTextBox");
				SearchOverlayGrid = (Grid)((FrameworkElement)this).FindName("SearchOverlayGrid");
				SearchResultsBorder = (Border)((FrameworkElement)this).FindName("SearchResultsBorder");
				ContactsSearchResults = (FacebookListView)((FrameworkElement)this).FindName("ContactsSearchResults");
				EmptyStateTextBlock = (TextBlock)((FrameworkElement)this).FindName("EmptyStateTextBlock");
			}
		}
	}
}
