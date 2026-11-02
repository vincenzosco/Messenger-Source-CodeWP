using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

public class PinnedThread : MailThread
{
	public int Position;

	[DataMember]
	[JsonProperty("auto_pinned")]
	public bool AutoPinned { get; set; }

	[DataMember]
	[JsonProperty("update_time")]
	public long UpdateTime { get; set; }

	public PinnedThread()
	{
	}

	public PinnedThread(MailThread fromThread)
		: base(fromThread)
	{
	}

	public PinnedThread(PinnedThread fromThread)
		: base(fromThread)
	{
		AutoPinned = fromThread.AutoPinned;
		UpdateTime = fromThread.UpdateTime;
		Position = fromThread.Position;
	}
}
