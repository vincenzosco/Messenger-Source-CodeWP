using System;
using System.Text.RegularExpressions;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.PhoneNumberRegistration;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Command;
using Google.PhoneNumbers;

namespace Facebook.Messenger.WP8.ViewModels;

public class PhoneNumberRegistrationViewModel : BasePhoneNumberViewModel
{
	private bool _sessionExpired;

	private RelayCommand _registerPhoneNumberCommand;

	private RelayCommand _navigateToLoginPageCommand;

	private Error _error;

	public bool SessionExpired
	{
		get
		{
			return _sessionExpired;
		}
		set
		{
			Set(ref _sessionExpired, value, broadcast: false, "SessionExpired");
		}
	}

	public RelayCommand RegisterPhoneNumberCommand => _registerPhoneNumberCommand ?? (_registerPhoneNumberCommand = new RelayCommand(ExecuteRegisterPhoneNumberCommand));

	public RelayCommand NavigateToLoginPageCommand => _navigateToLoginPageCommand ?? (_navigateToLoginPageCommand = new RelayCommand(ExecuteNavigateToLoginPageCommand));

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

	public PhoneNumberRegistrationViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
		: base(apiService, context, navigator, dispatcher, networkStatus)
	{
	}

	public override void SetPath(Uri uri)
	{
		base.SetPath(uri);
		if (PathUri.Params.ContainsKey("expired"))
		{
			SessionExpired = true;
		}
	}

	private void ExecuteNavigateToLoginPageCommand()
	{
		Navigator.Navigate(FacebookUri.LoginUri);
	}

	private async void ExecuteRegisterPhoneNumberCommand()
	{
		if (IsLoading || string.IsNullOrWhiteSpace(MyPhoneNumber) || ValidatedCountryCode == null)
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
		try
		{
			string phoneNumber = Regex.Replace(MyPhoneNumber, "[\\(\\)\\-\\.\\s]", "");
			PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
			Google.PhoneNumbers.PhoneNumber proto = phoneUtil.parse(phoneNumber, ValidatedCountryCode.RegionCode);
			if (!phoneUtil.isValidNumberForRegion(proto, ValidatedCountryCode.RegionCode))
			{
				Error = new Error
				{
					Message = StringLibrary.Error_Invalid_Phone_Number
				};
				IsLoading = false;
				return;
			}
			_normalizedPhoneNumber = $"+{ValidatedCountryCode.PhonePrefix}{phoneNumber}";
			Facebook.Services.Api.Methods.PhoneNumberRegistration.Request request = new Facebook.Services.Api.Methods.PhoneNumberRegistration.Request(Context.GlobalSettings.DeviceIdentifier, ValidatedCountryCode.RegionCode, _normalizedPhoneNumber);
			BoolResponse response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.PhoneNumberRegistration.Request, BoolResponse>(request);
			if (response == null || response.HasError())
			{
				Error = (HasConnection ? Error.FromResponse(response, StringLibrary.MessageBox_Label_UpdatePhoneNumberFailed, string.Empty) : new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				});
			}
			else
			{
				NavigateToVerifyPhoneNumber();
			}
		}
		catch (NumberParseException ex)
		{
			Logger.Error("NumberParseException was thrown: " + ex, "ExecuteRegisterPhoneNumberCommand", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\PhoneNumberRegistrationViewModel.cs", 152);
			Logger.Error("Number : " + MyPhoneNumber, "ExecuteRegisterPhoneNumberCommand", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\PhoneNumberRegistrationViewModel.cs", 153);
		}
		IsLoading = false;
	}

	private void NavigateToVerifyPhoneNumber()
	{
		UriQuery uriQuery = new UriQuery();
		uriQuery.Add("phonenumber", MyPhoneNumber);
		uriQuery.Add("normalizedphonenumber", _normalizedPhoneNumber);
		uriQuery.Add("regioncode", ValidatedCountryCode.RegionCode);
		UriQuery uriQuery2 = uriQuery;
		uriQuery2.Add("registrationflow", "true");
		uriQuery2.Add("phoneprefix", ValidatedCountryCode.PhonePrefix.ToString());
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.VerifyPhoneNumber, uriQuery2));
	}
}
