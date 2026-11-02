using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class ThreadParticipantControl : ThreadListItemControl
	{
		private FacebookContext _context;







		private FacebookContext Context => _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);

		public ThreadParticipantControl()
		{
			InitializeComponent();
		}

		protected override void OnItemValueAssigned()
		{
			ThreadViewModel viewModel = ViewModel;
			viewModel.PropertyChanged += OnThreadPropertyChanged;
			SetThreadName(viewModel);
			SetThreadParticipants(viewModel);
			SetIndicator(viewModel);
		}

		protected override void Recycled(ThreadViewModel oldViewModel)
		{
			oldViewModel.PropertyChanged -= OnThreadPropertyChanged;
		}

		private void OnThreadPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName.Equals("Name"))
			{
				SetThreadName(ViewModel);
				SetThreadParticipants(ViewModel);
			}
		}

		private void SetThreadName(ThreadViewModel threadViewModel)
		{
			ThreadTitle.Text = ThreadNameUtils.GetThreadNamePreferCache(threadViewModel, Context);
		}

		private void SetIndicator(ThreadViewModel threadViewModel)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)IndicatorControl).Visibility = VisibilityUtil.FromBool(threadViewModel.IsCanonical());
			IndicatorControl.IsPartialUser = Context.IsPartialUser() || threadViewModel.IsPartialContact;
			IndicatorControl.IsMessengerUser = threadViewModel.IsCanonical() && threadViewModel.IsMessengerUser;
		}

		private void SetThreadParticipants(ThreadViewModel threadViewModel)
		{
			ThreadParticipants.Text = (threadViewModel.HasName() ? ThreadNameUtils.GetThreadParticipantsNameOrId(threadViewModel, Context) : string.Empty);
		}

	}
}
