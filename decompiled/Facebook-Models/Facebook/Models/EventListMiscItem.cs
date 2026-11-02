namespace Facebook.Models;

public class EventListMiscItem : IEventType
{
	public static LoadFinishedSignal LoadFinishedSignal = new LoadFinishedSignal();

	public static LoadStartedSignal LoadStartedSignal = new LoadStartedSignal();

	public EventType EventType { get; set; }

	public string Text { get; set; }

	public LoadMoreType LoadMoreType { get; set; }
}
