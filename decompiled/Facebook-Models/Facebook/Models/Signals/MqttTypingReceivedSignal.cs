namespace Facebook.Models.Signals;

public class MqttTypingReceivedSignal
{
	public string SenderId { get; set; }

	public bool State { get; set; }
}
