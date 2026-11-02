using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Base.Settings;
using Facebook.Base.Strategy;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.RegisterPush;
using Facebook.Services.Api.Methods.UnregisterPush;
using Facebook.Services.Common;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Push;

public class FacebookPushRegistration
{
	private class Parameters
	{
		public readonly FacebookContext Context;

		public readonly string PushUri;

		public Parameters(FacebookContext context, string pushUri)
		{
			Context = context;
			PushUri = pushUri;
		}
	}

	private class RegisterParameters : Parameters
	{
		public readonly string CurrentAppVersion;

		public RegisterParameters(FacebookContext context, string pushUri, string currentAppVersion)
			: base(context, pushUri)
		{
			CurrentAppVersion = currentAppVersion;
		}
	}

	private const string PushChannelUriKey = "PushChannelUri";

	private const string PushChannellLastRegistrationTime = "PushChannelLastRegistrationTime";

	private const int FacebookRegistrationFallbackInitalDelayMs = 5000;

	private const int FacebookRegistrationFallbackMaxDelayMs = 30000;

	private static readonly TimeSpan FacebookRegistrationInterval = TimeSpan.FromHours(3.0);

	public static async Task<bool> EnsureFacebookRegistration(FacebookContext context, string microsoftPushUri, string appVersion, CancellationToken token)
	{
		IUserSettings settings = context.Settings;
		string pushUriRegisteredWithFacebook = settings.GetSetting<string>("PushChannelUri");
		DateTime lastRegistrationTime = settings.GetSetting("PushChannelLastRegistrationTime", DateTime.MinValue);
		bool facebookHasAnyPushUri = !string.IsNullOrWhiteSpace(pushUriRegisteredWithFacebook);
		bool facebookHasCurrentPushUri = string.Equals(microsoftPushUri, pushUriRegisteredWithFacebook);
		bool registrationIntervalReached = DateTime.UtcNow - lastRegistrationTime > FacebookRegistrationInterval;
		if (facebookHasAnyPushUri && !facebookHasCurrentPushUri && !(await UnregisterWithFacebook(context, token)))
		{
			return false;
		}
		if ((!facebookHasAnyPushUri || !facebookHasCurrentPushUri || registrationIntervalReached) && !(await RegisterWithFacebook(context, microsoftPushUri, appVersion, token)))
		{
			return false;
		}
		return true;
	}

	public static async Task<bool> UnregisterWithFacebook(FacebookContext context, CancellationToken token)
	{
		string channelUri = context.Settings.GetSetting<string>("PushChannelUri");
		if (string.IsNullOrWhiteSpace(channelUri))
		{
			return true;
		}
		ExponentialFallback<Parameters, NullResponse> fallbackStrategy = new ExponentialFallback<Parameters, NullResponse>(UnregisterPushTokenWithFacebook);
		Parameters parameters = new Parameters(context, channelUri);
		if ((await fallbackStrategy.Execute(token, parameters, 5000, 30000)).ExecutionResult != ExecutionResult.Success)
		{
			return false;
		}
		context.Settings.RemoveSetting("PushChannelUri");
		context.Settings.RemoveSetting("PushChannelLastRegistrationTime");
		return true;
	}

	private static async Task<bool> RegisterWithFacebook(FacebookContext context, string channelUri, string appVersion, CancellationToken token)
	{
		ExponentialFallback<RegisterParameters, NullResponse> fallbackStrategy = new ExponentialFallback<RegisterParameters, NullResponse>(RegisterPushTokenWithFacebook);
		RegisterParameters parameters = new RegisterParameters(context, channelUri, appVersion);
		if ((await fallbackStrategy.Execute(token, parameters, 5000, 30000)).ExecutionResult != ExecutionResult.Success)
		{
			return false;
		}
		context.Settings.SaveSetting("PushChannelUri", channelUri);
		context.Settings.SaveSetting("PushChannelLastRegistrationTime", DateTime.UtcNow);
		return true;
	}

	private static async Task<StrategyResponse<NullResponse>> RegisterPushTokenWithFacebook(int attempt, CancellationToken token, RegisterParameters parameters)
	{
		Logger.LogLocation("Attempting to register with Facebook: " + parameters.PushUri, LogLevel.Info, "RegisterPushTokenWithFacebook", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\FacebookPushRegistration.cs", 110);
		FacebookContext context = parameters.Context;
		Facebook.Services.Api.Methods.RegisterPush.Request request = new Facebook.Services.Api.Methods.RegisterPush.Request
		{
			AccessToken = context.Session.Credentials.AccessToken,
			DeviceId = context.GlobalSettings.DeviceIdentifier,
			CancelToken = token,
			ChannelUri = parameters.PushUri,
			AppVersion = parameters.CurrentAppVersion,
			ExtraParams = new Dictionary<string, string> { { "wp_notif_type", "iconic" } }
		};
		Facebook.Services.Api.Service api = ServiceLocator.Current.GetInstance<Facebook.Services.Api.Service>();
		BoolResponse response = await api.ExecuteAsync<Facebook.Services.Api.Methods.RegisterPush.Request, BoolResponse>(request);
		if (response == null || response.HasError())
		{
			Logger.Error("Unable to update push token with Facebook: " + ((response == null) ? "Push response was null" : response.Error.Message), "RegisterPushTokenWithFacebook", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\FacebookPushRegistration.cs", 129);
			return StrategyResponse.Create(ExecutionResult.Failed);
		}
		return StrategyResponse.Create(ExecutionResult.Success);
	}

	private static async Task<StrategyResponse<NullResponse>> UnregisterPushTokenWithFacebook(int attempt, CancellationToken token, Parameters parameters)
	{
		Logger.LogLocation("Attempting to unregister with Facebook: " + parameters.PushUri, LogLevel.Info, "UnregisterPushTokenWithFacebook", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\FacebookPushRegistration.cs", 138);
		Facebook.Services.Api.Methods.UnregisterPush.Request request = new Facebook.Services.Api.Methods.UnregisterPush.Request
		{
			AccessToken = parameters.Context.Session.Credentials.AccessToken,
			ChannelUri = parameters.PushUri,
			CancelToken = token
		};
		Facebook.Services.Api.Service api = ServiceLocator.Current.GetInstance<Facebook.Services.Api.Service>();
		BoolResponse response = await api.ExecuteAsync<Facebook.Services.Api.Methods.UnregisterPush.Request, BoolResponse>(request);
		if (response == null || response.HasError())
		{
			Logger.Error("Unable to update push token with Facebook: " + ((response == null) ? "Push response was null" : response.Error.Message), "UnregisterPushTokenWithFacebook", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Push\\FacebookPushRegistration.cs", 152);
			return StrategyResponse.Create(ExecutionResult.Failed);
		}
		return StrategyResponse.Create(ExecutionResult.Success);
	}
}
