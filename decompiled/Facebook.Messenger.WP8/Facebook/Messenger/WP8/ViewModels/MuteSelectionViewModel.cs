using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class MuteSelectionViewModel : FacebookMessengerViewModel
	{
		private ObservableCollection<MuteSelectionItemViewModel> _menuItems;

		private ThreadViewModel _threadViewModel;

		public ObservableCollection<MuteSelectionItemViewModel> MenuItems
		{
			get
			{
				return _menuItems;
			}
			set
			{
				Set(ref _menuItems, value, false, "MenuItems");
			}
		}

		public MuteSelectionViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_menuItems = new ObservableCollection<MuteSelectionItemViewModel>();
			_menuItems.Add(new MuteSelectionItemViewModel(this, MuteDuration.Forever));
			_menuItems.Add(new MuteSelectionItemViewModel(this, MuteDuration.Hour));
			_menuItems.Add(new MuteSelectionItemViewModel(this, MuteDuration.EightAM));
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			_threadViewModel = Navigator.GetAndClearNavigationData() as ThreadViewModel;
		}

		public override void LoadState(Dictionary<string, object> state)
		{
			base.LoadState(state);
			if (_threadViewModel == null)
			{
				Navigator.GoBack();
			}
		}

		public async Task TurnOnMute(MuteDuration duration)
		{
			IsLoading = true;
			Response response = await MuteHelper.SetThreadMuteState(duration, Context, ApiService, _threadViewModel.Id);
			IsLoading = false;
			if (!response.HasError())
			{
				Navigator.GoBack();
				return;
			}
			OnError(new Error
			{
				Message = StringLibrary.Error_Changing_Mute
			});
		}
	}
}
