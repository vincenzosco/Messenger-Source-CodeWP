using System.IO;

namespace Thrift.Transport
{
	public class TStreamTransport : TTransport
	{
		protected Stream inputStream;

		protected Stream outputStream;

		private bool _IsDisposed;

		public Stream OutputStream => outputStream;

		public Stream InputStream => inputStream;

		public override bool IsOpen => true;

		public TStreamTransport()
		{
		}

		public TStreamTransport(Stream inputStream, Stream outputStream)
		{
			this.inputStream = inputStream;
			this.outputStream = outputStream;
		}

		public override void Open()
		{
		}

		public override void Close()
		{
			if (inputStream != null)
			{
				inputStream.Dispose();
				inputStream = null;
			}
			if (outputStream != null)
			{
				outputStream.Dispose();
				outputStream = null;
			}
		}

		public override int Read(byte[] buf, int off, int len)
		{
			if (inputStream == null)
			{
				throw new TTransportException(TTransportException.ExceptionType.NotOpen, "Cannot read from null inputstream");
			}
			return inputStream.Read(buf, off, len);
		}

		public override void Write(byte[] buf, int off, int len)
		{
			if (outputStream == null)
			{
				throw new TTransportException(TTransportException.ExceptionType.NotOpen, "Cannot write to null outputstream");
			}
			outputStream.Write(buf, off, len);
		}

		public override void Flush()
		{
			if (outputStream == null)
			{
				throw new TTransportException(TTransportException.ExceptionType.NotOpen, "Cannot flush null outputstream");
			}
			outputStream.Flush();
		}

		protected override void Dispose(bool disposing)
		{
			if (!_IsDisposed && disposing)
			{
				if (InputStream != null)
				{
					InputStream.Dispose();
				}
				if (OutputStream != null)
				{
					OutputStream.Dispose();
				}
			}
			_IsDisposed = true;
		}
	}
}
