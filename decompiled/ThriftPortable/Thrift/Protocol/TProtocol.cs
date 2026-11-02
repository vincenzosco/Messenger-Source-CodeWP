using System;
using System.Text;
using Thrift.Transport;

namespace Thrift.Protocol
{
	public abstract class TProtocol : IDisposable
	{
		protected TTransport trans;

		private bool _IsDisposed;

		public TTransport Transport => trans;

		protected TProtocol(TTransport trans)
		{
			this.trans = trans;
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_IsDisposed && disposing && trans != null)
			{
				((IDisposable)trans).Dispose();
			}
			_IsDisposed = true;
		}

		public abstract void WriteMessageBegin(TMessage message);

		public abstract void WriteMessageEnd();

		public abstract void WriteStructBegin(TStruct struc);

		public abstract void WriteStructEnd();

		public abstract void WriteFieldBegin(TField field);

		public abstract void WriteFieldEnd();

		public abstract void WriteFieldStop();

		public abstract void WriteMapBegin(TMap map);

		public abstract void WriteMapEnd();

		public abstract void WriteListBegin(TList list);

		public abstract void WriteListEnd();

		public abstract void WriteSetBegin(TSet set);

		public abstract void WriteSetEnd();

		public abstract void WriteBool(bool b);

		public abstract void WriteByte(sbyte b);

		public abstract void WriteI16(short i16);

		public abstract void WriteI32(int i32);

		public abstract void WriteI64(long i64);

		public abstract void WriteDouble(double d);

		public virtual void WriteString(string s)
		{
			WriteBinary(Encoding.UTF8.GetBytes(s));
		}

		public abstract void WriteBinary(byte[] b);

		public abstract TMessage ReadMessageBegin();

		public abstract void ReadMessageEnd();

		public abstract TStruct ReadStructBegin();

		public abstract void ReadStructEnd();

		public abstract TField ReadFieldBegin();

		public abstract void ReadFieldEnd();

		public abstract TMap ReadMapBegin();

		public abstract void ReadMapEnd();

		public abstract TList ReadListBegin();

		public abstract void ReadListEnd();

		public abstract TSet ReadSetBegin();

		public abstract void ReadSetEnd();

		public abstract bool ReadBool();

		public abstract sbyte ReadByte();

		public abstract short ReadI16();

		public abstract int ReadI32();

		public abstract long ReadI64();

		public abstract double ReadDouble();

		public virtual string ReadString()
		{
			byte[] array = ReadBinary();
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		public abstract byte[] ReadBinary();
	}
}
