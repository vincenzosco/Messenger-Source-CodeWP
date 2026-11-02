using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class SearchResultControl : SearchListResultControl
	{



		public event EventHandler<FacebookProfile> OnContactTapped;

		public event EventHandler<ThreadViewModel> OnGroupTapped;

		public SearchResultControl()
		{
			InitializeComponent();
		}

		protected override void OnItemValueAssigned()
		{
			FacebookUser facebookUser = ViewModel as FacebookUser;
			if (facebookUser != null)
			{
				DisplayContactResult(facebookUser);
				return;
			}
			ThreadViewModel threadViewModel = ViewModel as ThreadViewModel;
			if (threadViewModel != null)
			{
				DisplayGroupResult(threadViewModel);
			}
		}

		private void DisplayGroupResult(ThreadViewModel group)
		{
			((UIElement)GroupSearchResultPresenter).Visibility = Visibility.Visible;
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
			((UIElement)ContactSearchResultPresenter).Visibility = Visibility.Visible;
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
			if (OnGroupTapped == null)
			{
				return;
			}
			FrameworkElement val = (FrameworkElement)((sender is FrameworkElement) ? sender : null);
			if (val != null)
			{
				ThreadViewModel threadViewModel = val.DataContext as ThreadViewModel;
				if (threadViewModel != null)
				{
					OnGroupTapped(sender, threadViewModel);
				}
			}
		}

		private void ContactUserControlOnTap(object sender, GestureEventArgs e)
		{
			if (OnContactTapped == null)
			{
				return;
			}
			FrameworkElement val = (FrameworkElement)((sender is FrameworkElement) ? sender : null);
			if (val != null)
			{
				FacebookProfile facebookProfile = val.DataContext as FacebookProfile;
				if (facebookProfile != null)
				{
					OnContactTapped(sender, facebookProfile);
				}
			}
		}

		protected override void Recycled(object oldViewModel)
		{
			((UIElement)ContactSearchResultPresenter).Visibility = Visibility.Collapsed;
			((UIElement)GroupSearchResultPresenter).Visibility = Visibility.Collapsed;
		}

	}
}
