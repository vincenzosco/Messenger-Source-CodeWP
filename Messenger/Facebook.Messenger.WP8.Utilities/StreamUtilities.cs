using System;
using System.IO;
using Facebook.Base.Logging;

namespace Facebook.Messenger.WP8.Utilities
{
	public class StreamUtilities
	{
		public static void DisposeStream(Stream s)
		{
			if (s == null)
			{
				return;
			}
			try
			{
				s.Dispose();
			}
			catch (Exception ex)
			{
				Logger.Error("Error disposing the stream", "DisposeStream", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\StreamUtilities.cs", 20);
				Logger.Error(ex.ToString(), "DisposeStream", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\StreamUtilities.cs", 21);
			}
		}
	}
}
