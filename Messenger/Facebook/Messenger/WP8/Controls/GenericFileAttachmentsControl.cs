using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Base;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Tasks;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class GenericFileAttachmentsControl : BaseControl, IResetableView
	{
		public class AttachmentInformation
		{
			public Uri Uri { get; set; }

			public FileUtil.FileType Type { get; set; }

			public string Filename { get; set; }

			public Uri DownloadIcon { get; set; }
		}

		private const int MaxFilesAttachmentsForDisplay = 3;

		public static readonly DependencyProperty ShowAllProperty = DependencyProperty.Register("ShowAll", typeof(bool), typeof(GenericFileAttachmentsControl), new PropertyMetadata((object)false));

		private static readonly Uri DownloadIconBlackUri = new Uri("/Assets/Messages/DownloadAttachmentBlack.png", UriKind.Relative);

		private static readonly Uri DownloadIconWhiteUri = new Uri("/Assets/Messages/DownloadAttachmentWhite.png", UriKind.Relative);

		private readonly ObservableCollection<AttachmentInformation> _attachmentCollection = new ObservableCollection<AttachmentInformation>();

		private FacebookContext _context;



		public bool ShowAll
		{
			get
			{
				return (bool)base.GetValue(ShowAllProperty);
			}
			set
			{
				base.SetValue(ShowAllProperty, (object)value);
			}
		}

		private FacebookContext Context => _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);

		public GenericFileAttachmentsControl()
		{
			InitializeComponent();
			AttachmentItemsControl.ItemsSource = _attachmentCollection;
		}

		public void UpdateView(MessageViewModel m)
		{
			ResetView(false);
			if (m != null && m.HasGenericFileAttachments())
			{
				DisplayFileAttachments(m);
			}
		}

		public void ResetView(bool recycled)
		{
			_attachmentCollection.Clear();
		}

		private void DisplayFileAttachments(MessageViewModel m)
		{
			FacebookProfile profile = Context.Session.Profile;
			Uri downloadIcon = ((m.Sender != null && profile.Id == m.Sender.Id && !ShowAll) ? DownloadIconWhiteUri : DownloadIconBlackUri);
			IEnumerable<MailAttachment> enumerable = (ShowAll ? m.GenericFileAttachments : m.GenericFileAttachments.Take(3));
			foreach (MailAttachment item2 in enumerable)
			{
				Uri uri = new Uri(AttachmentUriGenerator.UrlForAttachment(item2.Id, m.Id));
				AttachmentInformation attachmentInformation = new AttachmentInformation();
				attachmentInformation.Uri = uri;
				attachmentInformation.Filename = item2.FileName;
				attachmentInformation.Type = item2.Type;
				attachmentInformation.DownloadIcon = downloadIcon;
				AttachmentInformation item = attachmentInformation;
				_attachmentCollection.Add(item);
			}
		}

		private void Attachment_OnTap(object sender, GestureEventArgs e)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected Obj, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected Obj, but got Unknown
			FrameworkElement val = (FrameworkElement)((sender is FrameworkElement) ? sender : null);
			if (val == null)
			{
				return;
			}
			AttachmentInformation attachmentInformation = val.DataContext as AttachmentInformation;
			if (attachmentInformation != null)
			{
				FileUtil.FileType type = attachmentInformation.Type;
				if (type == FileUtil.FileType.Video)
				{
					MediaPlayerLauncher val2 = new MediaPlayerLauncher();
					val2.Media = attachmentInformation.Uri;
					val2.Controls = (MediaPlaybackControls)31;
					val2.Location = (MediaLocationType)2;
					MediaPlayerLauncher val3 = val2;
					val3.Show();
				}
				else
				{
					WebBrowserTask val4 = new WebBrowserTask();
					val4.Uri = attachmentInformation.Uri;
					WebBrowserTask val5 = val4;
					val5.Show();
				}
				e.Handled = true;
			}
		}


		static GenericFileAttachmentsControl()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected Obj, but got Unknown
		}
	}
}
