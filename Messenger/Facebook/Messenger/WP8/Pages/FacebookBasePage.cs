using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.Utilities.SystemTrayManager;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Practices.ServiceLocation;
using Windows.Foundation;
using Rect = System.Windows.Rect;

namespace Facebook.Messenger.WP8.Pages
{
	public abstract class FacebookBasePage<TViewModel> : PhoneApplicationPage where TViewModel : FacebookMessengerViewModel
	{
		protected const int AppbarHeight = 72;

		private const string StateKey = "ViewModelState";

		private readonly InputKeyboardOffsetHelper _inputKeyboardOffsetHelper;

		private readonly ViewModelBuilder _viewModelBuilder;

		private readonly Navigator _navigator;

		private Uri _currentUri;

		private bool _navigatedTo;

		private Transform _originalPageTransform;

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(TViewModel), typeof(FacebookBasePage<TViewModel>), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		protected bool IsNewInstance { get; set; }

		protected ViewModelBuilder ViewModelBuilder => _viewModelBuilder;

		protected Navigator Navigator => _navigator;

		protected SystemTrayManager SystemTrayManager => _navigator.SystemTrayManager;

		public TViewModel ViewModel
		{
			get
			{
				return (TViewModel)base.GetValue(ViewModelProperty);
			}
			set
			{
				base.SetValue(ViewModelProperty, (object)value);
			}
		}

		private static void PropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			object oldValue = args.OldValue;
			object newValue = args.NewValue;
			if (oldValue != null && newValue != null)
			{
				throw new Exception($"Changing ViewModel from {oldValue.GetType()} to {newValue.GetType()}");
			}
		}

		protected FacebookBasePage()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected Obj, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected Obj, but got Unknown
			base.SetBinding(ViewModelProperty, new Binding());
			_inputKeyboardOffsetHelper = new InputKeyboardOffsetHelper(OnKeyboardVisibilityChanged);
			_viewModelBuilder = ServiceLocator.Current.GetInstance<ViewModelBuilder>();
			_navigator = ServiceLocator.Current.GetInstance<Navigator>();
			IsNewInstance = true;
			SetDefaultSystemTray();
			SetupAnimations();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
		}

		protected void WithViewModel(Action<TViewModel> action)
		{
			TViewModel viewModel = ViewModel;
			if (viewModel == null)
			{
				Logger.Warning(this, "ViewModel is null - possibly recycled between UI operations", "WithViewModel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\FacebookBasePage.cs", 103);
			}
			else
			{
				action(viewModel);
			}
		}

		protected async Task WithViewModel(Func<TViewModel, Task> action)
		{
			TViewModel vm = ViewModel;
			if (vm == null)
			{
				Logger.Warning(this, "ViewModel is null - possibly recycled between UI operations", "WithViewModel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\FacebookBasePage.cs", 115);
			}
			else
			{
				await action(vm);
			}
		}

		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_inputKeyboardOffsetHelper.ClearListeners();
			((FrameworkElement)(object)this).CallWithViewModel((FacebookMessengerViewModel vm) =>
			{
				vm.ErrorOccurred -= OnErrorOccurred;
			});
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			base.NavigationService.PauseOnBack = true;
			_inputKeyboardOffsetHelper.SetListeners();
			((FrameworkElement)(object)this).CallWithViewModel((FacebookMessengerViewModel vm) =>
			{
				vm.ErrorOccurred += OnErrorOccurred;
			});
		}

		protected void RegisterInputElements(IReadOnlyList<UIElement> inputElements)
		{
			_inputKeyboardOffsetHelper.RegisterInputElements(inputElements);
		}

		protected virtual void OnKeyboardVisibilityChanged(bool visible, Rect occludedRect)
		{
		}

		protected virtual void OnErrorOccurred(object sender, Error error)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (error != null && !string.IsNullOrEmpty(error.Message))
			{
				if (!string.IsNullOrEmpty(error.Title))
				{
					MessageBox.Show(error.Message, error.Title, (MessageBoxButton)0);
				}
				else
				{
					MessageBox.Show(error.Message);
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected Obj, but got Unknown
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Invalid comparison between Unknown and I4
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Expected Obj, but got Unknown
			if (e == null)
			{
				Logger.Warning(this, "e is null", "OnNavigatedTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\FacebookBasePage.cs", 172);
			}
			_navigatedTo = true;
			_currentUri = e.Uri;
			base.OnNavigatedTo(e);
			SaveRootTransform();
			TViewModel vm = base.DataContext as TViewModel;
			if (vm != null)
			{
				base.SetBinding(ViewModelProperty, new Binding());
			}
			vm = ViewModel;
			if (DesignerProperties.IsInDesignTool)
			{
				IsNewInstance = false;
			}
			if (IsNewInstance || vm == null)
			{
				if (base.State == null)
				{
					Logger.Warning(this, "State is null", "OnNavigatedTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\FacebookBasePage.cs", 197);
				}
				if (base.State.ContainsKey("ViewModelState"))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					foreach (KeyValuePair<string, object> item in base.State)
					{
						dictionary[item.Key] = item.Value;
					}
					vm = ViewModelBuilder.Build<TViewModel>(e.Uri, dictionary);
				}
				else
				{
					vm = ViewModelBuilder.Build<TViewModel>(e.Uri);
				}
				Navigator.AddEntry((Page)(object)this, vm);
				base.DataContext = vm;
				base.SetBinding(ViewModelProperty, new Binding());
			}
			if (vm != null)
			{
				vm.Foregrounded = true;
				if ((int)e.NavigationMode != 4)
				{
					if (_currentUri == null)
					{
						Logger.Warning(this, "e.Uri is null", "OnNavigatedTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\FacebookBasePage.cs", 227);
					}
					vm.LogPageUrl = UriUtils.StripQueryString(_currentUri.ToString());
					base.Dispatcher.BeginInvoke((Action)(() =>
					{
						vm.LogNavigatedTo();
					}));
				}
			}
			SystemTrayManager.NavigateTo((Page)(object)this);
			IsNewInstance = false;
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			bool navigatedTo = _navigatedTo;
			_navigatedTo = false;
			ResetRootTransform();
			if ((int)e.NavigationMode == 1 && ViewModel != null)
			{
				Navigator.RemoveEntry((Page)(object)this);
				base.ClearValue(ViewModelProperty);
			}
			else if (ViewModel != null && navigatedTo)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				TViewModel viewModel = ViewModel;
				viewModel.SaveState(dictionary);
				TViewModel viewModel2 = ViewModel;
				viewModel2.Foregrounded = false;
				foreach (KeyValuePair<string, object> item in dictionary)
				{
					base.State[item.Key] = item.Value;
				}
				base.State["ViewModelState"] = string.Empty;
			}
			base.OnNavigatedFrom(e);
		}

		protected void SaveRootTransform()
		{
			UIElement rootVisual = Application.Current.RootVisual;
			PhoneApplicationFrame val = (PhoneApplicationFrame)(object)((rootVisual is PhoneApplicationFrame) ? rootVisual : null);
			if (val != null)
			{
				_originalPageTransform = ((UIElement)val).RenderTransform;
			}
		}

		protected void ResetRootTransform()
		{
			UIElement rootVisual = Application.Current.RootVisual;
			PhoneApplicationFrame val = (PhoneApplicationFrame)(object)((rootVisual is PhoneApplicationFrame) ? rootVisual : null);
			if (val != null && _originalPageTransform != null)
			{
				((UIElement)val).RenderTransform = _originalPageTransform;
			}
		}

		protected void InitializeAudioPlayback(Panel container)
		{
			base.Dispatcher.BeginInvoke((Action)(() =>
			{
				AudioAttachmentPlaybackViewModel instance = ServiceLocator.Current.GetInstance<AudioAttachmentPlaybackViewModel>();
				instance.Reset();
				instance.RemoveMediaFromVisualTree();
				((PresentationFrameworkCollection<UIElement>)(object)container.Children).Add((UIElement)(object)instance.Media);
			}));
		}

		protected virtual void SetRootFrameTransform(int translateY)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected Obj, but got Unknown
			UIElement rootVisual = Application.Current.RootVisual;
			PhoneApplicationFrame val = (PhoneApplicationFrame)(object)((rootVisual is PhoneApplicationFrame) ? rootVisual : null);
			if (val != null)
			{
				CompositeTransform val2 = new CompositeTransform();
				val2.TranslateY = translateY;
				((UIElement)val).RenderTransform = (Transform)(object)val2;
			}
		}

		private void SetDefaultSystemTray()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			SystemTray.SetForegroundColor((DependencyObject)(object)this, Colors.Black);
			SystemTray.SetIsVisible((DependencyObject)(object)this, true);
			SystemTray.SetOpacity((DependencyObject)(object)this, 0.01);
		}

		private void SetupAnimations()
		{
			NavigationInTransition navigationInTransition = new NavigationInTransition();
			navigationInTransition.Backward = new TurnstileTransition
			{
				Mode = TurnstileTransitionMode.BackwardIn
			};
			navigationInTransition.Forward = new TurnstileTransition
			{
				Mode = TurnstileTransitionMode.ForwardIn
			};
			NavigationInTransition value = navigationInTransition;
			NavigationOutTransition navigationOutTransition = new NavigationOutTransition();
			navigationOutTransition.Backward = new TurnstileTransition
			{
				Mode = TurnstileTransitionMode.BackwardOut
			};
			navigationOutTransition.Forward = new TurnstileTransition
			{
				Mode = TurnstileTransitionMode.ForwardOut
			};
			NavigationOutTransition value2 = navigationOutTransition;
			TransitionService.SetNavigationInTransition((UIElement)(object)this, value);
			TransitionService.SetNavigationOutTransition((UIElement)(object)this, value2);
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			if (_currentUri != null && _currentUri.ToString().Contains(UriParams.FromExternal))
			{
				e.Cancel = true;
				Navigator.Reset(FacebookUri.RootUri);
			}
			else
			{
				base.OnBackKeyPress(e);
			}
		}

		static FacebookBasePage()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
		}
	}
}
