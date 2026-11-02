using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Presence;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ActiveUsersViewModel : FacebookMessengerViewModel
	{
		private readonly Facebook.Services.Presence.Service _presence;

		private ObservableCollection<FacebookUser> _activeUsersToDisplay;

		private bool _isChatEnabled;

		public ObservableCollection<FacebookUser> ActiveUsers
		{
			get
			{
				return _activeUsersToDisplay;
			}
			set
			{
				Set(ref _activeUsersToDisplay, value, false, "ActiveUsers");
			}
		}

		public bool IsChatEnabled
		{
			get
			{
				return _isChatEnabled;
			}
			set
			{
				if (Set(ref _isChatEnabled, value, false, "IsChatEnabled"))
				{
					Context.SetChatEnabled(_isChatEnabled);
					GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ChatSettingSignal(IsChatEnabled));
				}
			}
		}

		public ActiveUsersViewModel(Facebook.Services.Presence.Service presence, Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_presence = presence;
			_isChatEnabled = Context.IsChatEnabled();
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<FriendsUpdatedSignal>(this, OnFriendsUpdatedSignal);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<OnlineUserSignal>(this, OnPresenceUpdatedSignal);
		}

		private void OnFriendsUpdatedSignal(FriendsUpdatedSignal obj)
		{
			UpdatePresence().DoNotWait();
		}

		private void OnPresenceUpdatedSignal(OnlineUserSignal obj)
		{
			UpdateListOfActiveUsers(obj.OnlineUsers);
		}

		public async Task UpdatePresence()
		{
			UpdateListOfActiveUsers(await _presence.GetOnlineUsers());
		}

		public bool LaunchUserThread(FacebookUser user)
		{
			FacebookUri conversationUri = FacebookUri.GetConversationUri(user.Id, user.Name);
			return Navigator.Navigate(conversationUri);
		}

		private void UpdateListOfActiveUsers(IEnumerable<FacebookUser> updatedActiveUsers)
		{
			ActiveUsers = new ObservableCollection<FacebookUser>(updatedActiveUsers);
		}
	}
}
