using Facebook.Base.Logging;
using Windows.UI.ViewManagement;

namespace Facebook.Messenger.WP8.Utilities
{
	public class SingleInputPaneHelper
	{
		private InputPaneHidingHandler _inputPaneHidingHandler;

		private InputPaneShowingHandler _inputPaneShowingHandler;

		public void SubscribeToKeyboardEvents(InputPaneShowingHandler showingHandler, InputPaneHidingHandler hidingHandler)
		{
			InputPane inputPane = InputPane.GetForCurrentView();
			inputPane.Showing += ShowingHandler;
			inputPane.Hiding += HidingHandler;
			AddShowingHandler(showingHandler);
			AddHidingHandler(hidingHandler);
		}

		public void UnsubscribeFromKeyboardEvents()
		{
			InputPane inputPane = InputPane.GetForCurrentView();
			inputPane.Showing -= ShowingHandler;
			inputPane.Hiding -= HidingHandler;
			_inputPaneShowingHandler = null;
			_inputPaneHidingHandler = null;
		}

		private void AddShowingHandler(InputPaneShowingHandler handler)
		{
			if (_inputPaneShowingHandler != null)
			{
				Logger.Warning(this, "A handler is already registered!", "AddShowingHandler", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\SingleInputPaneHelper.cs", 38);
			}
			else
			{
				_inputPaneShowingHandler = handler;
			}
		}

		private void AddHidingHandler(InputPaneHidingHandler handler)
		{
			if (_inputPaneHidingHandler != null)
			{
				Logger.Warning(this, "A handler is already registered!", "AddHidingHandler", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\SingleInputPaneHelper.cs", 50);
			}
			else
			{
				_inputPaneHidingHandler = handler;
			}
		}

		private void ShowingHandler(InputPane sender, InputPaneVisibilityEventArgs e)
		{
			if (_inputPaneShowingHandler != null)
			{
				e.EnsuredFocusedElementInView = true;
				_inputPaneShowingHandler(sender, e);
			}
		}

		private void HidingHandler(InputPane sender, InputPaneVisibilityEventArgs e)
		{
			if (_inputPaneHidingHandler != null)
			{
				e.EnsuredFocusedElementInView = true;
				_inputPaneHidingHandler(sender, e);
			}
		}
	}
}
