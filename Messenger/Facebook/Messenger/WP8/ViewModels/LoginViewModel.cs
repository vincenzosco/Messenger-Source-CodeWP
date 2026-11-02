using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchSearchability;
using Facebook.Services.Api.Methods.Login;
using Facebook.Services.Http;
using GalaSoft.MvvmLight.Command;
using Google.PhoneNumbers;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class LoginViewModel : FacebookMessengerViewModel
	{
		public const string PasswordPropertyName = "Password";

		public const string UsernamePropertyName = "Username";

		public const string ApprovalCodePropertyName = "ApprovalCode";

		private readonly FacebookConfig _config;

		private readonly Facebook.Services.Http.Service _http;

		private readonly ICrypto _crypto;

		public bool UsingAuthCode;

		private bool _sessionExpired;

		private string _password;

		private string _username;

		private string _phoneNumber;

		private bool _isPartialUser;

		private string _approvalCode;

		private RelayCommand _loginCommand;

		private RelayCommand _resendApprovalCodeCommand;

		private RelayCommand _navigateToRegisterCommand;

		private RelayCommand _navigateToPasswordRecoveryCommand;

		public bool SessionExpired
		{
			get
			{
				return _sessionExpired;
			}
			set
			{
				Set(ref _sessionExpired, value, false, "SessionExpired");
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				Set("Password", ref _password, value);
			}
		}

		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				Set("Username", ref _username, value);
			}
		}

		public string ApprovalCode
		{
			get
			{
				return _approvalCode;
			}
			set
			{
				Set("ApprovalCode", ref _approvalCode, value);
			}
		}

		public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(ExecuteLoginCommand));

		public RelayCommand ResendApprovalCodeCommand => _resendApprovalCodeCommand ?? (_resendApprovalCodeCommand = new RelayCommand(ExecuteResendApprovalCodeCommand));

		public RelayCommand NavigateToRegisterCommand => _navigateToRegisterCommand ?? (_navigateToRegisterCommand = new RelayCommand(ExecuteNavigateToRegisterCommand));

		public RelayCommand NavigateToPasswordRecoveryCommand => _navigateToPasswordRecoveryCommand ?? (_navigateToPasswordRecoveryCommand = new RelayCommand(ExecuteNavigateToPasswordRecoveryCommand));

		public LoginViewModel(Facebook.Services.Api.Service apiService, Facebook.Services.Http.Service http, FacebookContext context, Navigator navigation, ICrypto crypto, FacebookConfig config, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigation, dispatcher, networkStatus)
		{
			_http = http;
			_crypto = crypto;
			_config = config;
			PreloadPhoneLib();
		}

		private static void PreloadPhoneLib()
		{
			Task.Run(() =>
			{
				Thread.Sleep(1000);
				PhoneNumberUtil.getInstance();
			});
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("username"))
			{
				_username = PathUri.Params["username"];
			}
			if (PathUri.Params.ContainsKey("password"))
			{
				Password = PathUri.Params["password"];
			}
			if (PathUri.Params.ContainsKey("approval"))
			{
				UsingAuthCode = true;
			}
			if (PathUri.Params.ContainsKey("expired"))
			{
				SessionExpired = true;
			}
			if (PathUri.Params.ContainsKey("phonenumber"))
			{
				_phoneNumber = PathUri.Params["phonenumber"];
			}
			if (PathUri.Params.ContainsKey("ispartialuser"))
			{
				_isPartialUser = true;
			}
		}

		public override void SaveState(Dictionary<string, object> state)
		{
			state["email"] = Username;
			state["use_auth"] = UsingAuthCode;
		}

		public override void LoadState(Dictionary<string, object> state)
		{
			Username = GetStateValue(state, "email", string.Empty);
			UsingAuthCode = GetStateValue(state, "use_auth", false);
		}

		private async void ExecuteLoginCommand()
		{
			if (string.IsNullOrWhiteSpace(Username) || (string.IsNullOrWhiteSpace(Password) && string.IsNullOrWhiteSpace(ApprovalCode)) || (UsingAuthCode && string.IsNullOrWhiteSpace(ApprovalCode)))
			{
				return;
			}
			if (!HasConnection)
			{
				Password = null;
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				});
				return;
			}
			Facebook.Services.Api.Methods.Login.Request request = new Facebook.Services.Api.Methods.Login.Request(password: UsingAuthCode ? ApprovalCode : Password, machineId: Context.GlobalSettings.FacebookMachineId, name: Username.Trim());
			IsLoading = true;
			Facebook.Services.Api.Methods.Login.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.Login.Request, Facebook.Services.Api.Methods.Login.Response>(request);
			if (!string.IsNullOrWhiteSpace(response.MachineId))
			{
				Context.GlobalSettings.FacebookMachineId = response.MachineId;
				_http.SetMachineId(response.MachineId);
			}
			if (response.HasError())
			{
				ApprovalCode = null;
				if (response.LoginState == Facebook.Services.Api.Methods.Login.Response.State.NotAcceptable)
				{
					NavigateToLoginApproval();
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
						error2 = Error.FromResponse(response, StringLibrary.MessageBox_Label_LoginFailed, StringLibrary.MessageBox_Title_LoginFailed);
					}
					Error error3 = error2;
					OnError(error3);
				}
				if (!UsingAuthCode)
				{
					Password = null;
				}
				IsLoading = false;
				return;
			}
			Context.Login(response.Session);
			if (!string.IsNullOrEmpty(_phoneNumber))
			{
				Context.UpdatePhoneNumber(_phoneNumber);
				Context.UpdatePhoneNumberVerified();
			}
			if (_isPartialUser)
			{
				Context.SetIsPartialUser();
			}
			PhoneInfo mobilePhone = PhoneNumberUtility.GetBroadestPrivacyPhoneInfo(response.PhoneInfos);
			UriQuery param = new UriQuery { { "loginflow", "true" } };
			if (mobilePhone != null)
			{
				Context.UpdatePhoneNumber($"+{mobilePhone.CountryCode} {mobilePhone.Number}");
				param.Add("phonenumber", mobilePhone.Number);
				param.Add("phoneprefix", mobilePhone.CountryCode);
				param.Add("privacy", mobilePhone.Privacy);
			}
			Facebook.Services.Api.Methods.FetchSearchability.Request privacyRequest = new Facebook.Services.Api.Methods.FetchSearchability.Request
			{
				AccessToken = Context.Session.Credentials.AccessToken
			};
			PhoneSearchabilityTypes searchableByEveryone = (await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchSearchability.Request, Facebook.Services.Api.Methods.FetchSearchability.Response>(privacyRequest)).Searchability;
			param.Add("searchability", searchableByEveryone.ToString());
			string state = Context.GetPhoneNumberState();
			if (state != null && state.Equals(Context.PhoneNumberVerified))
			{
				Navigator.Reset(FacebookUri.NuxOnMessengerUri);
			}
			else
			{
				Navigator.Reset(FacebookUri.GetAppUri(UriId.PhoneNumber, param));
			}
		}

		private async void ExecuteResendApprovalCodeCommand()
		{
			if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
			{
				return;
			}
			if (!HasConnection)
			{
				Password = null;
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				});
				return;
			}
			Facebook.Services.Api.Methods.Login.Request request = new Facebook.Services.Api.Methods.Login.Request(password: Password, machineId: Context.GlobalSettings.FacebookMachineId, name: Username.Trim());
			IsLoading = true;
			Facebook.Services.Api.Methods.Login.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.Login.Request, Facebook.Services.Api.Methods.Login.Response>(request);
			if (!string.IsNullOrWhiteSpace(response.MachineId))
			{
				Context.GlobalSettings.FacebookMachineId = response.MachineId;
				_http.SetMachineId(response.MachineId);
			}
			if (response.HasError() && response.LoginState != Facebook.Services.Api.Methods.Login.Response.State.NotAcceptable)
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
					error2 = Error.FromResponse(response, StringLibrary.MessageBox_Label_LoginFailed, string.Empty);
				}
				Error error3 = error2;
				OnError(error3);
			}
			IsLoading = false;
		}

		private void ExecuteNavigateToRegisterCommand()
		{
			UriBuilder uriBuilder = new UriBuilder(_config.MobileApiUrl);
			StringBuilder stringBuilder = new StringBuilder();
			string deviceIdentifier = Context.GlobalSettings.DeviceIdentifier;
			stringBuilder.Append("cid=").Append(_config.ApplicationId);
			stringBuilder.Append("&id_hash=").Append(_crypto.ToMD5HexString(deviceIdentifier));
			stringBuilder.Append("&locale=").Append(CultureInfo.CurrentUICulture.Name.Replace('-', '_'));
			uriBuilder.Path = "r.php";
			uriBuilder.Query = stringBuilder.ToString();
			Navigator.Navigate(uriBuilder.Uri);
		}

		private void ExecuteNavigateToPasswordRecoveryCommand()
		{
			UriBuilder uriBuilder = new UriBuilder(_config.MobileApiUrl);
			uriBuilder.Path = "/recover/initiate";
			Navigator.Navigate(uriBuilder.Uri);
		}

		private void NavigateToLoginApproval()
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("username", Username);
			uriQuery.Add("password", Password);
			uriQuery.Add("approval", "true");
			uriQuery.Add("phonenumber", _phoneNumber);
			UriQuery query = uriQuery;
			Navigator.Navigate(FacebookUri.GetAppUri(UriId.LoginApproval, query));
		}
	}
}
