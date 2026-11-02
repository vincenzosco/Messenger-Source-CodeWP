using System;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.AddPinThread;
using Facebook.Services.Utilities;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class GroupComposerViewModel : FacebookMessengerViewModel
	{
		private Uri _threadImageUri;

		private string _groupName;

		private ThreadViewModel _thread;

		private MailUser _createWithUser;

		public Uri ThreadImageUri
		{
			get
			{
				return _threadImageUri;
			}
			set
			{
				Set(ref _threadImageUri, value, false, "ThreadImageUri");
			}
		}

		public string GroupName
		{
			get
			{
				return _groupName;
			}
			set
			{
				Set(ref _groupName, value, false, "GroupName");
			}
		}

		public ThreadViewModel Thread
		{
			get
			{
				return _thread;
			}
			set
			{
				Set(ref _thread, value, false, "Thread");
			}
		}

		public MailUser CreateWithUser
		{
			get
			{
				return _createWithUser;
			}
			set
			{
				Set(ref _createWithUser, value, false, "CreateWithUser");
			}
		}

		public bool HasExistingThread => Thread != null;

		public GroupComposerViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			object andClearNavigationData = Navigator.GetAndClearNavigationData();
			Thread = andClearNavigationData as ThreadViewModel;
			if (Thread == null)
			{
				CreateWithUser = andClearNavigationData as MailUser;
			}
		}

		public async Task<bool> ProceedToNextState()
		{
			if (HasExistingThread)
			{
				string groupName = (GroupName ?? string.Empty).Trim();
				Facebook.Services.Api.Methods.AddPinThread.Request request = new Facebook.Services.Api.Methods.AddPinThread.Request(Thread.Id)
				{
					AccessToken = Context.Session.Credentials.AccessToken,
					NewName = groupName,
					ImagePath = FileUtil.ConvertUrlToPath((ThreadImageUri != null) ? ThreadImageUri.ToString() : null),
					OfflineThreadingId = OfflineIdHelper.CreateOfflineThreadingId(Context)
				};
				Facebook.Services.Api.Methods.AddPinThread.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.AddPinThread.Request, Facebook.Services.Api.Methods.AddPinThread.Response>(request);
				if (response.HasError())
				{
					OnError(new Error
					{
						Message = StringLibrary.MessageBox_Label_PinGroupFailed
					});
					return false;
				}
				Navigator.GoBackToRoot();
				string threadId = ((response.Thread != null) ? response.Thread.Id : Thread.Id);
				GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new GroupPinnedSignal(threadId));
			}
			else
			{
				FacebookUri groupComposerAddUsersUri = FacebookUri.GetGroupComposerAddUsersUri(GroupName, (ThreadImageUri == null) ? null : ThreadImageUri.ToString());
				Navigator.Navigate(groupComposerAddUsersUri, CreateWithUser);
			}
			return true;
		}
	}
}
