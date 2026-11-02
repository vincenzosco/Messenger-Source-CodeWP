using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Controls.Base;

namespace Facebook.Messenger.WP8.Controls;

public class RageShakeControl : UserControl
{
	public class RageShakeAction
	{
		public readonly Action ChosenAction;

		public readonly string InputText;

		public RageShakeAction(Action action, string input)
		{
			ChosenAction = action;
			InputText = input;
		}
	}

	public enum Action
	{
		Cancel,
		Message,
		Email
	}

	internal Grid LayoutRoot;

	internal HintTextBox Details;

	private bool _contentLoaded;

	public event EventHandler<RageShakeAction> ActionChosen;

	public RageShakeControl()
	{
		InitializeComponent();
	}

	private void CancelButton_OnClick(object sender, RoutedEventArgs e)
	{
		TriggerAction(Action.Cancel);
	}

	private void MessageButton_OnClick(object sender, RoutedEventArgs e)
	{
		TriggerAction(Action.Message);
	}

	private void EmailButton_OnClick(object sender, RoutedEventArgs e)
	{
		TriggerAction(Action.Email);
	}

	private void TriggerAction(Action action)
	{
		ActionChosen(this, new RageShakeAction(action, Details.Text));
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/RageShakeControl.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			Details = (HintTextBox)((FrameworkElement)this).FindName("Details");
		}
	}
}
