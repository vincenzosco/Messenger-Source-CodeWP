using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.Localization;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls.Debugging;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Pages;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.Utilities.SystemTrayManager;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Facebook.Services.Localization;
using Microsoft.Phone.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Controls;

public class MessengerApplicationFrame : TransitionFrame
{
	private static readonly Type[] NavigationProtectedPages = new Type[5]
	{
		typeof(PhoneNumberRegistrationViewModel),
		typeof(LoginViewModel),
		typeof(PhoneNumberViewModel),
		typeof(VerifyPhoneNumberViewModel),
		typeof(NuxOnMessengerViewModel)
	};

	private readonly Lazy<DebugOverlay> _debugOverlay = new Lazy<DebugOverlay>(() => new DebugOverlay());

	private readonly Lazy<NotificationDrawer> _notificationDrawer;

	private BaseConnectionBanner _connectionBanner;

	public SystemTrayManager SystemTrayManager { get; set; }

	public MessengerApplicationFrame()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected Obj, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected Obj, but got Unknown
		_notificationDrawer = new Lazy<NotificationDrawer>(CreateNotificationDrawer);
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
	}

	private NotificationDrawer CreateNotificationDrawer()
	{
		NotificationDrawer notificationDrawer = new NotificationDrawer();
		((FrameworkElement)notificationDrawer).Width = ((FrameworkElement)this).ActualWidth;
		NotificationDrawer notificationDrawer2 = notificationDrawer;
		notificationDrawer2.NotificationTapped += NotificationTapped;
		return notificationDrawer2;
	}

	private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		_debugOverlay.Value.Load();
		((FrameworkElement)(object)this).CallWithViewModel((MessengerApplicationViewModel vm) =>
		{
			vm.MessageReceived += OnMessageReceived;
			ConnectionBannerType getConnectionBannerTypeToDisplay = vm.GetConnectionBannerTypeToDisplay;
			if (_connectionBanner == null && getConnectionBannerTypeToDisplay != ConnectionBannerType.None)
			{
				CreateConnectionBanner();
			}
		});
	}

	private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
	{
		if (_debugOverlay.IsValueCreated)
		{
			_debugOverlay.Value.Unload();
		}
		if (_connectionBanner != null)
		{
			_connectionBanner.BannerAnimationStarted -= OnBannerAnimationStarted;
			_connectionBanner.BannerAnimationCompleted -= OnBannerAnimationCompleted;
		}
		((FrameworkElement)(object)this).CallWithViewModel((MessengerApplicationViewModel vm) =>
		{
			vm.MessageReceived -= OnMessageReceived;
		});
	}

	private static bool ShouldShowPopup(MailThread thread, MailMessage message, FrameworkElement page, FacebookContext context)
	{
		bool flag = context.IsAuthenticated();
		bool flag2 = UserUtilities.IsCurrentUser(message.Sender, context);
		bool flag3 = MuteHelper.IsThreadMuted(thread.MuteUntil);
		bool flag4 = IsThreadInView(page, thread);
		if (flag && !flag2 && !flag3 && !flag4)
		{
			return message.SourceIsPush;
		}
		return false;
	}

	private void CreateConnectionBanner()
	{
		((FrameworkElement)(object)this).CallWithViewModel((MessengerApplicationViewModel vm) =>
		{
			ColoredConnectionBanner coloredConnectionBanner = new ColoredConnectionBanner();
			((FrameworkElement)coloredConnectionBanner).DataContext = vm.ConnectionBannerViewModel;
			_connectionBanner = coloredConnectionBanner;
			_connectionBanner.BannerAnimationStarted += OnBannerAnimationStarted;
			_connectionBanner.BannerAnimationCompleted += OnBannerAnimationCompleted;
		});
	}

	private void OnBannerAnimationStarted(object sender, EventArgs eventArgs)
	{
		SystemTrayManager.AddOperationToStack(((object)this).GetType());
	}

	private void OnBannerAnimationCompleted(object sender, EventArgs eventArgs)
	{
		SystemTrayManager.RemoveOperationFromStack(((object)this).GetType());
	}

	private static bool IsThreadInView(FrameworkElement page, MailThread thread)
	{
		MessengerMainPage messengerMainPage = page as MessengerMainPage;
		MainViewModel mainViewModel = page.DataContext as MainViewModel;
		MessagesViewModel messagesViewModel = page.DataContext as MessagesViewModel;
		bool flag = mainViewModel != null && messengerMainPage != null;
		bool flag2 = messagesViewModel != null && messagesViewModel.Messages != null;
		bool flag3 = flag && mainViewModel.ViewingThreads && messengerMainPage.IsAtTopOfThreads();
		bool result = flag2 && messagesViewModel.MatchesCurrentThread(thread);
		if (!flag3)
		{
			return result;
		}
		return true;
	}

	private void NotificationTapped(object sender, NotificationViewModel vm)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		object obj = ((((ContentControl)this).Content is Page) ? ((FrameworkElement)(Page)((ContentControl)this).Content).DataContext : null);
		if (obj == null || !NavigationProtectedPages.Contains(obj.GetType()))
		{
			FacebookUri threadUri = FacebookUri.GetThreadUri(vm.ThreadId, vm.ThreadName);
			Navigator instance = ServiceLocator.Current.GetInstance<Navigator>();
			instance.NavigateThenPopBackStack(threadUri);
		}
	}

	private void OnMessageReceived(object sender, MessagePopupArgs args)
	{
		MailThread thread = args.Thread;
		MailMessage message = args.Message;
		FacebookContext context = args.Context;
		object content = ((ContentControl)this).Content;
		if (ShouldShowPopup(thread, message, (FrameworkElement)((content is Page) ? content : null), context))
		{
			ThreadSnippetInfo snippet = ThreadSnippetGenerator.GenerateSnippet(message, context);
			NotificationViewModel notification = new NotificationViewModel(message, snippet, thread, context);
			_notificationDrawer.Value.SetNotification(notification);
		}
	}

	public void TearDownFrame()
	{
		if (_connectionBanner != null)
		{
			_connectionBanner.PreserveBannerState();
		}
	}

	public void ResumeFrame()
	{
		if (_connectionBanner != null)
		{
			_connectionBanner.RestoreBannerState();
		}
	}
}
