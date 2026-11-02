using System;

namespace Facebook.Base
{
	public static class EntryPointHelper
	{
		private const string LaunchUrlFormatWithEntryPoint = "/Views/Home.xaml?entry_point={0}";

		public static Uri GetLaunchUri(EntryPoint entryPoint)
		{
			return new Uri(string.Format("/Views/Home.xaml?entry_point={0}", new object[1] { entryPoint }), UriKind.Relative);
		}
	}
}
