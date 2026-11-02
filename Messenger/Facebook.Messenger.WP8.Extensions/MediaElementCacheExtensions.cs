using System;
using System.Windows;
using System.Windows.Controls;
using Facebook.Services.Api.Cache;

namespace Facebook.Messenger.WP8.Extensions
{
	public class MediaElementCacheExtensions
	{
		private static readonly VideoCache VideoCache = new VideoCache();

		internal static readonly DependencyProperty FetchingEntryProperty = DependencyProperty.RegisterAttached("FetchingEntryProperty", typeof(WeakReference<VideoFetchingEntry>), typeof(MediaElementCacheExtensions), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty VideoDownloadedHandlerProperty = DependencyProperty.RegisterAttached("VideoDownloadedHandlerProperty", typeof(WeakReference<Action<Uri>>), typeof(MediaElementCacheExtensions), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty VideoUriProperty = DependencyProperty.RegisterAttached("VideoUri", typeof(Uri), typeof(MediaElementCacheExtensions), new PropertyMetadata((object)null, (PropertyChangedCallback)OnVideoUriChanged));

		public static ICache Cache
		{
			get
			{
				return VideoCache;
			}
		}

		internal static WeakReference<VideoFetchingEntry> GetFetchEntry(FrameworkElement obj)
		{
			return (WeakReference<VideoFetchingEntry>)((DependencyObject)obj).GetValue(FetchingEntryProperty);
		}

		internal static void SetFetchEntry(FrameworkElement obj, VideoFetchingEntry entry)
		{
			((DependencyObject)obj).SetValue(FetchingEntryProperty, (object)new WeakReference<VideoFetchingEntry>(entry));
		}

		public static Action<Uri> GetVideoDownloadedHandler(FrameworkElement obj)
		{
			WeakReference<Action<Uri>> weakReference = (WeakReference<Action<Uri>>)((DependencyObject)obj).GetValue(VideoDownloadedHandlerProperty);
			if (weakReference == null)
			{
				return null;
			}
			Action<Uri> target = null;
			weakReference.TryGetTarget(out target);
			return target;
		}

		public static void SetVideoDownloadedHandler(FrameworkElement obj, Action<Uri> handler)
		{
			((DependencyObject)obj).SetValue(VideoDownloadedHandlerProperty, (object)new WeakReference<Action<Uri>>(handler));
		}

		public static Uri GetVideoUri(FrameworkElement obj)
		{
			return (Uri)((DependencyObject)obj).GetValue(VideoUriProperty);
		}

		public static void SetVideoUri(FrameworkElement obj, Uri value)
		{
			((DependencyObject)obj).SetValue(VideoUriProperty, (object)value);
		}

		private static void OnVideoUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null && e.OldValue != null && e.NewValue.Equals(e.OldValue))
			{
				return;
			}
			MediaElement val = (MediaElement)(object)((d is MediaElement) ? d : null);
			if (val != null)
			{
				Uri uri = e.NewValue as Uri;
				if (uri == null)
				{
					VideoCache.CancelExistingRequest(val);
					val.Source = null;
				}
				else if (uri.IsAbsoluteUri && uri.IsFile)
				{
					val.Source = uri;
				}
				else
				{
					VideoCache.LoadMedia(uri, val);
				}
			}
		}

		static MediaElementCacheExtensions()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected Obj, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected Obj, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected Obj, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected Obj, but got Unknown
		}
	}
}
