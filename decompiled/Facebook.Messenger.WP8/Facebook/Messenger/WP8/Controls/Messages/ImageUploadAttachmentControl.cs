using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Services.Api.Methods.SendMessage;
using Microsoft.Phone.Controls;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;

namespace Facebook.Messenger.WP8.Controls.Messages;

public class ImageUploadAttachmentControl : BaseControl
{
	public string FilePath;

	internal ScrollViewer ScrollViewer;

	internal ListBox AttachmentsListBox;

	private bool _contentLoaded;

	public ImageUploadAttachmentControl()
	{
		InitializeComponent();
	}

	private void AttachmentsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		Navigator instance = ServiceLocator.Current.GetInstance<Navigator>();
		if (((FrameworkElement)this).DataContext is MessageBaseViewModel { AttachedFiles: not null } messageBaseViewModel && messageBaseViewModel.AttachedFiles.Count != 0)
		{
			string value = JsonConvert.SerializeObject(messageBaseViewModel.AttachedFiles.Select((Attachment x) => x.Path));
			int selectedIndex = ((Selector)AttachmentsListBox).SelectedIndex;
			if (selectedIndex >= 0)
			{
				UriQuery uriQuery = new UriQuery();
				uriQuery.Add("filePaths", value);
				uriQuery.Add("selectedIndex", selectedIndex.ToString());
				uriQuery.Add("disableLandscapeOrientation", true.ToString());
				UriQuery query = uriQuery;
				FacebookUri uri = new FacebookUri("app", UriId.Attachments, query);
				instance.Navigate(uri);
			}
			((Selector)AttachmentsListBox).SelectedIndex = -1;
		}
	}

	private void RemovePhoto_OnClick(object sender, RoutedEventArgs e)
	{
		MessageBaseViewModel messageBaseViewModel = ((FrameworkElement)this).DataContext as MessageBaseViewModel;
		MenuItem menuItem = sender as MenuItem;
		if (messageBaseViewModel == null || menuItem == null)
		{
			return;
		}
		Attachment attachment = ((FrameworkElement)menuItem).DataContext as Attachment;
		if (attachment != null)
		{
			Attachment attachment2 = messageBaseViewModel.AttachedFiles.FirstOrDefault((Attachment x) => x.Path.Equals(attachment.Path));
			if (attachment2 != null)
			{
				messageBaseViewModel.AttachedFiles.Remove(attachment2);
			}
		}
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
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Messages/ImageUploadAttachmentControl.xaml", UriKind.Relative));
			ScrollViewer = (ScrollViewer)((FrameworkElement)this).FindName("ScrollViewer");
			AttachmentsListBox = (ListBox)((FrameworkElement)this).FindName("AttachmentsListBox");
		}
	}
}
