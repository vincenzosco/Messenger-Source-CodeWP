using System;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ExistingUserViewModel : FacebookMessengerViewModel
	{
		private string _firstName;

		private string _lastName;

		private string _userId;

		private Uri _profilePicUrl;

		private string _normalizedPhoneNumber;

		private string _phoneNumber;

		private RelayCommand _logInAsUserCommand;

		private RelayCommand _createNewAccountCommand;

		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				Set(ref _firstName, value, false, "FirstName");
			}
		}

		public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				Set(ref _lastName, value, false, "LastName");
			}
		}

		private string Username { get; set; }

		public string UserId
		{
			get
			{
				return _userId;
			}
			set
			{
				Set(ref _userId, value, false, "UserId");
			}
		}

		public Uri ProfilePicUrl
		{
			get
			{
				return _profilePicUrl;
			}
			set
			{
				Set(ref _profilePicUrl, value, false, "ProfilePicUrl");
			}
		}

		public RelayCommand LogInAsUserCommand
		{
			get
			{
				return _logInAsUserCommand ?? (_logInAsUserCommand = new RelayCommand(ExecuteLogInAsUserCommand));
			}
		}

		public RelayCommand CreateNewAccountCommand
		{
			get
			{
				return _createNewAccountCommand ?? (_createNewAccountCommand = new RelayCommand(ExecuteCreateNewUserCommand));
			}
		}

		public ExistingUserViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("firstname"))
			{
				FirstName = PathUri.Params["firstname"];
			}
			if (PathUri.Params.ContainsKey("lastName"))
			{
				LastName = PathUri.Params["lastName"];
			}
			if (PathUri.Params.ContainsKey("username"))
			{
				Username = PathUri.Params["username"];
			}
			if (PathUri.Params.ContainsKey("userid"))
			{
				UserId = PathUri.Params["userid"];
			}
			if (PathUri.Params.ContainsKey("profilepicurl"))
			{
				ProfilePicUrl = new Uri(PathUri.Params["profilepicurl"], UriKind.Absolute);
			}
			if (PathUri.Params.ContainsKey("normalizedphonenumber"))
			{
				_normalizedPhoneNumber = PathUri.Params["normalizedphonenumber"];
			}
			if (PathUri.Params.ContainsKey("phonenumber"))
			{
				_phoneNumber = PathUri.Params["phonenumber"];
			}
		}

		private void ExecuteLogInAsUserCommand()
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("username", Username);
			uriQuery.Add("phonenumber", _phoneNumber);
			UriQuery query = uriQuery;
			Navigator.Navigate(new FacebookUri(UriId.Login, "", query));
		}

		private void ExecuteCreateNewUserCommand()
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("normalizedphonenumber", _normalizedPhoneNumber);
			uriQuery.Add("firstname", _firstName);
			uriQuery.Add("lastName", _lastName);
			uriQuery.Add("phonenumber", _phoneNumber);
			UriQuery query = uriQuery;
			Navigator.Navigate(FacebookUri.GetAppUri(UriId.NewUserRegistration, query));
		}
	}
}
