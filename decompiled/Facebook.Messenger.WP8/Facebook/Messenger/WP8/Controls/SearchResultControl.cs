using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls;

public class SearchResultControl : SearchListResultControl
{
	internal ContentPresenter GroupSearchResultPresenter;

	internal ContentPresenter ContactSearchResultPresenter;

	private bool _contentLoaded;

	public event EventHandler<FacebookProfile> OnContactTapped;

	public event EventHandler<ThreadViewModel> OnGroupTapped;

	public SearchResultControl()
	{
		InitializeComponent();
	}

	protected override void OnItemValueAssigned()
	{
		if (ViewModel is FacebookUser contact)
		{
			DisplayContactResult(contact);
		}
		else if (ViewModel is ThreadViewModel threadViewModel)
		{
			DisplayGroupResult(threadViewModel);
		}
	}

	private void DisplayGroupResult(ThreadViewModel group)
	{
		((UIElement)GroupSearchResultPresenter).Visibility = (Visibility)0;
		ThreadParticipantControl threadParticipantControl;
		if (GroupSearchResultPresenter.Content == null)
		{
			threadParticipantControl = new ThreadParticipantControl();
			((UIElement)threadParticipantControl).Tap += GroupControlOnTap;
			GroupSearchResultPresenter.Content = threadParticipantControl;
		}
		else
		{
			threadParticipantControl = GroupSearchResultPresenter.Content as ThreadParticipantControl;
		}
		((FrameworkElement)threadParticipantControl).DataContext = group;
	}

	private void DisplayContactResult(FacebookUser contact)
	{
		((UIElement)ContactSearchResultPresenter).Visibility = (Visibility)0;
		ContactUserControl contactUserControl;
		if (ContactSearchResultPresenter.Content == null)
		{
			contactUserControl = new ContactUserControl();
			contactUserControl.ImageSize = 62.0;
			contactUserControl.NameFontSize = (double)Application.Current.Resources[(object)"PhoneFontSizeLarge"];
			((UIElement)contactUserControl).Tap += ContactUserControlOnTap;
			ContactSearchResultPresenter.Content = contactUserControl;
		}
		else
		{
			contactUserControl = ContactSearchResultPresenter.Content as ContactUserControl;
		}
		((FrameworkElement)contactUserControl).DataContext = contact;
	}

	private void GroupControlOnTap(object sender, GestureEventArgs e)
	{
		if (OnGroupTapped != null)
		{
			FrameworkElement val = (FrameworkElement)((sender is FrameworkElement) ? sender : null);
			if (val != null && val.DataContext is ThreadViewModel e2)
			{
				OnGroupTapped(sender, e2);
			}
		}
	}

	private void ContactUserControlOnTap(object sender, GestureEventArgs e)
	{
		if (OnContactTapped != null)
		{
			FrameworkElement val = (FrameworkElement)((sender is FrameworkElement) ? sender : null);
			if (val != null && val.DataContext is FacebookProfile e2)
			{
				OnContactTapped(sender, e2);
			}
		}
	}

	protected override void Recycled(object oldViewModel)
	{
		((UIElement)ContactSearchResultPresenter).Visibility = (Visibility)1;
		((UIElement)GroupSearchResultPresenter).Visibility = (Visibility)1;
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
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/SearchResultControl.xaml", UriKind.Relative));
			GroupSearchResultPresenter = (ContentPresenter)((FrameworkElement)this).FindName("GroupSearchResultPresenter");
			ContactSearchResultPresenter = (ContentPresenter)((FrameworkElement)this).FindName("ContactSearchResultPresenter");
		}
	}
}
