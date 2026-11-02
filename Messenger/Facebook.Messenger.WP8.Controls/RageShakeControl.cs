using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Controls.Base;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class RageShakeControl : UserControl
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

	}
}
