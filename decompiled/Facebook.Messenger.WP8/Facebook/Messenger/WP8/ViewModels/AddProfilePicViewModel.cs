using System;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels;

public class AddProfilePicViewModel : FacebookMessengerViewModel
{
	private RelayCommand<string> _confirmProfilePicCommand;

	private RelayCommand<Uri> _skipProfilePicCommand;

	public RelayCommand<string> ConfirmProfilePicCommand => _confirmProfilePicCommand ?? (_confirmProfilePicCommand = new RelayCommand<string>(ExecuteConfirmProfilePicCommand));

	public RelayCommand<Uri> SkipProfilePicCommand => _skipProfilePicCommand ?? (_skipProfilePicCommand = new RelayCommand<Uri>(ExecuteSkipProfilePicCommand));

	public AddProfilePicViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
		: base(apiService, context, navigator, dispatcher, networkStatus)
	{
	}

	private void ExecuteSkipProfilePicCommand(Uri obj)
	{
		Navigator.Navigate(FacebookUri.SyncContacts);
	}

	private void ExecuteConfirmProfilePicCommand(string url)
	{
		UriQuery uriQuery = new UriQuery();
		uriQuery.Add("profilepicurl", url);
		UriQuery query = uriQuery;
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.ProfilePicConfirmation, query));
	}
}
