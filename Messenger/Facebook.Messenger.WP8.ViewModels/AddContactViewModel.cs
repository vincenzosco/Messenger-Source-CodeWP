using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchContactsById;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Tasks;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class AddContactViewModel : BasePhoneNumberViewModel
	{
		public enum FindContactState
		{
			AddContact,
			MatchedExistingUser,
			MatchedNewUser,
			InviteContactToMessenger
		}

		private const string InviteUrl = "http://fb.me/getmessenger";

		private FindContactState _contactState;

		private FacebookUser _matchingUser;

		private readonly PhoneContactsManager _phoneContactsManager;

		private RelayCommand _findContactCommand;

		private RelayCommand _sendMessageCommand;

		private RelayCommand _sendInviteCommand;

		private RelayCommand _removeContactCommand;

		public FindContactState ContactState
		{
			get
			{
				return _contactState;
			}
			set
			{
				Set(ref _contactState, value, false, "ContactState");
			}
		}

		public FacebookUser MatchingUser
		{
			get
			{
				return _matchingUser;
			}
			set
			{
				Set(ref _matchingUser, value, false, "MatchingUser");
			}
		}

		public bool MatchingUserIsSelf
		{
			get
			{
				return MatchingUser.Id == Context.Session.UserId;
			}
		}

		public RelayCommand FindContactCommand
		{
			get
			{
				return _findContactCommand ?? (_findContactCommand = new RelayCommand(ExecuteFindContactCommand));
			}
		}

		public RelayCommand SendMessageCommand
		{
			get
			{
				return _sendMessageCommand ?? (_sendMessageCommand = new RelayCommand(ExecuteSendMessageCommand));
			}
		}

		public RelayCommand SendInviteCommand
		{
			get
			{
				return _sendInviteCommand ?? (_sendInviteCommand = new RelayCommand(ExecuteSendInviteCommand));
			}
		}

		public RelayCommand RemoveContactCommand
		{
			get
			{
				return _removeContactCommand ?? (_removeContactCommand = new RelayCommand(ExecuteRemoveContactCommand));
			}
		}

		public AddContactViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, PhoneContactsManager phoneContactsManager)
			: base(apiService, context, navigator, dispatcher, networkStatus, true)
		{
			_phoneContactsManager = phoneContactsManager;
		}

		private async void ExecuteFindContactCommand()
		{
			if (IsLoading || string.IsNullOrEmpty(MyPhoneNumber) || ValidatedCountryCode == null)
			{
				return;
			}
			IsLoading = true;
			Facebook.Services.Api.Methods.FetchContactsById.Response response = await _phoneContactsManager.AddPhoneContactAsync(MyPhoneNumber, ValidatedCountryCode.RegionCode);
			if (response == null || response.HasError())
			{
				Error error2;
				if (!HasConnection)
				{
					Error error = new Error();
					error.Message = StringLibrary.MessageBox_Label_NoInternet;
					error2 = error;
				}
				else
				{
					error2 = Error.FromResponse(response, StringLibrary.MessageBox_Title_AddContactFailed, StringLibrary.MessageBox_Title_AddContactFailed);
				}
				Error error3 = error2;
				OnError(error3);
			}
			else if (response.FetchedUsers != null && response.FetchedUsers.Count > 0)
			{
				MatchingUser = response.FetchedUsers[0];
				ContactState = ((!response.FriendsUpdated) ? FindContactState.MatchedExistingUser : FindContactState.MatchedNewUser);
			}
			else
			{
				ContactState = FindContactState.InviteContactToMessenger;
			}
			IsLoading = false;
		}

		private async void ExecuteRemoveContactCommand()
		{
			if (!IsLoading && MatchingUser != null)
			{
				IsLoading = true;
				await _phoneContactsManager.DeletePhoneContactAsync(MatchingUser);
				IsLoading = false;
				Navigator.GoBack();
			}
		}

		private void ExecuteSendInviteCommand()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			SmsComposeTask val = new SmsComposeTask();
			val.To = MyPhoneNumber;
			val.Body = string.Format(StringLibrary.Invite_Contact_ToMessenger, "http://fb.me/getmessenger");
			SmsComposeTask val2 = val;
			val2.Show();
			Navigator.GoBack();
		}

		private void ExecuteSendMessageCommand()
		{
			if (MatchingUser != null)
			{
				FacebookUri conversationUri = FacebookUri.GetConversationUri(MatchingUser.Id, MatchingUser.Name);
				Navigator.NavigateThenPopBackStack(conversationUri);
			}
		}
	}
}
