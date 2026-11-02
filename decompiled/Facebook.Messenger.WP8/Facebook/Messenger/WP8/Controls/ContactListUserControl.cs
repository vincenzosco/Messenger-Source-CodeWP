using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Contacts;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls
{
	public class ContactListUserControl : UserControl
	{
		public static readonly DependencyProperty AllowSelectionProperty = DependencyProperty.Register("AllowSelection", typeof(bool), typeof(ContactListUserControl), new PropertyMetadata((object)false));

		public static readonly DependencyProperty AllowMultipleSelectionProperty = DependencyProperty.Register("AllowMultipleSelection", typeof(bool), typeof(ContactListUserControl), new PropertyMetadata((object)false));

		public static readonly DependencyProperty ShowAllContactsProperty = DependencyProperty.Register("ShowAllContacts", typeof(bool), typeof(ContactListUserControl), new PropertyMetadata((object)false));

		internal UserControl ContactListUserControlRoot;

		internal Grid LayoutRoot;

		internal FacebookListView ContactsList;

		internal Grid ContactSyncPromoGrid;

		internal Grid PartialUserNux;

		internal TextBlock NuxTextBlock;

		internal Button NuxEmptyStateButton;

		private bool _contentLoaded;

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

		public bool AllowMultipleSelection
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(AllowMultipleSelectionProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(AllowMultipleSelectionProperty, (object)value);
			}
		}

		public bool ShowAllContacts
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(ShowAllContactsProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ShowAllContactsProperty, (object)value);
			}
		}

		public event EventHandler<NavigationEventArgs> NavigateEvent;

		public event EventHandler<UserSelectedEventArgs> UserSelectedEvent;

		public event EventHandler<EventArgs> UserSelectionChanged;

		public event EventHandler<IList> ContactSourceChanged;

		public event EventHandler<FacebookUser> ContactTapEvent;

		public event EventHandler<EventArgs> FoundContactsTapEvent;

		public ContactListUserControl()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			InitializeComponent();
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnloaded;
		}

		private void OnUnloaded(object sender, RoutedEventArgs args)
		{
			ContactsViewModel contactsViewModel = ((FrameworkElement)this).DataContext as ContactsViewModel;
			if (contactsViewModel != null)
			{
				contactsViewModel.PropertyChanged -= OnPropertyChanged;
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs args)
		{
			ContactsViewModel contactsViewModel = ((FrameworkElement)this).DataContext as ContactsViewModel;
			if (contactsViewModel != null)
			{
				contactsViewModel.PropertyChanged += OnPropertyChanged;
				UpdateContactItemSource();
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "ShouldGroupContacts")
			{
				UpdateContactItemSource();
			}
		}

		private void OnItemTapped(object sender, GestureEventArgs args)
		{
			ContactUserControl contactUserControl = sender as ContactUserControl;
			if (contactUserControl == null || NavigateEvent == null)
			{
				return;
			}
			if (AllowMultipleSelection)
			{
				FacebookUser facebookUser = ((FrameworkElement)contactUserControl).DataContext as FacebookUser;
				if (facebookUser != null)
				{
					facebookUser.IsSelected = !facebookUser.IsSelected;
				}
				return;
			}
			if (AllowSelection && UserSelectedEvent != null)
			{
				UserSelectedEvent(this, new UserSelectedEventArgs(new List<FacebookUser> { ((FrameworkElement)contactUserControl).DataContext as FacebookUser }));
				return;
			}
			FacebookProfile facebookProfile = ((FrameworkElement)contactUserControl).DataContext as FacebookProfile;
			if (facebookProfile != null)
			{
				FacebookUri conversationUri = FacebookUri.GetConversationUri(facebookProfile.Id, facebookProfile.Name);
				NavigateEvent(this, new NavigationEventArgs(conversationUri));
			}
		}

		private void UpdateContactItemSource()
		{
			ContactsViewModel contactsViewModel = ((FrameworkElement)this).DataContext as ContactsViewModel;
			if (contactsViewModel != null)
			{
				if (contactsViewModel.ShouldGroupContacts)
				{
					((LongListSelector)ContactsList).ItemsSource = contactsViewModel.AlphaGroupedContactList;
				}
				else
				{
					((LongListSelector)ContactsList).ItemsSource = ((ShowAllContacts || contactsViewModel.IsPartialUser) ? contactsViewModel.Contacts.AllUsers : contactsViewModel.ContactsWithMessenger);
				}
				if (ContactSourceChanged != null && ((LongListSelector)ContactsList).ItemsSource != null)
				{
					ContactSourceChanged(this, ((LongListSelector)ContactsList).ItemsSource);
				}
			}
		}

		private void ToggleButton_OnCheckChanged(object sender, RoutedEventArgs e)
		{
			if (UserSelectionChanged != null)
			{
				UserSelectionChanged(this, new EventArgs());
			}
		}

		private void ContactUserControl_OnContactCardTap(object sender, FacebookUser e)
		{
			if (ContactTapEvent != null)
			{
				ContactTapEvent(this, e);
			}
		}

		private void NuxEmptyStateButton_OnClick(object sender, RoutedEventArgs e)
		{
			ContactsViewModel vm = ((FrameworkElement)this).DataContext as ContactsViewModel;
			if (vm == null)
			{
				return;
			}
			if (vm.Context.IsContactSyncEnabled())
			{
				vm.AddContactCommand.Execute(null);
				return;
			}
			EnableContactSyncMessageBox.Show(vm.ContactSyncLearnMoreCommand, () =>
			{
				vm.FindContactsCommand.Execute(null);
			}, null);
		}

		private void FindContacts_OnClick(object sender, RoutedEventArgs e)
		{
			ContactsViewModel vm = ((FrameworkElement)this).DataContext as ContactsViewModel;
			if (vm != null && (vm.ContactSyncStatus.Status == SyncState.NotStarted || vm.ContactSyncStatus.Status == SyncState.Failed))
			{
				EnableContactSyncMessageBox.Show(vm.ContactSyncLearnMoreCommand, () =>
				{
					vm.FindContactsCommand.Execute(null);
				}, null);
			}
		}

		private void ShowAllFoundContacts_OnClick(object sender, RoutedEventArgs e)
		{
			ContactsViewModel contactsViewModel = ((FrameworkElement)this).DataContext as ContactsViewModel;
			if (contactsViewModel != null && FoundContactsTapEvent != null)
			{
				FoundContactsTapEvent(this, (EventArgs)(object)e);
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ContactListUserControl.xaml", UriKind.Relative));
				ContactListUserControlRoot = (UserControl)((FrameworkElement)this).FindName("ContactListUserControlRoot");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				ContactsList = (FacebookListView)((FrameworkElement)this).FindName("ContactsList");
				ContactSyncPromoGrid = (Grid)((FrameworkElement)this).FindName("ContactSyncPromoGrid");
				PartialUserNux = (Grid)((FrameworkElement)this).FindName("PartialUserNux");
				NuxTextBlock = (TextBlock)((FrameworkElement)this).FindName("NuxTextBlock");
				NuxEmptyStateButton = (Button)((FrameworkElement)this).FindName("NuxEmptyStateButton");
			}
		}

		static ContactListUserControl()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected Obj, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected Obj, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected Obj, but got Unknown
		}
	}
}
