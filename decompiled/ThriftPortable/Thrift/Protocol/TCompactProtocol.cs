using System;
using System.Collections.Generic;
using System.Text;
using Thrift.Transport;

namespace Thrift.Protocol
{
	public class TCompactProtocol : TProtocol
	{
		private static class Types
		{
			public const byte STOP = 0;

			public const byte BOOLEAN_TRUE = 1;

			public const byte BOOLEAN_FALSE = 2;

			public const byte BYTE = 3;

			public const byte I16 = 4;

			public const byte I32 = 5;

			public const byte I64 = 6;

			public const byte DOUBLE = 7;

			public const byte BINARY = 8;

			public const byte LIST = 9;

			public const byte SET = 10;

			public const byte MAP = 11;

			public const byte STRUCT = 12;
		}

		public class Factory : TProtocolFactory
		{
			public TProtocol GetProtocol(TTransport trans)
			{
				return new TCompactProtocol(trans);
			}
		}

		private const byte PROTOCOL_ID = 130;

		private const byte VERSION = 1;

		private const byte VERSION_MASK = 31;

		private const byte TYPE_MASK = 224;

		private const int TYPE_SHIFT_AMOUNT = 5;

		private static TStruct ANONYMOUS_STRUCT = new TStruct("");

		private static TField TSTOP = new TField("", TType.Stop, 0);

		private static byte[] ttypeToCompactType = new byte[16];

		private Stack<short> lastField_ = new Stack<short>(15);

		private short lastFieldId_;

		private TField? booleanField_;

		private bool? boolValue_;

		private byte[] byteDirectBuffer = new byte[1];

		private byte[] i32buf = new byte[5];

		private byte[] varint64out = new byte[10];

		private byte[] byteRawBuf = new byte[1];

		public TCompactProtocol(TTransport trans)
			: base(trans)
		{
			ttypeToCompactType[0] = 0;
			ttypeToCompactType[2] = 1;
			ttypeToCompactType[3] = 3;
			ttypeToCompactType[6] = 4;
			ttypeToCompactType[8] = 5;
			ttypeToCompactType[10] = 6;
			ttypeToCompactType[4] = 7;
			ttypeToCompactType[11] = 8;
			ttypeToCompactType[15] = 9;
			ttypeToCompactType[14] = 10;
			ttypeToCompactType[13] = 11;
			ttypeToCompactType[12] = 12;
		}

		public void reset()
		{
			lastField_.Clear();
			lastFieldId_ = 0;
		}

		private void WriteByteDirect(byte b)
		{
			byteDirectBuffer[0] = b;
			trans.Write(byteDirectBuffer);
		}

		private void WriteByteDirect(int n)
		{
			WriteByteDirect((byte)n);
		}

		private void WriteVarint32(uint n)
		{
			int len = 0;
			while ((n & -128) != 0)
			{
				i32buf[len++] = (byte)((n & 0x7F) | 0x80);
				n >>= 7;
			}
			i32buf[len++] = (byte)n;
			trans.Write(i32buf, 0, len);
		}

		public override void WriteMessageBegin(TMessage message)
		{
			WriteByteDirect((byte)130);
			WriteByteDirect((byte)(1 | (((int)message.Type << 5) & 0xE0)));
			WriteVarint32((uint)message.SeqID);
			WriteString(message.Name);
		}

		public override void WriteStructBegin(TStruct strct)
		{
			lastField_.Push(lastFieldId_);
			lastFieldId_ = 0;
		}

		public override void WriteStructEnd()
		{
			lastFieldId_ = lastField_.Pop();
		}

		public override void WriteFieldBegin(TField field)
		{
			if (field.Type == TType.Bool)
			{
				booleanField_ = field;
			}
			else
			{
				WriteFieldBeginInternal(field, byte.MaxValue);
			}
		}

		private void WriteFieldBeginInternal(TField field, byte typeOverride)
		{
			byte b = ((typeOverride == byte.MaxValue) ? getCompactType(field.Type) : typeOverride);
			if (field.ID > lastFieldId_ && field.ID - lastFieldId_ <= 15)
			{
				WriteByteDirect((field.ID - lastFieldId_ << 4) | b);
			}
			else
			{
				WriteByteDirect(b);
				WriteI16(field.ID);
			}
			lastFieldId_ = field.ID;
		}

		public override void WriteFieldStop()
		{
			WriteByteDirect((byte)0);
		}

		public override void WriteMapBegin(TMap map)
		{
			if (map.Count == 0)
			{
				WriteByteDirect(0);
				return;
			}
			WriteVarint32((uint)map.Count);
			WriteByteDirect((getCompactType(map.KeyType) << 4) | getCompactType(map.ValueType));
		}

		public override void WriteListBegin(TList list)
		{
			WriteCollectionBegin(list.ElementType, list.Count);
		}

		public override void WriteSetBegin(TSet set)
		{
			WriteCollectionBegin(set.ElementType, set.Count);
		}

		public override void WriteBool(bool b)
		{
			if (booleanField_.HasValue)
			{
				WriteFieldBeginInternal(booleanField_.Value, (byte)(b ? 1 : 2));
				booleanField_ = null;
			}
			else
			{
				WriteByteDirect((byte)(b ? 1 : 2));
			}
		}

		public override void WriteByte(sbyte b)
		{
			WriteByteDirect((byte)b);
		}

		public override void WriteI16(short i16)
		{
			WriteVarint32(intToZigZag(i16));
		}

		public override void WriteI32(int i32)
		{
			WriteVarint32(intToZigZag(i32));
		}

		public override void WriteI64(long i64)
		{
			WriteVarint64(longToZigzag(i64));
		}

		public override void WriteDouble(double dub)
		{
			byte[] array = new byte[8];
			byte[] buf = array;
			fixedLongToBytes(BitConverter.DoubleToInt64Bits(dub), buf, 0);
			trans.Write(buf);
		}

		public override void WriteString(string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			WriteBinary(bytes, 0, bytes.Length);
		}

		public override void WriteBinary(byte[] bin)
		{
			WriteBinary(bin, 0, bin.Length);
		}

		private void WriteBinary(byte[] buf, int offset, int length)
		{
			WriteVarint32((uint)length);
			trans.Write(buf, offset, length);
		}

		public override void WriteMessageEnd()
		{
		}

		public override void WriteMapEnd()
		{
		}

		public override void WriteListEnd()
		{
		}

		public override void WriteSetEnd()
		{
		}

		public override void WriteFieldEnd()
		{
		}

		protected void WriteCollectionBegin(TType elemType, int size)
		{
			if (size <= 14)
			{
				WriteByteDirect((size << 4) | getCompactType(elemType));
				return;
			}
			WriteByteDirect(0xF0 | getCompactType(elemType));
			WriteVarint32((uint)size);
		}

		private void WriteVarint64(ulong n)
		{
			int len = 0;
			while ((n & 0xFFFFFFFFFFFFFF80uL) != 0)
			{
				varint64out[len++] = (byte)((n & 0x7F) | 0x80);
				n >>= 7;
			}
			varint64out[len++] = (byte)n;
			trans.Write(varint64out, 0, len);
		}

		private ulong longToZigzag(long n)
		{
			return (ulong)((n << 1) ^ (n >> 63));
		}

		private uint intToZigZag(int n)
		{
			return (uint)((n << 1) ^ (n >> 31));
		}

		private void fixedLongToBytes(long n, byte[] buf, int off)
		{
			buf[off] = (byte)(n & 0xFF);
			buf[off + 1] = (byte)((n >> 8) & 0xFF);
			buf[off + 2] = (byte)((n >> 16) & 0xFF);
			buf[off + 3] = (byte)((n >> 24) & 0xFF);
			buf[off + 4] = (byte)((n >> 32) & 0xFF);
			buf[off + 5] = (byte)((n >> 40) & 0xFF);
			buf[off + 6] = (byte)((n >> 48) & 0xFF);
			buf[off + 7] = (byte)((n >> 56) & 0xFF);
		}

		public override TMessage ReadMessageBegin()
		{
			byte b = (byte)ReadByte();
			if (b != 130)
			{
				throw new TProtocolException("Expected protocol id " + ((byte)130).ToString("X") + " but got " + b.ToString("X"));
			}
			byte b2 = (byte)ReadByte();
			byte b3 = (byte)(b2 & 0x1F);
			if (b3 != 1)
			{
				throw new TProtocolException("Expected version " + (byte)1 + " but got " + b3);
			}
			byte type = (byte)((b2 >> 5) & 3);
			int seqid = (int)ReadVarint32();
			string name = ReadString();
			return new TMessage(name, (TMessageType)type, seqid);
		}

		public override TStruct ReadStructBegin()
		{
			lastField_.Push(lastFieldId_);
			lastFieldId_ = 0;
			return ANONYMOUS_STRUCT;
		}

		public override void ReadStructEnd()
		{
			lastFieldId_ = lastField_.Pop();
		}

		public override TField ReadFieldBegin()
		{
			byte b = (byte)ReadByte();
			if (b == 0)
			{
				return TSTOP;
			}
			short num = (short)((b & 0xF0) >> 4);
			TField result = new TField(id: (num != 0) ? ((short)(lastFieldId_ + num)) : ReadI16(), name: "", type: getTType((byte)(b & 0xF)));
			if (isBoolType(b))
			{
				boolValue_ = (byte)(b & 0xF) == 1;
			}
			lastFieldId_ = result.ID;
			return result;
		}

		public override TMap ReadMapBegin()
		{
			int num = (int)ReadVarint32();
			byte b = (byte)((num != 0) ? ((byte)ReadByte()) : 0);
			return new TMap(getTType((byte)(b >> 4)), getTType((byte)(b & 0xF)), num);
		}

		public override TList ReadListBegin()
		{
			byte b = (byte)ReadByte();
			int num = (b >> 4) & 0xF;
			if (num == 15)
			{
				num = (int)ReadVarint32();
			}
			TType tType = getTType(b);
			return new TList(tType, num);
		}

		public override TSet ReadSetBegin()
		{
			return new TSet(ReadListBegin());
		}

		public override bool ReadBool()
		{
			if (boolValue_.HasValue)
			{
				bool value = boolValue_.Value;
				boolValue_ = null;
				return value;
			}
			return ReadByte() == 1;
		}

		public override sbyte ReadByte()
		{
			trans.ReadAll(byteRawBuf, 0, 1);
			return (sbyte)byteRawBuf[0];
		}

		public override short ReadI16()
		{
			return (short)zigzagToInt(ReadVarint32());
		}

		public override int ReadI32()
		{
			return zigzagToInt(ReadVarint32());
		}

		public override long ReadI64()
		{
			return zigzagToLong(ReadVarint64());
		}

		public override double ReadDouble()
		{
			byte[] array = new byte[8];
			trans.ReadAll(array, 0, 8);
			return BitConverter.Int64BitsToDouble(bytesToLong(array));
		}

		public override string ReadString()
		{
			int num = (int)ReadVarint32();
			if (num == 0)
			{
				return "";
			}
			byte[] array = ReadBinary(num);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		public override byte[] ReadBinary()
		{
			int num = (int)ReadVarint32();
			if (num == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[num];
			trans.ReadAll(array, 0, num);
			return array;
		}

		private byte[] ReadBinary(int length)
		{
			if (length == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[length];
			trans.ReadAll(array, 0, length);
			return array;
		}

		public override void ReadMessageEnd()
		{
		}

		public override void ReadFieldEnd()
		{
		}

		public override void ReadMapEnd()
		{
		}

		public override void ReadListEnd()
		{
		}

		public override void ReadSetEnd()
		{
		}

		private uint ReadVarint32()
		{
			uint num = 0u;
			int num2 = 0;
			while (true)
			{
				byte b = (byte)ReadByte();
				num |= (uint)((b & 0x7F) << num2);
				if ((b & 0x80) != 128)
				{
					break;
				}
				num2 += 7;
			}
			return num;
		}

		private ulong ReadVarint64()
		{
			int num = 0;
			ulong num2 = 0uL;
			while (true)
			{
				byte b = (byte)ReadByte();
				num2 |= (ulong)((long)(b & 0x7F) << num);
				if ((b & 0x80) != 128)
				{
					break;
				}
				num += 7;
			}
			return num2;
		}

		private int zigzagToInt(uint n)
		{
			return (int)((n >> 1) ^ (0 - (n & 1)));
		}

		private long zigzagToLong(ulong n)
		{
			return (long)((n >> 1) ^ (0L - (n & 1)));
		}

		private long bytesToLong(byte[] bytes)
		{
			return (long)((((ulong)bytes[7] & 0xFFuL) << 56) | (((ulong)bytes[6] & 0xFFuL) << 48) | (((ulong)bytes[5] & 0xFFuL) << 40) | (((ulong)bytes[4] & 0xFFuL) << 32) | (((ulong)bytes[3] & 0xFFuL) << 24) | (((ulong)bytes[2] & 0xFFuL) << 16) | (((ulong)bytes[1] & 0xFFuL) << 8) | ((ulong)bytes[0] & 0xFFuL));
		}

		private bool isBoolType(byte b)
		{
			int num = b & 0xF;
			if (num != 1)
			{
				return num == 2;
			}
			return true;
		}

		private TType getTType(byte type)
		{
			switch ((byte)(type & 0xF))
			{
			case 0:
				return TType.Stop;
			case 1:
			case 2:
				return TType.Bool;
			case 3:
				return TType.Byte;
			case 4:
				return TType.I16;
			case 5:
				return TType.I32;
			case 6:
				return TType.I64;
			case 7:
				return TType.Double;
			case 8:
				return TType.String;
			case 9:
				return TType.List;
			case 10:
				return TType.Set;
			case 11:
				return TType.Map;
			case 12:
				return TType.Struct;
			default:
				throw new TProtocolException("don't know what type: " + (byte)(type & 0xF));
			}
		}

		private byte getCompactType(TType ttype)
		{
			return ttypeToCompactType[(uint)ttype];
		}
	}
}
