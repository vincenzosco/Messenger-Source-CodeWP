using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base.Extensions;
using Facebook.Messenger.Localization.Resources;
using Facebook.Models;
using Facebook.Services.Stickers;
using GalaSoft.MvvmLight;

namespace Facebook.Messenger.WP8.ViewModels;

public class StickerPackViewModel : ViewModelBase
{
	public enum DownloadState
	{
		Available,
		Downloading,
		Downloaded,
		Failed
	}

	public const string DownloadStatePropertyName = "State";

	public const string DownloadErrorPropertyName = "DownloadError";

	public const string DownloadStateTextPropertyName = "DownloadStateText";

	public const string CanDownloadPropertyName = "CanDownload";

	public const string DownloadProgressPropertyName = "DownloadProgress";

	public const string HasDownloadProgressPropertyName = "HasDownloadProgress";

	public const string CanRemovePropertyName = "CanRemove";

	private readonly IStickerStorage _stickerStorage;

	private string _id;

	private string _name;

	private string _artist;

	private string _description;

	private bool _isFeatured;

	private bool _isPromoted;

	private int _price;

	private FacebookImage _thumbnailImage;

	private FacebookImage _previewImage;

	private ObservableCollection<StickerViewModel> _stickers;

	private DownloadState _state;

	private double _downloadProgress;

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

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			Set(ref _name, value, broadcast: false, "Name");
		}
	}

	public string Artist
	{
		get
		{
			return _artist;
		}
		set
		{
			Set(ref _artist, value, broadcast: false, "Artist");
		}
	}

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			Set(ref _description, value, broadcast: false, "Description");
		}
	}

	public bool IsFeatured
	{
		get
		{
			return _isFeatured;
		}
		set
		{
			Set(ref _isFeatured, value, broadcast: false, "IsFeatured");
		}
	}

	public bool IsPromoted
	{
		get
		{
			return _isPromoted;
		}
		set
		{
			Set(ref _isPromoted, value, broadcast: false, "IsPromoted");
		}
	}

	public int Price
	{
		get
		{
			return _price;
		}
		set
		{
			Set(ref _price, value, broadcast: false, "Price");
		}
	}

	public FacebookImage ThumbnailImage
	{
		get
		{
			return _thumbnailImage;
		}
		set
		{
			Set(ref _thumbnailImage, value, broadcast: false, "ThumbnailImage");
		}
	}

	public FacebookImage PreviewImage
	{
		get
		{
			return _previewImage;
		}
		set
		{
			Set(ref _previewImage, value, broadcast: false, "PreviewImage");
		}
	}

	public ObservableCollection<StickerViewModel> Stickers
	{
		get
		{
			return _stickers;
		}
		set
		{
			Set(ref _stickers, value, broadcast: false, "Stickers");
		}
	}

	public DownloadState State
	{
		get
		{
			return _state;
		}
		set
		{
			if (Set(ref _state, value, broadcast: false, "State"))
			{
				RaisePropertyChanged("DownloadError");
				RaisePropertyChanged("DownloadStateText");
				RaisePropertyChanged("CanDownload");
				RaisePropertyChanged("CanRemove");
				if (_state == DownloadState.Downloaded)
				{
					DownloadProgress = 100.0;
					LoadStickerImageUrisAsync().DoNotWait();
				}
			}
		}
	}

	public string DownloadError
	{
		get
		{
			if (State != DownloadState.Failed)
			{
				return null;
			}
			return StringLibrary.Error_Cannot_Fetch_Sticker_Packs;
		}
	}

	public string DownloadStateText => State switch
	{
		DownloadState.Downloading => StringLibrary.Sticker_Pack_Downloading, 
		DownloadState.Downloaded => StringLibrary.Sticker_Pack_Downloaded, 
		_ => StringLibrary.Sticker_Pack_Download, 
	};

	public bool CanDownload
	{
		get
		{
			if (State != DownloadState.Available)
			{
				return State == DownloadState.Failed;
			}
			return true;
		}
	}

	public double DownloadProgress
	{
		get
		{
			return _downloadProgress;
		}
		set
		{
			if (Set(ref _downloadProgress, value, broadcast: false, "DownloadProgress"))
			{
				RaisePropertyChanged("HasDownloadProgress");
			}
		}
	}

	public bool HasDownloadProgress => DownloadProgress > 0.0;

	public bool CanRemove
	{
		get
		{
			if (State == DownloadState.Downloaded)
			{
				return !StickerPack.DefaultStickerPackIds.Contains(Id);
			}
			return false;
		}
	}

	public StickerPackViewModel(IStickerStorage stickerStorage)
	{
		_stickerStorage = stickerStorage;
	}

	public StickerPackViewModel(StickerPack stickerPack, IStickerStorage stickerStorage)
		: this(stickerStorage)
	{
		UpdateFrom(stickerPack);
	}

	public async Task LoadStickerImageUrisAsync()
	{
		if (Stickers == null)
		{
			return;
		}
		foreach (StickerViewModel sticker in Stickers)
		{
			await sticker.LoadImageUriAsync(_stickerStorage);
		}
	}

	private void UpdateFrom(StickerPack stickerPack)
	{
		Id = stickerPack.Id;
		Name = stickerPack.Name;
		Artist = stickerPack.Artist;
		Description = stickerPack.Description;
		IsFeatured = stickerPack.IsFeatured;
		IsPromoted = stickerPack.IsPromoted;
		Price = stickerPack.Price;
		ThumbnailImage = stickerPack.ThumbnailImage;
		PreviewImage = stickerPack.PreviewImage;
		UpdateStickers(stickerPack.Stickers);
		if (stickerPack.IsDownloaded)
		{
			State = DownloadState.Downloaded;
		}
	}

	private void UpdateStickers(IEnumerable<Sticker> stickers)
	{
		IEnumerable<StickerViewModel> enumerable = stickers?.Select((Sticker s) => new StickerViewModel(s, Id));
		Stickers = ((enumerable != null) ? new ObservableCollection<StickerViewModel>(enumerable) : null);
	}
}
