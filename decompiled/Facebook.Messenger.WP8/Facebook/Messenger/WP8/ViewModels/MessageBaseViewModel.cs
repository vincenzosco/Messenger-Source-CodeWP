using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.FBGeoLocator;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.MessageSender;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;

namespace Facebook.Messenger.WP8.ViewModels
{
	public abstract class MessageBaseViewModel : FacebookMessengerViewModel
	{
		public enum SendTrigger
		{
			Click,
			Enter
		}

		public const int MaxFilesToUpload = 6;

		public const int MaxFileSizeMegaBytes = 25;

		public const ulong MaxFileSizeBytes = 26214400uL;

		public const string SendLocationEnabledPropertyName = "SendLocationEnabled";

		public const string MessageComposerHasContentPropertyName = "MessageComposerHasContent";

		public const string SendingMessagePropertyName = "SendingMessage";

		protected readonly Facebook.Services.MessageSender.Service MessageSender;

		protected readonly Facebook.Messenger.WP8.FBGeoLocator.Service Geolocator;

		protected readonly Facebook.Messenger.WP8.AudioController.Service AudioController;

		protected readonly IFileAccess FileAccess;

		public readonly StickerDrawerViewModel StickerDrawerViewModel;

		private bool _sendLocationEnabled;

		private bool _isLoadingLocation;

		private ObservableCollection<Facebook.Services.Api.Methods.SendMessage.Attachment> _attachedFiles;

		private bool _messageComposerHasText;

		private bool _sendingMessage;

		private RelayCommand _setLocationSendingCommand;

		private RelayCommand<ImageResult> _sendImageCommand;

		private RelayCommand<VideoResult> _sendVideoCommand;

		private RelayCommand _sendLikeCommand;

		private RelayCommand<VoiceRecordingHelper.VoiceRecordingResult> _sendVoiceClip;

		public bool SendLocationEnabled
		{
			get
			{
				return _sendLocationEnabled;
			}
			protected set
			{
				Set(ref _sendLocationEnabled, value, false, "SendLocationEnabled");
			}
		}

		public bool IsLoadingLocation
		{
			get
			{
				return _isLoadingLocation;
			}
			set
			{
				Set(ref _isLoadingLocation, value, false, "IsLoadingLocation");
			}
		}

		public bool ShowLocationWarning
		{
			get
			{
				return Context.ShowLocationWarning();
			}
			set
			{
				if (Context.ShowLocationWarning())
				{
					Context.HideLocationWarning();
					RaisePropertyChanged("ShowLocationWarning");
				}
			}
		}

		public ObservableCollection<Facebook.Services.Api.Methods.SendMessage.Attachment> AttachedFiles
		{
			get
			{
				return _attachedFiles;
			}
			set
			{
				Set(ref _attachedFiles, value, false, "AttachedFiles");
			}
		}

		public bool HasAttachments
		{
			get
			{
				if (AttachedFiles != null)
				{
					return AttachedFiles.Any();
				}
				return false;
			}
		}

		public bool CanAttachMoreFiles
		{
			get
			{
				if (AttachedFiles != null)
				{
					return AttachedFiles.Count < 6;
				}
				return true;
			}
		}

		public virtual bool MessageComposerHasText
		{
			get
			{
				return _messageComposerHasText;
			}
			set
			{
				if (_messageComposerHasText != value)
				{
					_messageComposerHasText = value;
					RaisePropertyChanged("MessageComposerHasContent");
				}
			}
		}

		public virtual bool MessageComposerHasContent
		{
			get
			{
				if (!MessageComposerHasText)
				{
					return HasAttachments;
				}
				return true;
			}
		}

		public bool SendingMessage
		{
			get
			{
				return _sendingMessage;
			}
			set
			{
				Set(ref _sendingMessage, value, false, "SendingMessage");
			}
		}

		public bool AllowMultipleLines => !Context.IsSendOnEnterEnabled();

		public RelayCommand SetLocationSendingCommand => _setLocationSendingCommand ?? (_setLocationSendingCommand = new RelayCommand(ExecuteSetLocationSendingCommand));

		public RelayCommand<ImageResult> SendImageCommand => _sendImageCommand ?? (_sendImageCommand = new RelayCommand<ImageResult>(ExecuteSendImageCommand));

		public RelayCommand<VideoResult> SendVideoCommand => _sendVideoCommand ?? (_sendVideoCommand = new RelayCommand<VideoResult>(ExecuteSendVideoCommand));

		public RelayCommand SendLikeCommand => _sendLikeCommand ?? (_sendLikeCommand = new RelayCommand(ExecuteSendLikeCommand));

		public RelayCommand<VoiceRecordingHelper.VoiceRecordingResult> SendVoiceClip => _sendVoiceClip ?? (_sendVoiceClip = new RelayCommand<VoiceRecordingHelper.VoiceRecordingResult>(ExecuteSendVoiceClipCommand));

		protected MessageBaseViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, Facebook.Services.MessageSender.Service messageSender, Facebook.Messenger.WP8.FBGeoLocator.Service geolocator, StickerDrawerViewModel stickerDrawerViewModel, Facebook.Messenger.WP8.AudioController.Service audioController, IFileAccess fileAccess)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			MessageSender = messageSender;
			Geolocator = geolocator;
			AudioController = audioController;
			FileAccess = fileAccess;
			StickerDrawerViewModel = stickerDrawerViewModel;
			stickerDrawerViewModel.StickerSelected += OnStickerSelected;
			Geolocator.GeolocatorStatusChanged += OnGeolocatorStatusChanged;
			AttachedFiles = new ObservableCollection<Facebook.Services.Api.Methods.SendMessage.Attachment>();
			AttachedFiles.CollectionChanged += AttachedFilesOnCollectionChanged;
		}

		public override void Cleanup()
		{
			StickerDrawerViewModel.Cleanup();
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
			Geolocator.GeolocatorStatusChanged -= OnGeolocatorStatusChanged;
			base.Cleanup();
		}

		private void ExecuteSetLocationSendingCommand()
		{
			ShowLocationWarning = false;
			if (IsLoadingLocation)
			{
				return;
			}
			try
			{
				IsLoadingLocation = true;
				bool flag = !SendLocationEnabled;
				if (!SetLocationSendingState(flag))
				{
					string message = (Geolocator.IsDeviceLocationEnabled() ? StringLibrary.App_Location_Services_Enable : StringLibrary.Device_Location_Services_Enable);
					OnError(new Error
					{
						Message = message,
						Title = StringLibrary.Location_Services_Disabled
					});
				}
				else if (flag)
				{
					AudioController.PlayLocationOn();
				}
			}
			finally
			{
				IsLoadingLocation = false;
			}
		}

		public bool SendComposedMessage(string messageText, SendTrigger trigger)
		{
			if (trigger == SendTrigger.Enter && !Context.IsSendOnEnterEnabled())
			{
				return false;
			}
			if (!MessageComposerHasContent)
			{
				return false;
			}
			SendMessageContent content = CreateComposerMessageContent(messageText);
			SendMessage(content).DoNotWait();
			AttachedFiles.Clear();
			return true;
		}

		private async void ExecuteSendImageCommand(ImageResult imageResult)
		{
			Facebook.Services.Api.Methods.SendMessage.Attachment attachment = await AttachmentFactory.CreateAttachment(imageResult);
			if (attachment != null)
			{
				SendMessageContent content = SendMessageContent.CreateImageMessage(attachment);
				SendMessage(content).DoNotWait();
			}
		}

		private void ExecuteSendVideoCommand(VideoResult result)
		{
			Facebook.Services.Api.Methods.SendMessage.Attachment attachment = AttachmentFactory.CreateQuickCamVideoRecordingAttachment(result);
			if (attachment != null)
			{
				SendMessageContent content = SendMessageContent.CreateVideoMessage(attachment);
				SendMessage(content).DoNotWait();
			}
		}

		private void ExecuteSendLikeCommand()
		{
			SendMessage(SendMessageContent.CreateStickerMessage("227878347358915", "/Assets/Stickers/LikeSticker.png")).DoNotWait();
		}

		private void ExecuteSendVoiceClipCommand(VoiceRecordingHelper.VoiceRecordingResult clip)
		{
			Facebook.Services.Api.Methods.SendMessage.Attachment attachment = AttachmentFactory.CreateVoiceRecordingAttachment(clip);
			if (attachment != null)
			{
				SendMessageContent content = SendMessageContent.CreateVoiceClipMessage(attachment);
				SendMessage(content).DoNotWait();
			}
		}

		private void OnStickerSelected(object sender, StickerSelectedArgs e)
		{
			SendMessageContent content = SendMessageContent.CreateStickerMessage(e.StickerId, e.StickerUrl);
			SendMessage(content);
		}

		protected virtual void AttachedFilesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			RaisePropertyChanged("MessageComposerHasContent");
			if (args.OldItems == null || args.OldItems.Count == 0)
			{
				return;
			}
			foreach (Facebook.Services.Api.Methods.SendMessage.Attachment item in args.OldItems.OfType<Facebook.Services.Api.Methods.SendMessage.Attachment>())
			{
				FileAccess.DeleteFileAtPath(item.ThumbnailPath);
				FileAccess.DeleteFileAtPath(item.Path);
			}
		}

		protected abstract bool SetLocationSendingState(bool enable);

		protected abstract void OnGeolocatorStatusChanged(object sender, EventArgs eventArgs);

		protected abstract Facebook.Services.Api.Methods.SendMessage.Request CreateSendMessageRequest(SendMessageContent content);

		protected abstract Task SendMessage(SendMessageContent content);

		public void SavePreferences(IDictionary<string, string> settings)
		{
			foreach (KeyValuePair<string, string> setting in settings)
			{
				Context.Settings.SaveSetting(setting.Key, setting.Value);
			}
		}

		public IDictionary<string, string> LoadPreferences(IList<string> keys)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string key in keys)
			{
				dictionary[key] = Context.Settings.GetSetting<string>(key);
			}
			return dictionary;
		}

		public void PlayCameraSound()
		{
			AudioController.PlayCameraDownUp();
		}

		public void PlayDrawerSelectedSound()
		{
			AudioController.PlayDrawerSelected();
		}

		public void PlayTextDrawerSelectedSound()
		{
			AudioController.PlayTextDrawerSelected();
		}

		public void PlayCameraDownSound()
		{
			AudioController.PlayCameraDown();
		}

		public void PlayCameraUpSound()
		{
			AudioController.PlayCameraUp();
		}

		public void PlayCancelRecordSound()
		{
			AudioController.PlayCancelRecord();
		}

		public void PlayVoiceClipSentSound()
		{
			AudioController.PlayVoiceClipSend();
		}

		public void PlayVoiceClipCancelledSound()
		{
			AudioController.PlayVoiceClipCancel();
		}

		public void PlayTypingIndicatorSound()
		{
			AudioController.PlayTypingIndicator();
		}

		public void PlaySendingLikeSound()
		{
			AudioController.PlaySendingLike();
		}

		public void PlaySendingStickerSound()
		{
			AudioController.PlaySendingSticker();
		}

		public void PlaySendingMessageSound()
		{
			AudioController.PlaySendingMessage();
		}

		protected void PlaySentMessageSound(SendMessageContent content)
		{
			if (content.HasSticker())
			{
				if (MailShare.LikeStickerIds.Contains(content.StickerId))
				{
					AudioController.PlaySendingLike();
				}
				else
				{
					AudioController.PlaySendingSticker();
				}
			}
			else
			{
				AudioController.PlaySendingMessage();
			}
		}

		public async Task SetAttachment(PhotoResult result)
		{
			if ((int)((TaskEventArgs)result).TaskResult == 1)
			{
				PhotoData image = this.PhotoDataFromPhotoResult(result);
				SetAttachment(await this.PrepareImageInfoAsync(image, Path.GetFileName(result.OriginalFileName)), MediaSource.Gallery);
			}
		}

		protected async Task SetAttachment(string fileId, MediaSource source)
		{
			MediaLibrary library = new MediaLibrary();
			Picture photoFromLibrary = library.GetPictureFromToken(fileId);
			PhotoData image = this.PhotoDataFromPicture(photoFromLibrary);
			if (image != null)
			{
				SetAttachment(await this.PrepareImageInfoAsync(image, Path.GetFileName(photoFromLibrary.Name)), source);
			}
		}

		private void SetAttachment(PhotoAttachmentHelper.PhotoAttachmentInfo info, MediaSource source)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (!CanAttachMoreFiles)
			{
				MessageBox.Show(string.Format(StringLibrary.Error_Max_File_Attachment_Exceeded, 6), string.Empty, (MessageBoxButton)0);
			}
			else if (info != null)
			{
				Facebook.Services.Api.Methods.SendMessage.Attachment attachment = AttachmentFactory.CreateAttachment(info, source);
				if (attachment != null && AttachedFiles.All((Facebook.Services.Api.Methods.SendMessage.Attachment a) => a.Path != attachment.Path))
				{
					AttachedFiles.Add(attachment);
				}
			}
		}

		protected SendMessageContent CreateComposerMessageContent(string textToSend)
		{
			FacebookPlace location = (SendLocationEnabled ? Geolocator.GetCurrentLocation() : null);
			return SendMessageContent.CreateComposerBubbleMessage(textToSend, AttachedFiles, location);
		}

		protected void SetRequestContent(Facebook.Services.Api.Methods.SendMessage.Request request, SendMessageContent content)
		{
			request.Location = content.Location;
			request.FilePaths = content.Attachments;
			if (content.HasSticker())
			{
				request.ObjectAttachmentId = content.StickerId;
				request.ObjectAttachmentHref = content.StickerUrl;
			}
		}
	}
}
