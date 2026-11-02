using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages
{
	public class SyncContactsPage : SyncContactsViewModelPage
	{
		private AppBarHelper _appBarHelper;

		internal Grid RootGrid;

		internal RichTextBox DescriptionRichTextBox;

		private bool _contentLoaded;

		public SyncContactsPage()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			InitializeComponent();
			((FrameworkElement)this).Loaded += OnLoaded;
			InitializeAppBar();
		}

		private void InitializeAppBar()
		{
			_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
			_appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.AppBar_Ok, OkButtonOnClick, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
			_appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.Attachments_AppBar_Skip, SkipButtonOnClick, new Uri("/Assets/AppBar/Skip.png", UriKind.Relative)));
		}

		private void OkButtonOnClick(object sender, EventArgs e)
		{
			WithViewModel((SyncContactsViewModel vm) =>
			{
				vm.SyncContactsCommand.Execute(null);
			});
		}

		private void SkipButtonOnClick(object sender, EventArgs e)
		{
			WithViewModel((SyncContactsViewModel vm) =>
			{
				vm.SkipSyncCommand.Execute(null);
			});
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			SetupSubtitleText();
		}

		private void SetupSubtitleText()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected Obj, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected Obj, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected Obj, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected Obj, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected Obj, but got Unknown
			Run val = new Run();
			val.Text = StringLibrary.SyncContact_Subtitle;
			Run val2 = val;
			Hyperlink val3 = new Hyperlink();
			val3.Command = null;
			val3.CommandParameter = StringLibrary.SyncContact_Button_LearnMore;
			Hyperlink val4 = val3;
			InlineCollection inlines = ((Span)val4).Inlines;
			Run val5 = new Run();
			val5.Text = StringLibrary.SyncContact_Button_LearnMore;
			((PresentationFrameworkCollection<Inline>)(object)inlines).Add((Inline)(object)val5);
			((TextElement)val4).Foreground = (Brush)new SolidColorBrush(Colors.Black);
			val4.MouseOverForeground = (Brush)new SolidColorBrush(Colors.Gray);
			Paragraph val6 = new Paragraph();
			((PresentationFrameworkCollection<Inline>)(object)val6.Inlines).Add((Inline)(object)val2);
			((PresentationFrameworkCollection<Inline>)(object)val6.Inlines).Add((Inline)(object)val4);
			((PresentationFrameworkCollection<Block>)(object)DescriptionRichTextBox.Blocks).Add((Block)(object)val6);
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
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/SyncContactsPage.xaml", UriKind.Relative));
				RootGrid = (Grid)((FrameworkElement)this).FindName("RootGrid");
				DescriptionRichTextBox = (RichTextBox)((FrameworkElement)this).FindName("DescriptionRichTextBox");
			}
		}
	}
}
