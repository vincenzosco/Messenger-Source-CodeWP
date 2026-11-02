using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class ReportPage : ReportProblemViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private IconBarEntry<bool> _sendButton;








		public ReportPage()
		{
			InitializeComponent();
			InitializeAppBarItems();
		}

		private void InitializeAppBarItems()
		{
			_appBarHelper = new AppBarHelper(base.ApplicationBar);
			_sendButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(true, StringLibrary.Conversation_AppBar_Send, SendButton_Clicked, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
			_sendButton.IsEnabled = false;
			_appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(true, StringLibrary.AppBar_Cancel, CancelButton_Clicked, new Uri("/Assets/AppBar/Skip.png", UriKind.Relative)));
		}

		private void SendButton_Clicked(object sender, EventArgs e)
		{
			((FrameworkElement)ReportProblemTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
			WithViewModel((ReportProblemViewModel vm) =>
			{
				vm.SendReportCommand.Execute(null);
			});
		}

		private void CancelButton_Clicked(object sender, EventArgs e)
		{
			ReportProblemTextBox.Text = string.Empty;
			base.NavigationService.GoBack();
		}

		private void ReportPage_OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((ReportProblemViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
				((FrameworkElement)ReportProblemTextBox).Height = double.NaN;
				UpdateDisclaimer(vm, Disclaimer.Inlines);
				UpdateSendButtonState();
			});
		}

		private void ReportPage_OnUnloaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((ReportProblemViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
			});
			((PresentationFrameworkCollection<Inline>)(object)Disclaimer.Inlines).Clear();
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (e.PropertyName.Equals("SuccessfulSend"))
			{
				MessageBox.Show(StringLibrary.MessageBox_Description_FlyTrapSuccess, StringLibrary.MessageBox_Title_FlyTrapSuccess, (MessageBoxButton)0);
				ReportProblemTextBox.Text = string.Empty;
				base.NavigationService.GoBack();
			}
		}

		private void ReportProblemTextBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			ScrollViewer reportProblemScrollViewer = ReportProblemScrollViewer;
			object obj = Application.Current.Resources[(object)"MessengerBlackBrush"];
			((Control)reportProblemScrollViewer).BorderBrush = (Brush)((obj is SolidColorBrush) ? obj : null);
			TranslateGrid(false);
		}

		private void ReportProblemTextBox_OnGotFocus(object sender, RoutedEventArgs e)
		{
			ScrollViewer reportProblemScrollViewer = ReportProblemScrollViewer;
			object obj = Application.Current.Resources[(object)"MessengerBlueBrush"];
			((Control)reportProblemScrollViewer).BorderBrush = (Brush)((obj is SolidColorBrush) ? obj : null);
			TranslateGrid(true);
		}

		private void TranslateGrid(bool gotFocus)
		{
			SetRootFrameTransform(0);
			if (gotFocus)
			{
				((FrameworkElement)ReportProblemScrollViewer).Height = 150.0;
				((FrameworkElement)ReportProblemScrollViewer).VerticalAlignment = (VerticalAlignment)0;
			}
			else
			{
				((FrameworkElement)ReportProblemScrollViewer).Height = double.NaN;
				((FrameworkElement)ReportProblemScrollViewer).VerticalAlignment = (VerticalAlignment)3;
			}
		}

		private void ReportProblemTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			UpdateSendButtonState();
			if (ReportProblemTextBox.SelectionStart == ReportProblemTextBox.Text.Length)
			{
				((UIElement)ReportProblemScrollViewer).Measure(default(Size));
				ReportProblemScrollViewer.ScrollToVerticalOffset(((FrameworkElement)ReportProblemTextBox).ActualHeight);
			}
		}

		private void UpdateSendButtonState()
		{
			_sendButton.IsEnabled = !string.IsNullOrWhiteSpace(ReportProblemTextBox.Text);
		}

		private static void UpdateDisclaimer(ReportProblemViewModel vm, InlineCollection paragraph)
		{
			string flyTrap_Label_Disclaimer = StringLibrary.FlyTrap_Label_Disclaimer;
			Hyperlink val = GenerateDisclaimerLink(vm);
			if (flyTrap_Label_Disclaimer.StartsWith("{0}"))
			{
				((PresentationFrameworkCollection<Inline>)(object)paragraph).Add((Inline)(object)val);
				paragraph.Add(flyTrap_Label_Disclaimer.Substring(3));
				return;
			}
			if (flyTrap_Label_Disclaimer.EndsWith("{0}"))
			{
				paragraph.Add(flyTrap_Label_Disclaimer.Substring(0, flyTrap_Label_Disclaimer.Length - 3));
				((PresentationFrameworkCollection<Inline>)(object)paragraph).Add((Inline)(object)val);
				return;
			}
			string[] array = StringLibrary.FlyTrap_Label_Disclaimer.Split(new string[1] { "{0}" }, StringSplitOptions.None);
			paragraph.Add(array[0]);
			((PresentationFrameworkCollection<Inline>)(object)paragraph).Add((Inline)(object)val);
			paragraph.Add(array[1]);
		}

		private static Hyperlink GenerateDisclaimerLink(ReportProblemViewModel vm)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected Obj, but got Unknown
			Hyperlink val = new Hyperlink();
			val.Command = vm.ShowDataUsePolicyCommand;
			Hyperlink val2 = val;
			InlineCollection inlines = ((Span)val2).Inlines;
			Run val3 = new Run();
			val3.Text = StringLibrary.FlyTrap_Label_Disclaimer_DataUsePolicyLink;
			((PresentationFrameworkCollection<Inline>)(object)inlines).Add((Inline)(object)val3);
			object obj = Application.Current.Resources[(object)"MessengerBlueBrush"];
			((TextElement)val2).Foreground = (Brush)((obj is SolidColorBrush) ? obj : null);
			return val2;
		}

	}
}
