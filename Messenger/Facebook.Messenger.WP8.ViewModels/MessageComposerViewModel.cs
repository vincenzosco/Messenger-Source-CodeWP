using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.FBGeoLocator;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchFriends;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.MessageSender;
using Facebook.Services.Utilities;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class MessageComposerViewModel : MessageBaseViewModel
	{
		public const string HasRecipientsPropertyName = "HasRecipients";

		public const string DraftTextPropertyName = "DraftText";

		private readonly SearchTermHelper _searchHelper;

		private FacebookUserCollection _groupedContactList;

		private ObservableCollection<MessageArgs> _recipients;

		private Story _sharePost;

		private string _draftText;

		private RelayCommand<MessageArgs> _toggleRecipientCommand;

		private RelayCommand<MessageArgs> _removeRecipientCommand;

		public string SearchFilter
		{
			set
			{
				_searchHelper.TriggerSearch(value);
			}
		}

		public override bool MessageComposerHasContent
		{
			get
			{
				if (Recipients.Any())
				{
					return base.MessageComposerHasContent;
				}
				return false;
			}
		}

		public FacebookUserCollection GroupedContactList
		{
			get
			{
				return _groupedContactList;
			}
			set
			{
				Set(ref _groupedContactList, value, false, "GroupedContactList");
			}
		}

		public ObservableCollection<MessageArgs> Recipients
		{
			get
			{
				return _recipients;
			}
			set
			{
				if (Set(ref _recipients, value, false, "Recipients"))
				{
					RaisePropertyChanged("MessageComposerHasContent");
				}
			}
		}

		public bool HasRecipients
		{
			get
			{
				return Recipients.Any();
			}
		}

		public Story SharePost
		{
			get
			{
				return _sharePost;
			}
			set
			{
				Set(ref _sharePost, value, false, "SharePost");
			}
		}

		public string DraftText
		{
			get
			{
				return _draftText;
			}
			set
			{
				Set(ref _draftText, value, false, "DraftText");
			}
		}

		public RelayCommand<MessageArgs> ToggleRecipientCommand
		{
			get
			{
				return _toggleRecipientCommand ?? (_toggleRecipientCommand = new RelayCommand<MessageArgs>(ExecuteToggleRecipientCommand));
			}
		}

		public RelayCommand<MessageArgs> RemovedRecipientCommand
		{
			get
			{
				return _removeRecipientCommand ?? (_removeRecipientCommand = new RelayCommand<MessageArgs>(ExecuteRemoveRecipientCommand));
			}
		}

		public MessageComposerViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, Facebook.Services.MessageSender.Service messageSender, Facebook.Messenger.WP8.FBGeoLocator.Service geolocator, StickerDrawerViewModel stickerDrawerViewModel, Facebook.Messenger.WP8.AudioController.Service audioController, IFileAccess fileAccess)
			: base(apiService, context, navigator, dispatcher, networkStatus, messageSender, geolocator, stickerDrawerViewModel, audioController, fileAccess)
		{
			GroupedContactList = new FacebookUserCollection(apiService, Context.Session.Credentials.AccessToken);
			_searchHelper = new SearchTermHelper(GetLocalSearchResults, GetRemoteSearchResults);
			_searchHelper.TriggerSearch(string.Empty);
			InitRecipients();
			SendLocationEnabled = Geolocator.IsAppLocationEnabled();
		}

		public void ExecuteToggleRecipientCommand(MessageArgs args)
		{
			if (Recipients.Any((MessageArgs u) => u.Id == args.Id))
			{
				ExecuteRemoveRecipientCommand(args);
				return;
			}
			Recipients.Add(args);
			SelectExistingUsers();
		}

		private void ExecuteRemoveRecipientCommand(MessageArgs args)
		{
			MessageArgs messageArgs = Recipients.FirstOrDefault((MessageArgs u) => u.Id == args.Id);
			if (messageArgs != null)
			{
				Recipients.Remove(messageArgs);
				SelectExistingUsers();
			}
		}

		protected override void OnGeolocatorStatusChanged(object sender, EventArgs eventArgs)
		{
			SendLocationEnabled = Geolocator.IsLocationEnabled();
		}

		private void OnRecipientsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			RaisePropertyChanged("HasRecipients");
			RaisePropertyChanged("MessageComposerHasContent");
		}

		protected override bool SetLocationSendingState(bool enable)
		{
			if (Geolocator.IsLocationEnabled())
			{
				SendLocationEnabled = enable;
				if (SendLocationEnabled)
				{
					AudioController.PlayLocationOn();
				}
				return true;
			}
			return false;
		}

		protected override Facebook.Services.Api.Methods.SendMessage.Request CreateSendMessageRequest(SendMessageContent content)
		{
			List<MailUser> userIds = Recipients.Select((MessageArgs recipient) => new MailUser
			{
				Id = recipient.Id
			}).ToList();
			string offlineThreadingId = OfflineIdHelper.CreateOfflineThreadingId(Context);
			Facebook.Services.Api.Methods.SendMessage.Request request = new Facebook.Services.Api.Methods.SendMessage.Request(userIds, content.Text, offlineThreadingId);
			request.AccessToken = Context.Session.Credentials.AccessToken;
			request.CancelToken = CancelTokenSource.Token;
			Facebook.Services.Api.Methods.SendMessage.Request request2 = request;
			SetRequestContent(request2, content);
			return request2;
		}

		protected override async Task SendMessage(SendMessageContent content)
		{
			if (content == null)
			{
				return;
			}
			ShowLocationWarning = false;
			if (!HasConnection)
			{
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_NoInternet
				});
			}
			else
			{
				if (SendingMessage)
				{
					return;
				}
				SendingMessage = true;
				IsLoading = true;
				try
				{
					Facebook.Services.Api.Methods.SendMessage.Request request = CreateSendMessageRequest(content);
					Facebook.Services.Api.Methods.SendMessage.Response response = await MessageSender.ExecuteAsync<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>(request);
					if (!response.HasError())
					{
						Geolocator.SetLocationForThread(SendLocationEnabled, response.ThreadId);
						FacebookUri threadUri = FacebookUri.GetThreadUri(response.ThreadId, GenerateThreadName(), null, true);
						Navigator.ReplaceTopOfStack(threadUri);
					}
					else if (HasConnection)
					{
						bool flag = response.ApiError != null && !string.IsNullOrEmpty(response.ApiError.DisplayMessage);
						OnError(new Error
						{
							Title = (flag ? response.ApiError.DisplayTitle : null),
							Message = (flag ? response.ApiError.DisplayMessage : StringLibrary.Message_Sending_Failed)
						});
						if (response.ApiError != null)
						{
							Logger.Error(this, "Sending message failed with error code:" + response.ApiError.ErrorCode, "SendMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\MessageComposerViewModel.cs", 277);
						}
					}
					else
					{
						OnError(new Error
						{
							Message = StringLibrary.MessageBox_Label_NoInternet
						});
					}
				}
				finally
				{
					SendingMessage = false;
					IsLoading = false;
				}
			}
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("textToSend"))
			{
				DraftText = Uri.UnescapeDataString(PathUri.Params["textToSend"]);
			}
			if (PathUri.Params.ContainsKey("photoId"))
			{
				SetAttachment(PathUri.Params["photoId"], MediaSource.Share).DoNotWait();
			}
			if (PathUri.Params.ContainsKey("recipientId"))
			{
				string text = PathUri.Params["recipientId"];
				if (PathUri.Params.ContainsKey("recipientName"))
				{
					string name = PathUri.Params["recipientName"];
					ExecuteToggleRecipientCommand(new MessageArgs(text, name));
				}
				else
				{
					FetchAndAddRecipient(text).DoNotWait();
				}
			}
		}

		public override void SaveState(Dictionary<string, object> state)
		{
			state["recipients"] = JsonConvert.SerializeObject(Recipients);
			state["message"] = DraftText;
		}

		public override void LoadState(Dictionary<string, object> state)
		{
			if (state.ContainsKey("message"))
			{
				DraftText = state["message"] as string;
			}
			if (state.ContainsKey("recipients"))
			{
				string value = state["recipients"] as string;
				if (!string.IsNullOrEmpty(value))
				{
					ObservableCollection<MessageArgs> recipients = JsonConvert.DeserializeObject<ObservableCollection<MessageArgs>>(value);
					InitRecipients(recipients);
				}
			}
		}

		protected void InitRecipients(ObservableCollection<MessageArgs> recipients = null)
		{
			if (Recipients != null)
			{
				Recipients.CollectionChanged -= OnRecipientsCollectionChanged;
			}
			Recipients = recipients ?? new ObservableCollection<MessageArgs>();
			Recipients.CollectionChanged += OnRecipientsCollectionChanged;
		}

		private async Task FetchAndAddRecipient(string uid)
		{
			Facebook.Services.Api.Methods.FetchFriends.Request request = new Facebook.Services.Api.Methods.FetchFriends.Request
			{
				Uid = uid,
				CancelToken = CancelTokenSource.Token
			};
			Facebook.Services.Api.Methods.FetchFriends.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchFriends.Request, Facebook.Services.Api.Methods.FetchFriends.Response>(request);
			if (!response.HasError() && response.Friend != null && !Recipients.Any())
			{
				ExecuteToggleRecipientCommand(new MessageArgs(response.Friend));
			}
		}

		private void SelectExistingUsers()
		{
			foreach (AlphaKeyGroup<FacebookUser> groupedContact in GroupedContactList)
			{
				SelectExistingUsers(groupedContact);
			}
		}

		private void SelectExistingUsers(IEnumerable<FacebookUser> users)
		{
			Dictionary<string, MessageArgs> dictionary = Recipients.ToDictionary((MessageArgs u) => u.Id, (MessageArgs u) => u);
			foreach (FacebookUser user in users)
			{
				user.IsSelected = dictionary.ContainsKey(user.Id);
			}
		}

		private string GenerateThreadName()
		{
			if (Recipients.Count == 1)
			{
				return Recipients[0].Name;
			}
			IEnumerable<string> values = from r in Recipients
				select r.FirstName into n
				where !string.IsNullOrEmpty(n)
				select n;
			return string.Join(", ", values);
		}

		private async void GetLocalSearchResults(string term, CancellationToken token)
		{
			GroupedContactList.Clear();
			IsLoading = true;
			await GroupedContactList.FetchFriendsAsync(term, token);
			IsLoading = false;
		}

		private async void GetRemoteSearchResults(string term, CancellationToken token)
		{
			IsLoading = true;
			try
			{
				await GroupedContactList.GetSearchResults(token, term, SelectExistingUsers, true, SelectExistingUsers);
			}
			catch (ApiErrorException)
			{
			}
			IsLoading = false;
		}
	}
}
