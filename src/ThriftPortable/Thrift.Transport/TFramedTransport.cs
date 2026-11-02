using System;
using System.IO;

namespace Thrift.Transport
{
	public class TFramedTransport : TTransport, IDisposable
	{
		public class Factory : TTransportFactory
		{
			public override TTransport GetTransport(TTransport trans)
			{
				return new TFramedTransport(trans);
			}
		}

		private const int header_size = 4;

		protected TTransport transport;

		protected MemoryStream writeBuffer;

		protected MemoryStream readBuffer;

		private static byte[] header_dummy = new byte[4];

		private bool _IsDisposed;

		public override bool IsOpen
		{
			get
			{
				return transport.IsOpen;
			}
		}

		public TFramedTransport()
		{
			InitWriteBuffer();
		}

		public TFramedTransport(TTransport transport)
			: this()
		{
			this.transport = transport;
		}

		public override void Open()
		{
			transport.Open();
		}

		public override void Close()
		{
			transport.Close();
		}

		public override int Read(byte[] buf, int off, int len)
		{
			if (readBuffer != null)
			{
				int num = readBuffer.Read(buf, off, len);
				if (num > 0)
				{
					return num;
				}
			}
			ReadFrame();
			return readBuffer.Read(buf, off, len);
		}

		private void ReadFrame()
		{
			byte[] array = new byte[4];
			transport.ReadAll(array, 0, 4);
			int num = ((array[0] & 0xFF) << 24) | ((array[1] & 0xFF) << 16) | ((array[2] & 0xFF) << 8) | (array[3] & 0xFF);
			byte[] array2 = new byte[num];
			transport.ReadAll(array2, 0, num);
			readBuffer = new MemoryStream(array2);
		}

		public override void Write(byte[] buf, int off, int len)
		{
			writeBuffer.Write(buf, off, len);
		}

		public override void Flush()
		{
			byte[] array = writeBuffer.ToArray();
			int num = (int)writeBuffer.Length;
			int num2 = num - 4;
			if (num2 < 0)
			{
				throw new InvalidOperationException();
			}
			InitWriteBuffer();
			array[0] = (byte)(0xFF & (num2 >> 24));
			array[1] = (byte)(0xFF & (num2 >> 16));
			array[2] = (byte)(0xFF & (num2 >> 8));
			array[3] = (byte)(0xFF & num2);
			transport.Write(array, 0, num);
			transport.Flush();
		}

		private void InitWriteBuffer()
		{
			writeBuffer = new MemoryStream(1024);
			writeBuffer.Write(header_dummy, 0, 4);
		}

		protected override void Dispose(bool disposing)
		{
			if (!_IsDisposed && disposing && readBuffer != null)
			{
				readBuffer.Dispose();
			}
			_IsDisposed = true;
		}
	}
}
