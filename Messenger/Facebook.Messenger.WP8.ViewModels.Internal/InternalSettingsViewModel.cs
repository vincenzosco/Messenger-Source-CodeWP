using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.SyncProtocol;

namespace Facebook.Messenger.WP8.ViewModels.Internal
{
	public class InternalSettingsViewModel : FacebookMessengerViewModel
	{
		public AudioLevelsViewModel AudioLevelsViewModel { get; private set; }

		public DiskDetailsViewModel DiskDetailsViewModel { get; private set; }

		public BuildDetailsViewModel BuildDetailsViewModel { get; private set; }

		public NetworkInterfaceDetailsViewModel NetworkInterfaceDetailsViewModel { get; private set; }

		public SyncProtocolSettingsViewModel SyncProtocolSettingsViewModel { get; private set; }

		public InternalSettingsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, Facebook.Messenger.WP8.AudioController.Service audioService, Facebook.Services.SyncProtocol.Service syncProtocolService, ThreadCache threadCache)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			AudioLevelsViewModel = new AudioLevelsViewModel(audioService);
			DiskDetailsViewModel = new DiskDetailsViewModel(context);
			BuildDetailsViewModel = new BuildDetailsViewModel();
			NetworkInterfaceDetailsViewModel = new NetworkInterfaceDetailsViewModel();
			SyncProtocolSettingsViewModel = new SyncProtocolSettingsViewModel(syncProtocolService, threadCache);
		}
	}
}
