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
	public class ThreadControl : ThreadListItemControl
	{
		private static readonly BitmapImage mutedIcon = new BitmapImage(new Uri("/Assets/Threads/inboxMutedIcon.png", UriKind.Relative));

		private static readonly BitmapImage failedIcon = new BitmapImage(new Uri("/Assets/Threads/inboxFailedIcon.png", UriKind.Relative));

		private FacebookContext _context;

		internal Grid LayoutRoot;

		internal FaceMash FaceMash;

		internal ProfileIndicatorControl IndicatorControl;

		internal DynamicTextBlock ThreadTitle;

		internal TextBlock Timestamp;

		internal RichTextBox SnippetTextBox;

		internal Image ThreadStatus;

		internal SeenHeadsControl SeenHeadsControl;

		private bool _contentLoaded;

		private FacebookContext Context => _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);

		public ThreadControl()
		{
			InitializeComponent();
		}

		protected override void OnItemValueAssigned()
		{
			ThreadViewModel viewModel = ViewModel;
			viewModel.PropertyChanged += OnThreadPropertyChanged;
			((UIElement)IndicatorControl).Visibility = (Visibility)(!viewModel.IsCanonical());
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
				((UIElement)ThreadStatus).Visibility = (Visibility)0;
			}
			else
			{
				((UIElement)ThreadStatus).Visibility = (Visibility)1;
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
				((UIElement)SeenHeadsControl).Visibility = (Visibility)1;
				return;
			}
			SeenHeadsControl.UpdateSeenHeads(vm.LastMessageReadUserIds);
			((UIElement)SeenHeadsControl).Visibility = (Visibility)0;
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
			Size result = ((FrameworkElement)this).MeasureOverride(availableSize);
			if (availableSize.Height > 0.0 && availableSize.Width > 0.0 && ViewModel != null && ThreadTitle.EllipsizedText != ViewModel.CachedEllipsizedThreadName)
			{
				ViewModel.CachedEllipsizedThreadName = ThreadTitle.EllipsizedText;
			}
			return result;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ThreadControl.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				FaceMash = (FaceMash)((FrameworkElement)this).FindName("FaceMash");
				IndicatorControl = (ProfileIndicatorControl)((FrameworkElement)this).FindName("IndicatorControl");
				ThreadTitle = (DynamicTextBlock)((FrameworkElement)this).FindName("ThreadTitle");
				Timestamp = (TextBlock)((FrameworkElement)this).FindName("Timestamp");
				SnippetTextBox = (RichTextBox)((FrameworkElement)this).FindName("SnippetTextBox");
				ThreadStatus = (Image)((FrameworkElement)this).FindName("ThreadStatus");
				SeenHeadsControl = (SeenHeadsControl)((FrameworkElement)this).FindName("SeenHeadsControl");
			}
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
