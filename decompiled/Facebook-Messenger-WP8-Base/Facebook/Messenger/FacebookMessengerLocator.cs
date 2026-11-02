using System;
using System.Collections.Generic;
using Facebook.Base;
using Facebook.Base.BackgroundTransfer;
using Facebook.Base.Mqtt;
using Facebook.Base.Network;
using Facebook.Base.Settings;
using Facebook.Messenger.Localization;
using Facebook.Messenger.Mqtt;
using Facebook.Messenger.Network;
using Facebook.Messenger.Settings;
using Facebook.Models;
using Facebook.Models.Utilities;
using Facebook.Services;
using Facebook.Services.Analytics;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.BackgroundTransfer;
using Facebook.Services.Cache;
using Facebook.Services.Friends;
using Facebook.Services.Http;
using Facebook.Services.Localization;
using Facebook.Services.MessageSender;
using Facebook.Services.Mqtt;
using Facebook.Services.Presence;
using Facebook.Services.ScheduledTasksService;
using Facebook.Services.Stickers;
using Facebook.Services.SyncProtocol;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger;

public abstract class FacebookMessengerLocator : IServiceLocator, IServiceProvider
{
	private readonly Dictionary<Type, object> _container;

	protected FacebookMessengerLocator()
	{
		_container = new Dictionary<Type, object>();
		RegisterSingle((Func<ICrypto>)(() => new Crypto()));
		RegisterSingle((Func<IFileAccess>)(() => new FileAccess()));
		RegisterSingle((Func<IUserDataStorage>)(() => new UserDataStorage(GetInstance<FacebookContext>())));
		RegisterSingle((Func<IUserAgentGenerator>)(() => new UserAgentGenerator(GetInstance<IEnvironmentInformation>(), GetInstance<ITranslator>())));
		RegisterSingle(CreateThreadCache);
		RegisterSingle(() => new ContactCache(GetInstance<IUserDataStorage>()));
		RegisterSingle(() => new AnalyticsCache(GetInstance<IUserDataStorage>()));
		RegisterSingle(CreateStickerPackCache);
		RegisterSingle(() => new PhoneContactCache(GetInstance<IUserDataStorage>()));
		RegisterSingle((Func<AbstractBackgroundTransferClient>)(() => new BackgroundTransferClient()));
		RegisterSingle(() => new ContactSyncFeatureEnabledProvider(GetInstance<FacebookContext>()));
		RegisterSingle(() => new DeltaSentMessageEnabledProvider(GetInstance<FacebookContext>()));
		RegisterSingle(() => new BackgroundSendingAgentEnabledProvider(GetInstance<FacebookContext>()));
		RegisterSingle(() => new UpgradePromptEnabledProvider(GetInstance<FacebookContext>()));
		RegisterSingle(() => new StickerDrawerBadgeEnabledProvider(GetInstance<FacebookContext>()));
		RegisterSingle((Func<IEnvironmentInformation>)(() => new EnvironmentInformation(GetInstance<FacebookConfig>())));
		RegisterSingle((Func<INetworkStatus>)(() => new NetworkStatus(GetInstance<IDispatcher>())));
		RegisterSingle(CreateApplicationState);
		RegisterSingle(CreateAnalyticsService);
		RegisterSingle(CreateApiService);
		RegisterSingle((Func<IPendingRequestStorage<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>>)(() => new PendingMessageStorage()));
		RegisterSingle(CreateBackgroundTransferService);
		RegisterSingle(CreateHttpService);
		RegisterSingle(CreateMqttService);
		RegisterSingle(CreatePresenceService);
		RegisterSingle(CreateFriendsService);
		RegisterSingle(CreateStickersService);
		RegisterSingle(CreateMessageSenderService);
		RegisterSingle(CreateScheduledTasksService);
		RegisterSingle(CreateSyncProtocolService);
		RegisterSingle(CreateCacheService);
		RegisterSingle(CreateServiceEngine);
		RegisterSingle((Func<AbstractMqttClient>)(() => new MqttClient()));
		RegisterSingle((Func<ITranslator>)(() => new Translator()));
		RegisterSingle(CreateActiveUserUtils);
		RegisterSingle(GetDefaultConfig);
		RegisterSingle(CreateFacebookContext);
		RegisterSingle((Func<IUserSettingsProvider>)(() => new UserSettingsProvider()));
		RegisterSingle((Func<IGlobalSettings>)(() => new GlobalSettings()));
		RegisterSingle((Func<IPasswordSettings>)(() => new PasswordSettings()));
		Register(CreateSyncProtocolConnectionHelper);
		Register((Func<IThreadSnippetGenerator>)(() => new ThreadSnippetGenerator(GetInstance<FacebookContext>())));
	}

	private ThreadCache CreateThreadCache()
	{
		return new ThreadCache(GetInstance<IUserDataStorage>(), GetInstance<IDispatcher>(), GetInstance<IThreadSnippetGenerator>());
	}

	private Facebook.Services.Analytics.Service CreateAnalyticsService()
	{
		return new Facebook.Services.Analytics.Service(GetInstance<IDispatcher>(), GetInstance<IEnvironmentInformation>(), GetInstance<AnalyticsCache>(), GetInstance<ApplicationState>());
	}

	private Facebook.Services.Api.Service CreateApiService()
	{
		return new Facebook.Services.Api.Service(GetInstance<FacebookContext>(), GetInstance<FacebookConfig>(), GetInstance<Facebook.Services.Http.Service>(), GetInstance<Facebook.Services.Mqtt.Service>(), GetInstance<Facebook.Services.Analytics.Service>(), GetInstance<Facebook.Services.BackgroundTransfer.Service>(), GetInstance<Facebook.Services.MessageSender.Service>(), GetInstance<IDispatcher>(), GetInstance<ICrypto>(), GetInstance<ThreadCache>(), GetInstance<ContactCache>(), GetInstance<StickerPackCache>(), GetInstance<PhoneContactCache>(), GetInstance<ITranslator>(), GetInstance<ApplicationState>());
	}

	protected virtual ApplicationState CreateApplicationState()
	{
		return new ApplicationState(ApplicationState.Runtype.Foreground);
	}

	private Facebook.Services.BackgroundTransfer.Service CreateBackgroundTransferService()
	{
		return new Facebook.Services.BackgroundTransfer.Service(GetInstance<FacebookContext>(), GetInstance<IFileAccess>(), GetInstance<INetworkStatus>(), GetInstance<AbstractBackgroundTransferClient>(), GetInstance<IUserAgentGenerator>());
	}

	private Facebook.Services.Http.Service CreateHttpService()
	{
		return new Facebook.Services.Http.Service(GetInstance<FacebookContext>(), GetInstance<IFileAccess>(), GetInstance<INetworkStatus>(), GetInstance<IUserAgentGenerator>());
	}

	private Facebook.Services.Mqtt.Service CreateMqttService()
	{
		return new Facebook.Services.Mqtt.Service(GetInstance<IDispatcher>(), GetInstance<IUserAgentGenerator>(), GetInstance<Facebook.Services.Analytics.Service>(), GetInstance<DeltaSentMessageEnabledProvider>(), GetInstance<AbstractMqttClient>(), GetInstance<INetworkStatus>(), GetInstance<ApplicationState>());
	}

	private Facebook.Services.Presence.Service CreatePresenceService()
	{
		return new Facebook.Services.Presence.Service(GetInstance<IDispatcher>(), GetInstance<Facebook.Services.Api.Service>());
	}

	private Facebook.Services.Friends.Service CreateFriendsService()
	{
		return new Facebook.Services.Friends.Service(GetInstance<IDispatcher>(), GetInstance<Facebook.Services.Api.Service>(), GetInstance<ContactCache>());
	}

	private Facebook.Services.Stickers.Service CreateStickersService()
	{
		return new Facebook.Services.Stickers.Service(GetInstance<StickerPackCache>(), GetInstance<Facebook.Services.Api.Service>(), GetInstance<IStickerStorage>(), GetInstance<INetworkStatus>(), GetInstance<IDispatcher>());
	}

	private Facebook.Services.MessageSender.Service CreateMessageSenderService()
	{
		return new Facebook.Services.MessageSender.Service(GetInstance<IDispatcher>(), GetInstance<IFileAccess>(), GetInstance<IPendingRequestStorage<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>>());
	}

	private Facebook.Services.ScheduledTasksService.Service CreateScheduledTasksService()
	{
		return new Facebook.Services.ScheduledTasksService.Service(GetInstance<IDispatcher>(), GetInstance<Facebook.Services.Api.Service>());
	}

	private Facebook.Services.SyncProtocol.Service CreateSyncProtocolService()
	{
		return new Facebook.Services.SyncProtocol.Service(GetInstance<IDispatcher>(), GetInstance<Facebook.Services.Api.Service>(), GetInstance<ThreadCache>(), GetInstance<LoggedInUserUtils>(), GetInstance<ApplicationState>(), GetInstance<SyncConnectionHelper>());
	}

	private SyncConnectionHelper CreateSyncProtocolConnectionHelper()
	{
		return new SyncConnectionHelper(GetInstance<IDispatcher>(), GetInstance<Facebook.Services.Mqtt.Service>(), GetInstance<FacebookContext>());
	}

	private Facebook.Services.Cache.Service CreateCacheService()
	{
		return new Facebook.Services.Cache.Service(GetInstance<IDispatcher>(), GetCaches(), GetInstance<ApplicationState>());
	}

	private StickerPackCache CreateStickerPackCache()
	{
		return new StickerPackCache(GetInstance<IUserDataStorage>(), GetInstance<IDispatcher>(), GetInstance<FacebookContext>());
	}

	protected abstract List<ICache> GetCaches();

	protected abstract IList<BackgroundService> GetInitializationOrderedServices();

	protected ServiceEngine CreateServiceEngine()
	{
		return new ServiceEngine(GetInitializationOrderedServices());
	}

	private FacebookContext CreateFacebookContext()
	{
		return new FacebookContext(GetInstance<IPasswordSettings>(), GetInstance<IGlobalSettings>(), GetInstance<IUserSettingsProvider>(), GetInstance<ApplicationState>());
	}

	private LoggedInUserUtils CreateActiveUserUtils()
	{
		return new LoggedInUserUtils(GetInstance<FacebookContext>());
	}

	protected void RegisterSingle<T>() where T : class, new()
	{
		_container[typeof(T)] = new Builder<T>(() => new T(), singleton: true);
	}

	protected void RegisterSingle<T>(Func<T> build) where T : class
	{
		_container[typeof(T)] = new Builder<T>(build, singleton: true);
	}

	protected void Register<T>(Func<T> build) where T : class
	{
		_container[typeof(T)] = new Builder<T>(build);
	}

	private FacebookConfig GetDefaultConfig()
	{
		FacebookConfig facebookConfig = new FacebookConfig();
		facebookConfig.ApplicationId = "192652190921494";
		facebookConfig.FlytrapCategoryId = "589198827839665";
		facebookConfig.LoginSecret = "b186d38d44bee2d9e87dd60147d651d8";
		facebookConfig.AppSecret = "4ff2ec07718d0e469655e5095761cd35";
		facebookConfig.RestApiUrl = "https://api.facebook.com/restserver.php";
		facebookConfig.GraphApiUrl = "https://graph.facebook.com/";
		facebookConfig.MobileApiUrl = "https://m.facebook.com/";
		return facebookConfig;
	}

	public IEnumerable<TService> GetAllInstances<TService>()
	{
		throw new NotSupportedException();
	}

	public IEnumerable<object> GetAllInstances(Type serviceType)
	{
		throw new NotSupportedException();
	}

	public TService GetInstance<TService>(string key)
	{
		throw new NotSupportedException();
	}

	public TService GetInstance<TService>()
	{
		try
		{
			return ((IBuilder<TService>)_container[typeof(TService)]).Get();
		}
		catch (Exception ex)
		{
			throw new ActivationException(ex.Message, ex);
		}
	}

	public object GetInstance(Type serviceType, string key)
	{
		throw new NotSupportedException();
	}

	public object GetInstance(Type serviceType)
	{
		try
		{
			return ((IBuilder<object>)_container[serviceType]).Get();
		}
		catch (Exception ex)
		{
			throw new ActivationException(ex.Message, ex);
		}
	}

	public object GetService(Type serviceType)
	{
		return GetInstance(serviceType);
	}
}
