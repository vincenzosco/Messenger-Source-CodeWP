using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class GroupTileControl : BaseControl
	{
		private FacebookContext _context;

		private ThreadViewModel _currentThread;

















		public event EventHandler<GestureEventArgs> ThreadTap;

		public event EventHandler<GestureEventArgs> UnpinTap;

		public GroupTileControl()
		{
			InitializeComponent();
			if (_context == null)
			{
				_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext;
			}
		}

		protected override void ViewModelUpdated(object oldViewModel)
		{
			base.ViewModelUpdated(oldViewModel);
			if (_currentThread != null)
			{
				_currentThread.PropertyChanged -= OnThreadPropertyChanged;
			}
			_currentThread = base.DataContext as ThreadViewModel;
			if (_currentThread != null)
			{
				_currentThread.PropertyChanged += OnThreadPropertyChanged;
			}
			UpdateNameAndParticipants();
			UpdateTimestamp();
			UpdateFaceMash();
		}

		private void OnThreadPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
			case "Name":
				UpdateNameAndParticipants();
				break;
			case "Timestamp":
				UpdateTimestamp();
				break;
			case "PictureUrl":
				UpdateFaceMash();
				break;
			case "Participants":
				if (_currentThread != null && !_currentThread.HasPicture())
				{
					UpdateFaceMash();
				}
				UpdateNameAndParticipants();
				break;
			}
		}

		private void UpdateFaceMash()
		{
			((FrameworkElement)FaceMash).DataContext = null;
			((FrameworkElement)FaceMash).DataContext = _currentThread;
		}

		private void UpdateNameAndParticipants()
		{
			string text = string.Empty;
			string text2 = string.Empty;
			if (_currentThread != null)
			{
				text = ThreadNameUtils.GetThreadNameOrId(_currentThread, _context);
				text2 = ThreadNameUtils.GetThreadNameOrId(_currentThread, _context, true);
			}
			NameBlock.Text = text;
			ParticipantsBlock.Text = text2;
		}

		private void UpdateTimestamp()
		{
			string text = string.Empty;
			if (_currentThread != null)
			{
				long timestamp = _currentThread.Timestamp;
				DateTime now = DateTime.Now;
				DateTime date = DateExtensions.FromUnixTimeMilliseconds(timestamp).ToLocalTime();
				DateTime dateTime = now.AddDays(-1.0);
				long num = now.ToUnixTimeMilliseconds() - date.ToUnixTimeMilliseconds();
				if (num < 86400000 && now.Day == date.Day)
				{
					text = StringLibrary.GroupTile_Date_ActiveToday;
				}
				else if (num < 172800000 && date.Day == dateTime.Day)
				{
					text = StringLibrary.GroupTile_Date_ActiveYesterday;
				}
				else if (now.Year == date.Year)
				{
					if (now.Month == date.Month)
					{
						if (now.Day == date.Day)
						{
							text = StringLibrary.GroupTile_Date_ActiveToday;
						}
						else
						{
							text = ((now.Day != date.Day + 1) ? string.Format(StringLibrary.GroupTile_Date_ActiveDaysAgo, now.Day - date.Day) : StringLibrary.GroupTile_Date_ActiveYesterday);
						}
					}
					else
					{
						text = ((now.Month != date.Month + 1) ? string.Format(StringLibrary.GroupTile_Date_ActiveMonthsAgo, now.Month - date.Month) : StringLibrary.GroupTile_Date_ActiveLastMonth);
					}
				}
				else
				{
					text = ((now.Year != date.Year + 1) ? string.Format(StringLibrary.GroupTile_Date_ActiveYearsAgo, now.Year - date.Year) : StringLibrary.GroupTile_Date_ActiveLastYear);
				}
			}
			TimestampBlock.Text = text;
		}

		private void PinnedThread_OnTap(object sender, GestureEventArgs e)
		{
			if (ThreadTap != null)
			{
				ThreadTap(sender, e);
			}
		}

		private void UnpinOverlay_OnTap(object sender, GestureEventArgs e)
		{
			if (UnpinTap != null)
			{
				UnpinTap(sender, e);
			}
		}

		public void ResetTransform()
		{
			Transform renderTransform = ((UIElement)PinnedStackPanel).RenderTransform;
			ScaleTransform val = (ScaleTransform)(object)((renderTransform is ScaleTransform) ? renderTransform : null);
			val.ScaleX = 1.0;
			val.ScaleY = 1.0;
			((UIElement)PinnedStackPanel).Opacity = 1.0;
		}

		public bool IsZoomedOut()
		{
			return ((UIElement)PinnedStackPanel).Opacity != 1.0;
		}

	}
}
