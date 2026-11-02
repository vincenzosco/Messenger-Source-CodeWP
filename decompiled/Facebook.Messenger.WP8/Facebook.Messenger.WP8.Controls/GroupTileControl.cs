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
	public class GroupTileControl : BaseControl
	{
		private FacebookContext _context;

		private ThreadViewModel _currentThread;

		internal CubicEase ZoomEase;

		internal ElasticEase ElasticEase;

		internal Storyboard ZoomOut;

		internal Storyboard ZoomIn;

		internal Storyboard BounceOut;

		internal Storyboard BounceIn;

		internal VisualStateGroup CommonStates;

		internal VisualState Active;

		internal VisualState Inactive;

		internal StackPanel PinnedStackPanel;

		internal FaceMash FaceMash;

		internal DynamicTextBlock NameBlock;

		internal TextBlock TimestampBlock;

		internal DynamicTextBlock ParticipantsBlock;

		internal ImageButton UnpinOverlay;

		private bool _contentLoaded;

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
			_currentThread = ((FrameworkElement)this).DataContext as ThreadViewModel;
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

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected Obj, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected Obj, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/GroupTileControl.xaml", UriKind.Relative));
				ZoomEase = (CubicEase)((FrameworkElement)this).FindName("ZoomEase");
				ElasticEase = (ElasticEase)((FrameworkElement)this).FindName("ElasticEase");
				ZoomOut = (Storyboard)((FrameworkElement)this).FindName("ZoomOut");
				ZoomIn = (Storyboard)((FrameworkElement)this).FindName("ZoomIn");
				BounceOut = (Storyboard)((FrameworkElement)this).FindName("BounceOut");
				BounceIn = (Storyboard)((FrameworkElement)this).FindName("BounceIn");
				CommonStates = (VisualStateGroup)((FrameworkElement)this).FindName("CommonStates");
				Active = (VisualState)((FrameworkElement)this).FindName("Active");
				Inactive = (VisualState)((FrameworkElement)this).FindName("Inactive");
				PinnedStackPanel = (StackPanel)((FrameworkElement)this).FindName("PinnedStackPanel");
				FaceMash = (FaceMash)((FrameworkElement)this).FindName("FaceMash");
				NameBlock = (DynamicTextBlock)((FrameworkElement)this).FindName("NameBlock");
				TimestampBlock = (TextBlock)((FrameworkElement)this).FindName("TimestampBlock");
				ParticipantsBlock = (DynamicTextBlock)((FrameworkElement)this).FindName("ParticipantsBlock");
				UnpinOverlay = (ImageButton)((FrameworkElement)this).FindName("UnpinOverlay");
			}
		}
	}
}
