using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class UserTile : BaseControl
	{
		private readonly ProfilePhotoConverter _profilePhotoConverter = new ProfilePhotoConverter();

		private FacebookContext _context;

		public static readonly DependencyProperty IndicatorDiameterProperty = DependencyProperty.Register("IndicatorDiameter", typeof(double), typeof(UserTile), new PropertyMetadata((object)double.NaN, (PropertyChangedCallback)OnIndicatorDiameterChanged));





		[CompilerGenerated]
		private Visibility IndicatorVisibility__BackingField;

		private FacebookContext Context => _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);

		public Visibility IndicatorVisibility
		{
			[CompilerGenerated]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return IndicatorVisibility__BackingField;
			}
			[CompilerGenerated]
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				IndicatorVisibility__BackingField = value;
			}
		}

		public double IndicatorDiameter
		{
			get
			{
				return (double)base.GetValue(IndicatorDiameterProperty);
			}
			set
			{
				base.SetValue(IndicatorDiameterProperty, (object)value);
			}
		}

		public void HideUserTile()
		{
			base.Opacity = 0.0;
		}

		public void ShowUserTile()
		{
			base.Opacity = 1.0;
		}

		public UserTile()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected Obj, but got Unknown
			IndicatorVisibility = Visibility.Visible;
			InitializeComponent();
			IndicatorDiameter = 25.0;
			base.SizeChanged += OnSizeChanged;
		}

		protected override void ViewModelUpdated(object oldViewModel)
		{
			base.ViewModelUpdated(oldViewModel);
			AssignImages();
		}

		public void AssignImages()
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			FacebookUser facebookUser = base.DataContext as FacebookUser;
			if (facebookUser != null)
			{
				UpdateProfilePicture(base.ActualWidth);
				ProfileIndicator.IsPartialUser = Context.IsPartialUser() || facebookUser.IsPartial;
				ProfileIndicator.IsMessengerUser = facebookUser.IsMessengerUser;
				((UIElement)ProfileIndicator).Visibility = IndicatorVisibility;
			}
		}

		private void UpdateProfilePicture(double width)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected Obj, but got Unknown
			if (width != 0.0)
			{
				Image profilePicture = ProfilePicture;
				DependencyProperty imageUriProperty = ImageCacheExtensions.ImageUriProperty;
				Binding val = new Binding();
				val.Path = new PropertyPath("Id", new object[0]);
				val.Converter = (IValueConverter)(object)_profilePhotoConverter;
				val.ConverterParameter = width;
				((FrameworkElement)profilePicture).SetBinding(imageUriProperty, val);
				((DependencyObject)ProfilePicture).SetValue(RoundContentExtension.RadiusProperty, (object)(width / 2.0));
			}
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			UpdateProfilePicture(base.ActualWidth);
		}

		private static void OnIndicatorDiameterChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UserTile userTile = o as UserTile;
			if (userTile != null)
			{
				((FrameworkElement)userTile.ProfileIndicator).Width = userTile.IndicatorDiameter;
				((FrameworkElement)userTile.ProfileIndicator).Height = userTile.IndicatorDiameter;
			}
		}


		static UserTile()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected Obj, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected Obj, but got Unknown
		}
	}
}
