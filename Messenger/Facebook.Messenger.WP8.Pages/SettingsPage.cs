using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Contacts;
using Facebook.Messenger.WP8.Controls.Photos;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Practices.ServiceLocation;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class SettingsPage : SettingsViewModelPage
	{
		private const string NewUserProfilePicFileName = "NewUserProfilePic.jpg";

		private MenuItem _turnOnNotificationsMenuItem;

		private MenuItem _turnOffForAnHourMenuItem;

		private MenuItem _turnOffUntil8MenuItem;

		private PhotoAttachmentPopup _photoPopup;

		private PhotoCroppingPopup _croppingPopup;

		public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(Error), typeof(SettingsPage), new PropertyMetadata((object)null, (PropertyChangedCallback)OnErrorChangedCallback));


















		public Error Error
		{
			get
			{
				return (Error)base.GetValue(ErrorProperty);
			}
			set
			{
				base.SetValue(ErrorProperty, (object)value);
			}
		}

		public SettingsPage()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected Obj, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeMenuItems();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
		}

		private static void OnErrorChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			SettingsPage settingsPage = dependencyObject as SettingsPage;
			if (settingsPage != null)
			{
				Error error = dependencyPropertyChangedEventArgs.NewValue as Error;
				if (error != null)
				{
					MessageBox.Show(error.Message, error.Title, (MessageBoxButton)0);
				}
			}
		}

		private void InitializeMenuItems()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected Obj, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected Obj, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected Obj, but got Unknown
			_turnOnNotificationsMenuItem = new MenuItem();
			_turnOnNotificationsMenuItem.Header = StringLibrary.Notifications_Turn_On;
			_turnOnNotificationsMenuItem.Click += TurnOn_OnTap;
			_turnOffForAnHourMenuItem = new MenuItem();
			_turnOffForAnHourMenuItem.Header = StringLibrary.Notifications_Turn_Off_For_An_Hour;
			_turnOffForAnHourMenuItem.Click += TurnOffForHour_OnTap;
			_turnOffUntil8MenuItem = new MenuItem();
			_turnOffUntil8MenuItem.Header = StringLibrary.Notifications_Turn_Until_8AM;
			_turnOffUntil8MenuItem.Click += TurnOff8AM_OnTap;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected Obj, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected Obj, but got Unknown
			DependencyProperty errorProperty = ErrorProperty;
			Binding val = new Binding();
			val.Path = new PropertyPath("Error", new object[0]);
			base.SetBinding(errorProperty, val);
			UpdatePhoneNumber();
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
				LocationToggleSwitch.IsChecked = vm.LocationEnabled;
				LocationToggleSwitch.Checked += LocationToggleSwitch_OnChecked;
				LocationToggleSwitch.Unchecked += LocationToggleSwitch_OnUnchecked;
				SendOnEnterToggleSwitch.IsChecked = vm.SendOnEnterEnabled;
				SendOnEnterToggleSwitch.Checked += SendOnEnterToggleSwitch_OnChecked;
				SendOnEnterToggleSwitch.Unchecked += SendOnEnterToggleSwitch_OnUnchecked;
				SyncContactsToggleSwitch.IsChecked = vm.ContactSyncEnabled;
				VersionTextBlock.Text = string.Format(StringLibrary.Settings_Version_Number, vm.VersionString);
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
			});
			FacebookPopup.SafeCleanUp(_croppingPopup);
			FacebookPopup.SafeCleanUp(_photoPopup);
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == "LocationEnabled")
			{
				WithViewModel((SettingsViewModel vm) =>
				{
					LocationToggleSwitch.IsChecked = vm.LocationEnabled;
				});
			}
			else if (propertyChangedEventArgs.PropertyName == "ContactSyncEnabled")
			{
				WithViewModel((SettingsViewModel vm) =>
				{
					SyncContactsToggleSwitch.IsChecked = vm.ContactSyncEnabled;
				});
			}
		}

		private void UpdatePhoneNumber()
		{
			FacebookContext instance = ServiceLocator.Current.GetInstance<FacebookContext>();
			string phoneNumber = instance.GetPhoneNumber();
			if (string.IsNullOrEmpty(phoneNumber))
			{
				((UIElement)PhoneNumberTextBlock).Visibility = Visibility.Collapsed;
				((UIElement)PhoneNumberConfirmLink).Visibility = Visibility.Visible;
				return;
			}
			((UIElement)PhoneNumberTextBlock).Visibility = Visibility.Visible;
			((UIElement)PhoneNumberConfirmLink).Visibility = Visibility.Collapsed;
			string phoneNumberState = instance.GetPhoneNumberState();
			if (phoneNumberState != null && (phoneNumberState.Equals(instance.PhoneNumberUnverified) || phoneNumberState.Equals(instance.PhoneNumberSkipped)))
			{
				string text = phoneNumber + " " + StringLibrary.Settings_PhoneNumber_VerificationNeeded;
				PhoneNumberTextBlock.Text = text;
			}
			else
			{
				PhoneNumberTextBlock.Text = phoneNumber;
			}
		}

		private void PhoneNumberConfirmLink_OnTap(object sender, GestureEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.NavigateToPhoneNumberPage.Execute(null);
			});
		}

		private void NotificationBoxOnTapped(object sender, GestureEventArgs e)
		{
			DisplayContextMenu();
		}

		private void DisplayContextMenu()
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				ContextMenu contextMenu = ContextMenuService.GetContextMenu((DependencyObject)(object)Notifications);
				if (contextMenu == null)
				{
					ContextMenu contextMenu2 = new ContextMenu();
					object obj = Application.Current.Resources[(object)"ContextMenuStyle"];
					((FrameworkElement)contextMenu2).Style = (Style)((obj is Style) ? obj : null);
					contextMenu = contextMenu2;
				}
				ContextMenu contextMenu3 = contextMenu;
				((PresentationFrameworkCollection<object>)(object)((ItemsControl)contextMenu3).Items).Clear();
				if (vm.IsMuted)
				{
					((PresentationFrameworkCollection<object>)(object)((ItemsControl)contextMenu3).Items).Add((object)_turnOnNotificationsMenuItem);
				}
				((PresentationFrameworkCollection<object>)(object)((ItemsControl)contextMenu3).Items).Add((object)_turnOffForAnHourMenuItem);
				if (!vm.IsMutedUntil8AM)
				{
					((PresentationFrameworkCollection<object>)(object)((ItemsControl)contextMenu3).Items).Add((object)_turnOffUntil8MenuItem);
				}
				ContextMenuService.SetContextMenu((DependencyObject)(object)Notifications, contextMenu3);
				contextMenu3.IsOpen = true;
			});
		}

		private async void TurnOffForHour_OnTap(object sender, RoutedEventArgs args)
		{
			await WithViewModel((SettingsViewModel vm) => vm.TurnOnMute(MuteDuration.Hour));
		}

		private async void TurnOff8AM_OnTap(object sender, RoutedEventArgs args)
		{
			await WithViewModel((SettingsViewModel vm) => vm.TurnOnMute(MuteDuration.EightAM));
		}

		private async void TurnOn_OnTap(object sender, RoutedEventArgs args)
		{
			await WithViewModel((SettingsViewModel vm) => vm.TurnOnMute(MuteDuration.Off));
		}

		private void PrivacyAndTerms_OnTapped(object sender, GestureEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.ShowDataUsePolicy();
			});
		}

		private void Help_OnTapped(object sender, GestureEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.ShowMessengerHelp();
			});
		}

		private void Licenses_OnTapped(object sender, GestureEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				if (vm.DisplayLicensesCommand.CanExecute(null))
				{
					vm.DisplayLicensesCommand.Execute(null);
				}
			});
		}

		private void LocationToggleSwitch_OnChecked(object sender, RoutedEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.SetLocationEnabledCommand.Execute(true);
			});
		}

		private void LocationToggleSwitch_OnUnchecked(object sender, RoutedEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.SetLocationEnabledCommand.Execute(false);
			});
		}

		private void SendOnEnterToggleSwitch_OnChecked(object sender, RoutedEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.SendOnEnterEnabled = true;
			});
		}

		private void SendOnEnterToggleSwitch_OnUnchecked(object sender, RoutedEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				vm.SendOnEnterEnabled = false;
			});
		}

		private void SyncContactsToggleSwitch_OnClick(object sender, RoutedEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				if (vm.ContactSyncEnabled)
				{
					DisableContactSyncMessageBox.Show(() =>
					{
						vm.ContactSyncEnabled = false;
					}, () =>
					{
						SyncContactsToggleSwitch.IsChecked = true;
					});
				}
				else
				{
					EnableContactSyncMessageBox.Show(vm.ContactSyncLearnMoreCommand, () =>
					{
						vm.ContactSyncEnabled = true;
					}, () =>
					{
						SyncContactsToggleSwitch.IsChecked = false;
					});
				}
			});
		}

		private void ProfilePic_OnTapped(object sender, GestureEventArgs e)
		{
			WithViewModel((SettingsViewModel vm) =>
			{
				if (!vm.IsLoading && vm.IsPartialUser)
				{
					if (_photoPopup == null)
					{
						_photoPopup = new PhotoAttachmentPopup();
						PhotoAttachmentPopup photoPopup = _photoPopup;
						object obj = Application.Current.Resources[(object)"MessengerBackgroundGreyBrush"];
						((Control)photoPopup).Background = (Brush)((obj is SolidColorBrush) ? obj : null);
						PhotoAttachmentPopup photoPopup2 = _photoPopup;
						object obj2 = Application.Current.Resources[(object)"MessengerBlackBrush"];
						((Control)photoPopup2).Foreground = (Brush)((obj2 is SolidColorBrush) ? obj2 : null);
						_photoPopup.Completed += OnPhotoPopupCompleted;
						_photoPopup.PopupTitle = StringLibrary.AddPhotos_Title_EditProfilePicture;
						_photoPopup.IsLightTheme = true;
						((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)_photoPopup);
					}
					_photoPopup.IsOpen = true;
				}
			});
		}

		private void OnPhotoPopupCompleted(object sender, PhotoResult e)
		{
			_photoPopup.IsOpen = false;
			if (e != null && e.ChosenPhoto != null)
			{
				WithViewModel((SettingsViewModel vm) =>
				{
					ShowCropPopup(vm.PhotoDataFromPhotoResult(e));
				});
			}
		}

		protected void ShowCropPopup(PhotoData image)
		{
			if (_croppingPopup == null)
			{
				_croppingPopup = new PhotoCroppingPopup();
				_croppingPopup.CropCompleted += OnCropCompleted;
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)_croppingPopup);
			}
			_croppingPopup.SetPhoto(image);
			_croppingPopup.IsOpen = true;
		}

		private void OnCropCompleted(object sender, BitmapSource image)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			_croppingPopup.IsOpen = false;
			if (image == null)
			{
				MessageBox.Show(StringLibrary.Error_Cannot_Attach_File);
				return;
			}
			WithViewModel(async (SettingsViewModel vm) =>
			{
				Uri uri = await vm.PrepareImageUriAsync(PhotoData.FromImage(image), "NewUserProfilePic.jpg");
				if (!(uri == null))
				{
					vm.UploadProfilePictureCommand.Execute(uri.LocalPath);
				}
			}).DoNotWait();
		}

		private void OnBackKeyPress(object sender, CancelEventArgs e)
		{
			if (_croppingPopup != null && _croppingPopup.IsOpen)
			{
				e.Cancel = true;
				_croppingPopup.IsOpen = false;
			}
			else if (_photoPopup != null && _photoPopup.IsOpen)
			{
				e.Cancel = true;
				_photoPopup.IsOpen = false;
			}
		}


		static SettingsPage()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
		}
	}
}
