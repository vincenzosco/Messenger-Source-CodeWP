using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Facebook.Messenger.WP8.ViewModels;

public class UriImageDrawerItemViewModel : ImageDrawerItemViewModel
{
	private readonly BitmapImage _image;

	private readonly string _name;

	private bool _isLoaded;

	public override bool CanSend
	{
		get
		{
			if (base.CanSend)
			{
				return _isLoaded;
			}
			return false;
		}
	}

	public UriImageDrawerItemViewModel(Uri source)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected Obj, but got Unknown
		_image = new BitmapImage(source);
		BitmapImage image = _image;
		EventHandler<RoutedEventArgs> eventHandler = (object sender, RoutedEventArgs args) =>
		{
			_isLoaded = true;
		};
		image.ImageOpened += eventHandler;
		_name = source.AbsoluteUri;
	}

	public override string GetName()
	{
		return _name;
	}

	public override PhotoData GetImageToSend()
	{
		return PhotoData.FromImage((BitmapSource)(object)_image);
	}

	public override ImageSource GetDisplayImage(int width, int height)
	{
		return (ImageSource)(object)_image;
	}

	public override RotationAngle GetRotationAngle()
	{
		return RotationAngle.Portrait;
	}
}
