namespace Facebook.Base.Mqtt.Messages;

public class FixedHeader
{
	public const int SizeInBytes = 2;

	public MessageType MessageType { get; set; }

	public bool DupFlag { get; set; }

	public int QosLevel { get; set; }

	public bool Retain { get; set; }

	public int RemainingLength { get; set; }

	public int MessageTypeValue => (int)MessageType;
}
