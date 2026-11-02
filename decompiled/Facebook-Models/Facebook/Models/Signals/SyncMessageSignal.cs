using Facebook.Models.SyncProtocol;

namespace Facebook.Models.Signals;

public class SyncMessageSignal
{
	public ClientPayload Payload { get; private set; }

	public SyncMessageSignal(ClientPayload payload)
	{
		Payload = payload;
	}
}
