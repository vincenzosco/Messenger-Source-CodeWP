using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Google.PhoneNumbers;

namespace Facebook.Messenger.WP8.ViewModels;

public class BasePhoneNumberViewModel : FacebookMessengerViewModel
{
	private Task _initializeTask;

	private readonly bool _isOtherContact;

	protected List<CountryCode> _countryCodes;

	protected ObservableCollection<AlphaKeyGroup<CountryCode>> _alphaGroupedCountryCodes;

	protected CountryCode _validatedCountryCode;

	protected string _normalizedPhoneNumber;

	protected string _myPhoneNumber;

	private PhoneNumberUtil _phoneNumberUtil;

	public List<CountryCode> CountryCodes
	{
		get
		{
			return _countryCodes;
		}
		set
		{
			Set(ref _countryCodes, value, broadcast: false, "CountryCodes");
		}
	}

	public ObservableCollection<AlphaKeyGroup<CountryCode>> AlphaGroupedCountryCodes
	{
		get
		{
			return _alphaGroupedCountryCodes;
		}
		set
		{
			Set(ref _alphaGroupedCountryCodes, value, broadcast: false, "AlphaGroupedCountryCodes");
		}
	}

	public CountryCode ValidatedCountryCode
	{
		get
		{
			if (_validatedCountryCode != null)
			{
				return _validatedCountryCode;
			}
			DetermineValidatedCountryCode();
			return _validatedCountryCode;
		}
		set
		{
			if (_validatedCountryCode != null)
			{
				_validatedCountryCode.IsSelected = false;
			}
			Set(ref _validatedCountryCode, value, broadcast: false, "ValidatedCountryCode");
			RaisePropertyChanged("PrefixWithPlus");
			if (_validatedCountryCode != null)
			{
				_validatedCountryCode.IsSelected = true;
				UpdatePhoneNumber(MyPhoneNumber).DoNotWait();
			}
		}
	}

	public string MyPhoneNumber
	{
		get
		{
			return _myPhoneNumber;
		}
		set
		{
			UpdatePhoneNumber(value).DoNotWait();
		}
	}

	public PhoneNumberUtil PhoneNumberUtil => _phoneNumberUtil;

	public string PrefixWithPlus
	{
		get
		{
			if (_validatedCountryCode == null)
			{
				return string.Empty;
			}
			return _validatedCountryCode.PhonePrefixWithPlus;
		}
	}

	public BasePhoneNumberViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, bool isOtherContact = false)
		: base(apiService, context, navigator, dispatcher, networkStatus)
	{
		_isOtherContact = isOtherContact;
		InitializeCountriesAndPhoneNumber().DoNotWait();
	}

	public async Task InitializeUtility()
	{
		_initializeTask = Task.Run(() =>
		{
			_phoneNumberUtil = PhoneNumberUtil.getInstance();
		});
		await _initializeTask;
	}

	private async Task<T> PerformAfterInitialize<T>(Func<T> action)
	{
		if (_initializeTask != null && !_initializeTask.IsCompleted)
		{
			await _initializeTask;
		}
		return action();
	}

	private async Task InitializeCountriesAndPhoneNumber()
	{
		IsLoading = true;
		try
		{
			InitializeUtility().DoNotWait();
			List<CountryCode> countryCodes = await GetCountryCodesAsync();
			ObservableCollection<AlphaKeyGroup<CountryCode>> groupedCountries = GetGroupedCountries(countryCodes);
			await Dispatcher.RunAsync(() =>
			{
				CountryCodes = countryCodes;
				DetermineValidatedCountryCode();
				AlphaGroupedCountryCodes = groupedCountries;
				if (!_isOtherContact)
				{
					UpdatePhoneNumber(Context.GetPhoneNumber(), removeCountryCode: true).DoNotWait();
				}
			});
		}
		catch (Exception ex)
		{
			Logger.Error(this, "Unable to initialize phone library and countries", "InitializeCountriesAndPhoneNumber", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\BasePhoneNumberViewModel.cs", 151);
			Logger.Error(this, ex.ToString(), "InitializeCountriesAndPhoneNumber", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\BasePhoneNumberViewModel.cs", 152);
		}
		IsLoading = false;
	}

	protected async Task<List<CountryCode>> GetCountryCodesAsync()
	{
		return await PerformAfterInitialize(() =>
		{
			IDictionary<int, List<string>> getCountryCallingCodeToRegionCodeMap = PhoneNumberUtil.GetCountryCallingCodeToRegionCodeMap;
			List<CountryCode> list = new List<CountryCode>();
			foreach (KeyValuePair<int, List<string>> codeEntries in getCountryCallingCodeToRegionCodeMap)
			{
				KeyValuePair<int, List<string>> keyValuePair = codeEntries;
				list.AddRange(keyValuePair.Value.Where((string entry) => entry != "001").Select((string entry) =>
				{
					CountryCode countryCode = new CountryCode();
					KeyValuePair<int, List<string>> keyValuePair2 = codeEntries;
					countryCode.PhonePrefix = keyValuePair2.Key;
					countryCode.RegionCode = entry;
					return countryCode;
				}));
			}
			list.RemoveAll((CountryCode cc) => string.IsNullOrEmpty(cc.CountryName));
			list.Sort((CountryCode country1, CountryCode country2) => country1.FormattedCountryCode.CompareTo(country2.FormattedCountryCode));
			return list;
		});
	}

	protected ObservableCollection<AlphaKeyGroup<CountryCode>> GetGroupedCountries(List<CountryCode> countryCodes)
	{
		if (countryCodes == null)
		{
			return null;
		}
		return AlphaKeyGroup<CountryCode>.CreateGroups(countryCodes, Thread.CurrentThread.CurrentUICulture, (CountryCode s) => s.CountryName, sort: true);
	}

	protected CountryCode GetValidatedCountryCode(List<CountryCode> countryCodes, string regionCode)
	{
		if (countryCodes == null)
		{
			return null;
		}
		CountryCode countryCode = null;
		if (regionCode != null)
		{
			countryCode = countryCodes.FirstOrDefault((CountryCode x) => x.RegionCode.ToUpper().Equals(regionCode.ToUpper()));
		}
		if (countryCode == null)
		{
			countryCode = countryCodes.FirstOrDefault((CountryCode x) => x.RegionCode.ToUpper().Equals(RegionInfo.CurrentRegion.TwoLetterISORegionName)) ?? countryCodes.FirstOrDefault();
		}
		return countryCode;
	}

	protected async Task<string> GetFormattedPhoneNumber(string unformattedPhoneNumber, CountryCode validatedCountryCode, bool removeCountryCode, Action<string> regionChangedAction = null)
	{
		if (unformattedPhoneNumber == null)
		{
			return null;
		}
		string phoneNumber = unformattedPhoneNumber.Replace(" ", string.Empty);
		if (phoneNumber.Length <= 4 || phoneNumber.Length >= 20)
		{
			return phoneNumber;
		}
		return await PerformAfterInitialize(() =>
		{
			string text = ((validatedCountryCode != null) ? validatedCountryCode.RegionCode : "US");
			try
			{
				Google.PhoneNumbers.PhoneNumber phoneNumber2 = PhoneNumberUtil.parse(phoneNumber, text);
				if (phoneNumber.StartsWith("+") && !removeCountryCode)
				{
					phoneNumber = PhoneNumberUtil.format(phoneNumber2, PhoneNumberUtil.PhoneNumberFormat.INTERNATIONAL);
				}
				else if (phoneNumber2.HasNationalNumber())
				{
					phoneNumber = PhoneNumberUtil.format(phoneNumber2, PhoneNumberUtil.PhoneNumberFormat.NATIONAL);
				}
				string regionCodeForNumber = PhoneNumberUtil.getRegionCodeForNumber(phoneNumber2);
				if (regionCodeForNumber != null && !text.ToUpper().Equals(regionCodeForNumber.ToUpper()) && regionChangedAction != null)
				{
					regionChangedAction(regionCodeForNumber);
				}
			}
			catch (NumberParseException ex)
			{
				Logger.Error("NumberParseException was thrown: " + ex, "GetFormattedPhoneNumber", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\BasePhoneNumberViewModel.cs", 256);
				Logger.Error("Number : " + phoneNumber, "GetFormattedPhoneNumber", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\BasePhoneNumberViewModel.cs", 257);
			}
			return phoneNumber;
		});
	}

	protected async Task<string> GetRegionCodeForPrefix(int prefix)
	{
		return await PerformAfterInitialize(() => PhoneNumberUtil.getRegionCodeForCountryCode(prefix));
	}

	protected void DetermineValidatedCountryCode(string regionCode = null)
	{
		ValidatedCountryCode = GetValidatedCountryCode(CountryCodes, regionCode);
	}

	protected async Task UpdatePhoneNumber(string newPhoneNumber, bool removeCountryCode = false)
	{
		_myPhoneNumber = await GetFormattedPhoneNumber(newPhoneNumber, ValidatedCountryCode, removeCountryCode, (string regionCode) =>
		{
			if (_validatedCountryCode != null)
			{
				_validatedCountryCode.IsSelected = false;
				_validatedCountryCode = null;
			}
			DetermineValidatedCountryCode(regionCode);
		});
		RaisePropertyChanged("MyPhoneNumber");
	}

	public bool UpdateValidatedCountryCode(string phonePrefix)
	{
		bool result = true;
		try
		{
			string regionCodeForCountryCode = PhoneNumberUtil.getRegionCodeForCountryCode(Convert.ToInt32(phonePrefix));
			if (regionCodeForCountryCode != null && !regionCodeForCountryCode.Equals("ZZ"))
			{
				DetermineValidatedCountryCode(regionCodeForCountryCode);
			}
			else
			{
				OnError(new Error
				{
					Message = StringLibrary.Error_Invalid_Phone_Prefix
				});
				result = false;
			}
		}
		catch (FormatException)
		{
			OnError(new Error
			{
				Message = StringLibrary.Error_Invalid_Phone_Prefix
			});
			result = false;
		}
		return result;
	}
}
