using System;

namespace Thrift.Transport
{
	public abstract class TTransport : IDisposable
	{
		public abstract bool IsOpen { get; }

		public bool Peek()
		{
			return IsOpen;
		}

		public abstract void Open();

		public abstract void Close();

		public abstract int Read(byte[] buf, int off, int len);

		public int ReadAll(byte[] buf, int off, int len)
		{
			int i = 0;
			int num = 0;
			for (; i < len; i += num)
			{
				num = Read(buf, off + i, len - i);
				if (num <= 0)
				{
					throw new TTransportException(TTransportException.ExceptionType.EndOfFile, "Cannot read, Remote side has closed");
				}
			}
			return i;
		}

		public virtual void Write(byte[] buf)
		{
			Write(buf, 0, buf.Length);
		}

		public abstract void Write(byte[] buf, int off, int len);

		public virtual void Flush()
		{
		}

		public virtual IAsyncResult BeginFlush(AsyncCallback callback, object state)
		{
			return null;
		}

		public virtual void EndFlush(IAsyncResult asyncResult)
		{
		}

		protected abstract void Dispose(bool disposing);

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
