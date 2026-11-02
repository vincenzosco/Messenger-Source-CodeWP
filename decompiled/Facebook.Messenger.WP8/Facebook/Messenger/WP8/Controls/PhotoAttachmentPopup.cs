using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Tasks;

namespace Facebook.Messenger.WP8.Controls;

public class PhotoAttachmentPopup : FacebookPopup
{
	private CameraCaptureTask _cameraCaptureTask;

	private PhotoChooserTask _photoChooserTask;

	private bool _showingPhotoChooser;

	public static readonly DependencyProperty IsLightThemeProperty = DependencyProperty.Register("IsLightTheme", typeof(bool), typeof(PhotoAttachmentPopup), new PropertyMetadata((object)false, (PropertyChangedCallback)IsLightTheme_OnChanged));

	public static readonly DependencyProperty PopupTitleProperty = DependencyProperty.Register("PopupTitle", typeof(string), typeof(PhotoAttachmentPopup), new PropertyMetadata((object)string.Empty));

	public static readonly DependencyProperty ShowRemovePhotoOptionProperty = DependencyProperty.Register("ShowRemovePhotoOption", typeof(bool), typeof(PhotoAttachmentPopup), new PropertyMetadata((object)false));

	internal FacebookPopup RootControl;

	internal Grid AddPhotosControl;

	internal TextBlock PhotoChooserPopupTitle;

	internal Button TakePhotoButton;

	internal CircledIcon TakePhotoIconWhite;

	internal Image TakePhotoIconBlack;

	internal TextBlock TakePhotoText;

	internal CircledIcon AttachImageIconWhite;

	internal Image AttachImageIconBlack;

	internal TextBlock AttachImageText;

	internal CircledIcon RemoveImageIconWhite;

	internal Image RemoveImageIconBlack;

	internal TextBlock RemoveImageText;

	private bool _contentLoaded;

	public bool IsLightTheme
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsLightThemeProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsLightThemeProperty, (object)value);
		}
	}

	public string PopupTitle
	{
		get
		{
			return (string)((DependencyObject)this).GetValue(PopupTitleProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(PopupTitleProperty, (object)value);
		}
	}

	public bool ShowRemovePhotoOption
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(ShowRemovePhotoOptionProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ShowRemovePhotoOptionProperty, (object)value);
		}
	}

	public event EventHandler<PhotoResult> Completed;

	private static void IsLightTheme_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		PhotoAttachmentPopup photoAttachmentPopup = d as PhotoAttachmentPopup;
		if (photoAttachmentPopup.IsLightTheme)
		{
			Grid addPhotosControl = photoAttachmentPopup.AddPhotosControl;
			object obj = Application.Current.Resources[(object)"MessengerBackgroundGreyBrush"];
			((Panel)addPhotosControl).Background = (Brush)((obj is SolidColorBrush) ? obj : null);
			TextBlock photoChooserPopupTitle = photoAttachmentPopup.PhotoChooserPopupTitle;
			object obj2 = Application.Current.Resources[(object)"MessengerBlackBrush"];
			photoChooserPopupTitle.Foreground = (Brush)((obj2 is SolidColorBrush) ? obj2 : null);
			TextBlock takePhotoText = photoAttachmentPopup.TakePhotoText;
			object obj3 = Application.Current.Resources[(object)"MessengerBlackBrush"];
			takePhotoText.Foreground = (Brush)((obj3 is SolidColorBrush) ? obj3 : null);
			TextBlock attachImageText = photoAttachmentPopup.AttachImageText;
			object obj4 = Application.Current.Resources[(object)"MessengerBlackBrush"];
			attachImageText.Foreground = (Brush)((obj4 is SolidColorBrush) ? obj4 : null);
			TextBlock removeImageText = photoAttachmentPopup.RemoveImageText;
			object obj5 = Application.Current.Resources[(object)"MessengerBlackBrush"];
			removeImageText.Foreground = (Brush)((obj5 is SolidColorBrush) ? obj5 : null);
			((UIElement)photoAttachmentPopup.TakePhotoIconWhite).Visibility = (Visibility)1;
			((UIElement)photoAttachmentPopup.AttachImageIconWhite).Visibility = (Visibility)1;
			((UIElement)photoAttachmentPopup.RemoveImageIconWhite).Visibility = (Visibility)1;
			((UIElement)photoAttachmentPopup.TakePhotoIconBlack).Visibility = (Visibility)0;
			((UIElement)photoAttachmentPopup.AttachImageIconBlack).Visibility = (Visibility)0;
			((UIElement)photoAttachmentPopup.RemoveImageIconBlack).Visibility = (Visibility)0;
		}
	}

	public PhotoAttachmentPopup()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
	}

	protected override void Setup()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected Obj, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected Obj, but got Unknown
		base.Setup();
		if (_cameraCaptureTask == null)
		{
			_cameraCaptureTask = new CameraCaptureTask();
			((ChooserBase<PhotoResult>)(object)_cameraCaptureTask).Completed += PhotoTask_Completed;
		}
		if (_photoChooserTask == null)
		{
			_photoChooserTask = new PhotoChooserTask();
			((ChooserBase<PhotoResult>)(object)_photoChooserTask).Completed += PhotoTask_Completed;
		}
		((Control)TakePhotoButton).Focus();
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		((FrameworkElement)AddPhotosControl).Width = MaxContentPanelWidth;
		((FrameworkElement)AddPhotosControl).Height = MaxContentPanelHeight;
	}

	private void PhotoTask_Completed(object sender, PhotoResult e)
	{
		_showingPhotoChooser = false;
		if (Completed != null)
		{
			Completed(this, e);
		}
	}

	private void TakePhoto_OnTap(object sender, GestureEventArgs e)
	{
		ShowChooser((ChooserBase<PhotoResult>)(object)_cameraCaptureTask);
	}

	private void ChoosePhoto_OnTap(object sender, GestureEventArgs e)
	{
		ShowChooser((ChooserBase<PhotoResult>)(object)_photoChooserTask);
	}

	private void RemovePhoto_OnTap(object sender, GestureEventArgs e)
	{
		if (Completed != null)
		{
			Completed(this, null);
		}
	}

	public void ShowChooser(ChooserBase<PhotoResult> chooser)
	{
		if (!_showingPhotoChooser)
		{
			_showingPhotoChooser = true;
			chooser.Show();
		}
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected Obj, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected Obj, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected Obj, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected Obj, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected Obj, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/PhotoAttachmentPopup.xaml", UriKind.Relative));
			RootControl = (FacebookPopup)((FrameworkElement)this).FindName("RootControl");
			AddPhotosControl = (Grid)((FrameworkElement)this).FindName("AddPhotosControl");
			PhotoChooserPopupTitle = (TextBlock)((FrameworkElement)this).FindName("PhotoChooserPopupTitle");
			TakePhotoButton = (Button)((FrameworkElement)this).FindName("TakePhotoButton");
			TakePhotoIconWhite = (CircledIcon)((FrameworkElement)this).FindName("TakePhotoIconWhite");
			TakePhotoIconBlack = (Image)((FrameworkElement)this).FindName("TakePhotoIconBlack");
			TakePhotoText = (TextBlock)((FrameworkElement)this).FindName("TakePhotoText");
			AttachImageIconWhite = (CircledIcon)((FrameworkElement)this).FindName("AttachImageIconWhite");
			AttachImageIconBlack = (Image)((FrameworkElement)this).FindName("AttachImageIconBlack");
			AttachImageText = (TextBlock)((FrameworkElement)this).FindName("AttachImageText");
			RemoveImageIconWhite = (CircledIcon)((FrameworkElement)this).FindName("RemoveImageIconWhite");
			RemoveImageIconBlack = (Image)((FrameworkElement)this).FindName("RemoveImageIconBlack");
			RemoveImageText = (TextBlock)((FrameworkElement)this).FindName("RemoveImageText");
		}
	}

	static PhotoAttachmentPopup()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected Obj, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected Obj, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected Obj, but got Unknown
	}
}
