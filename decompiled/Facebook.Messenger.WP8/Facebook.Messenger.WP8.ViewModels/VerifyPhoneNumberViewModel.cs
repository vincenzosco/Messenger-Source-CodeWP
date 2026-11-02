using System;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials;
using Facebook.Services.Api.Methods.Login;
using Facebook.Services.Api.Methods.PhoneNumber;
using Facebook.Services.Api.Methods.PhoneNumberConfirmCode;
using Facebook.Services.Api.Methods.PhoneNumberRegistration;
using Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class VerifyPhoneNumberViewModel : FacebookMessengerViewModel
	{
		private string _phoneNumber;

		private string _regionCode;

		private string _phonePrefix;

		private string _normalizedPhoneNumber;

		private bool _isLoginFlow;

		private bool _isRegistrationFlow;

		private string _confirmCode;

		private RelayCommand _confirmPhoneNumberCommand;

		private RelayCommand _skipPhoneNumberCommand;

		private RelayCommand _sendCodeAgainCommand;

		public string PhoneNumber
		{
			get
			{
				return _phoneNumber ?? string.Empty;
			}
			set
			{
				Set(ref _phoneNumber, value, false, "PhoneNumber");
			}
		}

		public bool IsLoginFlow
		{
			get
			{
				return _isLoginFlow;
			}
			set
			{
				Set(ref _isLoginFlow, value, false, "IsLoginFlow");
			}
		}

		public bool IsRegistrationFlow
		{
			get
			{
				return _isRegistrationFlow;
			}
			set
			{
				Set(ref _isRegistrationFlow, value, false, "IsRegistrationFlow");
			}
		}

		public string ConfirmCode
		{
			get
			{
				return _confirmCode;
			}
			set
			{
				Set(ref _confirmCode, value, false, "ConfirmCode");
			}
		}

		public RelayCommand ConfirmPhoneNumberCommand
		{
			get
			{
				return _confirmPhoneNumberCommand ?? (_confirmPhoneNumberCommand = new RelayCommand(ExecuteConfirmPhoneNumberCommand));
			}
		}

		public RelayCommand SkipPhoneNumberCommand
		{
			get
			{
				return _skipPhoneNumberCommand ?? (_skipPhoneNumberCommand = new RelayCommand(SkipPhoneNumber));
			}
		}

		public RelayCommand SendCodeAgainCommand
		{
			get
			{
				return _sendCodeAgainCommand ?? (_sendCodeAgainCommand = new RelayCommand(ExecuteSendCodeAgainCommand));
			}
		}

		public VerifyPhoneNumberViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("phonenumber"))
			{
				PhoneNumber = PathUri.Params["phonenumber"];
			}
			if (PathUri.Params.ContainsKey("loginflow"))
			{
				IsLoginFlow = true;
			}
			if (PathUri.Params.ContainsKey("registrationflow"))
			{
				IsRegistrationFlow = true;
			}
			if (PathUri.Params.ContainsKey("phoneprefix"))
			{
				_phonePrefix = PathUri.Params["phoneprefix"];
			}
			if (PathUri.Params.ContainsKey("normalizedphonenumber"))
			{
				_normalizedPhoneNumber = PathUri.Params["normalizedphonenumber"];
			}
			if (PathUri.Params.ContainsKey("regioncode"))
			{
				_regionCode = PathUri.Params["regioncode"];
			}
		}

		private async void ExecuteConfirmPhoneNumberCommand()
		{
			if (IsLoading || string.IsNullOrWhiteSpace(ConfirmCode))
			{
				return;
			}
			if (!HasConnection)
			{
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				});
				return;
			}
			IsLoading = true;
			if (_isRegistrationFlow)
			{
				await RegistrationConfirmCode();
			}
			else
			{
				await PhoneNumberConfirmCode();
			}
			IsLoading = false;
		}

		private async Task RegistrationConfirmCode()
		{
			Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation.Request request = new Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation.Request(Context.GlobalSettings.DeviceIdentifier, ConfirmCode, _normalizedPhoneNumber);
			ExistingUserResponse response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation.Request, ExistingUserResponse>(request);
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
					error2 = Error.FromResponse(response, StringLibrary.MessageBox_Label_ConfirmationCodeInvalid, string.Empty);
				}
				Error error3 = error2;
				OnError(error3);
			}
			else if (!string.IsNullOrEmpty(response.AccountId) && !string.IsNullOrEmpty(response.FirstName) && !string.IsNullOrEmpty(response.LastName))
			{
				if (response.PartialUser)
				{
					await BypassLoginWithValidMessengerCredentials(response.AccountId);
				}
				else
				{
					NavigateToExistingUserPage(response.FirstName, response.LastName, response.ProfilePicUrl, _normalizedPhoneNumber);
				}
			}
			else
			{
				NavigateToUserRegistrationPage();
			}
		}

		private async Task PhoneNumberConfirmCode()
		{
			Facebook.Services.Api.Methods.PhoneNumberConfirmCode.Request request = new Facebook.Services.Api.Methods.PhoneNumberConfirmCode.Request
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				CancelToken = CancelTokenSource.Token,
				ConfirmCode = ConfirmCode
			};
			BoolResponse response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.PhoneNumberConfirmCode.Request, BoolResponse>(request);
			if (response != null && response.HasError())
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
					error2 = Error.FromResponse(response, StringLibrary.MessageBox_Label_ConfirmPhoneNumberFailed, string.Empty);
				}
				Error error3 = error2;
				OnError(error3);
			}
			else
			{
				Context.UpdatePhoneNumber(PhoneNumber);
				Context.UpdatePhoneNumberVerified();
				NavigateToNUXPage();
			}
		}

		private async Task BypassLoginWithValidMessengerCredentials(string accountId)
		{
			string machineId = Context.GlobalSettings.FacebookMachineId;
			string deviceId = Context.GlobalSettings.DeviceIdentifier;
			Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials.Request request = new Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials.Request(deviceId, accountId, machineId);
			Facebook.Services.Api.Methods.Login.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials.Request, Facebook.Services.Api.Methods.Login.Response>(request);
			if (response != null && !response.HasError())
			{
				Context.Login(response.Session);
				Context.SetIsPartialUser();
				Context.UpdatePhoneNumber(PhoneNumber);
				Context.UpdatePhoneNumberVerified();
				Navigator.Reset(FacebookUri.NuxOnMessengerUri);
			}
			else if (response != null && response.HasError() && response.Error.Code == ApiErrorCodes.MessengerRegistrationRequireLogin)
			{
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_BypassLogin2Fac
				});
				UriQuery uriQuery = new UriQuery();
				uriQuery.Add("username", _normalizedPhoneNumber);
				uriQuery.Add("phonenumber", PhoneNumber);
				uriQuery.Add("ispartialuser", "1");
				UriQuery query = uriQuery;
				Navigator.Navigate(new FacebookUri(UriId.Login, "", query));
			}
			else
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
					error2 = Error.FromResponse(response, StringLibrary.MessageBox_Label_BypassLoginFail, string.Empty);
				}
				Error error3 = error2;
				OnError(error3);
			}
		}

		private void NavigateToExistingUserPage(string firstName, string lastName, string photoUrl, string username = null, string userId = null)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("firstname", firstName);
			uriQuery.Add("lastName", lastName);
			uriQuery.Add("profilepicurl", photoUrl);
			uriQuery.Add("normalizedphonenumber", _normalizedPhoneNumber);
			uriQuery.Add("phonenumber", PhoneNumber);
			UriQuery uriQuery2 = uriQuery;
			if (!string.IsNullOrEmpty(username))
			{
				uriQuery2.Add("username", username);
			}
			if (!string.IsNullOrEmpty(userId))
			{
				uriQuery2.Add("userid", userId);
			}
			Navigator.Reset(FacebookUri.GetAppUri(UriId.ExistingUser, uriQuery2));
		}

		private async void ExecuteSendCodeAgainCommand()
		{
			if (IsLoading || string.IsNullOrWhiteSpace(PhoneNumber))
			{
				return;
			}
			if (!HasConnection)
			{
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				});
				return;
			}
			IsLoading = true;
			BoolResponse response;
			if (IsRegistrationFlow)
			{
				Facebook.Services.Api.Methods.PhoneNumberRegistration.Request request = new Facebook.Services.Api.Methods.PhoneNumberRegistration.Request(Context.GlobalSettings.DeviceIdentifier, _regionCode, _normalizedPhoneNumber);
				response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.PhoneNumberRegistration.Request, BoolResponse>(request);
			}
			else
			{
				Facebook.Services.Api.Methods.PhoneNumber.Request request2 = new Facebook.Services.Api.Methods.PhoneNumber.Request
				{
					AccessToken = Context.Session.Credentials.AccessToken,
					CancelToken = CancelTokenSource.Token,
					PhoneNumber = PhoneNumber
				};
				response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.PhoneNumber.Request, BoolResponse>(request2);
			}
			if (response != null && response.HasError())
			{
				if (response.Error.Code == ApiErrorCodes.PhoneNumberAlreadyVerified)
				{
					Context.UpdatePhoneNumberVerified();
					NavigateToNUXPage();
				}
				else
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
						error2 = Error.FromResponse(response, StringLibrary.MessageBox_Label_UpdatePhoneNumberFailed, string.Empty);
					}
					Error error3 = error2;
					error3.Code = (HasConnection ? (response.ErrorCode ?? 0) : 0);
					OnError(error3);
				}
			}
			IsLoading = false;
		}

		private void NavigateToUserRegistrationPage()
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("normalizedphonenumber", _normalizedPhoneNumber);
			uriQuery.Add("phonenumber", _phoneNumber);
			UriQuery query = uriQuery;
			Navigator.Reset(FacebookUri.GetAppUri(UriId.NewUserRegistration, query));
		}

		private void NavigateToNUXPage()
		{
			if (_isLoginFlow)
			{
				Navigator.Reset(FacebookUri.NuxOnMessengerUri);
				return;
			}
			Navigator.RemoveBackEntry();
			Navigator.GoBack();
		}

		public void SkipPhoneNumber()
		{
			if (_isLoginFlow)
			{
				Context.UpdatePhoneNumberSkipped();
				Context.UpdatePhoneNumber(PhoneNumber);
			}
			else if (!Context.GetPhoneNumberState().Equals(Context.PhoneNumberVerified))
			{
				Context.UpdatePhoneNumber(PhoneNumber);
			}
			NavigateToNUXPage();
		}
	}
}
