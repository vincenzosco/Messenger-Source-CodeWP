using ThriftPortable.Extensions;

namespace Thrift.Transport;

public class TSocket : TStreamTransport
{
	private ISocket client;

	private int timeout;

	private bool _IsDisposed;

	public int Timeout
	{
		set
		{
			ISocket socket = client;
			int receiveTimeout = (client.SendTimeout = (timeout = value));
			socket.ReceiveTimeout = receiveTimeout;
		}
	}

	public override bool IsOpen
	{
		get
		{
			if (client == null)
			{
				return false;
			}
			return client.IsConnected;
		}
	}

	public TSocket(ISocket client)
	{
		this.client = client;
		if (IsOpen)
		{
			inputStream = client.GetStream();
			outputStream = client.GetStream();
		}
	}

	public override void Open()
	{
		if (IsOpen)
		{
			throw new TTransportException(TTransportException.ExceptionType.AlreadyOpen, "Socket already connected");
		}
		if (timeout == 0)
		{
			client.Connect();
		}
		else
		{
			client.Connect(timeout);
		}
		inputStream = client.GetStream();
		outputStream = client.GetStream();
	}

	public override void Close()
	{
		base.Close();
		if (client != null)
		{
			client.Dispose();
			client = null;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!_IsDisposed && disposing)
		{
			if (client != null)
			{
				client.Dispose();
			}
			base.Dispose(disposing);
		}
		_IsDisposed = true;
	}
}
