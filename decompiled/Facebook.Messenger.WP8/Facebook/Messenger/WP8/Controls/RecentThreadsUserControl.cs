using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Facebook.Models.Signals;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls;

public class RecentThreadsUserControl : BaseControl
{
	private bool _loaded;

	private bool _shouldScrollToTop;

	internal Grid LayoutRoot;

	internal FacebookListView ThreadList;

	internal TextBlock EmptyTextBlock;

	private bool _contentLoaded;

	public event EventHandler<NavigationEventArgs> NavigateEvent;

	public RecentThreadsUserControl()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheMessagesUpdatedSignal>(this, OnCacheMessageUpdatedSignal);
	}

	private void OnCacheMessageUpdatedSignal(CacheMessagesUpdatedSignal signal)
	{
		foreach (MailMessage message in signal.Messages)
		{
			bool flag = message.Source == MailMessage.MessageSource.Local;
			bool flag2 = !_loaded && message.SourceIsPush;
			if (flag || flag2)
			{
				_shouldScrollToTop = true;
			}
		}
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (_shouldScrollToTop)
		{
			_shouldScrollToTop = false;
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				ThreadList.ScrollToTop();
			}));
		}
		return ((FrameworkElement)this).ArrangeOverride(finalSize);
	}

	private void OnLoaded(object sender, RoutedEventArgs args)
	{
		_loaded = true;
		if (((FrameworkElement)this).DataContext is ThreadsViewModel threadsViewModel)
		{
			threadsViewModel.ErrorOccurred += OnErrorOccurred;
		}
	}

	private void OnErrorOccurred(object sender, Error e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (e != null)
		{
			MessageBox.Show(e.Message);
		}
	}

	private void OnUnloaded(object sender, RoutedEventArgs args)
	{
		_loaded = false;
		if (((FrameworkElement)this).DataContext is ThreadsViewModel threadsViewModel)
		{
			threadsViewModel.ErrorOccurred -= OnErrorOccurred;
		}
	}

	private void OnItemTapped(object sender, GestureEventArgs args)
	{
		if (sender is ThreadControl threadControl && NavigateEvent != null)
		{
			ThreadViewModel viewModel = threadControl.ViewModel;
			if (viewModel != null && ((FrameworkElement)this).DataContext is ThreadsViewModel threadsViewModel)
			{
				FacebookUri threadUri = FacebookUri.GetThreadUri(viewModel.Id, ThreadNameUtils.GetThreadNameOrPlaceholder(viewModel, threadsViewModel.Context));
				NavigateEvent(this, new NavigationEventArgs(threadUri));
			}
		}
	}

	private void DeleteThread_OnClick(object sender, RoutedEventArgs e)
	{
		if (MessageBoxUtil.Confirm(StringLibrary.MessageBox_Description_DeleteThread, StringLibrary.MessageBox_Label_DeleteThread))
		{
			TakeActionOnThread(sender as MenuItem, (ThreadsViewModel vm, ThreadViewModel thread) =>
			{
				vm.DeleteThread(thread.Id);
			});
		}
	}

	private void ArchiveThread_OnClick(object sender, RoutedEventArgs e)
	{
		if (MessageBoxUtil.Confirm(StringLibrary.MessageBox_Description_ArchiveThread, StringLibrary.MessageBox_Label_ArchiveThread))
		{
			TakeActionOnThread(sender as MenuItem, (ThreadsViewModel vm, ThreadViewModel thread) =>
			{
				vm.ArchiveThread(thread.Id);
			});
		}
	}

	private void LeaveThread_OnClick(object sender, RoutedEventArgs e)
	{
		if (MessageBoxUtil.Confirm(StringLibrary.MessageBox_Description_LeaveThread, StringLibrary.MessageBox_Label_LeaveThread))
		{
			TakeActionOnThread(sender as MenuItem, (ThreadsViewModel vm, ThreadViewModel thread) =>
			{
				vm.LeaveThread(thread.Id);
			});
		}
	}

	private void SpamThread_OnClick(object sender, RoutedEventArgs e)
	{
		if (MessageBoxUtil.Confirm(StringLibrary.MessageBox_Description_SpamThread, StringLibrary.MessageBox_Label_SpamThread))
		{
			TakeActionOnThread(sender as MenuItem, (ThreadsViewModel vm, ThreadViewModel thread) =>
			{
				vm.SpamThread(thread.Id);
			});
		}
	}

	private void Mute_OnClicked(object sender, RoutedEventArgs e)
	{
		FacebookUri uri = FacebookUri.MuteSelectionUri;
		TakeActionOnThread(sender as MenuItem, (ThreadsViewModel vm, ThreadViewModel thread) =>
		{
			NavigateEvent(this, new NavigationEventArgs(uri, thread));
		});
	}

	private void UnMute_OnClicked(object sender, RoutedEventArgs e)
	{
		TakeActionOnThread(sender as MenuItem, (ThreadsViewModel vm, ThreadViewModel thread) =>
		{
			vm.TurnOffMute(thread).DoNotWait();
		});
	}

	private void TakeActionOnThread(MenuItem menuItem, Action<ThreadsViewModel, ThreadViewModel> action)
	{
		if (menuItem != null && ((FrameworkElement)menuItem).DataContext is ThreadViewModel arg && ((FrameworkElement)this).DataContext is ThreadsViewModel arg2)
		{
			action(arg2, arg);
		}
	}

	private void ContextMenu_OnUnloaded(object sender, RoutedEventArgs e)
	{
		if (sender is ContextMenu contextMenu)
		{
			((DependencyObject)contextMenu).ClearValue(FrameworkElement.DataContextProperty);
		}
	}

	private void OnMarkUnreadClicked(object sender, RoutedEventArgs e)
	{
		TakeActionOnThread(sender as MenuItem, (ThreadsViewModel vm, ThreadViewModel thread) =>
		{
			vm.MarkThreadUnreadCommand.Execute(thread);
		});
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/RecentThreadsUserControl.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			ThreadList = (FacebookListView)((FrameworkElement)this).FindName("ThreadList");
			EmptyTextBlock = (TextBlock)((FrameworkElement)this).FindName("EmptyTextBlock");
		}
	}
}
