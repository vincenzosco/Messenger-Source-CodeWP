using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Analytics;
using Facebook.Services.Api.Cache;
using Facebook.Services.BackgroundTransfer;
using Facebook.Services.Http;
using Facebook.Services.MessageSender;
using Facebook.Services.Mqtt;

namespace Facebook.Services.Api
{
	public class ServiceContext
	{
		public Service ApiService;

		public Facebook.Services.Analytics.Service AnalyticsService;

		public Facebook.Services.Http.Service HttpService;

		public Facebook.Services.Mqtt.Service MqttService;

		public Facebook.Services.BackgroundTransfer.Service BackgroundTransferService;

		public Facebook.Services.MessageSender.Service MessageSender;

		public FacebookConfig Config;

		public FacebookContext FBContext;

		public ICrypto Crypto;

		public ApplicationState ApplicationState;

		public ThreadCache ThreadCache;

		public ContactCache ContactCache;

		public StickerPackCache StickerPackCache;

		public PhoneContactCache PhoneContactCache;
	}
}
