using System;
using System.Windows;
using System.Windows.Controls;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.ViewModels;

public class MessengerApplicationViewModel : FacebookMessengerViewModel
{
	private readonly ApplicationState _applicationState;

	private readonly ConnectionBannerViewModel _connectionBannerViewModel;

	public ConnectionBannerType GetConnectionBannerTypeToDisplay => ConnectionBannerType.Colored;

	public ConnectionBannerViewModel ConnectionBannerViewModel => _connectionBannerViewModel;

	public event EventHandler<MessagePopupArgs> MessageReceived;

	public MessengerApplicationViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, ApplicationState applicationState, ConnectionBannerViewModel connectionBannerViewModel)
		: base(apiService, context, navigator, dispatcher, networkStatus)
	{
		_applicationState = applicationState;
		_connectionBannerViewModel = connectionBannerViewModel;
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheMarkedQueuedThreadsFailedSignal>(this, OnCacheMarkedQueuedThreadsFailedSignal);
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheMessagesUpdatedSignal>(this, OnMessageUpdatedSignal);
	}

	private void OnCacheMarkedQueuedThreadsFailedSignal(CacheMarkedQueuedThreadsFailedSignal signal)
	{
		if (!_applicationState.ActivatedFromMessageFailedToast)
		{
			Dispatcher.RunAsync(() =>
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected Obj, but got Unknown
				CustomMessageBox customMessageBox = new CustomMessageBox
				{
					Caption = StringLibrary.Message_Sending_Failed,
					Message = StringLibrary.Message_Sending_Failed_App_Exited_Description
				};
				TextBlock val = new TextBlock();
				val.Text = StringLibrary.AppBar_Ok;
				val.TextWrapping = (TextWrapping)2;
				customMessageBox.RightButtonContent = val;
				customMessageBox.IsFullScreen = false;
				CustomMessageBox customMessageBox2 = customMessageBox;
				customMessageBox2.Show();
			});
		}
	}

	private void OnMessageUpdatedSignal(CacheMessagesUpdatedSignal signal)
	{
		if (!signal.ShouldNotify || MessageReceived == null)
		{
			return;
		}
		foreach (MailMessage message in signal.Messages)
		{
			MessageReceived(this, new MessagePopupArgs
			{
				Thread = signal.Thread,
				Message = message,
				Context = Context
			});
		}
	}
}
