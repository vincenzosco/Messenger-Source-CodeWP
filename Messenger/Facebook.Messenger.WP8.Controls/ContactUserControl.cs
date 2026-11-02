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

namespace Facebook.Messenger.WP8.Controls
{
	public partial class ContactUserControl : ContactListItemControl
	{
		public static readonly DependencyProperty NameCharacterSpacingProperty = DependencyProperty.Register("NameCharacterSpacing", typeof(int), typeof(ContactUserControl), new PropertyMetadata((object)0));

		public static readonly DependencyProperty NameFontSizeProperty = DependencyProperty.Register("NameFontSize", typeof(double), typeof(ContactUserControl), new PropertyMetadata((object)(double)Application.Current.Resources[(object)"PhoneFontSizeLarge"]));

		public static readonly DependencyProperty ImageSizeProperty = DependencyProperty.Register("ImageSize", typeof(double), typeof(ContactUserControl), new PropertyMetadata((object)80.0));

		public static readonly DependencyProperty AllowSelectionProperty = DependencyProperty.Register("AllowSelection", typeof(bool), typeof(ContactUserControl), new PropertyMetadata((object)false));

		public static readonly DependencyProperty ShowDeselectOptionProperty = DependencyProperty.Register("ShowDeselectOption", typeof(bool), typeof(ContactUserControl), new PropertyMetadata((object)false, (PropertyChangedCallback)OnShowDeselectUpdated));







		public double NameFontSize
		{
			get
			{
				return (double)base.GetValue(NameFontSizeProperty);
			}
			set
			{
				base.SetValue(NameFontSizeProperty, (object)value);
			}
		}

		public int NameCharacterSpacing
		{
			get
			{
				return (int)base.GetValue(NameCharacterSpacingProperty);
			}
			set
			{
				base.SetValue(NameCharacterSpacingProperty, (object)value);
			}
		}

		public double ImageSize
		{
			get
			{
				return (double)base.GetValue(ImageSizeProperty);
			}
			set
			{
				base.SetValue(ImageSizeProperty, (object)value);
			}
		}

		public bool AllowSelection
		{
			get
			{
				return (bool)base.GetValue(AllowSelectionProperty);
			}
			set
			{
				base.SetValue(AllowSelectionProperty, (object)value);
			}
		}

		public bool ShowDeselectOption
		{
			get
			{
				return (bool)base.GetValue(ShowDeselectOptionProperty);
			}
			set
			{
				base.SetValue(ShowDeselectOptionProperty, (object)value);
			}
		}

		public event EventHandler<GestureEventArgs> OnDeselected;

		public event EventHandler<MailUser> ContactCardTap;

		private static void OnShowDeselectUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			ContactUserControl contactUserControl = d as ContactUserControl;
			if (contactUserControl != null)
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
				ContactCardTap(this, base.DataContext as MailUser);
				e.Handled = true;
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
}
