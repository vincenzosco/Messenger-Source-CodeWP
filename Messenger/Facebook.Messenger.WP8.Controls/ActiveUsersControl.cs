using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Models;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class ActiveUsersControl : ActiveUsersItemControl
	{






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

	}
}
