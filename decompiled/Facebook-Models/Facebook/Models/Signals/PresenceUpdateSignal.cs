using System.Collections.Generic;

namespace Facebook.Models.Signals;

public class PresenceUpdateSignal
{
	public HashSet<string> OnlineIds { get; set; }

	public HashSet<string> OfflineIds { get; set; }

	public Dictionary<string, int> UidToClientType { get; set; }

	public Dictionary<string, long> UidToLastActiveTime { get; set; }

	public PresenceUpdateType Type { get; set; }
}
