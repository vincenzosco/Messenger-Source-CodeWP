using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Converters;

public class ProfilePhotoConverter : IValueConverter
{
	private const string ImageUrlAuthenticated = "https://graph.facebook.com/{0}/picture?width={1}&height={1}&access_token={2}";

	private const string ImageUrl = "https://graph.facebook.com/{0}/picture?width={1}&height={1}";

	private const string DefaultProfilePic = "/Assets/Messages/Silhouette.png";

	private FacebookContext _context;

	public ProfilePhotoConverter()
	{
		_context = ServiceLocator.Current.GetInstance<FacebookContext>();
	}

	public static string CreatePhotoUrl(FacebookContext context, string id, int? size = 60)
	{
		if (size <= 0)
		{
			size = 60;
		}
		if (string.IsNullOrWhiteSpace(id))
		{
			return "/Assets/Messages/Silhouette.png";
		}
		if (context.IsAuthenticated())
		{
			return string.Format("https://graph.facebook.com/{0}/picture?width={1}&height={1}&access_token={2}", id, size, context.Session.Credentials.AccessToken);
		}
		return string.Format("https://graph.facebook.com/{0}/picture?width={1}&height={1}", id, size);
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (DesignerProperties.IsInDesignTool)
		{
			return value;
		}
		string id = value as string;
		int result = 0;
		if (parameter != null)
		{
			if (parameter is int)
			{
				result = (int)parameter;
			}
			else if (parameter is double)
			{
				result = System.Convert.ToInt32((double)parameter);
			}
			else
			{
				int.TryParse(parameter.ToString(), out result);
			}
		}
		return CreatePhotoUrl(_context, id, result);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
