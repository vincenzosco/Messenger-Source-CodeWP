using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Analytics;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.AddContact;
using Facebook.Services.Api.Methods.AddPinThread;
using Facebook.Services.Api.Methods.AddStickerPack;
using Facebook.Services.Api.Methods.AddThreadParticipants;
using Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials;
using Facebook.Services.Api.Methods.ClearThreadPicture;
using Facebook.Services.Api.Methods.ContactsMessengerSync;
using Facebook.Services.Api.Methods.CreateGroup;
using Facebook.Services.Api.Methods.CreateNewUserAccount;
using Facebook.Services.Api.Methods.DeleteContact;
using Facebook.Services.Api.Methods.DeleteMessage;
using Facebook.Services.Api.Methods.DeleteThread;
using Facebook.Services.Api.Methods.FetchAppConfig;
using Facebook.Services.Api.Methods.FetchChangedPhoneContacts;
using Facebook.Services.Api.Methods.FetchContactsById;
using Facebook.Services.Api.Methods.FetchFriendLastActive;
using Facebook.Services.Api.Methods.FetchFriends;
using Facebook.Services.Api.Methods.FetchFriendsOfFriends;
using Facebook.Services.Api.Methods.FetchGatekeeperSettings;
using Facebook.Services.Api.Methods.FetchGroupSearchResults;
using Facebook.Services.Api.Methods.FetchGroupThreads;
using Facebook.Services.Api.Methods.FetchInitialThreadListSyncData;
using Facebook.Services.Api.Methods.FetchLocalThreadData;
using Facebook.Services.Api.Methods.FetchMessage;
using Facebook.Services.Api.Methods.FetchMessages;
using Facebook.Services.Api.Methods.FetchPinnedThreads;
using Facebook.Services.Api.Methods.FetchPresence;
using Facebook.Services.Api.Methods.FetchProfile;
using Facebook.Services.Api.Methods.FetchRecentStickers;
using Facebook.Services.Api.Methods.FetchSearchability;
using Facebook.Services.Api.Methods.FetchSticker;
using Facebook.Services.Api.Methods.FetchStickerPack;
using Facebook.Services.Api.Methods.FetchStickerPackIds;
using Facebook.Services.Api.Methods.FetchStickerPacks;
using Facebook.Services.Api.Methods.FetchThread;
using Facebook.Services.Api.Methods.FetchThreads;
using Facebook.Services.Api.Methods.FetchThreadsAndMessages;
using Facebook.Services.Api.Methods.FetchUser;
using Facebook.Services.Api.Methods.FetchUserById;
using Facebook.Services.Api.Methods.LeaveThread;
using Facebook.Services.Api.Methods.Login;
using Facebook.Services.Api.Methods.MarkInboxSeen;
using Facebook.Services.Api.Methods.MarkMessageFailedById;
using Facebook.Services.Api.Methods.MarkMessagesSent;
using Facebook.Services.Api.Methods.MarkQueuedMessagesFailed;
using Facebook.Services.Api.Methods.MarkThreadState;
using Facebook.Services.Api.Methods.MediaItemUpload;
using Facebook.Services.Api.Methods.PhoneNumber;
using Facebook.Services.Api.Methods.PhoneNumberConfirmCode;
using Facebook.Services.Api.Methods.PhoneNumberRegistration;
using Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation;
using Facebook.Services.Api.Methods.RegisterPush;
using Facebook.Services.Api.Methods.RemovePhoneContacts;
using Facebook.Services.Api.Methods.RemoveThreadParticipants;
using Facebook.Services.Api.Methods.ReportCrash;
using Facebook.Services.Api.Methods.SaveOutgoingMessage;
using Facebook.Services.Api.Methods.SearchImages;
using Facebook.Services.Api.Methods.SendAnalyticsEvents;
using Facebook.Services.Api.Methods.SendFlytrapReport;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.Api.Methods.SetGlobalMute;
using Facebook.Services.Api.Methods.SetPinnedThreads;
using Facebook.Services.Api.Methods.SetStickerTrayPacks;
using Facebook.Services.Api.Methods.SetThreadMute;
using Facebook.Services.Api.Methods.SetThreadName;
using Facebook.Services.Api.Methods.SetThreadPicture;
using Facebook.Services.Api.Methods.SyncContacts;
using Facebook.Services.Api.Methods.SyncFriends;
using Facebook.Services.Api.Methods.SyncStickerPacks;
using Facebook.Services.Api.Methods.ThreadQuery;
using Facebook.Services.Api.Methods.UnregisterPush;
using Facebook.Services.Api.Methods.UploadProfilePicture;
using Facebook.Services.BackgroundTransfer;
using Facebook.Services.Common;
using Facebook.Services.Http;
using Facebook.Services.MessageSender;
using Facebook.Services.Mqtt;

namespace Facebook.Services.Api;

public class Service : Facebook.Base.Service
{
	private readonly Dictionary<Type, FilterChain> _classTypeToChain;

	private readonly Dictionary<QueueType, FilterChain> _typeToChain;

	private readonly Dictionary<BundleType, Bundle> _typeToBundle;

	public Service(FacebookContext fbcontext, FacebookConfig config, Facebook.Services.Http.Service http, Facebook.Services.Mqtt.Service mqtt, Facebook.Services.Analytics.Service analytics, Facebook.Services.BackgroundTransfer.Service backgroundTransferService, Facebook.Services.MessageSender.Service sender, IDispatcher dispatcher, ICrypto crypto, ThreadCache threadCache, ContactCache contactCache, StickerPackCache stickerPackCache, PhoneContactCache phoneContactCache, ITranslator translator, ApplicationState applicationState)
	{
		_classTypeToChain = new Dictionary<Type, FilterChain>();
		_typeToBundle = new Dictionary<BundleType, Bundle>();
		_typeToChain = new Dictionary<QueueType, FilterChain>();
		ServiceContext serviceContext = new ServiceContext
		{
			Config = config,
			ApiService = this,
			HttpService = http,
			Crypto = crypto,
			ThreadCache = threadCache,
			ContactCache = contactCache,
			StickerPackCache = stickerPackCache,
			PhoneContactCache = phoneContactCache,
			MqttService = mqtt,
			AnalyticsService = analytics,
			BackgroundTransferService = backgroundTransferService,
			MessageSender = sender,
			FBContext = fbcontext,
			ApplicationState = applicationState
		};
		Bundle bundle = new Bundle(serviceContext);
		Bundle bundle2 = new Bundle(serviceContext);
		AuthCheckFilter authCheckFilter = new AuthCheckFilter(fbcontext, dispatcher);
		LocaleFilter localeFilter = new LocaleFilter(translator);
		Bundle bundle3 = new Bundle(serviceContext);
		Bundle bundle4 = new Bundle(serviceContext);
		Bundle bundle5 = new Bundle(serviceContext);
		FilterChain value = CreateTerminatingFilterChain(authCheckFilter, bundle5, bundle2, bundle3, localeFilter, bundle4, bundle);
		FilterChain value2 = CreateTerminatingFilterChain(bundle5, bundle2);
		_typeToChain[QueueType.Web] = value;
		_typeToChain[QueueType.Local] = value2;
		_typeToBundle[BundleType.Web] = bundle;
		_typeToBundle[BundleType.InMemoryCache] = bundle2;
		_typeToBundle[BundleType.Mqtt] = bundle3;
		_typeToBundle[BundleType.BackgroundTransfer] = bundle4;
		_typeToBundle[BundleType.PreCache] = bundle5;
		InitTypes();
	}

	protected override Task<TResponse> InnerExecuteAsync<TRequest, TResponse>(TRequest request)
	{
		Type typeFromHandle = typeof(TRequest);
		FilterChain filterChain = _classTypeToChain[typeFromHandle];
		return filterChain.ExecuteAsync<TRequest, TResponse>(request);
	}

	private static FilterChain CreateTerminatingFilterChain(params IFilter[] filtersInOrder)
	{
		IEnumerable<IFilter> source = filtersInOrder.Reverse();
		TerminatingHandler next = new TerminatingHandler();
		FilterChain filterChain = new FilterChain(source.First(), next);
		foreach (IFilter item in source.Skip(1))
		{
			filterChain = new FilterChain(item, filterChain);
		}
		return filterChain;
	}

	public void InitTypes()
	{
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Request), typeof(Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Response), typeof(Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchRecentStickers.Request), typeof(Facebook.Services.Api.Methods.FetchRecentStickers.Response), typeof(Facebook.Services.Api.Methods.FetchRecentStickers.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchStickerPack.Request), typeof(Facebook.Services.Api.Methods.FetchStickerPack.Response), typeof(Facebook.Services.Api.Methods.FetchStickerPack.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchThreadsAndMessages.Request), typeof(Facebook.Services.Api.Methods.FetchThreadsAndMessages.Response), typeof(Facebook.Services.Api.Methods.FetchThreadsAndMessages.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.MarkMessageFailedById.Request), typeof(Facebook.Services.Api.Methods.MarkMessageFailedById.Response), typeof(Facebook.Services.Api.Methods.MarkMessageFailedById.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.MarkMessagesSent.Request), typeof(Facebook.Services.Api.Methods.MarkMessagesSent.Response), typeof(Facebook.Services.Api.Methods.MarkMessagesSent.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchStickerPacks.Request), typeof(Facebook.Services.Api.Methods.FetchStickerPacks.Response), typeof(Facebook.Services.Api.Methods.FetchStickerPacks.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.SyncStickerPacks.Request), typeof(Facebook.Services.Api.Methods.SyncStickerPacks.Response), typeof(Facebook.Services.Api.Methods.SyncStickerPacks.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SyncStickerPacks.Request), typeof(Facebook.Services.Api.Methods.SyncStickerPacks.Response), typeof(Facebook.Services.Api.Methods.SyncStickerPacks.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.ThreadQuery.Request), typeof(Facebook.Services.Api.Methods.ThreadQuery.Response), typeof(Facebook.Services.Api.Methods.ThreadQuery.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.ThreadQuery.Request), typeof(Facebook.Services.Api.Methods.ThreadQuery.Response), typeof(Facebook.Services.Api.Methods.ThreadQuery.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.AddContact.Request), typeof(Facebook.Services.Api.Methods.AddContact.Response), typeof(Facebook.Services.Api.Methods.AddContact.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.AddPinThread.Request), typeof(Facebook.Services.Api.Methods.AddPinThread.Response), typeof(Facebook.Services.Api.Methods.AddPinThread.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.AddPinThread.Request), typeof(Facebook.Services.Api.Methods.AddPinThread.Response), typeof(Facebook.Services.Api.Methods.AddPinThread.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.AddStickerPack.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.AddStickerPack.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.AddThreadParticipants.Request), typeof(Facebook.Services.Api.Methods.AddThreadParticipants.Response), typeof(Facebook.Services.Api.Methods.AddThreadParticipants.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.AddThreadParticipants.Request), typeof(Facebook.Services.Api.Methods.AddThreadParticipants.Response), typeof(Facebook.Services.Api.Methods.AddThreadParticipants.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials.Request), typeof(Facebook.Services.Api.Methods.Login.Response), typeof(Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.ClearThreadPicture.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.ClearThreadPicture.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.ClearThreadPicture.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.ClearThreadPicture.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.ContactsMessengerSync.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.ContactsMessengerSync.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.CreateGroup.Request), typeof(Facebook.Services.Api.Methods.CreateGroup.Response), typeof(Facebook.Services.Api.Methods.CreateGroup.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.CreateGroup.Request), typeof(Facebook.Services.Api.Methods.CreateGroup.Response), typeof(Facebook.Services.Api.Methods.CreateGroup.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.CreateNewUserAccount.Request), typeof(Facebook.Services.Api.Methods.Login.Response), typeof(Facebook.Services.Api.Methods.CreateNewUserAccount.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.DeleteContact.Request), typeof(Facebook.Services.Api.Methods.DeleteContact.Response), typeof(Facebook.Services.Api.Methods.DeleteContact.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.DeleteContact.Request), typeof(Facebook.Services.Api.Methods.DeleteContact.Response), typeof(Facebook.Services.Api.Methods.DeleteContact.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.DeleteMessage.Request), typeof(Facebook.Base.Response), typeof(Facebook.Services.Api.Methods.DeleteMessage.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.DeleteMessage.Request), typeof(Facebook.Base.Response), typeof(Facebook.Services.Api.Methods.DeleteMessage.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.DeleteThread.Request), typeof(Facebook.Base.Response), typeof(Facebook.Services.Api.Methods.DeleteThread.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.DeleteThread.Request), typeof(Facebook.Base.Response), typeof(Facebook.Services.Api.Methods.DeleteThread.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchAppConfig.Request), typeof(Facebook.Services.Api.Methods.FetchAppConfig.Response), typeof(Facebook.Services.Api.Methods.FetchAppConfig.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchChangedPhoneContacts.Request), typeof(Facebook.Services.Api.Methods.FetchChangedPhoneContacts.Response), typeof(Facebook.Services.Api.Methods.FetchChangedPhoneContacts.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchContactsById.Request), typeof(Facebook.Services.Api.Methods.FetchContactsById.Response), typeof(Facebook.Services.Api.Methods.FetchContactsById.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchContactsById.Request), typeof(Facebook.Services.Api.Methods.FetchContactsById.Response), typeof(Facebook.Services.Api.Methods.FetchContactsById.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchFriendLastActive.Request), typeof(Facebook.Services.Api.Methods.FetchFriendLastActive.Response), typeof(Facebook.Services.Api.Methods.FetchFriendLastActive.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchFriendLastActive.Request), typeof(Facebook.Services.Api.Methods.FetchFriendLastActive.Response), typeof(Facebook.Services.Api.Methods.FetchFriendLastActive.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchFriends.Request), typeof(Facebook.Services.Api.Methods.FetchFriends.Response), typeof(Facebook.Services.Api.Methods.FetchFriends.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchGroupSearchResults.Request), typeof(Facebook.Services.Api.Methods.FetchGroupSearchResults.Response), typeof(Facebook.Services.Api.Methods.FetchGroupSearchResults.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchGroupThreads.Request), typeof(Facebook.Services.Api.Methods.FetchGroupThreads.Response), typeof(Facebook.Services.Api.Methods.FetchGroupThreads.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchLocalThreadData.Request), typeof(Facebook.Services.Api.Methods.FetchLocalThreadData.Response), typeof(Facebook.Services.Api.Methods.FetchLocalThreadData.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchMessages.Request), typeof(Facebook.Services.Api.Methods.FetchMessages.Response), typeof(Facebook.Services.Api.Methods.FetchMessages.CacheFilter));
		_typeToBundle[BundleType.PreCache].Add(typeof(Facebook.Services.Api.Methods.FetchMessages.Request), typeof(Facebook.Services.Api.Methods.FetchMessages.Response), typeof(SendFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchMessage.Request), typeof(Facebook.Services.Api.Methods.FetchMessage.Response), typeof(Facebook.Services.Api.Methods.FetchMessage.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchMessage.Request), typeof(Facebook.Services.Api.Methods.FetchMessage.Response), typeof(Facebook.Services.Api.Methods.FetchMessage.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchPinnedThreads.Request), typeof(Facebook.Services.Api.Methods.FetchPinnedThreads.Response), typeof(Facebook.Services.Api.Methods.FetchPinnedThreads.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchPinnedThreads.Request), typeof(Facebook.Services.Api.Methods.FetchPinnedThreads.Response), typeof(Facebook.Services.Api.Methods.FetchPinnedThreads.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchPresence.Request), typeof(Facebook.Services.Api.Methods.FetchPresence.Response), typeof(Facebook.Services.Api.Methods.FetchPresence.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.AddStickerPack.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.AddStickerPack.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchSearchability.Request), typeof(Facebook.Services.Api.Methods.FetchSearchability.Response), typeof(Facebook.Services.Api.Methods.FetchSearchability.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchStickerPackIds.Request), typeof(Facebook.Services.Api.Methods.FetchStickerPackIds.Response), typeof(Facebook.Services.Api.Methods.FetchStickerPackIds.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchSticker.Request), typeof(Facebook.Services.Api.Methods.FetchSticker.Response), typeof(Facebook.Services.Api.Methods.FetchSticker.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchThreads.Request), typeof(Facebook.Services.Api.Methods.FetchThreads.Response), typeof(Facebook.Services.Api.Methods.FetchThreads.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.FetchThread.Request), typeof(Facebook.Services.Api.Methods.FetchThread.Response), typeof(Facebook.Services.Api.Methods.FetchThread.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchThread.Request), typeof(Facebook.Services.Api.Methods.FetchThread.Response), typeof(Facebook.Services.Api.Methods.FetchThread.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchFriendsOfFriends.Request), typeof(Facebook.Services.Api.Methods.FetchFriendsOfFriends.Response), typeof(Facebook.Services.Api.Methods.FetchFriendsOfFriends.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchUserById.Request), typeof(Facebook.Services.Api.Methods.FetchUserById.Response), typeof(Facebook.Services.Api.Methods.FetchUserById.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchUser.Request), typeof(Facebook.Services.Api.Methods.FetchUser.Response), typeof(Facebook.Services.Api.Methods.FetchUser.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.LeaveThread.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.LeaveThread.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.MarkQueuedMessagesFailed.Request), typeof(Facebook.Services.Api.Methods.MarkQueuedMessagesFailed.Response), typeof(Facebook.Services.Api.Methods.MarkQueuedMessagesFailed.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.LeaveThread.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.LeaveThread.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.MediaItemUpload.Request), typeof(Facebook.Services.Api.Methods.MediaItemUpload.Response), typeof(Facebook.Services.Api.Methods.MediaItemUpload.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.MarkThreadState.Request), typeof(Facebook.Services.Api.Methods.MarkThreadState.Response), typeof(Facebook.Services.Api.Methods.MarkThreadState.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.PhoneNumberConfirmCode.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.PhoneNumberConfirmCode.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.PhoneNumberRegistration.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.PhoneNumberRegistration.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.PhoneNumber.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.PhoneNumber.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation.Request), typeof(ExistingUserResponse), typeof(Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.RemovePhoneContacts.Request), typeof(Facebook.Services.Api.Methods.RemovePhoneContacts.Response), typeof(Facebook.Services.Api.Methods.RemovePhoneContacts.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.RemoveThreadParticipants.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.RemoveThreadParticipants.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.RemoveThreadParticipants.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.RemoveThreadParticipants.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.ReportCrash.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.ReportCrash.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.SendMessage.Request), typeof(Facebook.Services.Api.Methods.SaveOutgoingMessage.Response), typeof(Facebook.Services.Api.Methods.SaveOutgoingMessage.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SearchImages.Request), typeof(Facebook.Services.Api.Methods.SearchImages.Response), typeof(Facebook.Services.Api.Methods.SearchImages.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SendAnalyticsEvents.Request), typeof(Facebook.Services.Api.Methods.SendAnalyticsEvents.Response), typeof(Facebook.Services.Api.Methods.SendAnalyticsEvents.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SendFlytrapReport.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.SendFlytrapReport.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.SendMessage.Request), typeof(Facebook.Services.Api.Methods.SendMessage.Response), typeof(Facebook.Services.Api.Methods.SendMessage.CacheFilter));
		_typeToBundle[BundleType.BackgroundTransfer].Add(typeof(Facebook.Services.Api.Methods.SendMessage.Request), typeof(Facebook.Services.Api.Methods.SendMessage.Response), typeof(MediaUploadFilter));
		_typeToBundle[BundleType.Mqtt].Add(typeof(Facebook.Services.Api.Methods.SendMessage.Request), typeof(Facebook.Services.Api.Methods.SendMessage.Response), typeof(MqttFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SetGlobalMute.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.SetGlobalMute.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.SetPinnedThreads.Request), typeof(Facebook.Services.Api.Methods.SetPinnedThreads.Response), typeof(Facebook.Services.Api.Methods.SetPinnedThreads.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SetPinnedThreads.Request), typeof(Facebook.Services.Api.Methods.SetPinnedThreads.Response), typeof(Facebook.Services.Api.Methods.SetPinnedThreads.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SetStickerTrayPacks.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.SetStickerTrayPacks.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.SetThreadMute.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.SetThreadMute.CacheFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.SetThreadName.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.SetThreadName.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SetThreadName.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.SetThreadName.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.SetThreadPicture.Request), typeof(Facebook.Services.Api.Methods.SetThreadPicture.Response), typeof(Facebook.Services.Api.Methods.SetThreadPicture.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SetThreadPicture.Request), typeof(Facebook.Services.Api.Methods.SetThreadPicture.Response), typeof(Facebook.Services.Api.Methods.SetThreadPicture.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.RegisterPush.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.RegisterPush.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SyncContacts.Request), typeof(Facebook.Services.Api.Methods.SyncContacts.Response), typeof(Facebook.Services.Api.Methods.SyncContacts.WebFilter));
		_typeToBundle[BundleType.InMemoryCache].Add(typeof(Facebook.Services.Api.Methods.SyncFriends.Request), typeof(Facebook.Services.Api.Methods.SyncFriends.Response), typeof(Facebook.Services.Api.Methods.SyncFriends.CacheFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.UnregisterPush.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.UnregisterPush.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SetThreadMute.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.SetThreadMute.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchGatekeeperSettings.Request), typeof(Facebook.Services.Api.Methods.FetchGatekeeperSettings.Response), typeof(Facebook.Services.Api.Methods.FetchGatekeeperSettings.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.MarkThreadState.Request), typeof(Facebook.Services.Api.Methods.MarkThreadState.Response), typeof(Facebook.Services.Api.Methods.MarkThreadState.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchMessages.Request), typeof(Facebook.Services.Api.Methods.FetchMessages.Response), typeof(Facebook.Services.Api.Methods.FetchMessages.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchProfile.Request), typeof(Facebook.Services.Api.Methods.FetchProfile.Response), typeof(Facebook.Services.Api.Methods.FetchProfile.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.MarkInboxSeen.Request), typeof(BoolResponse), typeof(Facebook.Services.Api.Methods.MarkInboxSeen.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SendMessage.Request), typeof(Facebook.Services.Api.Methods.SendMessage.Response), typeof(Facebook.Services.Api.Methods.SendMessage.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.SyncFriends.Request), typeof(Facebook.Services.Api.Methods.SyncFriends.Response), typeof(Facebook.Services.Api.Methods.SyncFriends.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.Login.Request), typeof(Facebook.Services.Api.Methods.Login.Response), typeof(Facebook.Services.Api.Methods.Login.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.FetchThreads.Request), typeof(Facebook.Services.Api.Methods.FetchThreads.Response), typeof(Facebook.Services.Api.Methods.FetchThreads.WebFilter));
		_typeToBundle[BundleType.Web].Add(typeof(Facebook.Services.Api.Methods.UploadProfilePicture.Request), typeof(Facebook.Services.Api.Methods.UploadProfilePicture.Response), typeof(Facebook.Services.Api.Methods.UploadProfilePicture.WebFilter));
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchRecentStickers.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchStickerPack.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchThreadsAndMessages.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.MarkMessageFailedById.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.MarkMessagesSent.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchStickerPacks.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SyncStickerPacks.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.ThreadQuery.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.AddContact.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.AddPinThread.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.AddThreadParticipants.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.ClearThreadPicture.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.ContactsMessengerSync.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.CreateGroup.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.CreateNewUserAccount.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.DeleteContact.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.DeleteMessage.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.DeleteThread.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchAppConfig.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchChangedPhoneContacts.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchContactsById.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchFriendLastActive.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchGroupSearchResults.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchGroupThreads.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchMessage.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchPinnedThreads.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchPresence.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.AddStickerPack.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchLocalThreadData.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchSearchability.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchStickerPackIds.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchSticker.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchThread.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchFriendsOfFriends.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchUserById.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchUser.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.LeaveThread.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.MarkQueuedMessagesFailed.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.MediaItemUpload.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.PhoneNumberConfirmCode.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.PhoneNumberRegistration.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.PhoneNumber.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.PhoneNumberRegistrationConfirmation.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.RemovePhoneContacts.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.RemoveThreadParticipants.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.ReportCrash.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SearchImages.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SendAnalyticsEvents.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SendFlytrapReport.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SetGlobalMute.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SetPinnedThreads.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SetStickerTrayPacks.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SetThreadName.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SetThreadPicture.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.RegisterPush.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SyncContacts.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.UnregisterPush.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SetThreadMute.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchGatekeeperSettings.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.MarkThreadState.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchFriends.Request)] = _typeToChain[QueueType.Local];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchMessages.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchProfile.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.FetchThreads.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.MarkInboxSeen.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SendMessage.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.SyncFriends.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.Login.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(Facebook.Services.Api.Methods.UploadProfilePicture.Request)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(CreateSyncQueueRequest)] = _typeToChain[QueueType.Web];
		_classTypeToChain[typeof(CreateSyncDiffsRequest)] = _typeToChain[QueueType.Web];
	}
}
