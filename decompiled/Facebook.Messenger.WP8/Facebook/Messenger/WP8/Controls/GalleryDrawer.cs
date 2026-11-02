using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Facebook.Base.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Xna.Framework.Media;

namespace Facebook.Messenger.WP8.Controls;

public class GalleryDrawer : ComposerDrawer
{
	public bool RefreshGalleryDrawer;

	internal ImageDrawer GalleryImageDrawer;

	internal CircledIcon ImageChooser;

	private bool _contentLoaded;

	public event EventHandler<ImageResult> SendPhoto;

	public event EventHandler<ImageResult> EditPhoto;

	public event EventHandler GalleryButtonClicked;

	public GalleryDrawer()
		: base(DrawerType.Gallery)
	{
		InitializeComponent();
	}

	public async Task GetPhotos()
	{
		GalleryImageDrawer.IsLoading = true;
		List<GalleryImageDrawerItemViewModel> galleryItems = new List<GalleryImageDrawerItemViewModel>();
		await Task.Run(() =>
		{
			List<Picture> allPictures = new List<Picture>();
			IList<MediaSource> availableMediaSources = MediaSource.GetAvailableMediaSources();
			List<MediaLibrary> list = ((IEnumerable<MediaSource>)availableMediaSources).Select((Func<MediaSource, MediaLibrary>)((MediaSource source) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Expected Obj, but got Unknown
				return new MediaLibrary(source);
			})).ToList();
			list.ForEach((MediaLibrary library) =>
			{
				allPictures.AddRange((IEnumerable<Picture>)library.Pictures);
			});
			galleryItems = (from pic in allPictures
				orderby pic.Date descending
				select new GalleryImageDrawerItemViewModel(pic)).ToList();
		});
		GalleryImageDrawer.ItemsSource = galleryItems;
		GalleryImageDrawer.IsLoading = false;
		((UIElement)ImageChooser).Visibility = VisibilityUtil.FromBool(galleryItems.Any());
	}

	public override void Show()
	{
		((UIElement)this).Visibility = (Visibility)0;
		if (RefreshGalleryDrawer)
		{
			RefreshGalleryDrawer = false;
			GetPhotos().DoNotWait();
		}
	}

	public override void Hide()
	{
		GalleryImageDrawer.Reset();
		((UIElement)this).Visibility = (Visibility)1;
	}

	public override void Cleanup()
	{
	}

	public void ScrollToStart()
	{
		GalleryImageDrawer.Reset();
	}

	private void GalleryButton_Tap(object sender, GestureEventArgs e)
	{
		if (GalleryButtonClicked != null)
		{
			GalleryButtonClicked(this, null);
		}
	}

	private void GalleryImageDrawer_OnSendClicked(object sender, PhotoData image)
	{
		if (SendPhoto != null)
		{
			ImageResult e = new ImageResult(MediaSource.Gallery, image);
			SendPhoto(this, e);
		}
	}

	private void GalleryImageDrawer_OnEditClicked(object sender, PhotoData image)
	{
		if (EditPhoto != null)
		{
			ImageResult e = new ImageResult(MediaSource.Gallery, image);
			EditPhoto(this, e);
		}
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/GalleryDrawer.xaml", UriKind.Relative));
			GalleryImageDrawer = (ImageDrawer)((FrameworkElement)this).FindName("GalleryImageDrawer");
			ImageChooser = (CircledIcon)((FrameworkElement)this).FindName("ImageChooser");
		}
	}
}
