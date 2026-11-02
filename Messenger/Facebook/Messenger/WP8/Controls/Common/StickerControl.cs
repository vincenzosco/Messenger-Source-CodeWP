using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Base.Extensions;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.Utilities.Stickers;
using Facebook.Models;
using ImageTools;
using ImageTools.Controls;
using ImageTools.IO;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Controls.Common
{
	public partial class StickerControl : BaseControl, IResetableView
	{
		private const int DefaultStickerSize = 120;

		private const int SmallLikeStickerSize = 36;

		private const int MediumLikeStickerSize = 84;

		private const int LargeLikeStickerSize = 150;

		private static readonly BitmapImage StickerPlaceholderImage;

		private static readonly BitmapImage LikeStickerImage;

		private MailShare _share;

		private bool _animated;

		private readonly Image NestedImage;

		private readonly StickerImageUrlHelper _stickerImageUrlHelper;



		static StickerControl()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected Obj, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected Obj, but got Unknown
			StickerPlaceholderImage = new BitmapImage(new Uri("/Assets/Stickers/StickerPlaceholder.png", UriKind.Relative));
			LikeStickerImage = new BitmapImage(new Uri("/Assets/Stickers/LikeSticker.png", UriKind.Relative));
			Decoders.AddDecoder<WebPDecoder>();
		}

		public StickerControl()
		{
			InitializeComponent();
			_stickerImageUrlHelper = ServiceLocator.Current.GetInstance<StickerImageUrlHelper>();
			NestedImage = ((DependencyObject)(object)StickerImage).GetFirstDescendantOfType<Image>(true);
		}

		public async Task UpdateShare(MailShare share, bool animated)
		{
			int stickerImageSize = GetShareStickerSize(share);
			AnimatedImage stickerImage = StickerImage;
			double width = (((FrameworkElement)StickerImage).Height = stickerImageSize);
			((FrameworkElement)stickerImage).Width = width;
			if (share == null)
			{
				SetRegularImage(StickerPlaceholderImage);
			}
			else if ("227878347358915" == share.StickerId)
			{
				_share = share;
				_animated = animated;
				SetRegularImage(LikeStickerImage);
			}
			else
			{
				if (_share != null && !(share.StickerId != _share.StickerId) && animated == _animated)
				{
					return;
				}
				bool isSameSticker = _share != null && share.StickerId == _share.StickerId;
				bool wasAnimated = _animated;
				_share = share;
				_animated = animated;
				StickerImage.Stop();
				if (isSameSticker)
				{
					if (wasAnimated && !animated)
					{
						return;
					}
				}
				else
				{
					SetRegularImage(StickerPlaceholderImage);
				}
				Uri uri;
				Uri.TryCreate(share.Href, UriKind.Absolute, out uri);
				StickerImageUris stickerImageUris = await _stickerImageUrlHelper.GetStickerImageUris(share.StickerId, uri);
				if (stickerImageUris.AnimatedStickerUri != null && animated)
				{
					LoadExtendImage(stickerImageUris.AnimatedStickerUri, share.StickerId);
				}
				else
				{
					LoadRegularImage(stickerImageUris.StickerUri, share.StickerId);
				}
			}
		}

		private static int GetShareStickerSize(MailShare share)
		{
			if (share == null || !MailShare.LikeStickerIds.Contains(share.StickerId))
			{
				return 120;
			}
			switch (share.StickerId)
			{
			case "369239263222822":
				return 36;
			case "227878347358915":
			case "369239343222814":
				return 84;
			default:
				return 150;
			}
		}

		private bool HasControlRecycled(string stickerId)
		{
			if (_share != null)
			{
				return _share.StickerId != stickerId;
			}
			return true;
		}

		private void LoadRegularImage(Uri uri, string stickerId)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected Obj, but got Unknown
			BitmapImage val = new BitmapImage(uri);
			val.CreateOptions = (BitmapCreateOptions)16;
			BitmapImage newImage = val;
			newImage.ImageOpened += (object param0, RoutedEventArgs param1) =>
			{
				if (!HasControlRecycled(stickerId))
				{
					SetRegularImage(newImage);
				}
			};
		}

		private void SetRegularImage(BitmapImage bitmapImage)
		{
			StickerImage.Source = null;
			NestedImage.Source = (ImageSource)(object)bitmapImage;
		}

		public void LoadExtendImage(Uri uri, string stickerId)
		{
			ExtendedImage newImage = new ExtendedImage(uri);
			newImage.LoadingCompleted += (object param0, EventArgs param1) =>
			{
				((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(() =>
				{
					if (!HasControlRecycled(stickerId))
					{
						SetExtendImage(newImage);
					}
				}));
			};
		}

		public void SetExtendImage(ExtendedImage image)
		{
			NestedImage.Source = null;
			StickerImage.Source = image;
		}

		private void StickerImage_OnTap(object sender, GestureEventArgs e)
		{
			UpdateShare(_share, true).DoNotWait();
			StickerImage.Start();
			e.Handled = true;
		}

		public void ResetView(bool recycled)
		{
			if (recycled)
			{
				if (StickerImage.Source != null)
				{
					StickerImage.Source.Frames.Clear();
				}
				NestedImage.Source = null;
				StickerImage.Source = null;
				_animated = false;
				_share = null;
			}
		}

	}
}
