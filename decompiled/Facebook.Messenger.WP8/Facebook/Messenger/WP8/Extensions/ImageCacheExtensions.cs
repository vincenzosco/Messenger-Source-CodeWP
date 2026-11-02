using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Services.Api.Cache;

namespace Facebook.Messenger.WP8.Extensions;

public class ImageCacheExtensions
{
	private static readonly Uri BlankUri = new Uri(string.Empty, UriKind.Relative);

	private static readonly ImageCache ImageCache = new ImageCache();

	public static readonly DependencyProperty ImageUriProperty = DependencyProperty.RegisterAttached("ImageUri", typeof(Uri), typeof(ImageCacheExtensions), new PropertyMetadata((object)null, (PropertyChangedCallback)OnImageUriChanged));

	public static readonly DependencyProperty PlaceholderUriProperty = DependencyProperty.RegisterAttached("PlaceholderUri", typeof(Uri), typeof(ImageCacheExtensions), new PropertyMetadata((object)null, (PropertyChangedCallback)OnPlaceHolderChanged));

	public static readonly DependencyProperty ClearExistingImageProperty = DependencyProperty.RegisterAttached("ClearExistingImageProperty", typeof(bool), typeof(ImageCacheExtensions), new PropertyMetadata((object)true));

	public static readonly DependencyProperty ImageLoadedHandlerProperty = DependencyProperty.RegisterAttached("ImageLoadedHandlerProperty", typeof(WeakReference<Action<BitmapImage>>), typeof(ImageCacheExtensions), new PropertyMetadata((PropertyChangedCallback)null));

	internal static readonly DependencyProperty FetchingEntryProperty = DependencyProperty.RegisterAttached("FetchingEntryProperty", typeof(WeakReference<ImageFetchingEntry>), typeof(ImageCacheExtensions), new PropertyMetadata((PropertyChangedCallback)null));

	public static ICache Cache => ImageCache;

	public static Uri GetImageUri(FrameworkElement obj)
	{
		return (Uri)((DependencyObject)obj).GetValue(ImageUriProperty);
	}

	public static void SetImageUri(FrameworkElement obj, Uri value, bool clearExistingImage = true)
	{
		((DependencyObject)obj).SetValue(ClearExistingImageProperty, (object)clearExistingImage);
		((DependencyObject)obj).SetValue(ImageUriProperty, (object)value);
	}

	public static Uri GetPlaceholderUri(FrameworkElement obj)
	{
		return (Uri)((DependencyObject)obj).GetValue(PlaceholderUriProperty);
	}

	public static void SetPlaceholderUri(FrameworkElement obj, Uri value)
	{
		((DependencyObject)obj).SetValue(PlaceholderUriProperty, (object)value);
	}

	public static Action<BitmapImage> GetImageLoadedHandler(FrameworkElement obj)
	{
		WeakReference<Action<BitmapImage>> weakReference = (WeakReference<Action<BitmapImage>>)((DependencyObject)obj).GetValue(ImageLoadedHandlerProperty);
		if (weakReference == null)
		{
			return null;
		}
		Action<BitmapImage> target = null;
		weakReference.TryGetTarget(out target);
		return target;
	}

	public static void SetImageLoadedHandler(FrameworkElement obj, Action<BitmapImage> handler)
	{
		((DependencyObject)obj).SetValue(ImageLoadedHandlerProperty, (object)new WeakReference<Action<BitmapImage>>(handler));
	}

	internal static WeakReference<ImageFetchingEntry> GetFetchEntry(FrameworkElement obj)
	{
		return (WeakReference<ImageFetchingEntry>)((DependencyObject)obj).GetValue(FetchingEntryProperty);
	}

	internal static void SetFetchEntry(FrameworkElement obj, ImageFetchingEntry entry)
	{
		((DependencyObject)obj).SetValue(FetchingEntryProperty, (object)new WeakReference<ImageFetchingEntry>(entry));
	}

	private static void OnImageUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue != null && e.OldValue != null && e.NewValue.Equals(e.OldValue))
		{
			return;
		}
		bool flag = (bool)d.GetValue(ClearExistingImageProperty);
		Uri uri = e.NewValue as Uri;
		Image val = (Image)(object)((d is Image) ? d : null);
		if (val == null)
		{
			return;
		}
		if (flag)
		{
			ImageSource source = val.Source;
			BitmapImage val2 = (BitmapImage)(object)((source is BitmapImage) ? source : null);
			if (val2 != null)
			{
				val2.UriSource = null;
			}
			val.Source = null;
		}
		if (uri == null)
		{
			ImageCache.CancelExistingRequest(val);
			LoadPlaceHolder(val, GetPlaceholderUri((FrameworkElement)(object)val));
			return;
		}
		if (flag)
		{
			LoadPlaceHolder(val, GetPlaceholderUri((FrameworkElement)(object)val));
		}
		if (IsLocalUri(uri))
		{
			SetLocalBitmap(val, uri, isPlaceholder: false);
		}
		else
		{
			ImageCache.LoadMedia(uri, val);
		}
	}

	private static void OnPlaceHolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		Uri uri = e.NewValue as Uri;
		Image image = (Image)(object)((d is Image) ? d : null);
		LoadPlaceHolder(image, uri);
	}

	private static void LoadPlaceHolder(Image image, Uri uri)
	{
		if (uri == null)
		{
			return;
		}
		ImageSource source = image.Source;
		BitmapImage val = (BitmapImage)(object)((source is BitmapImage) ? source : null);
		Uri imageUri = GetImageUri((FrameworkElement)(object)image);
		if (val == null)
		{
			SetLocalBitmap(image, uri, isPlaceholder: true);
		}
		else if (val.UriSource != null && val.UriSource != BlankUri)
		{
			bool flag = val.UriSource == uri;
			if (!(imageUri == val.UriSource) && !flag)
			{
				SetLocalBitmap(image, uri, isPlaceholder: true);
			}
		}
	}

	private static void SetLocalBitmap(Image image, Uri uri, bool isPlaceholder)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected Obj, but got Unknown
		BitmapImage val = new BitmapImage();
		val.CreateOptions = (BitmapCreateOptions)18;
		val.UriSource = uri;
		BitmapImage source = val;
		ImageSource source2 = image.Source;
		BitmapImage val2 = (BitmapImage)(object)((source2 is BitmapImage) ? source2 : null);
		Uri imageUri = GetImageUri((FrameworkElement)(object)image);
		if (val2 == null)
		{
			image.Source = (ImageSource)(object)source;
		}
		else if (isPlaceholder)
		{
			bool flag = val2.UriSource == uri;
			if (!(val2.UriSource == imageUri) && !flag)
			{
				image.Source = (ImageSource)(object)source;
			}
		}
		else if (uri == imageUri && imageUri != val2.UriSource)
		{
			image.Source = (ImageSource)(object)source;
		}
	}

	public static bool IsLocalUri(Uri uri)
	{
		string text = uri.ToString();
		if (!text.StartsWith("/Assets", StringComparison.CurrentCultureIgnoreCase))
		{
			if (uri.IsAbsoluteUri)
			{
				return uri.IsFile;
			}
			return false;
		}
		return true;
	}

	public static void PrefetchImage(string url)
	{
		if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var result))
		{
			ImageCache.LoadMedia(result, null);
		}
	}

	static ImageCacheExtensions()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected Obj, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected Obj, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected Obj, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected Obj, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected Obj, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected Obj, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected Obj, but got Unknown
	}
}
