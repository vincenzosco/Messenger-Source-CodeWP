using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Models;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls
{
	public class ActiveUsersControl : ActiveUsersItemControl
	{
		internal ActiveUsersItemControl ActiveUserControl;

		internal Grid LayoutRoot;

		internal UserTile UserTile;

		internal DynamicTextBlock NameTextBlock;

		internal TextBlock ClientTypeTextBlock;

		private bool _contentLoaded;

		public ActiveUsersControl()
		{
			InitializeComponent();
		}

		protected override void OnItemValueAssigned()
		{
			ViewModel.PropertyChanged += OnPropertyChanged;
		}

		protected override void Recycled(FacebookUser oldUser)
		{
			oldUser.PropertyChanged -= OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			UserTile.AssignImages();
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ActiveUsersControl.xaml", UriKind.Relative));
				ActiveUserControl = (ActiveUsersItemControl)((FrameworkElement)this).FindName("ActiveUserControl");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				UserTile = (UserTile)((FrameworkElement)this).FindName("UserTile");
				NameTextBlock = (DynamicTextBlock)((FrameworkElement)this).FindName("NameTextBlock");
				ClientTypeTextBlock = (TextBlock)((FrameworkElement)this).FindName("ClientTypeTextBlock");
			}
		}
	}
}
