using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Models;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls.Contacts;

public class FoundContactsPopup : FacebookPopup
{
	internal Grid FoundContactPopupControl;

	internal TextBlock FoundContactsHeader;

	internal FacebookListView FoundContactsList;

	private bool _contentLoaded;

	public event EventHandler<FacebookUser> ContactTapped;

	public event EventHandler<FacebookUser> ContactCardTapped;

	public FoundContactsPopup()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		((FrameworkElement)FoundContactPopupControl).Width = MaxContentPanelWidth;
		((FrameworkElement)FoundContactPopupControl).Height = MaxContentPanelHeight;
	}

	protected override void ViewModelUpdated(object oldViewModel)
	{
		base.ViewModelUpdated(oldViewModel);
		if (ViewModel is ObservableCollection<FacebookUser> observableCollection)
		{
			((LongListSelector)FoundContactsList).ItemsSource = observableCollection;
			FoundContactsHeader.Text = string.Format(StringLibrary.NewContacts_Header_FoundContacts, observableCollection.Count);
		}
	}

	private void Contact_OnTap(object sender, GestureEventArgs e)
	{
		if (ContactTapped != null && sender is ContactPhoneUserControl contactPhoneUserControl)
		{
			ContactTapped(this, ((FrameworkElement)contactPhoneUserControl).DataContext as FacebookUser);
		}
	}

	private void Contact_OnContactCardTap(object sender, FacebookUser user)
	{
		if (ContactCardTapped != null)
		{
			ContactCardTapped(this, user);
		}
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Contacts/FoundContactsPopup.xaml", UriKind.Relative));
			FoundContactPopupControl = (Grid)((FrameworkElement)this).FindName("FoundContactPopupControl");
			FoundContactsHeader = (TextBlock)((FrameworkElement)this).FindName("FoundContactsHeader");
			FoundContactsList = (FacebookListView)((FrameworkElement)this).FindName("FoundContactsList");
		}
	}
}
