using System.Collections.Generic;

namespace Facebook.Models.Signals
{
	public class OnlineUserSignal
	{
		public HashSet<string> OnlineIds { get; set; }

		public IReadOnlyList<FacebookUser> OnlineUsers { get; set; }

		public Dictionary<string, int> UidToClientType { get; set; }
	}
}
