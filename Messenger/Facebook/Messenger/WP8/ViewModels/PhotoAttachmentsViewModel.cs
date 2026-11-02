using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Api;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class PhotoAttachmentsViewModel : FacebookMessengerViewModel
	{
		private List<string> _photoIds;

		private List<string> _photoFbids;

		private List<string> _filePaths;

		private string _messageId;

		private int _selectedIndex;

		private string _indicatorString;

		private ObservableCollection<IImage> _imageCollection;

		private bool _isPreviousEnabled;

		private bool _isNextEnabled;

		private bool _isLocalImages;

		private bool _isSaveEnabled;

		private bool _isImageLoading;

		private bool _disableLandscapeOrientation;

		private RelayCommand<int> _saveImageCommand;

		private RelayCommand<int> _forwardImageCommand;

		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			set
			{
				Set(ref _selectedIndex, value, false, "SelectedIndex");
				UpdateIndicatorString(_selectedIndex);
			}
		}

		public string IndicatorString
		{
			get
			{
				return _indicatorString;
			}
			set
			{
				Set(ref _indicatorString, value, false, "IndicatorString");
			}
		}

		public ObservableCollection<IImage> ImageCollection
		{
			get
			{
				return _imageCollection;
			}
			set
			{
				Set(ref _imageCollection, value, false, "ImageCollection");
			}
		}

		public bool IsPreviousEnabled
		{
			get
			{
				return _isPreviousEnabled;
			}
			set
			{
				Set(ref _isPreviousEnabled, value, false, "IsPreviousEnabled");
			}
		}

		public bool IsNextEnabled
		{
			get
			{
				return _isNextEnabled;
			}
			set
			{
				Set(ref _isNextEnabled, value, false, "IsNextEnabled");
			}
		}

		public bool IsLocalImages
		{
			get
			{
				return _isLocalImages;
			}
			set
			{
				Set(ref _isLocalImages, value, false, "IsLocalImages");
			}
		}

		public bool IsSaveEnabled
		{
			get
			{
				return _isSaveEnabled;
			}
			set
			{
				Set(ref _isSaveEnabled, value, false, "IsSaveEnabled");
			}
		}

		public bool IsForwardingEnabled
		{
			get
			{
				if (!_isLocalImages)
				{
					return _photoFbids != null;
				}
				return false;
			}
		}

		public bool IsImageLoading
		{
			get
			{
				return _isImageLoading;
			}
			set
			{
				Set(ref _isImageLoading, value, false, "IsImageLoading");
			}
		}

		public bool DisableLandscapeOrientation
		{
			get
			{
				return _disableLandscapeOrientation;
			}
			set
			{
				Set(ref _disableLandscapeOrientation, value, false, "DisableLandscapeOrientation");
			}
		}

		public RelayCommand<int> SaveImageCommand => _saveImageCommand ?? (_saveImageCommand = new RelayCommand<int>(OnSaveImageSelected));

		public RelayCommand<int> ForwardImageCommand => _forwardImageCommand ?? (_forwardImageCommand = new RelayCommand<int>(OnForwardImageSelected));

		public event EventHandler<ImageSaveEventArgs> ImageSaveCompleted;

		public PhotoAttachmentsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<PhotoLoadingSignal>(this, PhotoLoadingSignalReceived);
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			int result;
			if (PathUri.Params.ContainsKey("selectedIndex") && int.TryParse(PathUri.Params["selectedIndex"], out result))
			{
				SelectedIndex = result;
			}
			if (PathUri.Params.ContainsKey("photoIds"))
			{
				string text = PathUri.Params["photoIds"];
				if (!string.IsNullOrWhiteSpace(text))
				{
					_photoIds = new List<string>(text.Split(','));
				}
			}
			if (PathUri.Params.ContainsKey("photoFbids"))
			{
				string text2 = PathUri.Params["photoFbids"];
				if (!string.IsNullOrWhiteSpace(text2))
				{
					_photoFbids = new List<string>(text2.Split(','));
				}
			}
			if (PathUri.Params.ContainsKey("messageId"))
			{
				_messageId = PathUri.Params["messageId"];
			}
			bool result2;
			if (PathUri.Params.ContainsKey("disableLandscapeOrientation") && bool.TryParse(PathUri.Params["disableLandscapeOrientation"], out result2))
			{
				DisableLandscapeOrientation = result2;
			}
			if (PathUri.Params.ContainsKey("filePaths"))
			{
				_filePaths = JsonConvert.DeserializeObject<List<string>>(PathUri.Params["filePaths"]);
				IsLocalImages = true;
			}
			if (_photoIds != null && _photoIds.Any() && !string.IsNullOrWhiteSpace(_messageId))
			{
				IsLocalImages = false;
				SetImageList(SelectedIndex);
			}
			else if (_filePaths != null && _filePaths.Any())
			{
				IsLocalImages = true;
				SetImageList(SelectedIndex);
			}
		}

		public override void Cleanup()
		{
			base.Cleanup();
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister<PhotoLoadingSignal>(this);
		}

		private void SetImageList(int index)
		{
			if (index < 0)
			{
				return;
			}
			List<IImage> list = new List<IImage>();
			if (_photoIds != null && _photoIds.Count > index)
			{
				SetImages(index, _photoIds, list, (string s) => AttachmentUriGenerator.UrlForAttachment(s, _messageId));
			}
			else if (_filePaths.Count > index)
			{
				SetImages(index, _filePaths, list, (string s) => s);
			}
			if (list.Any())
			{
				ImageCollection = new ObservableCollection<IImage>(list);
				IsSaveEnabled = true;
			}
		}

		private void SetImages(int index, ICollection imagePaths, List<IImage> images, Func<string, string> imageUriGenerator)
		{
			foreach (string imagePath in imagePaths)
			{
				Uri result;
				if (Uri.TryCreate(imageUriGenerator(imagePath), UriKind.Absolute, out result))
				{
					images.Add(new UriImage(result));
				}
			}
			SetButtonStates(index, imagePaths.Count - 1);
		}

		private void PhotoLoadingSignalReceived(PhotoLoadingSignal signal)
		{
			switch (signal.LoadingStatus)
			{
			case PhotoLoadingStatus.Loading:
				IsImageLoading = true;
				break;
			case PhotoLoadingStatus.Loaded:
				IsImageLoading = false;
				break;
			}
		}

		public void SetButtonStates(int index)
		{
			SetButtonStates(index, (IsLocalImages ? _filePaths.Count : _photoIds.Count) - 1);
		}

		private void SetButtonStates(int index, int maxCount)
		{
			IsNextEnabled = index != maxCount;
			IsPreviousEnabled = index != 0;
		}

		public void UpdateIndicatorString(int index)
		{
			IndicatorString = ((ImageCollection != null && ImageCollection.Count > 1) ? string.Format(StringLibrary.PhotoAttachment_IndexDisplay_Label, index + 1, ImageCollection.Count) : string.Empty);
		}

		private void OnSaveImageSelected(int index)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected Obj, but got Unknown
			if (ImageCollection == null)
			{
				IsSaveEnabled = false;
				InvokeImageSaveCompleted(ActionState.Failed);
				ShowImageSaveFailureMessageBox();
				return;
			}
			IsImageLoading = true;
			BitmapImage val = new BitmapImage();
			val.CreateOptions = (BitmapCreateOptions)0;
			BitmapImage val2 = val;
			val2.ImageOpened += bitmapImage_ImageOpened;
			val2.ImageFailed += bitmapImage_ImageFailed;
			val2.UriSource = ImageCollection[index].GetImageUri();
		}

		private void OnForwardImageSelected(int index)
		{
			if (_photoFbids != null)
			{
				FacebookUri forwardAttachmentUri = FacebookUri.GetForwardAttachmentUri(_photoFbids[index]);
				Navigator.Navigate(forwardAttachmentUri);
			}
		}

		private void DisplaySaveImageError()
		{
			IsImageLoading = false;
			IsSaveEnabled = true;
			ShowImageSaveFailureMessageBox();
			InvokeImageSaveCompleted(ActionState.Failed);
		}

		private void ShowImageSaveFailureMessageBox()
		{
			Error error = new Error();
			error.Message = StringLibrary.Photos_SavePhoto_Error_StateIndicator;
			Error error2 = error;
			OnError(error2);
		}

		private void bitmapImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
		{
			DisplaySaveImageError();
		}

		private async void bitmapImage_ImageOpened(object sender, RoutedEventArgs e)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				int num4 = num;
				int num5 = 0;
				try
				{
					WriteableBitmap writeableBitmap = new WriteableBitmap((BitmapSource)((sender is BitmapImage) ? sender : null));
					Picture img = null;
					byte[] imageBytes;
					await Task.Run(() =>
					{
						//IL_0037: Unknown result type (might be due to invalid IL or missing references)
						//IL_003d: Expected Obj, but got Unknown
						using (MemoryStream memoryStream = new MemoryStream())
						{
							writeableBitmap.SaveJpeg((Stream)memoryStream, ((BitmapSource)writeableBitmap).PixelWidth, ((BitmapSource)writeableBitmap).PixelHeight, 0, 100);
							imageBytes = memoryStream.ToArray();
							MediaLibrary val = new MediaLibrary();
							try
							{
								img = val.SavePicture(DateTime.Now.Ticks.ToString(), imageBytes);
							}
							finally
							{
								((IDisposable)val)?.Dispose();
							}
						}
					});
					if (img == (Picture)null)
					{
						DisplaySaveImageError();
					}
					else
					{
						InvokeImageSaveCompleted(ActionState.Succeeded);
					}
				}
				catch (InvalidOperationException ex)
				{
					Logger.Info($"Error while saving image, likely low storage space: {ex.Message}", "bitmapImage_ImageOpened", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\PhotoAttachmentsViewModel.cs", 395);
					DisplaySaveImageError();
				}
				catch (IOException ex2)
				{
					Logger.Info($"Error while saving image, likely low storage space: {ex2.Message}", "bitmapImage_ImageOpened", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\PhotoAttachmentsViewModel.cs", 400);
					DisplaySaveImageError();
				}
			}
			finally
			{
				IsImageLoading = false;
				IsSaveEnabled = true;
			}
		}

		private void InvokeImageSaveCompleted(ActionState actionState)
		{
			if (ImageSaveCompleted != null)
			{
				ImageSaveCompleted(this, new ImageSaveEventArgs(actionState));
			}
		}
	}
}
