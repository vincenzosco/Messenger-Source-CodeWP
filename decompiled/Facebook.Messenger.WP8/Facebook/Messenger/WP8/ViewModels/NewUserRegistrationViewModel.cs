using System;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.CreateNewUserAccount;
using Facebook.Services.Api.Methods.Login;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels;

public class NewUserRegistrationViewModel : FacebookMessengerViewModel
{
	private string _normalizedPhoneNumber;

	private string _phoneNumber;

	private string _firstName;

	private string _lastName;

	private Error _error;

	private RelayCommand _createAccountCommand;

	public string FirstName
	{
		get
		{
			return _firstName;
		}
		set
		{
			Set(ref _firstName, value, broadcast: false, "FirstName");
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
			Set(ref _lastName, value, broadcast: false, "LastName");
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
			Set(ref _error, value, broadcast: false, "Error");
		}
	}

	public RelayCommand CreateAccountCommand => _createAccountCommand ?? (_createAccountCommand = new RelayCommand(ExecuteCreateAccountCommand));

	public NewUserRegistrationViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
		: base(apiService, context, navigator, dispatcher, networkStatus)
	{
	}

	public override void SetPath(Uri uri)
	{
		base.SetPath(uri);
		if (PathUri.Params.ContainsKey("normalizedphonenumber"))
		{
			_normalizedPhoneNumber = PathUri.Params["normalizedphonenumber"];
		}
		if (PathUri.Params.ContainsKey("firstname"))
		{
			_firstName = PathUri.Params["firstname"];
		}
		if (PathUri.Params.ContainsKey("lastName"))
		{
			_lastName = PathUri.Params["lastName"];
		}
		if (PathUri.Params.ContainsKey("phonenumber"))
		{
			_phoneNumber = PathUri.Params["phonenumber"];
		}
	}

	private async void ExecuteCreateAccountCommand()
	{
		if (IsLoading || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
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
		Facebook.Services.Api.Methods.CreateNewUserAccount.Request request = new Facebook.Services.Api.Methods.CreateNewUserAccount.Request(machineId: Context.GlobalSettings.FacebookMachineId, deviceId: Context.GlobalSettings.DeviceIdentifier, firstName: FirstName, lastName: LastName, phoneNumber: _normalizedPhoneNumber);
		Facebook.Services.Api.Methods.Login.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.CreateNewUserAccount.Request, Facebook.Services.Api.Methods.Login.Response>(request);
		if (response.HasError())
		{
			Error = (HasConnection ? Error.FromResponse(response, StringLibrary.MessageBox_Label_LoginFailed, StringLibrary.MessageBox_Title_LoginFailed) : new Error
			{
				Message = StringLibrary.MessageBox_Label_NoInternet
			});
			IsLoading = false;
		}
		else
		{
			Context.Login(response.Session);
			Context.SetIsPartialUser();
			Context.UpdatePhoneNumber(_phoneNumber);
			Context.UpdatePhoneNumberVerified();
			Navigator.Reset(FacebookUri.CreateProfilePic);
		}
	}
}
