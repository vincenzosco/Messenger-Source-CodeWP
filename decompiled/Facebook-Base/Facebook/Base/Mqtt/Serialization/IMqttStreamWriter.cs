namespace Facebook.Base.Mqtt.Serialization;

public interface IMqttStreamWriter
{
	void WriteChar(char value);

	void WriteInt16(int value);

	void WriteByte(int value);

	void WriteByte(byte value);

	void WriteBytes(byte[] value);
}
