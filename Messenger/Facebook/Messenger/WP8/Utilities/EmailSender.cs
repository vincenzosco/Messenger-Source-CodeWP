using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Microsoft.Phone.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Storage.Streams;

namespace Facebook.Messenger.WP8.Utilities
{
	public class EmailSender
	{
		public static async Task<bool> AttemptEmail(string to, string subject, string body, string extra)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				await EmailWindowsPhone81(to, subject, body, extra);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error("Unable to send email: " + ex, "AttemptEmail", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\EmailSender.cs", 29);
				return false;
			}
		}

		private static Task EmailWindowsPhone80(string to, string subject, string body)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected Obj, but got Unknown
			if (body.Length > 30000)
			{
				body = body.Substring(0, 30000);
			}
			EmailComposeTask val = new EmailComposeTask();
			val.To = to;
			val.Subject = subject;
			val.Body = body;
			EmailComposeTask val2 = val;
			val2.Show();
			return Task.FromResult<object>(null);
		}

		private static async Task EmailWindowsPhone81(string to, string subject, string body, string attachmentString)
		{
			EmailMessage val = new EmailMessage();
			val.Subject = subject;
			val.Body = body;
			EmailMessage email = val;
			EmailRecipient recipient = new EmailRecipient(to);
			email.To.Add(recipient);
			byte[] bytes = Encoding.UTF8.GetBytes(attachmentString);
			InMemoryRandomAccessStream randomStream = new InMemoryRandomAccessStream();
			using (Stream stream = ((IRandomAccessStream)(object)randomStream).AsStream())
			{
				await stream.WriteAsync(bytes, 0, bytes.Length);
				RandomAccessStreamReference streamReference = RandomAccessStreamReference.CreateFromStream((IRandomAccessStream)(object)randomStream);
				EmailAttachment attachment = new EmailAttachment("attachment.txt", (IRandomAccessStreamReference)(object)streamReference);
				email.Attachments.Add(attachment);
				await EmailManager.ShowComposeNewEmailAsync(email);
			}
		}
	}
}
