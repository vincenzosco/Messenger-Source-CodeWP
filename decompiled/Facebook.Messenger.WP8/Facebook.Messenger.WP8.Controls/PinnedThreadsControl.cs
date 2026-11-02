using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Controls
{
	public class PinnedThreadsControl : BaseControl
	{
		internal CubicEase ZoomEase;

		internal Storyboard FadeNuxAway;

		internal Grid LayoutRoot;

		internal PinnedThreadGridView List;

		internal ContentPresenter GroupsNuxPresenter;

		private bool _contentLoaded;

		public bool IsRearranging
		{
			get
			{
				return List.IsRearranging;
			}
		}

		protected Navigator Navigator
		{
			get
			{
				return ServiceLocator.Current.GetInstance<Navigator>();
			}
		}

		public PinnedThreadsControl()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			InitializeComponent();
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnloaded;
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			PinnedThreadsViewModel pinnedThreadsViewModel = ((FrameworkElement)this).DataContext as PinnedThreadsViewModel;
			if (pinnedThreadsViewModel != null)
			{
				pinnedThreadsViewModel.PropertyChanged -= OnPropertyChanged;
				pinnedThreadsViewModel.PinsToAdd.CollectionChanged -= PinsToAddOnCollectionChanged;
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			PinnedThreadsViewModel pinnedThreadsViewModel = ((FrameworkElement)this).DataContext as PinnedThreadsViewModel;
			if (pinnedThreadsViewModel != null)
			{
				pinnedThreadsViewModel.PropertyChanged += OnPropertyChanged;
				pinnedThreadsViewModel.PinsToAdd.CollectionChanged += PinsToAddOnCollectionChanged;
				if (pinnedThreadsViewModel.ShowGroupsNux)
				{
					DisplayNux(pinnedThreadsViewModel.HasPinnedThreads());
				}
				if (pinnedThreadsViewModel.HasPinnedThreadsToAdd())
				{
					List.AnimatePinAdd(pinnedThreadsViewModel.PinsToAdd);
				}
			}
		}

		private void PinsToAddOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			PinnedThreadsViewModel pinnedThreadsViewModel = ((FrameworkElement)this).DataContext as PinnedThreadsViewModel;
			List.AnimatePinAdd(pinnedThreadsViewModel.PinsToAdd);
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("ShowGroupsNux"))
			{
				PinnedThreadsViewModel pinnedThreadsViewModel = ((FrameworkElement)this).DataContext as PinnedThreadsViewModel;
				if (pinnedThreadsViewModel.ShowGroupsNux)
				{
					DisplayNux(pinnedThreadsViewModel.HasPinnedThreads());
				}
			}
		}

		public void CancelRearranging()
		{
			List.CancelRearranging();
		}

		private void DisplayNux(bool hasPinnedThreads)
		{
			GroupsNuxControl groupsNuxControl = GroupsNuxPresenter.Content as GroupsNuxControl;
			if (groupsNuxControl == null)
			{
				groupsNuxControl = new GroupsNuxControl();
				groupsNuxControl.OnDismissClick += OnNuxDismissClick;
				GroupsNuxPresenter.Content = groupsNuxControl;
			}
			groupsNuxControl.SetButtonLabel(hasPinnedThreads);
		}

		private void OnNuxDismissClick(object sender, EventArgs eventArgs)
		{
			PinnedThreadsViewModel pinnedThreadsViewModel = ((FrameworkElement)this).DataContext as PinnedThreadsViewModel;
			if (pinnedThreadsViewModel.HasPinnedThreads())
			{
				object obj = ((FrameworkElement)this).Resources[(object)"FadeNuxAway"];
				Storyboard val = (Storyboard)((obj is Storyboard) ? obj : null);
				val.Begin();
			}
			else
			{
				pinnedThreadsViewModel.OpenGroupComposer();
			}
		}

		private void PinnedThread_OnTap(object sender, GestureEventArgs e)
		{
			if (List.IsRearranging)
			{
				return;
			}
			StackPanel val = (StackPanel)((sender is StackPanel) ? sender : null);
			if (val != null)
			{
				ThreadViewModel threadViewModel = ((FrameworkElement)val).DataContext as ThreadViewModel;
				if (threadViewModel != null)
				{
					FacebookContext context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext;
					string threadNameOrId = ThreadNameUtils.GetThreadNameOrId(threadViewModel, context);
					Navigator.Navigate(FacebookUri.GetThreadUri(threadViewModel.Id, threadNameOrId));
				}
			}
		}

		private void LayoutRoot_OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			if (List.IsRearranging)
			{
				e.Handled = true;
			}
		}

		private void LayoutRoot_OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			if (List.IsRearranging)
			{
				e.Handled = true;
			}
		}

		private void LayoutRoot_OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			if (List.IsRearranging)
			{
				e.Handled = true;
			}
		}

		private void FadeNuxAway_OnCompleted(object sender, EventArgs e)
		{
			PinnedThreadsViewModel pinnedThreadsViewModel = ((FrameworkElement)this).DataContext as PinnedThreadsViewModel;
			pinnedThreadsViewModel.DismissNux();
			((UIElement)GroupsNuxPresenter).Opacity = 1.0;
		}

		public void OnNavigatedFrom()
		{
			List.CancelRearranging();
		}

		private void UnpinOverlay_OnTap(object sender, GestureEventArgs e)
		{
			e.Handled = true;
			List.AnimatePinRemove();
		}

		private void List_OnPinRemoved(object sender, ThreadViewModel thread)
		{
			PinnedThreadsViewModel pinnedThreadsViewModel = ((FrameworkElement)this).DataContext as PinnedThreadsViewModel;
			if (pinnedThreadsViewModel != null)
			{
				pinnedThreadsViewModel.UnpinThread(thread);
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
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/PinnedThreadsControl.xaml", UriKind.Relative));
				ZoomEase = (CubicEase)((FrameworkElement)this).FindName("ZoomEase");
				FadeNuxAway = (Storyboard)((FrameworkElement)this).FindName("FadeNuxAway");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				List = (PinnedThreadGridView)((FrameworkElement)this).FindName("List");
				GroupsNuxPresenter = (ContentPresenter)((FrameworkElement)this).FindName("GroupsNuxPresenter");
			}
		}
	}
}
