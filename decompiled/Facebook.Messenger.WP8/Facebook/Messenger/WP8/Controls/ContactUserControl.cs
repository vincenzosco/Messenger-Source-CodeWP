using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Facebook.Messenger.WP8.Converters;
using Facebook.Models;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls;

public class ContactUserControl : ContactListItemControl
{
	public static readonly DependencyProperty NameCharacterSpacingProperty = DependencyProperty.Register("NameCharacterSpacing", typeof(int), typeof(ContactUserControl), new PropertyMetadata((object)0));

	public static readonly DependencyProperty NameFontSizeProperty = DependencyProperty.Register("NameFontSize", typeof(double), typeof(ContactUserControl), new PropertyMetadata((object)(double)Application.Current.Resources[(object)"PhoneFontSizeLarge"]));

	public static readonly DependencyProperty ImageSizeProperty = DependencyProperty.Register("ImageSize", typeof(double), typeof(ContactUserControl), new PropertyMetadata((object)80.0));

	public static readonly DependencyProperty AllowSelectionProperty = DependencyProperty.Register("AllowSelection", typeof(bool), typeof(ContactUserControl), new PropertyMetadata((object)false));

	public static readonly DependencyProperty ShowDeselectOptionProperty = DependencyProperty.Register("ShowDeselectOption", typeof(bool), typeof(ContactUserControl), new PropertyMetadata((object)false, (PropertyChangedCallback)OnShowDeselectUpdated));

	internal ContactListItemControl ContactUserControlRoot;

	internal Grid LayoutRoot;

	internal UserTile UserTile;

	internal DynamicTextBlock NameTextBlock;

	internal Image DeselectOption;

	private bool _contentLoaded;

	public double NameFontSize
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(NameFontSizeProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(NameFontSizeProperty, (object)value);
		}
	}

	public int NameCharacterSpacing
	{
		get
		{
			return (int)((DependencyObject)this).GetValue(NameCharacterSpacingProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(NameCharacterSpacingProperty, (object)value);
		}
	}

	public double ImageSize
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(ImageSizeProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ImageSizeProperty, (object)value);
		}
	}

	public bool AllowSelection
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(AllowSelectionProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(AllowSelectionProperty, (object)value);
		}
	}

	public bool ShowDeselectOption
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(ShowDeselectOptionProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ShowDeselectOptionProperty, (object)value);
		}
	}

	public event EventHandler<GestureEventArgs> OnDeselected;

	public event EventHandler<MailUser> ContactCardTap;

	private static void OnShowDeselectUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (d is ContactUserControl contactUserControl)
		{
			((UIElement)contactUserControl.DeselectOption).Visibility = BoolToVisibilityConverter.Convert(e.NewValue);
		}
	}

	public ContactUserControl()
	{
		InitializeComponent();
		SetBinding((FrameworkElement)(object)NameTextBlock, TextBlock.FontSizeProperty, "NameFontSize", this);
		SetBinding((FrameworkElement)(object)NameTextBlock, TextBlock.CharacterSpacingProperty, "NameCharacterSpacing", this);
		SetBinding((FrameworkElement)(object)UserTile, FrameworkElement.HeightProperty, "ImageSize", this);
		SetBinding((FrameworkElement)(object)UserTile, FrameworkElement.WidthProperty, "ImageSize", this);
	}

	private void SetBinding(FrameworkElement obj, DependencyProperty prop, string path, object source)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected Obj, but got Unknown
		Binding val = new Binding();
		val.Path = new PropertyPath(path, new object[0]);
		val.Source = source;
		Binding val2 = val;
		obj.SetBinding(prop, val2);
	}

	protected override void OnItemValueAssigned()
	{
		ViewModel.PropertyChanged += OnPropertyChanged;
	}

	protected override void Recycled(FacebookUser oldUser)
	{
		oldUser.PropertyChanged -= OnPropertyChanged;
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		UserTile.AssignImages();
	}

	private void Deselect_OnTap(object sender, GestureEventArgs e)
	{
		if (OnDeselected != null)
		{
			OnDeselected(this, e);
			e.Handled = true;
		}
	}

	private void ContactCard_OnTap(object sender, GestureEventArgs e)
	{
		if (ContactCardTap != null)
		{
			ContactCardTap(this, ((FrameworkElement)this).DataContext as MailUser);
			e.Handled = true;
		}
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ContactUserControl.xaml", UriKind.Relative));
			ContactUserControlRoot = (ContactListItemControl)((FrameworkElement)this).FindName("ContactUserControlRoot");
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			UserTile = (UserTile)((FrameworkElement)this).FindName("UserTile");
			NameTextBlock = (DynamicTextBlock)((FrameworkElement)this).FindName("NameTextBlock");
			DeselectOption = (Image)((FrameworkElement)this).FindName("DeselectOption");
		}
	}

	static ContactUserControl()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected Obj, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected Obj, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected Obj, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected Obj, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected Obj, but got Unknown
	}
}
