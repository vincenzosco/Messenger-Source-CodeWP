using System;
using System.Device.Location;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Base.Extensions;
using Facebook.Models;
using Microsoft.Phone.Maps.Controls;
using Windows.Devices.Geolocation;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class LocationDrawer : ComposerDrawer
	{



		public event EventHandler<ImageResult> SendLocation;

		public LocationDrawer()
			: base(DrawerType.ImageSearch)
		{
			InitializeComponent();
		}

		public override void Show()
		{
			base.Visibility = Visibility.Visible;
			MapDisplay.Center = new GeoCoordinate();
			SetLocation().DoNotWait();
		}

		public override void Hide()
		{
			base.Visibility = Visibility.Collapsed;
		}

		public override void Cleanup()
		{
		}

		private async Task SetLocation()
		{
			Geolocator geolocator = new Geolocator();
			Geocoordinate geocoodinates = (await geolocator.GetGeopositionAsync()).Coordinate;
			GeoCoordinate locationCoodinates = CoordinateConverter.ConvertGeocoordinate(geocoodinates);
			PositionOverlay.GeoCoordinate = locationCoodinates;
			MapDisplay.Center = locationCoodinates;
			MapDisplay.ZoomLevel = 13.0;
		}

		private void MapDisplay_OnTap(object sender, GestureEventArgs e)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			WriteableBitmap val = new WriteableBitmap((int)((FrameworkElement)MapDisplay).ActualWidth, (int)((FrameworkElement)MapDisplay).ActualHeight);
			val.Render((UIElement)(object)MapDisplay, (Transform)null);
			val.Invalidate();
			ImageResult e2 = new ImageResult(MediaSource.Location, PhotoData.FromImage((BitmapSource)(object)val));
			if (SendLocation != null)
			{
				SendLocation(this, e2);
			}
		}

	}
}
