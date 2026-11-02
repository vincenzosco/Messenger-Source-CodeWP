using System;
using System.Collections.Generic;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Gatekeepers;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.FBGeoLocator;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Push;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Messenger.WP8.Utilities.Stickers;
using Facebook.Messenger.WP8.Utilities.SystemTrayManager;
using Facebook.Messenger.WP8.ViewModels.Internal;
using Facebook.Models;
using Facebook.Services;
using Facebook.Services.Analytics;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.Cache;
using Facebook.Services.Friends;
using Facebook.Services.Http;
using Facebook.Services.MessageSender;
using Facebook.Services.Mqtt;
using Facebook.Services.Presence;
using Facebook.Services.ScheduledTasksService;
using Facebook.Services.Stickers;
using Facebook.Services.SyncProtocol;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class FacebookMessengerUiLocator : FacebookMessengerLocator
	{
		public static class PageUrls
		{
			public const string FirstLaunch = "/Pages/FirstLaunchPage.xaml";

			public const string Login = "/Pages/LoginPage.xaml";

			public const string Main = "/Pages/MessengerMainPage.xaml";

			public const string Thread = "/Pages/ThreadPage.xaml";

			public const string Composer = "/Pages/ComposerBarPage.xaml";

			public const string GroupComposer = "/Pages/GroupComposerPage.xaml";

			public const string GroupDetails = "/Pages/GroupDetailsPage.xaml";

			public const string LoginApproval = "/Pages/LoginApprovalPage.xaml";

			public const string NuxOnMessenger = "/Pages/NuxOnMessengerPage.xaml";

			public const string PhoneNumber = "/Pages/PhoneNumberPage.xaml";

			public const string VerifyPhoneNumber = "/Pages/VerifyPhoneNumberPage.xaml";

			public const string PhotoAttachment = "/Pages/PhotoAttachmentsPage.xaml";

			public const string MessageDetails = "/Pages/MessageDetailsPage.xaml";

			public const string LongMessageDetails = "/Pages/LongMessageDetailsPage.xaml";

			public const string StickerStore = "/Pages/StickerStorePage.xaml";

			public const string StickerPack = "/Pages/StickerStorePackPage.xaml";

			public const string NameConversation = "/Pages/NameConversationPage.xaml";

			public const string AddPeopleToGroup = "/Pages/AddUserToGroup.xaml";

			public const string WebView = "/Pages/WebViewPage.xaml";

			public const string Settings = "/Pages/SettingsPage.xaml";

			public const string ReportProblem = "/Pages/ReportPage.xaml";

			public const string MuteSelection = "/Pages/MuteSelectionPage.xaml";

			public const string GroupComposerAddUsers = "/Pages/GroupComposerAddUsersPage.xaml";

			public const string SuggestedPins = "/Pages/SuggestedPinsPage.xaml";

			public const string InternalSettings = "/Pages/InternalSettingsPage.xaml";

			public const string Forward = "/Pages/ForwardingPage.xaml";

			public const string PhoneRegistrationPage = "/Pages/PhoneNumberRegistrationPage.xaml";

			public const string AddContact = "/Pages/AddContactPage.xaml";

			public const string NewUserRegistrationPage = "/Pages/NewUserRegistrationPage.xaml";

			public const string CreateProfilePic = "/Pages/AddProfilePicPage.xaml";

			public const string SyncContacts = "/Pages/SyncContactsPage.xaml";

			public const string ProfilePicConfirmation = "/Pages/ProfilePicConfirmationPage.xaml";

			public const string ExistingUser = "/Pages/ExistingUserPage.xaml";

			public const string CanonicalThreadDetails = "/Pages/CanonicalThreadDetailsPage.xaml";
		}

		private static readonly FacebookMessengerUiLocator PrivateInstance = new FacebookMessengerUiLocator();

		public static FacebookMessengerUiLocator Instance
		{
			get
			{
				return PrivateInstance;
			}
		}

		protected FacebookMessengerUiLocator()
		{
			Register(CreateContacts);
			Register(CreateContactSearch);
			Register(CreateThreads);
			Register(CreatePinnedThreads);
			Register(CreateComposer);
			Register(CreateGroupComposer);
			Register(CreateGroupDetails);
			Register(CreateComposerAddUsers);
			Register(CreateLogin);
			Register(CreateNuxOnMessenger);
			Register(CreateMainViewModel);
			Register(CreateUpgradePromptViewModel);
			Register(CreateMessages);
			Register(CreateGeneric);
			Register(CreatePhoneNumber);
			Register(CreateVerifyPhone);
			Register(CreatePhotoAttachments);
			Register(CreateMessageDetails);
			Register(CreateStickerStore);
			Register(CreateStickerPack);
			Register(CreateActiveUsers);
			Register(CreateSettings);
			Register(CreateNameConversation);
			Register(CreateGroupContacts);
			Register(CreateWebView);
			Register(CreateReportProblem);
			Register(CreateMuteSelection);
			Register(CreateSuggestedPins);
			Register(CreateInternalSettings);
			Register(CreateForwarding);
			Register(CreatePhoneNumberRegistration);
			Register(CreateAddContact);
			Register(CreateNewUserRegistration);
			Register(CreateProfilePic);
			Register(CreateSyncContacts);
			Register(CreateProfilePicConfirmation);
			Register(CreateExistingUser);
			Register(CreateStickerDrawerViewModel);
			Register(CreateMessengerApplicationViewModel);
			Register(CreateCanonicalThreadDetails);
			RegisterSingle((Func<IDispatcher>)(() => new UiDispatcher()));
			RegisterSingle(CreateServiceEngine);
			RegisterSingle(() => new DebugLogDisplayEnabledProvider(GetInstance<FacebookContext>()));
			RegisterSingle(() => new Navigator(GetInstance<UriMapper>(), GetInstance<Facebook.Services.Analytics.Service>(), GetInstance<SystemTrayManager>()));
			RegisterSingle(() => new ViewModelBuilder(GetInstance<UriMapper>()));
			RegisterSingle(() => new Facebook.Messenger.WP8.FBGeoLocator.Service(GetInstance<IDispatcher>(), GetInstance<FacebookContext>()));
			RegisterSingle(() => new Facebook.Messenger.WP8.Push.Service(GetInstance<IDispatcher>(), GetInstance<IEnvironmentInformation>()));
			RegisterSingle<AudioAttachmentPlaybackViewModel>();
			RegisterSingle<VideoAttachmentPlaybackViewModel>();
			RegisterSingle(CreateAudioController);
			RegisterSingle((Func<IStickerStorage>)CreateStickerStorage);
			RegisterSingle(CreatePhoneContactsManager);
			RegisterSingle(CreateMqttConnectionBannerViewModel);
			RegisterSingle(CreateStickerImageUrlHelper);
			RegisterSingle(() => new SystemTrayManager());
			RegisterSingle(CreateMessengerUriMapper);
		}

		private ConnectionBannerViewModel CreateMqttConnectionBannerViewModel()
		{
			return new ConnectionBannerViewModel(GetInstance<FacebookContext>(), GetInstance<INetworkStatus>());
		}

		private MessengerApplicationViewModel CreateMessengerApplicationViewModel()
		{
			return new MessengerApplicationViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<ApplicationState>(), GetInstance<ConnectionBannerViewModel>());
		}

		protected override List<ICache> GetCaches()
		{
			List<ICache> list = new List<ICache>();
			list.Add(GetInstance<ThreadCache>());
			list.Add(GetInstance<ContactCache>());
			list.Add(GetInstance<AnalyticsCache>());
			list.Add(GetInstance<StickerPackCache>());
			list.Add(GetInstance<PhoneContactCache>());
			list.Add(ImageCacheExtensions.Cache);
			list.Add(MediaElementCacheExtensions.Cache);
			return list;
		}

		private StickerStorage CreateStickerStorage()
		{
			return new StickerStorage(GetInstance<FacebookContext>(), GetInstance<Facebook.Services.Analytics.Service>());
		}

		private StickerImageUrlHelper CreateStickerImageUrlHelper()
		{
			return new StickerImageUrlHelper(GetInstance<FacebookContext>(), GetInstance<Facebook.Services.Stickers.Service>());
		}

		private PhoneContactsManager CreatePhoneContactsManager()
		{
			return new PhoneContactsManager(GetInstance<Facebook.Services.Api.Service>(), GetInstance<Facebook.Services.Friends.Service>(), GetInstance<ContactSyncFeatureEnabledProvider>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private Facebook.Messenger.WP8.AudioController.Service CreateAudioController()
		{
			return new Facebook.Messenger.WP8.AudioController.Service(GetInstance<IDispatcher>(), GetInstance<FacebookContext>());
		}

		protected override IList<BackgroundService> GetInitializationOrderedServices()
		{
			return new BackgroundService[12]
			{
				GetInstance<Facebook.Messenger.WP8.AudioController.Service>(),
				GetInstance<Facebook.Services.Cache.Service>(),
				GetInstance<Facebook.Services.Analytics.Service>(),
				GetInstance<Facebook.Services.Mqtt.Service>(),
				GetInstance<Facebook.Services.MessageSender.Service>(),
				GetInstance<Facebook.Services.SyncProtocol.Service>(),
				GetInstance<Facebook.Services.Friends.Service>(),
				GetInstance<Facebook.Services.Stickers.Service>(),
				GetInstance<Facebook.Services.Presence.Service>(),
				GetInstance<Facebook.Services.ScheduledTasksService.Service>(),
				GetInstance<Facebook.Messenger.WP8.Push.Service>(),
				GetInstance<Facebook.Messenger.WP8.FBGeoLocator.Service>()
			};
		}

		private InternalSettingsViewModel CreateInternalSettings()
		{
			return new InternalSettingsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<Facebook.Messenger.WP8.AudioController.Service>(), GetInstance<Facebook.Services.SyncProtocol.Service>(), GetInstance<ThreadCache>());
		}

		private ForwardingViewModel CreateForwarding()
		{
			return new ForwardingViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<ContactSearchViewModel>());
		}

		private AddContactViewModel CreateAddContact()
		{
			return new AddContactViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<PhoneContactsManager>());
		}

		private SuggestedPinsViewModel CreateSuggestedPins()
		{
			return new SuggestedPinsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private MuteSelectionViewModel CreateMuteSelection()
		{
			return new MuteSelectionViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private ReportProblemViewModel CreateReportProblem()
		{
			return new ReportProblemViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<IEnvironmentInformation>());
		}

		private WebViewViewModel CreateWebView()
		{
			return new WebViewViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private GroupContactsViewModel CreateGroupContacts()
		{
			return new GroupContactsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<ContactSyncFeatureEnabledProvider>(), GetInstance<PhoneContactsManager>());
		}

		private NameConversationViewModel CreateNameConversation()
		{
			return new NameConversationViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private SettingsViewModel CreateSettings()
		{
			return new SettingsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<Facebook.Messenger.WP8.FBGeoLocator.Service>(), GetInstance<PhoneContactsManager>(), GetInstance<ContactSyncFeatureEnabledProvider>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<IEnvironmentInformation>());
		}

		private ActiveUsersViewModel CreateActiveUsers()
		{
			return new ActiveUsersViewModel(GetInstance<Facebook.Services.Presence.Service>(), GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private StickerStorePackViewModel CreateStickerPack()
		{
			return new StickerStorePackViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<Facebook.Services.Stickers.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<IStickerStorage>());
		}

		private StickerStoreViewModel CreateStickerStore()
		{
			return new StickerStoreViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<Facebook.Services.Stickers.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<IStickerStorage>());
		}

		private MessageDetailsViewModel CreateMessageDetails()
		{
			return new MessageDetailsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private PhotoAttachmentsViewModel CreatePhotoAttachments()
		{
			return new PhotoAttachmentsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private VerifyPhoneNumberViewModel CreateVerifyPhone()
		{
			return new VerifyPhoneNumberViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private PhoneNumberViewModel CreatePhoneNumber()
		{
			return new PhoneNumberViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private PhoneNumberRegistrationViewModel CreatePhoneNumberRegistration()
		{
			return new PhoneNumberRegistrationViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private NewUserRegistrationViewModel CreateNewUserRegistration()
		{
			return new NewUserRegistrationViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private AddProfilePicViewModel CreateProfilePic()
		{
			return new AddProfilePicViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private ProfilePicConfirmationViewModel CreateProfilePicConfirmation()
		{
			return new ProfilePicConfirmationViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private ExistingUserViewModel CreateExistingUser()
		{
			return new ExistingUserViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private StickerDrawerViewModel CreateStickerDrawerViewModel()
		{
			return new StickerDrawerViewModel(GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<Facebook.Services.Stickers.Service>(), GetInstance<StickerDrawerBadgeEnabledProvider>(), GetInstance<IStickerStorage>());
		}

		private SyncContactsViewModel CreateSyncContacts()
		{
			return new SyncContactsViewModel(GetInstance<PhoneContactsManager>(), GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private MessagesViewModel CreateMessages()
		{
			return new MessagesViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<Facebook.Services.MessageSender.Service>(), GetInstance<Facebook.Messenger.WP8.FBGeoLocator.Service>(), GetInstance<StickerDrawerViewModel>(), GetInstance<Facebook.Messenger.WP8.AudioController.Service>(), GetInstance<Facebook.Services.Presence.Service>(), GetInstance<IFileAccess>());
		}

		private FacebookMessengerViewModel CreateGeneric()
		{
			return new FacebookMessengerViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private MainViewModel CreateMainViewModel()
		{
			return new MainViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<INetworkStatus>(), GetInstance<IDispatcher>());
		}

		private UpgradePromptViewModel CreateUpgradePromptViewModel()
		{
			return new UpgradePromptViewModel(GetInstance<FacebookContext>(), GetInstance<UpgradePromptEnabledProvider>());
		}

		private NuxOnMessengerViewModel CreateNuxOnMessenger()
		{
			return new NuxOnMessengerViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private LoginViewModel CreateLogin()
		{
			return new LoginViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<Facebook.Services.Http.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<ICrypto>(), GetInstance<FacebookConfig>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private GroupComposerAddUsersViewModel CreateComposerAddUsers()
		{
			return new GroupComposerAddUsersViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<ContactSyncFeatureEnabledProvider>(), GetInstance<PhoneContactsManager>());
		}

		private GroupComposerViewModel CreateGroupComposer()
		{
			return new GroupComposerViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private GroupDetailsViewModel CreateGroupDetails()
		{
			return new GroupDetailsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<IFileAccess>());
		}

		private CanonicalThreadDetailsViewModel CreateCanonicalThreadDetails()
		{
			return new CanonicalThreadDetailsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private MessageComposerViewModel CreateComposer()
		{
			return new MessageComposerViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<Facebook.Services.MessageSender.Service>(), GetInstance<Facebook.Messenger.WP8.FBGeoLocator.Service>(), GetInstance<StickerDrawerViewModel>(), GetInstance<Facebook.Messenger.WP8.AudioController.Service>(), GetInstance<IFileAccess>());
		}

		private PinnedThreadsViewModel CreatePinnedThreads()
		{
			return new PinnedThreadsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<Facebook.Messenger.WP8.AudioController.Service>());
		}

		private ThreadsViewModel CreateThreads()
		{
			return new ThreadsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<Facebook.Messenger.WP8.AudioController.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<ConnectionBannerViewModel>());
		}

		private ContactSearchViewModel CreateContactSearch()
		{
			return new ContactSearchViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>());
		}

		private ContactsViewModel CreateContacts()
		{
			return new ContactsViewModel(GetInstance<Facebook.Services.Api.Service>(), GetInstance<FacebookContext>(), GetInstance<Navigator>(), GetInstance<IDispatcher>(), GetInstance<INetworkStatus>(), GetInstance<ContactSyncFeatureEnabledProvider>(), GetInstance<PhoneContactsManager>());
		}

		private UriMapper CreateMessengerUriMapper()
		{
			UriMapper uriMapper = new UriMapper();
			uriMapper.Map(FacebookUri.LoginUri, "/Pages/LoginPage.xaml");
			uriMapper.Map(FacebookUri.PhoneNumberRegistration, "/Pages/PhoneNumberRegistrationPage.xaml");
			uriMapper.Map(FacebookUri.NewUserRegistration, "/Pages/NewUserRegistrationPage.xaml");
			uriMapper.Map(FacebookUri.ExistingUser, "/Pages/ExistingUserPage.xaml");
			uriMapper.Map(FacebookUri.CreateProfilePic, "/Pages/AddProfilePicPage.xaml");
			uriMapper.Map(FacebookUri.SyncContacts, "/Pages/SyncContactsPage.xaml");
			uriMapper.Map(FacebookUri.ProfilePicConfirmation, "/Pages/ProfilePicConfirmationPage.xaml");
			uriMapper.Map(FacebookUri.RootUri, "/Pages/MessengerMainPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.Thread), "/Pages/ThreadPage.xaml");
			uriMapper.Map(FacebookUri.ComposerUri, "/Pages/ComposerBarPage.xaml");
			uriMapper.Map(FacebookUri.GroupComposerUri, "/Pages/GroupComposerPage.xaml");
			uriMapper.Map(FacebookUri.GroupDetailsUri, "/Pages/GroupDetailsPage.xaml");
			uriMapper.Map(FacebookUri.CanonicalThreadDetailsUri, "/Pages/CanonicalThreadDetailsPage.xaml");
			uriMapper.Map(FacebookUri.LoginApprovalUri, "/Pages/LoginApprovalPage.xaml");
			uriMapper.Map(FacebookUri.NuxOnMessengerUri, "/Pages/NuxOnMessengerPage.xaml");
			uriMapper.Map(FacebookUri.PhoneNumberUri, "/Pages/PhoneNumberPage.xaml");
			uriMapper.Map(FacebookUri.VerifyPhoneNumberUri, "/Pages/VerifyPhoneNumberPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.Attachments), "/Pages/PhotoAttachmentsPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.MessageDetails), "/Pages/MessageDetailsPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.LongMessageDetails), "/Pages/LongMessageDetailsPage.xaml");
			uriMapper.Map(FacebookUri.StickerStoreUri, "/Pages/StickerStorePage.xaml");
			uriMapper.Map(FacebookUri.StickerPackUri, "/Pages/StickerStorePackPage.xaml");
			uriMapper.Map(FacebookUri.NameConversationUri, "/Pages/NameConversationPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.AddPeopleToGroup), "/Pages/AddUserToGroup.xaml");
			uriMapper.Map(FacebookUri.WebViewUri, "/Pages/WebViewPage.xaml");
			uriMapper.Map(FacebookUri.SettingsUri, "/Pages/SettingsPage.xaml");
			uriMapper.Map(FacebookUri.ReportProblemUri, "/Pages/ReportPage.xaml");
			uriMapper.Map(FacebookUri.MuteSelectionUri, "/Pages/MuteSelectionPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.GroupComposerAddUser), "/Pages/GroupComposerAddUsersPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.SuggestedPins), "/Pages/SuggestedPinsPage.xaml");
			uriMapper.Map(FacebookUri.InternalSettingsUri, "/Pages/InternalSettingsPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.Forwarding), "/Pages/ForwardingPage.xaml");
			uriMapper.Map(FacebookUri.GetAppUri(UriId.AddContact), "/Pages/AddContactPage.xaml");
			return uriMapper;
		}
	}
}
