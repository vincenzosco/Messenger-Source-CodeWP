using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class ThreadControl : ThreadListItemControl
	{
		private static readonly BitmapImage mutedIcon = new BitmapImage(new Uri("/Assets/Threads/inboxMutedIcon.png", UriKind.Relative));

		private static readonly BitmapImage failedIcon = new BitmapImage(new Uri("/Assets/Threads/inboxFailedIcon.png", UriKind.Relative));

		private FacebookContext _context;










		private FacebookContext Context
		{
			get
			{
				return _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);
			}
		}

		public ThreadControl()
		{
			InitializeComponent();
		}

		protected override void OnItemValueAssigned()
		{
			ThreadViewModel viewModel = ViewModel;
			viewModel.PropertyChanged += OnThreadPropertyChanged;
			((UIElement)IndicatorControl).Visibility = (!viewModel.IsCanonical() ? Visibility.Collapsed : Visibility.Visible);
			SetIndicatorControl(viewModel);
			SetThreadStatus(viewModel);
			SetUnread(viewModel.Unread);
			SetThreadName(viewModel);
			SetTimestampDisplay(viewModel);
			SetDisplaySnippet(viewModel);
			UpdateSeenHeads(viewModel);
		}

		protected override void Recycled(ThreadViewModel oldViewModel)
		{
			oldViewModel.PropertyChanged -= OnThreadPropertyChanged;
			SnippetTextBox.Blocks.ClearWithBugFix();
		}

		private void OnThreadPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			ThreadViewModel viewModel = ViewModel;
			SetIndicatorControl(viewModel);
			SetUnread(viewModel.Unread);
			SetThreadStatus(viewModel);
			switch (args.PropertyName)
			{
			case "Name":
				SetThreadName(viewModel);
				break;
			case "Timestamp":
				SetTimestampDisplay(viewModel);
				break;
			case "Snippet":
				SetDisplaySnippet(viewModel);
				break;
			case "PictureUrl":
				FaceMash.SetFaceMash();
				break;
			case "Participants":
				if (!viewModel.HasPicture())
				{
					FaceMash.SetFaceMash();
				}
				if (!viewModel.HasName())
				{
					SetThreadName(viewModel);
				}
				break;
			}
		}

		private void SetTimestampDisplay(ThreadViewModel thread)
		{
			Timestamp.Text = ThreadDateConverter.FormatDateAsString(thread.Timestamp);
		}

		private void SetDisplaySnippet(ThreadViewModel thread)
		{
			SnippetTextBox.Blocks.ClearWithBugFix();
			Paragraph displaySnippet = thread.Snippet.DisplaySnippet;
			((PresentationFrameworkCollection<Block>)(object)SnippetTextBox.Blocks).Add((Block)(object)displaySnippet);
		}

		private void SetThreadName(ThreadViewModel threadViewModel)
		{
			ThreadTitle.Text = ThreadNameUtils.GetThreadNamePreferCache(threadViewModel, Context);
		}

		private void SetIndicatorControl(ThreadViewModel vm)
		{
			IndicatorControl.IsPartialUser = Context.IsPartialUser() || vm.IsPartialContact;
			IndicatorControl.IsMessengerUser = vm.IsMessengerUser;
		}

		private void SetThreadStatus(ThreadViewModel threadViewModel)
		{
			BitmapSource val = CalculateThreadStatusIcon(threadViewModel);
			if (val != null)
			{
				ThreadStatus.Source = (ImageSource)(object)val;
				((UIElement)ThreadStatus).Visibility = Visibility.Visible;
			}
			else
			{
				((UIElement)ThreadStatus).Visibility = Visibility.Collapsed;
			}
		}

		private BitmapSource CalculateThreadStatusIcon(ThreadViewModel threadViewModel)
		{
			if (threadViewModel.HasFailedUnsentMessages)
			{
				return (BitmapSource)(object)failedIcon;
			}
			if (threadViewModel.IsMuted)
			{
				return (BitmapSource)(object)mutedIcon;
			}
			if (threadViewModel.Snippet.SnippetFromMe && threadViewModel.LastMessageState != MailMessage.MessageState.Seen)
			{
				return (BitmapSource)(object)MessageDetailsUtils.GrayMessageStateImage(threadViewModel.LastMessageState);
			}
			return null;
		}

		private void UpdateSeenHeads(ThreadViewModel vm)
		{
			if (vm.HasFailedUnsentMessages || vm.IsMuted || !vm.Snippet.SnippetFromMe || vm.LastMessageState != MailMessage.MessageState.Seen)
			{
				((UIElement)SeenHeadsControl).Visibility = Visibility.Collapsed;
				return;
			}
			SeenHeadsControl.UpdateSeenHeads(vm.LastMessageReadUserIds);
			((UIElement)SeenHeadsControl).Visibility = Visibility.Visible;
		}

		private void SetUnread(bool isUnread)
		{
			string text = (isUnread ? "MessengerBlueBrush" : "MessengerTextLightGreyBrush");
			RichTextBox snippetTextBox = SnippetTextBox;
			object obj = Application.Current.Resources[(object)text];
			((Control)snippetTextBox).Foreground = (Brush)((obj is SolidColorBrush) ? obj : null);
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			Size result = base.MeasureOverride(availableSize);
			if (availableSize.Height > 0.0 && availableSize.Width > 0.0 && ViewModel != null && ThreadTitle.EllipsizedText != ViewModel.CachedEllipsizedThreadName)
			{
				ViewModel.CachedEllipsizedThreadName = ThreadTitle.EllipsizedText;
			}
			return result;
		}


		static ThreadControl()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected Obj, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected Obj, but got Unknown
		}
	}
}
