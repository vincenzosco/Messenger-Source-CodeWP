using System;
using System.IO;
using System.Reflection;
using Thrift.Protocol;

namespace Thrift.Transport
{
	public class TMemoryBuffer : TTransport
	{
		private readonly MemoryStream byteStream;

		private bool _IsDisposed;

		public override bool IsOpen
		{
			get
			{
				return true;
			}
		}

		public TMemoryBuffer()
		{
			byteStream = new MemoryStream();
		}

		public TMemoryBuffer(byte[] buf)
		{
			byteStream = new MemoryStream(buf);
		}

		public override void Open()
		{
		}

		public override void Close()
		{
		}

		public override int Read(byte[] buf, int off, int len)
		{
			return byteStream.Read(buf, off, len);
		}

		public override void Write(byte[] buf, int off, int len)
		{
			byteStream.Write(buf, off, len);
		}

		public byte[] GetBuffer()
		{
			return byteStream.ToArray();
		}

		public static byte[] Serialize(TAbstractBase s)
		{
			TMemoryBuffer tMemoryBuffer = new TMemoryBuffer();
			TBinaryProtocol tProtocol = new TBinaryProtocol(tMemoryBuffer);
			s.Write(tProtocol);
			return tMemoryBuffer.GetBuffer();
		}

		public static T DeSerialize<T>(byte[] buf) where T : TAbstractBase
		{
			TMemoryBuffer trans = new TMemoryBuffer(buf);
			TBinaryProtocol tBinaryProtocol = new TBinaryProtocol(trans);
			if (typeof(TBase).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
			{
				MethodInfo declaredMethod = typeof(T).GetTypeInfo().GetDeclaredMethod("Read");
				T val = Activator.CreateInstance<T>();
				declaredMethod.Invoke(val, new object[1] { tBinaryProtocol });
				return val;
			}
			MethodInfo declaredMethod2 = typeof(T).GetTypeInfo().GetDeclaredMethod("Read");
			return (T)declaredMethod2.Invoke(null, new object[1] { tBinaryProtocol });
		}

		protected override void Dispose(bool disposing)
		{
			if (!_IsDisposed && disposing && byteStream != null)
			{
				byteStream.Dispose();
			}
			_IsDisposed = true;
		}
	}
}
