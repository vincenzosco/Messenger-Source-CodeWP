using System;
using System.IO;

namespace Thrift.Transport
{
	public class TBufferedTransport : TTransport, IDisposable
	{
		private BufferedStream inputBuffer;

		private BufferedStream outputBuffer;

		private int bufSize;

		private TStreamTransport transport;

		private bool _IsDisposed;

		public TTransport UnderlyingTransport => transport;

		public override bool IsOpen => transport.IsOpen;

		public TBufferedTransport(TStreamTransport transport)
			: this(transport, 1024)
		{
		}

		public TBufferedTransport(TStreamTransport transport, int bufSize)
		{
			this.bufSize = bufSize;
			this.transport = transport;
			InitBuffers();
		}

		private void InitBuffers()
		{
			if (transport.InputStream != null)
			{
				inputBuffer = new BufferedStream(transport.InputStream, bufSize);
			}
			if (transport.OutputStream != null)
			{
				outputBuffer = new BufferedStream(transport.OutputStream, bufSize);
			}
		}

		public override void Open()
		{
			transport.Open();
			InitBuffers();
		}

		public override void Close()
		{
			if (inputBuffer != null && inputBuffer.CanRead)
			{
				inputBuffer.Dispose();
			}
			if (outputBuffer != null && outputBuffer.CanWrite)
			{
				outputBuffer.Dispose();
			}
		}

		public override int Read(byte[] buf, int off, int len)
		{
			return inputBuffer.Read(buf, off, len);
		}

		public override void Write(byte[] buf, int off, int len)
		{
			outputBuffer.Write(buf, off, len);
		}

		public override void Flush()
		{
			outputBuffer.Flush();
		}

		protected override void Dispose(bool disposing)
		{
			if (!_IsDisposed && disposing)
			{
				if (inputBuffer != null)
				{
					inputBuffer.Dispose();
				}
				if (outputBuffer != null)
				{
					outputBuffer.Dispose();
				}
			}
			_IsDisposed = true;
		}
	}
}
