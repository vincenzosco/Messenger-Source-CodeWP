using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls;

public class ThreadParticipantControl : ThreadListItemControl
{
	private FacebookContext _context;

	internal Grid LayoutRoot;

	internal FaceMash FaceMash;

	internal ProfileIndicatorControl IndicatorControl;

	internal DynamicTextBlock ThreadTitle;

	internal DynamicTextBlock ThreadParticipants;

	private bool _contentLoaded;

	private FacebookContext Context => _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);

	public ThreadParticipantControl()
	{
		InitializeComponent();
	}

	protected override void OnItemValueAssigned()
	{
		ThreadViewModel viewModel = ViewModel;
		viewModel.PropertyChanged += OnThreadPropertyChanged;
		SetThreadName(viewModel);
		SetThreadParticipants(viewModel);
		SetIndicator(viewModel);
	}

	protected override void Recycled(ThreadViewModel oldViewModel)
	{
		oldViewModel.PropertyChanged -= OnThreadPropertyChanged;
	}

	private void OnThreadPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName.Equals("Name"))
		{
			SetThreadName(ViewModel);
			SetThreadParticipants(ViewModel);
		}
	}

	private void SetThreadName(ThreadViewModel threadViewModel)
	{
		ThreadTitle.Text = ThreadNameUtils.GetThreadNamePreferCache(threadViewModel, Context);
	}

	private void SetIndicator(ThreadViewModel threadViewModel)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((UIElement)IndicatorControl).Visibility = VisibilityUtil.FromBool(threadViewModel.IsCanonical());
		IndicatorControl.IsPartialUser = Context.IsPartialUser() || threadViewModel.IsPartialContact;
		IndicatorControl.IsMessengerUser = threadViewModel.IsCanonical() && threadViewModel.IsMessengerUser;
	}

	private void SetThreadParticipants(ThreadViewModel threadViewModel)
	{
		ThreadParticipants.Text = (threadViewModel.HasName() ? ThreadNameUtils.GetThreadParticipantsNameOrId(threadViewModel, Context) : string.Empty);
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ThreadParticipantControl.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			FaceMash = (FaceMash)((FrameworkElement)this).FindName("FaceMash");
			IndicatorControl = (ProfileIndicatorControl)((FrameworkElement)this).FindName("IndicatorControl");
			ThreadTitle = (DynamicTextBlock)((FrameworkElement)this).FindName("ThreadTitle");
			ThreadParticipants = (DynamicTextBlock)((FrameworkElement)this).FindName("ThreadParticipants");
		}
	}
}
