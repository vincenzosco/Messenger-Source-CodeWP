using System;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Stickers;
using GalaSoft.MvvmLight;

namespace Facebook.Messenger.WP8.ViewModels;

public class StickerViewModel : ViewModelBase
{
	private string _id;

	private string _stickerPackId;

	private Uri _imageUri;

	private FacebookImage _threadImage;

	private FacebookImage _composerImage;

	private FacebookImage _animatedImage;

	public string Id
	{
		get
		{
			return _id;
		}
		set
		{
			Set(ref _id, value, broadcast: false, "Id");
		}
	}

	public string StickerPackId
	{
		get
		{
			return _stickerPackId;
		}
		set
		{
			Set(ref _stickerPackId, value, broadcast: false, "StickerPackId");
		}
	}

	public Uri ImageUri
	{
		get
		{
			return _imageUri;
		}
		set
		{
			Set(ref _imageUri, value, broadcast: false, "ImageUri");
		}
	}

	public FacebookImage ThreadImage
	{
		get
		{
			return _threadImage;
		}
		set
		{
			Set(ref _threadImage, value, broadcast: false, "ThreadImage");
		}
	}

	public FacebookImage ComposerImage
	{
		get
		{
			return _composerImage;
		}
		set
		{
			Set(ref _composerImage, value, broadcast: false, "ComposerImage");
		}
	}

	public FacebookImage AnimatedImage
	{
		get
		{
			return _animatedImage;
		}
		set
		{
			Set(ref _animatedImage, value, broadcast: false, "AnimatedImage");
		}
	}

	public StickerViewModel(Sticker sticker, string stickerPackId)
	{
		UpdateFrom(sticker);
		StickerPackId = stickerPackId;
	}

	public async Task LoadImageUriAsync(IStickerStorage stickerStorage)
	{
		ImageUri = await stickerStorage.GenerateStickerUri(StickerPackId, Id, animatedSticker: false);
	}

	private void UpdateFrom(Sticker sticker)
	{
		Id = sticker.Id;
		ThreadImage = sticker.ThreadImage;
		ComposerImage = sticker.ComposerImage;
		AnimatedImage = sticker.AnimatedImage;
	}
}
