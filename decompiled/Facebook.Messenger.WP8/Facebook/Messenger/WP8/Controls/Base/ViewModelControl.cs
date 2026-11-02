using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Facebook.Base.Logging;

namespace Facebook.Messenger.WP8.Controls.Base
{
	public abstract class ViewModelControl<TViewModel> : UserControl where TViewModel : class
	{
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(TViewModel), typeof(ViewModelControl<TViewModel>), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public TViewModel ViewModel
		{
			get
			{
				return (TViewModel)((DependencyObject)this).GetValue(ViewModelProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ViewModelProperty, (object)value);
			}
		}

		private static void PropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			ViewModelControl<TViewModel> viewModelControl = (ViewModelControl<TViewModel>)(object)obj;
			viewModelControl.ViewModelUpdated((TViewModel)args.OldValue);
		}

		protected ViewModelControl()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			((FrameworkElement)this).SetBinding(ViewModelProperty, new Binding());
		}

		protected virtual void ViewModelUpdated(TViewModel oldViewModel)
		{
		}

		protected void WithViewModel(Action<TViewModel> action)
		{
			TViewModel viewModel = ViewModel;
			if (viewModel == null)
			{
				Logger.Warning(this, "ViewModel is null - possibly recycled between UI operations", "WithViewModel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\Base\\BaseControl.cs", 48);
			}
			else
			{
				action(viewModel);
			}
		}

		static ViewModelControl()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
		}
	}
}
