using System;
using System.Globalization;
using Facebook.Models;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class MappedMessageViewModel : MessageViewModel
	{
		private const string MapFormat = "http://fb.ecn.api.tiles.virtualearth.net/api/GetMap.ashx?b=r,mkt.{4},stl.fb&h={2}&w={3}&z=16&jq=100&ppl=37,,{0},{1}";

		public const string MapImageHeightProperty = "MapImageHeight";

		public const string MapImageUrlProperty = "MapImageUrl";

		private double _mapImageWidth;

		public double MapImageWidth
		{
			get
			{
				return _mapImageWidth;
			}
			set
			{
				if (Set(ref _mapImageWidth, value, false, "MapImageWidth"))
				{
					RaisePropertyChanged("MapImageHeight");
					RaisePropertyChanged("MapImageUrl");
				}
			}
		}

		public double MapImageHeight
		{
			get
			{
				return MapImageWidth * 0.3;
			}
		}

		public string MapImageUrl
		{
			get
			{
				if (!HasCoordinates)
				{
					return null;
				}
				Uri uri = new Uri(string.Format(CultureInfo.InvariantCulture, "http://fb.ecn.api.tiles.virtualearth.net/api/GetMap.ashx?b=r,mkt.{4},stl.fb&h={2}&w={3}&z=16&jq=100&ppl=37,,{0},{1}", Coordinates.Latitude, Coordinates.Longitude, MapImageHeight, MapImageWidth, CultureInfo.CurrentUICulture.Name), UriKind.Absolute);
				return uri.AbsoluteUri;
			}
		}

		protected MappedMessageViewModel()
		{
		}

		public MappedMessageViewModel(MailMessage message)
			: base(message)
		{
		}

		public static MappedMessageViewModel CreateFrom(MailMessage message)
		{
			MappedMessageViewModel mappedMessageViewModel = new MappedMessageViewModel();
			mappedMessageViewModel.UpdateMessage(message);
			return mappedMessageViewModel;
		}
	}
}
