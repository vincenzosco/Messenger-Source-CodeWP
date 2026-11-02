using System.Threading;
using System.Threading.Tasks;

namespace Facebook.Base.Mqtt.Serialization
{
	public interface IMqttStreamReader
	{
		Task<byte> ReadByte(CancellationToken token);

		Task ReadBytes(byte[] buff);
	}
}
