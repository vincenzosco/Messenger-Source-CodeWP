using System;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.SetThreadName;
using Facebook.Services.Common;
using Facebook.Services.Utilities;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class NameConversationViewModel : FacebookMessengerViewModel
	{
		public const int MaxNameLength = 100;

		private string _threadId;

		private string _threadName;

		private RelayCommand _submitNameCommand;

		public string ThreadName
		{
			get
			{
				return _threadName;
			}
			set
			{
				if (value != null)
				{
					value = value.Trim();
					if (value.Length > 100)
					{
						value = value.Substring(0, 100);
					}
				}
				Set(ref _threadName, value, false, "ThreadName");
			}
		}

		public string OriginalThreadName { get; set; }

		public RelayCommand SubmitNameCommand => _submitNameCommand ?? (_submitNameCommand = new RelayCommand(NameConversation));

		public NameConversationViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("thread_id"))
			{
				_threadId = PathUri.Params["thread_id"];
			}
			if (PathUri.Params.ContainsKey("thread_name"))
			{
				OriginalThreadName = PathUri.Params["thread_name"];
				ThreadName = OriginalThreadName;
			}
		}

		public async void NameConversation()
		{
			if (IsLoading)
			{
				return;
			}
			IsLoading = true;
			Facebook.Services.Api.Methods.SetThreadName.Request request = new Facebook.Services.Api.Methods.SetThreadName.Request
			{
				ThreadId = _threadId,
				Name = ThreadName,
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
			NavigateBackCommand.Execute(null);
		}
	}
}
