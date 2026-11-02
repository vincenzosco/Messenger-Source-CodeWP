using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class DrawerContainer : UserControl
	{
		private SolidColorBrush _backgroundBrush;

		public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register("BackgroundOpacity", typeof(double), typeof(DrawerContainer), new PropertyMetadata((object)0.0, (PropertyChangedCallback)PropertyChangedCallback));




		public double BackgroundOpacity
		{
			get
			{
				return (double)base.GetValue(BackgroundOpacityProperty);
			}
			set
			{
				base.SetValue(BackgroundOpacityProperty, (object)value);
			}
		}

		public double DrawerWidth
		{
			get
			{
				return ((FrameworkElement)ContainerGrid).ActualWidth;
			}
		}

		public double DrawerHeight
		{
			get
			{
				return ((FrameworkElement)ContainerGrid).ActualHeight;
			}
		}

		private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			DrawerContainer drawerContainer = dependencyObject as DrawerContainer;
			if (drawerContainer != null)
			{
				double num = (dependencyPropertyChangedEventArgs.NewValue as double?) ?? 0.0;
				((Brush)drawerContainer._backgroundBrush).Opacity = num;
				if (num == 0.0)
				{
					((Panel)drawerContainer.RootGrid).Background = null;
				}
			}
		}

		public DrawerContainer()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected Obj, but got Unknown
			InitializeComponent();
			SolidColorBrush val = new SolidColorBrush(Colors.Black);
			((Brush)val).Opacity = 0.0;
			_backgroundBrush = val;
		}

		public void AddDrawer(FrameworkElement drawer)
		{
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)ContainerGrid).Children).Add((UIElement)(object)drawer);
			if (drawer is ViewfinderDrawer)
			{
				ViewfinderDrawer viewfinderDrawer = drawer as ViewfinderDrawer;
				viewfinderDrawer.ExpandStoryboardCreated += VfdOnExpandStoryboardCreated;
				viewfinderDrawer.CollapseStoryboardCreated += VfdOnCollapseStoryboardCreated;
			}
		}

		public Point GetRelativePoint(GestureEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return e.GetPosition((UIElement)(object)ContainerGrid);
		}

		private void VfdOnCollapseStoryboardCreated(object sender, Storyboard storyboard)
		{
			SetupOpacityStoryboard(0.6, 0.0, storyboard);
		}

		private void VfdOnExpandStoryboardCreated(object sender, Storyboard storyboard)
		{
			SetupOpacityStoryboard(0.0, 0.6, storyboard);
			((Panel)RootGrid).Background = (Brush)(object)_backgroundBrush;
		}

		private void SetupOpacityStoryboard(double from, double to, Storyboard storyboard)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected Obj, but got Unknown
			DoubleAnimation val = new DoubleAnimation();
			((Timeline)val).Duration = TimeSpan.FromSeconds(0.3);
			val.To = to;
			val.From = from;
			DoubleAnimation val2 = val;
			Storyboard.SetTarget((Timeline)(object)val2, (DependencyObject)(object)this);
			Storyboard.SetTargetProperty((Timeline)(object)val2, new PropertyPath("BackgroundOpacity", new object[0]));
			((PresentationFrameworkCollection<Timeline>)(object)storyboard.Children).Add((Timeline)(object)val2);
		}


		static DrawerContainer()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected Obj, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected Obj, but got Unknown
		}
	}
}
