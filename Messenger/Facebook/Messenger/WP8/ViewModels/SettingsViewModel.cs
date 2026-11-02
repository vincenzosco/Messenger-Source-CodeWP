using System;
using System.Threading.Tasks;
using System.Windows;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.FBGeoLocator;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.UploadProfilePicture;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class SettingsViewModel : FacebookMessengerViewModel
	{
		private readonly Facebook.Messenger.WP8.FBGeoLocator.Service _geoLocator;

		private readonly PhoneContactsManager _phoneContactsManager;

		private readonly IEnvironmentInformation _environmentInformation;

		private readonly ContactSyncFeatureEnabledProvider _contactSyncFeatureEnabledProvider;

		private Error _error;

		private FacebookProfile _currentUserProfile;

		private bool _isMuteReady = true;

		private RelayCommand _navigateToPhoneNumberPage;

		private RelayCommand _contactSyncLearnMoreCommand;

		private RelayCommand _displayLicensesCommand;

		private RelayCommand<string> _uploadProfilePictureCommand;

		private bool _togglingLocation;

		private bool _locationEnabled;

		private RelayCommand<bool> _setLocationEnabledCommand;

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

		public FacebookProfile CurrentUser
		{
			get
			{
				return _currentUserProfile;
			}
			set
			{
				Set(ref _currentUserProfile, value, false, "CurrentUser");
			}
		}

		public bool IsMuted => MuteHelper.IsMuted(Context);

		public bool IsMutedUntil8AM => MuteHelper.IsMutedUntil8AM(Context);

		public bool IsMuteReady
		{
			get
			{
				return _isMuteReady;
			}
			set
			{
				Set(ref _isMuteReady, value, false, "IsMuteReady");
			}
		}

		public bool AudioEnabled
		{
			get
			{
				return Context.IsAudioEnabled();
			}
			set
			{
				Context.SetAudioEnabled(value);
				RaisePropertyChanged("AudioEnabled");
			}
		}

		public bool VibrationEnabled
		{
			get
			{
				return Context.IsVibrationEnabled();
			}
			set
			{
				Context.SetVibrationEnabled(value);
				RaisePropertyChanged("VibrationEnabled");
			}
		}

		public bool IsSyncContactsFeatureEnabled => _contactSyncFeatureEnabledProvider.Enabled;

		public bool ContactSyncEnabled
		{
			get
			{
				return Context.IsContactSyncEnabled();
			}
			set
			{
				if (value)
				{
					Task.Run(() =>
					{
						_phoneContactsManager.StartContactSync();
					});
				}
				else
				{
					Task.Run(() =>
					{
						_phoneContactsManager.StopContactSync();
					});
				}
				RaisePropertyChanged("ContactSyncEnabled");
			}
		}

		public bool IsContactSyncInProgress => _phoneContactsManager.ContactSyncStatus.Status == SyncState.InProgress;

		public RelayCommand NavigateToPhoneNumberPage => _navigateToPhoneNumberPage ?? (_navigateToPhoneNumberPage = new RelayCommand(ExecuteNavigateToPhoneNumberPageCommand));

		public RelayCommand ContactSyncLearnMoreCommand => _contactSyncLearnMoreCommand ?? (_contactSyncLearnMoreCommand = new RelayCommand(ExecuteContactSyncLearnMoreCommand));

		public RelayCommand DisplayLicensesCommand => _displayLicensesCommand ?? (_displayLicensesCommand = new RelayCommand(ExecuteDisplayLicensesCommand));

		public RelayCommand<string> UploadProfilePictureCommand => _uploadProfilePictureCommand ?? (_uploadProfilePictureCommand = new RelayCommand<string>(ExecuteUploadProfilePictureCommand));

		public bool TogglingLocation
		{
			get
			{
				return _togglingLocation;
			}
			set
			{
				Set(ref _togglingLocation, value, false, "TogglingLocation");
			}
		}

		public bool LocationEnabled
		{
			get
			{
				return _locationEnabled;
			}
			set
			{
				Set(ref _locationEnabled, value, false, "LocationEnabled");
			}
		}

		public RelayCommand<bool> SetLocationEnabledCommand => _setLocationEnabledCommand ?? (_setLocationEnabledCommand = new RelayCommand<bool>(ExecuteSetLocationEnabledCommand));

		public bool SendOnEnterEnabled
		{
			get
			{
				return Context.IsSendOnEnterEnabled();
			}
			set
			{
				Context.SetSendOnEnterEnabled(value);
				RaisePropertyChanged("SendOnEnterEnabled");
			}
		}

		public string VersionString => _environmentInformation.GetApplicationVersionAsThreePartString();

		public string NotificationString
		{
			get
			{
				long globalMuteDuration = Context.GetGlobalMuteDuration();
				long num = globalMuteDuration;
				if (num <= 0 && num >= -1)
				{
					switch ((int)(num - -1))
					{
					case 0:
						return StringLibrary.Global_Notification_Off;
					case 1:
						return StringLibrary.Global_Notification_On;
					}
				}
				return GetMuteDurationTime(globalMuteDuration);
			}
		}

		public SettingsViewModel(Facebook.Services.Api.Service apiService, Facebook.Messenger.WP8.FBGeoLocator.Service geoLocator, PhoneContactsManager phoneContactsManager, ContactSyncFeatureEnabledProvider contactSyncFeatureEnabledProvider, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, IEnvironmentInformation environmentInformation)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			CurrentUser = context.Session.Profile;
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<GlobalMuteSignal>(this, OnGlobalMuteChanged);
			_geoLocator = geoLocator;
			_geoLocator.GeolocatorStatusChanged += OnGeolocatorStatusChanged;
			_phoneContactsManager = phoneContactsManager;
			_environmentInformation = environmentInformation;
			LocationEnabled = _geoLocator.IsAppLocationEnabled();
			_phoneContactsManager.SyncStatusUpdate += PhoneContactsManagerOnSyncStatusUpdate;
			_contactSyncFeatureEnabledProvider = contactSyncFeatureEnabledProvider;
		}

		private void OnGeolocatorStatusChanged(object sender, EventArgs eventArgs)
		{
			LocationEnabled = _geoLocator.IsAppLocationEnabled();
		}

		private void PhoneContactsManagerOnSyncStatusUpdate(object sender, ContactSyncStatus args)
		{
			if (args.Status == SyncState.Failed)
			{
				((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(() =>
				{
					OnError(new Error
					{
						Message = StringLibrary.SyncContact_ErrorMessage_UnableToToggle
					});
				}));
			}
			((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(() =>
			{
				RaisePropertyChanged(() => IsContactSyncInProgress);
			}));
			((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(() =>
			{
				RaisePropertyChanged(() => ContactSyncEnabled);
			}));
		}

		public override void Cleanup()
		{
			_phoneContactsManager.SyncStatusUpdate -= PhoneContactsManagerOnSyncStatusUpdate;
			base.Cleanup();
		}

		private void OnGlobalMuteChanged(GlobalMuteSignal obj)
		{
			((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(() =>
			{
				RaisePropertyChanged("NotificationString");
				RaisePropertyChanged("IsMuted");
			}));
		}

		private void ExecuteNavigateToPhoneNumberPageCommand()
		{
			Navigator.Navigate(FacebookUri.GetAppUri(UriId.PhoneNumber));
		}

		public void ExecuteContactSyncLearnMoreCommand()
		{
			FacebookUri webViewDeepLinkUri = FacebookAppDeepLinkUtil.GetWebViewDeepLinkUri(FacebookAppDeepLinkUtil.Page.ContactSyncLearnMore);
			Navigator.Navigate(webViewDeepLinkUri);
		}

		private void ExecuteDisplayLicensesCommand()
		{
			if (Navigator != null)
			{
				Navigator.Navigate(FacebookUri.GetWebViewUri("/Assets/Html/Licenses.html"));
			}
		}

		private async void ExecuteUploadProfilePictureCommand(string url)
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
				ImageUrl = url,
				AccessToken = Context.Session.Credentials.AccessToken,
				CancelToken = CancelTokenSource.Token
			};
			Facebook.Services.Api.Methods.UploadProfilePicture.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.UploadProfilePicture.Request, Facebook.Services.Api.Methods.UploadProfilePicture.Response>(request);
			if (response == null || response.HasError())
			{
				Error = (HasConnection ? Error.FromResponse(response, StringLibrary.MessageBox_Label_UploadProfilePic_Failed, string.Empty) : new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				});
			}
			IsLoading = false;
		}

		private async void ExecuteSetLocationEnabledCommand(bool enable)
		{
			if (!TogglingLocation && enable != LocationEnabled)
			{
				TogglingLocation = true;
				if (await _geoLocator.SetLocationEnabledAsync(enable))
				{
					LocationEnabled = enable;
				}
				else
				{
					OnError(new Error
					{
						Message = StringLibrary.Device_Location_Services_Enable,
						Title = StringLibrary.Location_Services_Disabled
					});
					RaisePropertyChanged("LocationEnabled");
				}
				TogglingLocation = false;
			}
		}

		private string GetMuteDurationTime(long muteDuration)
		{
			string arg = DateExtensions.FromUnixTimeSeconds(muteDuration).ToLocalTime().ToShortTimeString();
			return string.Format(StringLibrary.Global_Notifcation_Off_Until, arg);
		}

		public Task TurnOffMute()
		{
			return TurnOnMute(MuteDuration.Off);
		}

		public async Task TurnOnMute(MuteDuration muteDuration)
		{
			IsMuteReady = false;
			BoolResponse response = await MuteHelper.SetGlobalMuteState(muteDuration, Context, ApiService);
			IsMuteReady = true;
			if (response.HasError())
			{
				MessageBox.Show(StringLibrary.Error_Changing_Mute);
			}
		}

		public void ShowDataUsePolicy()
		{
			NavigateToWebPage(FacebookAppDeepLinkUtil.Page.DataUsePolicies);
		}

		public void ShowMessengerHelp()
		{
			NavigateToWebPage(FacebookAppDeepLinkUtil.Page.MessengerHelp);
		}

		private void NavigateToWebPage(FacebookAppDeepLinkUtil.Page page)
		{
			Navigator.Navigate(FacebookAppDeepLinkUtil.GetWebViewDeepLinkUri(page));
		}
	}
}
