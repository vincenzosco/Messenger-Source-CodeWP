using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Facebook.Models;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls.Contacts
{
	public class ContactPhoneUserControl : ContactListItemControl
	{
		public static readonly DependencyProperty NameFontSizeProperty = DependencyProperty.Register("NameFontSize", typeof(double), typeof(ContactPhoneUserControl), new PropertyMetadata((object)(double)Application.Current.Resources[(object)"PhoneFontSizeLarge"]));

		public static readonly DependencyProperty ImageSizeProperty = DependencyProperty.Register("ImageSize", typeof(double), typeof(ContactPhoneUserControl), new PropertyMetadata((object)80.0));

		internal Grid LayoutRoot;

		internal UserTile ImageUserTile;

		internal DynamicTextBlock NameTextBlock;

		internal TextBlock PhoneNumberTextBlock;

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

		public event EventHandler<FacebookUser> ContactCardTap;

		public ContactPhoneUserControl()
		{
			InitializeComponent();
			SetBinding((FrameworkElement)(object)NameTextBlock, TextBlock.FontSizeProperty, "NameFontSize", this);
			SetBinding((FrameworkElement)(object)ImageUserTile, FrameworkElement.HeightProperty, "ImageSize", this);
			SetBinding((FrameworkElement)(object)ImageUserTile, FrameworkElement.WidthProperty, "ImageSize", this);
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
			PhoneInfo phoneInfo = ViewModel.Phones.FirstOrDefault();
			PhoneNumberTextBlock.Text = ((phoneInfo != null) ? phoneInfo.Number : string.Empty);
		}

		protected override void Recycled(FacebookUser oldUser)
		{
			oldUser.PropertyChanged -= OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			ImageUserTile.AssignImages();
		}

		private void ContactCard_OnTap(object sender, GestureEventArgs e)
		{
			if (ContactCardTap != null)
			{
				ContactCardTap(this, ((FrameworkElement)this).DataContext as FacebookUser);
				e.Handled = true;
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Contacts/ContactPhoneUserControl.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				ImageUserTile = (UserTile)((FrameworkElement)this).FindName("ImageUserTile");
				NameTextBlock = (DynamicTextBlock)((FrameworkElement)this).FindName("NameTextBlock");
				PhoneNumberTextBlock = (TextBlock)((FrameworkElement)this).FindName("PhoneNumberTextBlock");
			}
		}

		static ContactPhoneUserControl()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected Obj, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected Obj, but got Unknown
		}
	}
}
