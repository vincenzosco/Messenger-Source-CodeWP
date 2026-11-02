using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchThreads;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.Utilities;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ForwardingViewModel : FacebookMessengerViewModel
	{
		private ObservableCollection<ThreadViewModel> _threads;

		private readonly ObservableCollection<ThreadViewModel> _selectedRecipients = new ObservableCollection<ThreadViewModel>();

		public ContactSearchViewModel SearchViewModel { get; private set; }

		public string MessageId { get; private set; }

		public string AttachmentId { get; private set; }

		public ObservableCollection<ThreadViewModel> Threads
		{
			get
			{
				return _threads;
			}
			set
			{
				Set(ref _threads, value, false, "Threads");
			}
		}

		public ObservableCollection<ThreadViewModel> SelectedRecipients
		{
			get
			{
				return _selectedRecipients;
			}
		}

		public ForwardingViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, ContactSearchViewModel contactSearch)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			Threads = new ObservableCollection<ThreadViewModel>();
			SearchViewModel = contactSearch;
			FetchThreads();
		}

		public override void Cleanup()
		{
			base.Cleanup();
			SearchViewModel.Cleanup();
		}

		private async void FetchThreads()
		{
			Facebook.Services.Api.Methods.FetchThreads.Request request = new Facebook.Services.Api.Methods.FetchThreads.Request
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				ThreadLimit = 20
			};
			Facebook.Services.Api.Methods.FetchThreads.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchThreads.Request, Facebook.Services.Api.Methods.FetchThreads.Response>(request);
			if (response.HasError())
			{
				return;
			}
			foreach (MailThread item in response.Threads.Where((MailThread thread) => !thread.Deleted && thread.CanReply))
			{
				_threads.Add(new ThreadViewModel(item));
			}
		}

		public bool IsThreadSelected(string threadId)
		{
			return SelectedRecipients.Any((ThreadViewModel t) => t.Id == threadId);
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey(UriParams.ForwardedMessage))
			{
				MessageId = PathUri.Params[UriParams.ForwardedMessage];
			}
			else if (PathUri.Params.ContainsKey(UriParams.ForwardedAttachment))
			{
				AttachmentId = PathUri.Params[UriParams.ForwardedAttachment];
			}
			else
			{
				Logger.Error(this, "No message or attachment included to forward: " + uri, "SetPath", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\ForwardingViewModel.cs", 105);
			}
		}

		public void ToggleMembership(ThreadViewModel thread)
		{
			IEnumerable<ThreadViewModel> source = SelectedRecipients.Where((ThreadViewModel t) => t.Id == thread.Id);
			ThreadViewModel item = (string.IsNullOrWhiteSpace(thread.Id) ? source.FirstOrDefault((ThreadViewModel t) => t.Participants.Contains(thread.Participants.FirstOrDefault())) : source.FirstOrDefault());
			if (!SelectedRecipients.Remove(item))
			{
				SelectedRecipients.Add(thread);
				if (thread.IsCanonical())
				{
					string canonicalParticipantId = UserUtilities.GetCanonicalParticipantId(thread, Context);
					SearchViewModel.SearchResults.SelectedContactIds.Add(canonicalParticipantId);
				}
				else
				{
					SearchViewModel.SelectedThreadIds.Add(thread.Id);
				}
				thread.IsSelected = true;
			}
			else
			{
				if (thread.IsCanonical())
				{
					string canonicalParticipantId2 = UserUtilities.GetCanonicalParticipantId(thread, Context);
					SearchViewModel.SearchResults.SelectedContactIds.Remove(canonicalParticipantId2);
				}
				else
				{
					SearchViewModel.SelectedThreadIds.Remove(thread.Id);
				}
				thread.IsSelected = false;
			}
		}

		public async void EnqueueAllMessages()
		{
			IsLoading = true;
			foreach (ThreadViewModel recipient in SelectedRecipients)
			{
				await ForwardTo(recipient);
			}
			Navigator.RemoveBackEntry();
			IsLoading = false;
			if (SelectedRecipients.Count == 1)
			{
				FacebookUri threadUri = GetThreadUri(SelectedRecipients.Single());
				if (threadUri != null)
				{
					Navigator.ReplaceTopOfStack(threadUri);
					return;
				}
			}
			Navigator.GoBackToRoot();
		}

		private async Task ForwardTo(ThreadViewModel recipient)
		{
			string offlineId = OfflineIdHelper.CreateOfflineThreadingId(Context);
			string recipientId = (recipient.IsCanonical() ? recipient.SingleRecipient : recipient.Id);
			Facebook.Services.Api.Methods.SendMessage.Request request = (recipient.IsCanonical() ? new Facebook.Services.Api.Methods.SendMessage.Request(new List<MailUser>
			{
				new MailUser
				{
					Id = recipient.SingleRecipient
				}
			}, string.Empty, offlineId) : new Facebook.Services.Api.Methods.SendMessage.Request(recipientId, string.Empty, offlineId));
			request.CopyMessageId = MessageId;
			request.CopyAttachmentId = AttachmentId;
			request.AccessToken = Context.Session.Credentials.AccessToken;
			await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>(request);
		}

		private static FacebookUri GetThreadUri(ThreadViewModel vm)
		{
			if (!string.IsNullOrWhiteSpace(vm.Id))
			{
				return FacebookUri.GetThreadUri(vm.Id, vm.Name);
			}
			if (!string.IsNullOrWhiteSpace(vm.SingleRecipient))
			{
				return FacebookUri.GetConversationUri(vm.SingleRecipient, vm.Name);
			}
			return null;
		}
	}
}
