using System;
using System.Collections.Generic;
using Facebook.Base;
using Facebook.Messenger.WP8.Agents.BackgroundInstance;
using Facebook.Models;
using Facebook.Services;
using Facebook.Services.Analytics;
using Facebook.Services.Api.Cache;
using Facebook.Services.Cache;
using Facebook.Services.MessageSender;
using Facebook.Services.Mqtt;

namespace Facebook.Messenger.WP8.Agents
{
	public class FacebookMessengerBackgroundLocator : FacebookMessengerLocator
	{
		private static readonly FacebookMessengerBackgroundLocator PrivateInstance = new FacebookMessengerBackgroundLocator();

		public static FacebookMessengerBackgroundLocator Instance
		{
			get
			{
				return PrivateInstance;
			}
		}

		protected FacebookMessengerBackgroundLocator()
		{
			RegisterSingle(CreateBackgroundInstance);
			RegisterSingle((Func<IDispatcher>)(() => new BackgroundDispatcher()));
		}

		protected override IList<BackgroundService> GetInitializationOrderedServices()
		{
			return new BackgroundService[4]
			{
				GetInstance<Facebook.Services.Cache.Service>(),
				GetInstance<Facebook.Services.Analytics.Service>(),
				GetInstance<Facebook.Services.Mqtt.Service>(),
				GetInstance<Facebook.Services.MessageSender.Service>()
			};
		}

		protected override ApplicationState CreateApplicationState()
		{
			return new ApplicationState(ApplicationState.Runtype.BackgroundSending);
		}

		private IBackgroundInstance CreateBackgroundInstance()
		{
			return new MessageSendingBackgroundInstance(GetInstance<FacebookContext>(), GetInstance<ServiceEngine>(), GetInstance<Facebook.Services.MessageSender.Service>());
		}

		protected override List<ICache> GetCaches()
		{
			List<ICache> list = new List<ICache>();
			list.Add(GetInstance<AnalyticsCache>());
			return list;
		}
	}
}
