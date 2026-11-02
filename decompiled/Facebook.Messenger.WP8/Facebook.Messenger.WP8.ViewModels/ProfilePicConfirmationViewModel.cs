using System;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.UploadProfilePicture;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ProfilePicConfirmationViewModel : FacebookMessengerViewModel
	{
		private string _profilePicUrl;

		private Uri _profilePicUri;

		private Error _error;

		private RelayCommand _confirmProfilePicCommand;

		public Uri ProfilePicUri
		{
			get
			{
				return _profilePicUri;
			}
			set
			{
				Set(ref _profilePicUri, value, false, "ProfilePicUri");
			}
		}

		public Error Error
		{
			get
			{
				return _error;
			}
			set
			{
				Set(ref _error, value, false, "Error");
			}
		}

		public RelayCommand ConfirmProfilePicCommand
		{
			get
			{
				return _confirmProfilePicCommand ?? (_confirmProfilePicCommand = new RelayCommand(ExecuteConfirmProfilePicCommand));
			}
		}

		public ProfilePicConfirmationViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("profilepicurl"))
			{
				_profilePicUrl = PathUri.Params["profilepicurl"];
				Uri result;
				Uri.TryCreate(_profilePicUrl, UriKind.Absolute, out result);
				ProfilePicUri = result;
			}
		}

		private async void ExecuteConfirmProfilePicCommand()
		{
			if (IsLoading)
			{
				return;
			}
			if (!HasConnection)
			{
				Error = new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				};
				return;
			}
			IsLoading = true;
			Facebook.Services.Api.Methods.UploadProfilePicture.Request request = new Facebook.Services.Api.Methods.UploadProfilePicture.Request
			{
				ImageUrl = _profilePicUrl,
				AccessToken = Context.Session.Credentials.AccessToken,
				CancelToken = CancelTokenSource.Token
			};
			Facebook.Services.Api.Methods.UploadProfilePicture.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.UploadProfilePicture.Request, Facebook.Services.Api.Methods.UploadProfilePicture.Response>(request);
			if (response.HasError())
			{
				Error = (HasConnection ? Error.FromResponse(response, StringLibrary.MessageBox_Label_UploadProfilePic_Failed, string.Empty) : new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				});
			}
			else
			{
				Navigator.Reset(FacebookUri.SyncContacts);
			}
			IsLoading = false;
		}
	}
}
