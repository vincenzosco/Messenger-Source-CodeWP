using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.ClearThreadPicture;
using Facebook.Services.Api.Methods.LeaveThread;
using Facebook.Services.Api.Methods.RemoveThreadParticipants;
using Facebook.Services.Api.Methods.SetThreadName;
using Facebook.Services.Api.Methods.SetThreadPicture;
using Facebook.Services.Common;
using Facebook.Services.Utilities;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class GroupDetailsViewModel : BaseThreadDetailsViewModel
	{
		private readonly IFileAccess _fileAccess;

		private ObservableCollection<FacebookUser> _members;

		private RelayCommand _nameConversationPageCommand;

		private RelayCommand _clearGroupNameCommand;

		private RelayCommand<string> _setThreadPictureCommand;

		private RelayCommand _clearThreadPictureCommand;

		private RelayCommand _addPeopleCommand;

		private RelayCommand<FacebookUser> _removeUserCommand;

		private RelayCommand _leaveThreadCommand;

		private RelayCommand<FacebookUser> _navigateToCanonicalThreadCommand;

		public ObservableCollection<FacebookUser> Members
		{
			get
			{
				return _members;
			}
			set
			{
				Set(ref _members, value, false, "Members");
			}
		}

		public RelayCommand NameConversationPageCommand => _nameConversationPageCommand ?? (_nameConversationPageCommand = new RelayCommand(ExecuteNameConversationPageCommand));

		public RelayCommand ClearGroupNameCommand => _clearGroupNameCommand ?? (_clearGroupNameCommand = new RelayCommand(ExecuteClearGroupNameCommand));

		public RelayCommand<string> SetThreadPictureCommand => _setThreadPictureCommand ?? (_setThreadPictureCommand = new RelayCommand<string>(ExecuteSetThreadPictureCommand));

		public RelayCommand ClearThreadPictureCommand => _clearThreadPictureCommand ?? (_clearThreadPictureCommand = new RelayCommand(ExecuteClearThreadPictureCommand));

		public RelayCommand AddPeopleCommand => _addPeopleCommand ?? (_addPeopleCommand = new RelayCommand(ExecuteAddPeopleCommand));

		public RelayCommand<FacebookUser> RemoveUserCommand => _removeUserCommand ?? (_removeUserCommand = new RelayCommand<FacebookUser>(ExecuteRemoveUserCommand));

		public RelayCommand LeaveThreadCommand => _leaveThreadCommand ?? (_leaveThreadCommand = new RelayCommand(ExecuteLeaveThreadCommand));

		public RelayCommand<FacebookUser> NavigateToCanonicalThreadCommand => _navigateToCanonicalThreadCommand ?? (_navigateToCanonicalThreadCommand = new RelayCommand<FacebookUser>(ExecuteNavigateToCanonicalThreadCommand));

		public GroupDetailsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, IFileAccess fileAccess)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_fileAccess = fileAccess;
		}

		private void ExecuteNameConversationPageCommand()
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("thread_id", Thread.Id);
			uriQuery.Add("thread_name", Thread.Name);
			UriQuery query = uriQuery;
			Navigator.Navigate(FacebookUri.GetAppUri(UriId.NameConversation, query));
		}

		private async void ExecuteClearGroupNameCommand()
		{
			if (IsLoading)
			{
				return;
			}
			IsLoading = true;
			Facebook.Services.Api.Methods.SetThreadName.Request request = new Facebook.Services.Api.Methods.SetThreadName.Request
			{
				ThreadId = Thread.Id,
				Name = string.Empty,
				AccessToken = Context.Session.Credentials.AccessToken,
				OfflineThreadingId = OfflineIdHelper.CreateOfflineThreadingId(Context)
			};
			BoolResponse response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.SetThreadName.Request, BoolResponse>(request);
			IsLoading = false;
			if (response.HasError())
			{
				Error error2;
				if (!HasConnection)
				{
					Error error = new Error();
					error.Message = StringLibrary.MessageBox_Label_NamingConversation_Error;
					error2 = error;
				}
				else
				{
					error2 = Error.FromResponse(response, StringLibrary.MessageBox_Label_NamingConversation_Error, string.Empty);
				}
				Error error3 = error2;
				error3.Code = (HasConnection ? (response.ErrorCode ?? 0) : 0);
				OnError(error3);
			}
		}

		private async void ExecuteSetThreadPictureCommand(string imagePath)
		{
			IsLoading = true;
			Facebook.Services.Api.Methods.SetThreadPicture.Request request = new Facebook.Services.Api.Methods.SetThreadPicture.Request
			{
				ThreadId = Thread.Id,
				AccessToken = Context.Session.Credentials.AccessToken,
				ImagePath = imagePath
			};
			if ((await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.SetThreadPicture.Request, Facebook.Services.Api.Methods.SetThreadPicture.Response>(request)).HasError())
			{
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_SetThreadPhotoFailed
				});
			}
			_fileAccess.DeleteFileAtPath(imagePath).DoNotWait();
			IsLoading = false;
		}

		private async void ExecuteClearThreadPictureCommand()
		{
			IsLoading = true;
			Facebook.Services.Api.Methods.ClearThreadPicture.Request request = new Facebook.Services.Api.Methods.ClearThreadPicture.Request
			{
				ThreadId = Thread.Id,
				AccessToken = Context.Session.Credentials.AccessToken
			};
			if ((await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.ClearThreadPicture.Request, BoolResponse>(request)).HasError())
			{
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_ClearThreadPhotoFailed
				});
			}
			IsLoading = false;
		}

		private void ExecuteAddPeopleCommand()
		{
			if (Thread != null)
			{
				Navigator.Navigate(FacebookUri.GetAddPeopleToGroupUri(Thread.Id, Thread.Participants));
			}
		}

		private async void ExecuteRemoveUserCommand(FacebookUser removedParticipant)
		{
			if (!IsLoading && removedParticipant != null)
			{
				IsLoading = true;
				Facebook.Services.Api.Methods.RemoveThreadParticipants.Request removeParticipantsRequest = new Facebook.Services.Api.Methods.RemoveThreadParticipants.Request(Thread.Id, new List<string> { removedParticipant.Id })
				{
					AccessToken = Context.Session.Credentials.AccessToken
				};
				BoolResponse response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.RemoveThreadParticipants.Request, BoolResponse>(removeParticipantsRequest);
				if (response.HasError() || !response.Succeeded)
				{
					MessageBox.Show(StringLibrary.Group_RemovingParticipants_Failed_Message, StringLibrary.Group_RemovingParticipants_Failed_Title, (MessageBoxButton)0);
				}
				else
				{
					Members.Remove(removedParticipant);
				}
				IsLoading = false;
			}
		}

		private async void ExecuteLeaveThreadCommand()
		{
			if (Thread == null)
			{
				return;
			}
			IsLoading = true;
			Facebook.Services.Api.Methods.LeaveThread.Request request = new Facebook.Services.Api.Methods.LeaveThread.Request
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				ThreadId = Thread.Id
			};
			BoolResponse response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.LeaveThread.Request, BoolResponse>(request);
			if (response.HasError())
			{
				if (string.IsNullOrEmpty(response.Error.Message))
				{
					response.Error.Message = StringLibrary.Group_Leaving_Failed_Message;
				}
				OnError(response.Error);
			}
			else
			{
				Navigator.GoBackToRoot();
			}
			IsLoading = false;
		}

		private void ExecuteNavigateToCanonicalThreadCommand(FacebookUser participant)
		{
			FacebookUri conversationUri = FacebookUri.GetConversationUri(participant.Id, participant.Name);
			Navigator.NavigateThenPopBackStack(conversationUri);
		}

		protected override void OnThreadUpdated()
		{
			CheckAndUpdateMembers();
		}

		private void CheckAndUpdateMembers()
		{
			if (!Members.IsNullOrEmpty())
			{
				List<string> list = (from p in Thread.Participants
					where !UserUtilities.IsCurrentUser(p, Context)
					select p.Id).ToList();
				List<string> list2 = Members.Select((FacebookUser m) => m.Id).ToList();
				IEnumerable<string> enumerable = list.Except(list2).Union(list2.Except(list));
				if (enumerable.IsNullOrEmpty())
				{
					return;
				}
			}
			UpdateMembers();
		}

		private void UpdateMembers()
		{
			IEnumerable<MailUser> collection = Thread.Participants.Where((MailUser p) => !UserUtilities.IsCurrentUser(p, Context));
			Members = new ObservableCollection<FacebookUser>(collection);
		}
	}
}
