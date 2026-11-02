using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.ViewModels.Internal;

namespace Facebook.Messenger.WP8.Controls.Internal
{
	public partial class DiskDetails : UserControl
	{

		public DiskDetails()
		{
			InitializeComponent();
		}

		private void FilesystemItem_OnTap(object sender, GestureEventArgs e)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Invalid comparison between Unknown and I4
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			FrameworkElement val = (FrameworkElement)((sender is FrameworkElement) ? sender : null);
			if (val == null)
			{
				return;
			}
			FileViewModel fileViewModel = val.DataContext as FileViewModel;
			DiskDetailsViewModel diskDetailsViewModel = base.DataContext as DiskDetailsViewModel;
			if (diskDetailsViewModel == null || fileViewModel == null)
			{
				return;
			}
			MessageBoxResult val2 = MessageBox.Show(InternalStringsLibrary.Internal_Storage_Settings_File_Options_Report_Problem, InternalStringsLibrary.Internal_Storage_Settings_File_Options, (MessageBoxButton)1);
			if ((int)val2 != 1)
			{
				return;
			}
			try
			{
				diskDetailsViewModel.ReportProblemWithFile(fileViewModel);
				MessageBox.Show(StringLibrary.MessageBox_Description_FlyTrapSuccess);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

	}
}
