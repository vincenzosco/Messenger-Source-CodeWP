using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.CreateGroup;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class GroupComposerAddUsersViewModel : GroupContactsViewModel
	{
		private string _threadImageUri;

		private string _groupName;

		private ObservableCollection<FacebookUser> _selectedUsers;

		private bool _isCreatingGroup;

		private Facebook.Messenger.WP8.AudioController.Service _audioService;

		protected RelayCommand<FacebookUser> _userDeselectedCommand;

		protected RelayCommand _createGroupCommand;

		public ObservableCollection<FacebookUser> SelectedUsers
		{
			get
			{
				return _selectedUsers;
			}
			set
			{
				Set(ref _selectedUsers, value, false, "SelectedUsers");
			}
		}

		public bool IsCreatingGroup
		{
			get
			{
				return _isCreatingGroup;
			}
			set
			{
				Set(ref _isCreatingGroup, value, false, "IsCreatingGroup");
			}
		}

		private Facebook.Messenger.WP8.AudioController.Service AudioService
		{
			get
			{
				return _audioService ?? (_audioService = ServiceLocator.Current.GetInstance<Facebook.Messenger.WP8.AudioController.Service>());
			}
		}

		public RelayCommand<FacebookUser> UserDeselectedCommand
		{
			get
			{
				return _userDeselectedCommand ?? (_userDeselectedCommand = new RelayCommand<FacebookUser>(ExecuteRemoveUser));
			}
		}

		public RelayCommand CreateGroupCommand
		{
			get
			{
				return _createGroupCommand ?? (_createGroupCommand = new RelayCommand(ExecuteCreateGroup));
			}
		}

		public GroupComposerAddUsersViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, ContactSyncFeatureEnabledProvider contactSyncFeatureEnabledProvider, PhoneContactsManager phoneContactsManager)
			: base(apiService, context, navigator, dispatcher, networkStatus, contactSyncFeatureEnabledProvider, phoneContactsManager)
		{
			IsSearching = true;
			SelectedUsers = new ObservableCollection<FacebookUser>();
			_contactsIdsToExclude = new HashSet<string>();
			MailUser mailUser = Navigator.GetAndClearNavigationData() as MailUser;
			if (mailUser != null)
			{
				AddUser(mailUser);
			}
		}

		private void AddUser(FacebookUser user)
		{
			SelectedUsers.Add(user);
			_contactsIdsToExclude.Add(user.Id);
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("groupName"))
			{
				_groupName = PathUri.Params["groupName"];
			}
			if (PathUri.Params.ContainsKey("threadImageUri"))
			{
				_threadImageUri = PathUri.Params["threadImageUri"];
			}
		}

		protected override void LoadContacts()
		{
		}

		public override void OnUserSelected(UserSelectedEventArgs args)
		{
			if (args.SelectedFacebookUsers == null || !args.SelectedFacebookUsers.Any())
			{
				return;
			}
			foreach (FacebookUser selectedFacebookUser in args.SelectedFacebookUsers)
			{
				if (!_contactsIdsToExclude.Contains(selectedFacebookUser.Id))
				{
					AddUser(selectedFacebookUser);
				}
			}
		}

		private void ExecuteRemoveUser(FacebookUser user)
		{
			if (!IsCreatingGroup)
			{
				SelectedUsers.Remove(user);
				_contactsIdsToExclude.Remove(user.Id);
			}
		}

		private async void ExecuteCreateGroup()
		{
			if (!IsCreatingGroup)
			{
				IsCreatingGroup = true;
				Facebook.Services.Api.Methods.CreateGroup.Request request = new Facebook.Services.Api.Methods.CreateGroup.Request(_groupName, SelectedUsers.Select((FacebookUser user) => user.Id).ToList())
				{
					AccessToken = Context.Session.Credentials.AccessToken
				};
				if (!string.IsNullOrWhiteSpace(_threadImageUri))
				{
					request.ImagePath = FileUtil.ConvertUrlToPath(_threadImageUri);
				}
				AudioService.PlayNewGroupCreatedTap();
				Facebook.Services.Api.Methods.CreateGroup.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.CreateGroup.Request, Facebook.Services.Api.Methods.CreateGroup.Response>(request);
				IsCreatingGroup = false;
				if (response.HasError())
				{
					OnError(new Error
					{
						Message = StringLibrary.MessageBox_Label_CreateGroupFailed
					});
				}
				else
				{
					Navigator.GoBackToRoot();
					GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new GroupPinnedSignal(response.Thread.Id));
				}
			}
		}
	}
}
