using System;
using System.Text;
using Thrift.Transport;

namespace Thrift.Protocol;

public class TBinaryProtocol : TProtocol
{
	public class Factory : TProtocolFactory
	{
		protected bool strictRead_;

		protected bool strictWrite_ = true;

		public Factory()
			: this(strictRead: false, strictWrite: true)
		{
		}

		public Factory(bool strictRead, bool strictWrite)
		{
			strictRead_ = strictRead;
			strictWrite_ = strictWrite;
		}

		public TProtocol GetProtocol(TTransport trans)
		{
			return new TBinaryProtocol(trans, strictRead_, strictWrite_);
		}
	}

	protected const uint VERSION_MASK = 4294901760u;

	protected const uint VERSION_1 = 2147549184u;

	protected bool strictRead_;

	protected bool strictWrite_ = true;

	private byte[] bout = new byte[1];

	private byte[] i16out = new byte[2];

	private byte[] i32out = new byte[4];

	private byte[] i64out = new byte[8];

	private byte[] bin = new byte[1];

	private byte[] i16in = new byte[2];

	private byte[] i32in = new byte[4];

	private byte[] i64in = new byte[8];

	public TBinaryProtocol(TTransport trans)
		: this(trans, strictRead: false, strictWrite: true)
	{
	}

	public TBinaryProtocol(TTransport trans, bool strictRead, bool strictWrite)
		: base(trans)
	{
		strictRead_ = strictRead;
		strictWrite_ = strictWrite;
	}

	public override void WriteMessageBegin(TMessage message)
	{
		if (strictWrite_)
		{
			uint i = (uint)((TMessageType)(-2147418112) | message.Type);
			WriteI32((int)i);
			WriteString(message.Name);
			WriteI32(message.SeqID);
		}
		else
		{
			WriteString(message.Name);
			WriteByte((sbyte)message.Type);
			WriteI32(message.SeqID);
		}
	}

	public override void WriteMessageEnd()
	{
	}

	public override void WriteStructBegin(TStruct struc)
	{
	}

	public override void WriteStructEnd()
	{
	}

	public override void WriteFieldBegin(TField field)
	{
		WriteByte((sbyte)field.Type);
		WriteI16(field.ID);
	}

	public override void WriteFieldEnd()
	{
	}

	public override void WriteFieldStop()
	{
		WriteByte(0);
	}

	public override void WriteMapBegin(TMap map)
	{
		WriteByte((sbyte)map.KeyType);
		WriteByte((sbyte)map.ValueType);
		WriteI32(map.Count);
	}

	public override void WriteMapEnd()
	{
	}

	public override void WriteListBegin(TList list)
	{
		WriteByte((sbyte)list.ElementType);
		WriteI32(list.Count);
	}

	public override void WriteListEnd()
	{
	}

	public override void WriteSetBegin(TSet set)
	{
		WriteByte((sbyte)set.ElementType);
		WriteI32(set.Count);
	}

	public override void WriteSetEnd()
	{
	}

	public override void WriteBool(bool b)
	{
		WriteByte((sbyte)(b ? 1 : 0));
	}

	public override void WriteByte(sbyte b)
	{
		bout[0] = (byte)b;
		trans.Write(bout, 0, 1);
	}

	public override void WriteI16(short s)
	{
		i16out[0] = (byte)(0xFF & (s >> 8));
		i16out[1] = (byte)(0xFF & s);
		trans.Write(i16out, 0, 2);
	}

	public override void WriteI32(int i32)
	{
		i32out[0] = (byte)(0xFF & (i32 >> 24));
		i32out[1] = (byte)(0xFF & (i32 >> 16));
		i32out[2] = (byte)(0xFF & (i32 >> 8));
		i32out[3] = (byte)(0xFF & i32);
		trans.Write(i32out, 0, 4);
	}

	public override void WriteI64(long i64)
	{
		i64out[0] = (byte)(0xFF & (i64 >> 56));
		i64out[1] = (byte)(0xFF & (i64 >> 48));
		i64out[2] = (byte)(0xFF & (i64 >> 40));
		i64out[3] = (byte)(0xFF & (i64 >> 32));
		i64out[4] = (byte)(0xFF & (i64 >> 24));
		i64out[5] = (byte)(0xFF & (i64 >> 16));
		i64out[6] = (byte)(0xFF & (i64 >> 8));
		i64out[7] = (byte)(0xFF & i64);
		trans.Write(i64out, 0, 8);
	}

	public override void WriteDouble(double d)
	{
		WriteI64(BitConverter.DoubleToInt64Bits(d));
	}

	public override void WriteBinary(byte[] b)
	{
		WriteI32(b.Length);
		trans.Write(b, 0, b.Length);
	}

	public override TMessage ReadMessageBegin()
	{
		TMessage result = default;
		int num = ReadI32();
		if (num < 0)
		{
			uint num2 = (uint)(num & -65536);
			if (num2 != 2147549184u)
			{
				throw new TProtocolException(4, "Bad version in ReadMessageBegin: " + num2);
			}
			result.Type = (TMessageType)(num & 0xFF);
			result.Name = ReadString();
			result.SeqID = ReadI32();
		}
		else
		{
			if (strictRead_)
			{
				throw new TProtocolException(4, "Missing version in readMessageBegin, old client?");
			}
			result.Name = ReadStringBody(num);
			result.Type = (TMessageType)ReadByte();
			result.SeqID = ReadI32();
		}
		return result;
	}

	public override void ReadMessageEnd()
	{
	}

	public override TStruct ReadStructBegin()
	{
		return default;
	}

	public override void ReadStructEnd()
	{
	}

	public override TField ReadFieldBegin()
	{
		TField result = new TField
		{
			Type = (TType)ReadByte()
		};
		if (result.Type != TType.Stop)
		{
			result.ID = ReadI16();
		}
		return result;
	}

	public override void ReadFieldEnd()
	{
	}

	public override TMap ReadMapBegin()
	{
		return new TMap
		{
			KeyType = (TType)ReadByte(),
			ValueType = (TType)ReadByte(),
			Count = ReadI32()
		};
	}

	public override void ReadMapEnd()
	{
	}

	public override TList ReadListBegin()
	{
		return new TList
		{
			ElementType = (TType)ReadByte(),
			Count = ReadI32()
		};
	}

	public override void ReadListEnd()
	{
	}

	public override TSet ReadSetBegin()
	{
		return new TSet
		{
			ElementType = (TType)ReadByte(),
			Count = ReadI32()
		};
	}

	public override void ReadSetEnd()
	{
	}

	public override bool ReadBool()
	{
		return ReadByte() == 1;
	}

	public override sbyte ReadByte()
	{
		ReadAll(bin, 0, 1);
		return (sbyte)bin[0];
	}

	public override short ReadI16()
	{
		ReadAll(i16in, 0, 2);
		return (short)(((i16in[0] & 0xFF) << 8) | (i16in[1] & 0xFF));
	}

	public override int ReadI32()
	{
		ReadAll(i32in, 0, 4);
		return ((i32in[0] & 0xFF) << 24) | ((i32in[1] & 0xFF) << 16) | ((i32in[2] & 0xFF) << 8) | (i32in[3] & 0xFF);
	}

	public override long ReadI64()
	{
		ReadAll(i64in, 0, 8);
		return ((long)(i64in[0] & 0xFF) << 56) | ((long)(i64in[1] & 0xFF) << 48) | ((long)(i64in[2] & 0xFF) << 40) | ((long)(i64in[3] & 0xFF) << 32) | ((long)(i64in[4] & 0xFF) << 24) | ((long)(i64in[5] & 0xFF) << 16) | ((long)(i64in[6] & 0xFF) << 8) | (i64in[7] & 0xFF);
	}

	public override double ReadDouble()
	{
		return BitConverter.Int64BitsToDouble(ReadI64());
	}

	public override byte[] ReadBinary()
	{
		int num = ReadI32();
		byte[] array = new byte[num];
		trans.ReadAll(array, 0, num);
		return array;
	}

	private string ReadStringBody(int size)
	{
		byte[] array = new byte[size];
		trans.ReadAll(array, 0, size);
		return Encoding.UTF8.GetString(array, 0, array.Length);
	}

	private int ReadAll(byte[] buf, int off, int len)
	{
		return trans.ReadAll(buf, off, len);
	}
}
