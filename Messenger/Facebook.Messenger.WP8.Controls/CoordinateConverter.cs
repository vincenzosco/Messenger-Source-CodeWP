using System.Device.Location;
using Windows.Devices.Geolocation;

namespace Facebook.Messenger.WP8.Controls
{
	public static class CoordinateConverter
	{
		public static GeoCoordinate ConvertGeocoordinate(Geocoordinate geocoordinate)
		{
			return new GeoCoordinate(geocoordinate.Latitude, geocoordinate.Longitude, geocoordinate.Altitude ?? double.NaN, geocoordinate.Accuracy, geocoordinate.AltitudeAccuracy ?? double.NaN, geocoordinate.Speed ?? double.NaN, geocoordinate.Heading ?? double.NaN);
		}
	}
}
