using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls;

public sealed class FaceMash : BaseControl
{
	private static readonly Uri SilhouettePlaceholderUri = new Uri("/Assets/Messages/Silhouette.png", UriKind.Relative);

	private static readonly Uri GroupPlaceholderUri = new Uri("/Assets/Messages/chatSquare.png", UriKind.Relative);

	private FacebookContext _context;

	private static readonly GridLength ZeroStar = new GridLength(0.0, (GridUnitType)2);

	private static readonly GridLength OneStar = new GridLength(1.0, (GridUnitType)2);

	internal Grid FaceMashGrid;

	internal EllipseGeometry ClipGeometry;

	internal ColumnDefinition Column;

	internal Image First;

	internal Image Second;

	internal Image Third;

	private bool _contentLoaded;

	private FacebookContext Context => _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);

	public int ImageSize => (int)((FrameworkElement)this).Width;

	private void OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (!double.IsNaN(((FrameworkElement)this).Width) && !object.Equals(((FrameworkElement)this).Width, 0.0))
		{
			double num = ((FrameworkElement)this).Width / 2.0;
			if (ClipGeometry.RadiusX != num)
			{
				ClipGeometry.Center = new Point(num, num);
				ClipGeometry.RadiusX = num;
				ClipGeometry.RadiusY = num;
			}
		}
	}

	public FaceMash()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).SizeChanged += OnSizeChanged;
	}

	protected override void ViewModelUpdated(object oldViewModel)
	{
		base.ViewModelUpdated(oldViewModel);
		SetFaceMash();
	}

	public void SetFaceMash()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (((FrameworkElement)this).DataContext is ThreadViewModel threadViewModel)
		{
			if (threadViewModel.HasPicture() || threadViewModel.IsCanonical())
			{
				Column.Width = ZeroStar;
			}
			else
			{
				IEnumerable<MailUser> source = threadViewModel.CalculateParticipantsByLastSend(Context.Session.UserId);
				GridLength width = ((source.Count() < 2) ? ZeroStar : OneStar);
				Column.Width = width;
			}
			Uri imageUri = new Uri(UrlForLargeImage(threadViewModel), UriKind.RelativeOrAbsolute);
			Uri imageUri2 = null;
			Uri imageUri3 = null;
			if (!threadViewModel.HasPicture() && !threadViewModel.IsCanonical())
			{
				imageUri2 = new Uri(UrlForSmallImage(1, threadViewModel), UriKind.RelativeOrAbsolute);
				imageUri3 = new Uri(UrlForSmallImage(2, threadViewModel), UriKind.RelativeOrAbsolute);
			}
			SetImageWithPlaceHolder(First, imageUri, threadViewModel.HasPicture() ? GroupPlaceholderUri : SilhouettePlaceholderUri);
			SetImageWithPlaceHolder(Second, imageUri2, SilhouettePlaceholderUri);
			SetImageWithPlaceHolder(Third, imageUri3, SilhouettePlaceholderUri);
		}
	}

	private void SetImageWithPlaceHolder(Image image, Uri imageUri, Uri placeHolderUri)
	{
		ImageCacheExtensions.SetPlaceholderUri((FrameworkElement)(object)image, placeHolderUri);
		ImageCacheExtensions.SetImageUri((FrameworkElement)(object)image, imageUri);
	}

	public string UrlForSmallImage(int position, ThreadViewModel thread)
	{
		string userId = Context.Session.UserId;
		IEnumerable<MailUser> source = thread.CalculateParticipantsByLastSend(userId);
		if (source.Count() > position)
		{
			string id = source.ElementAt(position).Id;
			return ProfilePhotoConverter.CreatePhotoUrl(Context, id, ImageSize / 2);
		}
		return ProfilePhotoConverter.CreatePhotoUrl(Context, userId, ImageSize / 2);
	}

	public string UrlForLargeImage(ThreadViewModel thread)
	{
		string userId = Context.Session.UserId;
		string text = "";
		if (thread.HasPicture())
		{
			return thread.PictureUrl;
		}
		if (thread.IsCanonical())
		{
			text = thread.SingleRecipient;
		}
		else
		{
			IEnumerable<MailUser> source = thread.CalculateParticipantsByLastSend(userId);
			text = (source.Any() ? source.First().Id : userId);
		}
		return ProfilePhotoConverter.CreatePhotoUrl(Context, text, ImageSize);
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected Obj, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected Obj, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/FaceMash.xaml", UriKind.Relative));
			FaceMashGrid = (Grid)((FrameworkElement)this).FindName("FaceMashGrid");
			ClipGeometry = (EllipseGeometry)((FrameworkElement)this).FindName("ClipGeometry");
			Column = (ColumnDefinition)((FrameworkElement)this).FindName("Column");
			First = (Image)((FrameworkElement)this).FindName("First");
			Second = (Image)((FrameworkElement)this).FindName("Second");
			Third = (Image)((FrameworkElement)this).FindName("Third");
		}
	}

	static FaceMash()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
	}
}
