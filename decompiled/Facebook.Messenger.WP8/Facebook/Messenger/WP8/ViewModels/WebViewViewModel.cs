using System;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class WebViewViewModel : FacebookMessengerViewModel
	{
		private Uri _uri;

		public Uri ContentUri
		{
			get
			{
				return _uri;
			}
			set
			{
				Set(ref _uri, value, false, "ContentUri");
			}
		}

		public WebViewViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			FacebookUri facebookUri = (uri as FacebookUri) ?? FacebookUri.UnknownUri;
			if (facebookUri.Params.ContainsKey("url"))
			{
				ContentUri = new Uri(facebookUri.Params["url"], UriKind.RelativeOrAbsolute);
			}
		}
	}
}
