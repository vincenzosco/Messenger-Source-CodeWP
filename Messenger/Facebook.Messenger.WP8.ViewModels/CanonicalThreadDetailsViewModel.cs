using System;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class CanonicalThreadDetailsViewModel : BaseThreadDetailsViewModel
	{
		public const string LastActiveTextParameter = "last_active_text";

		private string _lastActiveText;

		private RelayCommand _launchTimelineCommand;

		private RelayCommand _startGroupConversationWithRecipientCommand;

		public string LastActiveText
		{
			get
			{
				return _lastActiveText;
			}
			set
			{
				Set(ref _lastActiveText, value, false, "LastActiveText");
			}
		}

		public RelayCommand LaunchTimelineCommand
		{
			get
			{
				return _launchTimelineCommand ?? (_launchTimelineCommand = new RelayCommand(ExecuteLaunchTimelineCommand));
			}
		}

		public RelayCommand StartGroupConversationWithRecipientCommand
		{
			get
			{
				return _startGroupConversationWithRecipientCommand ?? (_startGroupConversationWithRecipientCommand = new RelayCommand(ExecuteStartGroupConversationWithRecipientCommand));
			}
		}

		public CanonicalThreadDetailsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
		}

		private void ExecuteLaunchTimelineCommand()
		{
			bool isPartial = Thread.CanonicalParticipant.IsPartial;
			if (!IsPartialUser && !isPartial)
			{
				FacebookAppDeepLinkUtil.LaunchProfile(Thread.SingleRecipient);
			}
		}

		private void ExecuteStartGroupConversationWithRecipientCommand()
		{
			FacebookUri composerUri = FacebookUri.GetComposerUri(null, false, Thread.SingleRecipient, Thread.CanonicalParticipant.Name);
			Navigator.Navigate(composerUri);
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("last_active_text"))
			{
				LastActiveText = PathUri.Params["last_active_text"];
			}
		}

		protected override void OnThreadUpdated()
		{
		}
	}
}
