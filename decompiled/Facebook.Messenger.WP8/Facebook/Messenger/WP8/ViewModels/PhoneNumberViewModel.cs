using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchSearchability;
using Facebook.Services.Api.Methods.FetchUser;
using Facebook.Services.Api.Methods.PhoneNumber;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels;

public class PhoneNumberViewModel : BasePhoneNumberViewModel
{
	private PhoneSearchabilityTypes _searchability;

	private PhonePrivacyTypes _phonePrivacy;

	private bool _shouldShowRetryButton;

	private RelayCommand _refetchPrivacySettingsCommand;

	private string _phoneNumberInstructions;

	private string _phoneNumberInstructionsTwo;

	private string _phoneNumberTitle;

	private bool _isLoginFlow;

	private RelayCommand _savePhoneNumberCommand;

	private RelayCommand _skipPhoneNumberCommand;

	public PhoneSearchabilityTypes Searchability
	{
		get
		{
			return _searchability;
		}
		set
		{
			if (Set(ref _searchability, value, broadcast: false, "Searchability"))
			{
				RaisePropertyChanged("ShouldShowInterstitial");
			}
		}
	}

	public PhonePrivacyTypes PhonePrivacy
	{
		get
		{
			return _phonePrivacy;
		}
		set
		{
			if (Set(ref _phonePrivacy, value, broadcast: false, "PhonePrivacy"))
			{
				RaisePropertyChanged("ShouldShowInterstitial");
			}
		}
	}

	public bool ShouldShowInterstitial
	{
		get
		{
			if (Searchability != PhoneSearchabilityTypes.Unknown)
			{
				return PhonePrivacy == PhonePrivacyTypes.Unknown;
			}
			return true;
		}
	}

	public bool ShouldShowRetryButton
	{
		get
		{
			return _shouldShowRetryButton;
		}
		set
		{
			Set(ref _shouldShowRetryButton, value, broadcast: false, "ShouldShowRetryButton");
		}
	}

	public RelayCommand RefetchPrivacySettingsCommand => _refetchPrivacySettingsCommand ?? (_refetchPrivacySettingsCommand = new RelayCommand(ExecuteRefetchPrivacySettingsCommand));

	public string PhoneNumberInstructions
	{
		get
		{
			return _phoneNumberInstructions;
		}
		set
		{
			Set(ref _phoneNumberInstructions, value, broadcast: false, "PhoneNumberInstructions");
		}
	}

	public string PhoneNumberInstructionsTwo
	{
		get
		{
			return _phoneNumberInstructionsTwo;
		}
		set
		{
			Set(ref _phoneNumberInstructionsTwo, value, broadcast: false, "PhoneNumberInstructionsTwo");
		}
	}

	public string PhoneNumberTitle
	{
		get
		{
			return _phoneNumberTitle;
		}
		set
		{
			Set(ref _phoneNumberTitle, value, broadcast: false, "PhoneNumberTitle");
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
			Set(ref _isLoginFlow, value, broadcast: false, "IsLoginFlow");
		}
	}

	public RelayCommand SavePhoneNumberCommand => _savePhoneNumberCommand ?? (_savePhoneNumberCommand = new RelayCommand(ExecuteSavePhoneNumberCommand));

	public RelayCommand SkipPhoneNumberCommand => _skipPhoneNumberCommand ?? (_skipPhoneNumberCommand = new RelayCommand(SkipPhoneNumber));

	public PhoneNumberViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
		: base(apiService, context, navigator, dispatcher, networkStatus)
	{
	}

	private async void ExecuteRefetchPrivacySettingsCommand()
	{
		await FetchPrivacySettings();
	}

	private void UpdateTitleDisplay()
	{
		if (!string.IsNullOrWhiteSpace(MyPhoneNumber))
		{
			if (Searchability == PhoneSearchabilityTypes.Everyone && PhonePrivacy != PhonePrivacyTypes.AllFriends)
			{
				PhoneNumberTitle = StringLibrary.orca_neue_nux_add_phone_title_verified_long;
			}
			else
			{
				PhoneNumberTitle = StringLibrary.orca_neue_nux_add_phone_title_verified_short;
			}
		}
		else
		{
			PhoneNumberTitle = StringLibrary.orca_neue_nux_add_phone_title_no_verified_number;
		}
	}

	private void UpdateInstructionsDisplay()
	{
		UpdateSecondInstructionsDisplay();
		bool flag = Searchability == PhoneSearchabilityTypes.Everyone;
		if (!string.IsNullOrEmpty(MyPhoneNumber))
		{
			if (!flag)
			{
				PhoneNumberInstructions = StringLibrary.orca_neue_nux_add_phone_body1_searchable_fof;
				return;
			}
			switch (PhonePrivacy)
			{
			case PhonePrivacyTypes.Everyone:
			case PhonePrivacyTypes.MutualFriends:
				PhoneNumberInstructions = StringLibrary.orca_neue_nux_add_phone_body1_searchable_all_visible_all;
				break;
			case PhonePrivacyTypes.Custom:
				PhoneNumberInstructions = StringLibrary.orca_neue_nux_add_phone_body1_searchable_all_visible_custom;
				break;
			case PhonePrivacyTypes.AllFriends:
				PhoneNumberInstructions = StringLibrary.orca_neue_nux_add_phone_body1_searchable_all_visible_friends;
				break;
			default:
				PhoneNumberInstructions = StringLibrary.orca_neue_nux_add_phone_body1_searchable_all_visible_self;
				break;
			}
		}
		else
		{
			PhoneNumberInstructions = (flag ? StringLibrary.orca_neue_nux_add_phone_body1_searchable_all_not_verified : StringLibrary.orca_neue_nux_add_phone_body1_searchable_fof);
		}
	}

	public void UpdateSecondInstructionsDisplay()
	{
		if (Searchability == PhoneSearchabilityTypes.Everyone)
		{
			PhoneNumberInstructionsTwo = string.Empty;
		}
		else if (!string.IsNullOrEmpty(MyPhoneNumber))
		{
			switch (PhonePrivacy)
			{
			case PhonePrivacyTypes.Everyone:
			case PhonePrivacyTypes.MutualFriends:
				PhoneNumberInstructionsTwo = string.Empty;
				break;
			case PhonePrivacyTypes.Custom:
				PhoneNumberInstructionsTwo = StringLibrary.orca_neue_nux_add_phone_body2_visible_custom;
				break;
			case PhonePrivacyTypes.AllFriends:
				PhoneNumberInstructionsTwo = StringLibrary.orca_neue_nux_add_phone_body2_visible_friends;
				break;
			default:
				PhoneNumberInstructionsTwo = StringLibrary.orca_neue_nux_add_phone_body2_visible_self;
				break;
			}
		}
		else
		{
			PhoneNumberInstructionsTwo = StringLibrary.orca_neue_nux_add_phone_body2_not_verified;
		}
	}

	public override async void SetPath(Uri uri)
	{
		base.SetPath(uri);
		IsLoginFlow = PathUri.Params.ContainsKey("loginflow");
		if (PathUri.Params.ContainsKey("privacy"))
		{
			PhonePrivacy = PhoneNumberUtility.PhonePrivacyFromString(PathUri.Params["privacy"]);
		}
		if (PathUri.Params.ContainsKey("searchability"))
		{
			Enum.TryParse<PhoneSearchabilityTypes>(PathUri.Params["searchability"], out _searchability);
		}
		if (PathUri.Params.ContainsKey("phoneprefix"))
		{
			int prefix = int.Parse(PathUri.Params["phoneprefix"]);
			GetRegionCodeForPrefix(prefix).ContinueWith((Task<string> task) => Dispatcher.RunAsync(() =>
			{
				DetermineValidatedCountryCode(task.Result);
			})).DoNotWait();
		}
		if (PathUri.Params.ContainsKey("phonenumber"))
		{
			string phone = PathUri.Params["phonenumber"];
			await UpdatePhoneNumber(phone);
		}
		FetchPrivacySettings().DoNotWait();
	}

	private async void ExecuteSavePhoneNumberCommand()
	{
		if (IsLoading || string.IsNullOrWhiteSpace(MyPhoneNumber) || ValidatedCountryCode == null)
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
		string phoneNumber = (MyPhoneNumber.Trim().StartsWith("+") ? MyPhoneNumber : $"+{ValidatedCountryCode.PhonePrefix} {MyPhoneNumber}");
		Facebook.Services.Api.Methods.PhoneNumber.Request request = new Facebook.Services.Api.Methods.PhoneNumber.Request
		{
			AccessToken = Context.Session.Credentials.AccessToken,
			CancelToken = CancelTokenSource.Token,
			PhoneNumber = phoneNumber,
			CountryCode = ValidatedCountryCode.RegionCode
		};
		IsLoading = true;
		BoolResponse response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.PhoneNumber.Request, BoolResponse>(request);
		if (response.HasError())
		{
			if (response.ErrorCode.HasValue && response.ErrorCode == ApiErrorCodes.PhoneNumberAlreadyVerified)
			{
				Context.UpdatePhoneNumber(phoneNumber);
				Context.UpdatePhoneNumberVerified();
				NavigateNext();
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
		else
		{
			if (_isLoginFlow)
			{
				Context.UpdatePhoneNumber(phoneNumber);
			}
			else if (string.IsNullOrEmpty(Context.GetPhoneNumber()))
			{
				Context.UpdatePhoneNumber(phoneNumber);
			}
			NavigateToVerifyPhoneNumber(phoneNumber);
			Context.UpdatePhoneNumberUnverified();
		}
		IsLoading = false;
	}

	private async Task FetchPrivacySettings()
	{
		ShouldShowRetryButton = false;
		List<Task<bool>> fetchPrivacyTasks = new List<Task<bool>>();
		if (PhonePrivacy == PhonePrivacyTypes.Unknown)
		{
			fetchPrivacyTasks.Add(FetchPrivacy());
		}
		if (Searchability == PhoneSearchabilityTypes.Unknown)
		{
			fetchPrivacyTasks.Add(FetchSearchability());
		}
		await Task.WhenAll(fetchPrivacyTasks);
		bool succeeded = fetchPrivacyTasks.Select((Task<bool> t) => t.Result).All((bool r) => r);
		ShouldShowRetryButton = !succeeded;
		UpdateTitleDisplay();
		UpdateInstructionsDisplay();
	}

	private async Task<bool> FetchPrivacy()
	{
		Facebook.Services.Api.Methods.FetchUser.Request fetchUserRequest = new Facebook.Services.Api.Methods.FetchUser.Request
		{
			AccessToken = Context.Session.Credentials.AccessToken
		};
		Facebook.Services.Api.Methods.FetchUser.Response users = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchUser.Request, Facebook.Services.Api.Methods.FetchUser.Response>(fetchUserRequest);
		if (users.HasError() || users.UserData == null || !users.UserData.Any())
		{
			return false;
		}
		PhoneInfo mobilePhone = PhoneNumberUtility.GetBroadestPrivacyPhoneInfo(users.UserData.FirstOrDefault().Phones);
		if (mobilePhone != null)
		{
			PhonePrivacy = PhoneNumberUtility.PhonePrivacyFromString(mobilePhone.Privacy);
		}
		else
		{
			PhonePrivacy = PhonePrivacyTypes.Self;
		}
		return PhonePrivacy != PhonePrivacyTypes.Unknown;
	}

	private async Task<bool> FetchSearchability()
	{
		Facebook.Services.Api.Methods.FetchSearchability.Request searchabilityRequest = new Facebook.Services.Api.Methods.FetchSearchability.Request
		{
			AccessToken = Context.Session.Credentials.AccessToken
		};
		Facebook.Services.Api.Methods.FetchSearchability.Response searchabilityResponse = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchSearchability.Request, Facebook.Services.Api.Methods.FetchSearchability.Response>(searchabilityRequest);
		if (searchabilityResponse.HasError())
		{
			return false;
		}
		Searchability = searchabilityResponse.Searchability;
		return Searchability != PhoneSearchabilityTypes.Unknown;
	}

	private void NavigateToVerifyPhoneNumber(string phoneNumber)
	{
		UriQuery uriQuery = new UriQuery();
		uriQuery.Add("phonenumber", phoneNumber);
		UriQuery uriQuery2 = uriQuery;
		if (_isLoginFlow)
		{
			uriQuery2.Add("loginflow", "true");
		}
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.VerifyPhoneNumber, uriQuery2));
	}

	private void NavigateNext()
	{
		if (_isLoginFlow)
		{
			Navigator.Reset(FacebookUri.NuxOnMessengerUri);
		}
		else
		{
			Navigator.GoBack();
		}
	}

	public void SkipPhoneNumber()
	{
		if (_isLoginFlow || (!Context.GetPhoneNumberState().Equals(Context.PhoneNumberVerified) && string.IsNullOrEmpty(MyPhoneNumber)))
		{
			Context.UpdatePhoneNumberSkipped();
		}
		NavigateNext();
	}
}
