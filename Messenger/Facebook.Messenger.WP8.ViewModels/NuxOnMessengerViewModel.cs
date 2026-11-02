using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class NuxOnMessengerViewModel : FacebookMessengerViewModel
	{
		private RelayCommand _loginCommand;

		public RelayCommand ContinueCommand
		{
			get
			{
				return _loginCommand ?? (_loginCommand = new RelayCommand(NavigateToRoot));
			}
		}

		public FacebookProfile CurrentUser
		{
			get
			{
				return Context.Session.Profile;
			}
		}

		public NuxOnMessengerViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigation, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigation, dispatcher, networkStatus)
		{
		}

		private void NavigateToRoot()
		{
			Navigator.Reset(FacebookUri.RootUri);
		}
	}
}
