using System;
using System.Reflection;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger
{
	public class ToastHelper
	{
		private static readonly Version ToastSoundsMinVersion = new Version(8, 0, 10492);

		public static void ShowPendingSendsToast()
		{
			ShowToast(StringLibrary.BackgroundToast_LeavingApp_Title, StringLibrary.BackgroundToast_LeavingApp_Warning, EntryPointHelper.GetLaunchUri(EntryPoint.FailedMessageToast));
		}

		public static void ShowToast(string title, string content, Uri navUri, Uri soundUri = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			ShellToast val = new ShellToast();
			val.Title = title;
			val.Content = content;
			val.NavigationUri = navUri;
			ShellToast val2 = val;
			if (soundUri != null && CanAddCustomSound())
			{
				MethodInfo setMethod = ((object)val2).GetType().GetProperty("Sound").GetSetMethod();
				setMethod.Invoke(val2, new object[1] { soundUri });
			}
			val2.Show();
		}

		private static bool CanAddCustomSound()
		{
			return Environment.OSVersion.Version >= ToastSoundsMinVersion;
		}
	}
}
