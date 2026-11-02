using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Thrift.Transport;

namespace Thrift.Protocol;

public class TJSONProtocol : TProtocol
{
	public class Factory : TProtocolFactory
	{
		public TProtocol GetProtocol(TTransport trans)
		{
			return new TJSONProtocol(trans);
		}
	}

	protected class JSONBaseContext
	{
		protected TJSONProtocol proto;

		public JSONBaseContext(TJSONProtocol proto)
		{
			this.proto = proto;
		}

		public virtual void Write()
		{
		}

		public virtual void Read()
		{
		}

		public virtual bool EscapeNumbers()
		{
			return false;
		}
	}

	protected class JSONListContext(TJSONProtocol protocol) : JSONBaseContext(protocol)
	{
		private bool first = true;

		public override void Write()
		{
			if (first)
			{
				first = false;
			}
			else
			{
				proto.trans.Write(COMMA);
			}
		}

		public override void Read()
		{
			if (first)
			{
				first = false;
			}
			else
			{
				proto.ReadJSONSyntaxChar(COMMA);
			}
		}
	}

	protected class JSONPairContext(TJSONProtocol proto) : JSONBaseContext(proto)
	{
		private bool first = true;

		private bool colon = true;

		public override void Write()
		{
			if (first)
			{
				first = false;
				colon = true;
			}
			else
			{
				proto.trans.Write(colon ? COLON : COMMA);
				colon = !colon;
			}
		}

		public override void Read()
		{
			if (first)
			{
				first = false;
				colon = true;
			}
			else
			{
				proto.ReadJSONSyntaxChar(colon ? COLON : COMMA);
				colon = !colon;
			}
		}

		public override bool EscapeNumbers()
		{
			return colon;
		}
	}

	protected class LookaheadReader
	{
		protected TJSONProtocol proto;

		private bool hasData;

		private byte[] data = new byte[1];

		public LookaheadReader(TJSONProtocol proto)
		{
			this.proto = proto;
		}

		public byte Read()
		{
			if (hasData)
			{
				hasData = false;
			}
			else
			{
				proto.trans.ReadAll(data, 0, 1);
			}
			return data[0];
		}

		public byte Peek()
		{
			if (!hasData)
			{
				proto.trans.ReadAll(data, 0, 1);
			}
			hasData = true;
			return data[0];
		}
	}

	private const long VERSION = 1L;

	private const int DEF_STRING_SIZE = 16;

	private static byte[] COMMA = new byte[1] { 44 };

	private static byte[] COLON = new byte[1] { 58 };

	private static byte[] LBRACE = new byte[1] { 123 };

	private static byte[] RBRACE = new byte[1] { 125 };

	private static byte[] LBRACKET = new byte[1] { 91 };

	private static byte[] RBRACKET = new byte[1] { 93 };

	private static byte[] QUOTE = new byte[1] { 34 };

	private static byte[] BACKSLASH = new byte[1] { 92 };

	private static byte[] ZERO = new byte[1] { 48 };

	private byte[] ESCSEQ = new byte[4] { 92, 117, 48, 48 };

	private byte[] JSON_CHAR_TABLE = new byte[48]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 98, 116,
		110, 0, 102, 114, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 1, 1, 34, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1
	};

	private char[] ESCAPE_CHARS = "\"\\bfnrt".ToCharArray();

	private byte[] ESCAPE_CHAR_VALS = new byte[7] { 34, 92, 8, 12, 10, 13, 9 };

	private static byte[] NAME_BOOL = new byte[2] { 116, 102 };

	private static byte[] NAME_BYTE = new byte[2] { 105, 56 };

	private static byte[] NAME_I16 = new byte[3] { 105, 49, 54 };

	private static byte[] NAME_I32 = new byte[3] { 105, 51, 50 };

	private static byte[] NAME_I64 = new byte[3] { 105, 54, 52 };

	private static byte[] NAME_DOUBLE = new byte[3] { 100, 98, 108 };

	private static byte[] NAME_STRUCT = new byte[3] { 114, 101, 99 };

	private static byte[] NAME_STRING = new byte[3] { 115, 116, 114 };

	private static byte[] NAME_MAP = new byte[3] { 109, 97, 112 };

	private static byte[] NAME_LIST = new byte[3] { 108, 115, 116 };

	private static byte[] NAME_SET = new byte[3] { 115, 101, 116 };

	protected Encoding utf8Encoding = Encoding.UTF8;

	protected Stack<JSONBaseContext> contextStack = new Stack<JSONBaseContext>();

	protected JSONBaseContext context;

	protected LookaheadReader reader;

	private byte[] tempBuffer = new byte[4];

	private static byte[] GetTypeNameForTypeID(TType typeID)
	{
		return typeID switch
		{
			TType.Bool => NAME_BOOL, 
			TType.Byte => NAME_BYTE, 
			TType.I16 => NAME_I16, 
			TType.I32 => NAME_I32, 
			TType.I64 => NAME_I64, 
			TType.Double => NAME_DOUBLE, 
			TType.String => NAME_STRING, 
			TType.Struct => NAME_STRUCT, 
			TType.Map => NAME_MAP, 
			TType.Set => NAME_SET, 
			TType.List => NAME_LIST, 
			_ => throw new TProtocolException(5, "Unrecognized type"), 
		};
	}

	private static TType GetTypeIDForTypeName(byte[] name)
	{
		TType tType = TType.Stop;
		if (name.Length > 1)
		{
			switch (name[0])
			{
			case 100:
				tType = TType.Double;
				break;
			case 105:
				switch (name[1])
				{
				case 56:
					tType = TType.Byte;
					break;
				case 49:
					tType = TType.I16;
					break;
				case 51:
					tType = TType.I32;
					break;
				case 54:
					tType = TType.I64;
					break;
				}
				break;
			case 108:
				tType = TType.List;
				break;
			case 109:
				tType = TType.Map;
				break;
			case 114:
				tType = TType.Struct;
				break;
			case 115:
				if (name[1] == 116)
				{
					tType = TType.String;
				}
				else if (name[1] == 101)
				{
					tType = TType.Set;
				}
				break;
			case 116:
				tType = TType.Bool;
				break;
			}
		}
		if (tType == TType.Stop)
		{
			throw new TProtocolException(5, "Unrecognized type");
		}
		return tType;
	}

	protected void PushContext(JSONBaseContext c)
	{
		contextStack.Push(context);
		context = c;
	}

	protected void PopContext()
	{
		context = contextStack.Pop();
	}

	public TJSONProtocol(TTransport trans)
		: base(trans)
	{
		context = new JSONBaseContext(this);
		reader = new LookaheadReader(this);
	}

	protected void ReadJSONSyntaxChar(byte[] b)
	{
		byte b2 = reader.Read();
		if (b2 != b[0])
		{
			throw new TProtocolException(1, "Unexpected character:" + (char)b2);
		}
	}

	private static byte HexVal(byte ch)
	{
		if (ch >= 48 && ch <= 57)
		{
			return (byte)(ch - 48);
		}
		if (ch >= 97 && ch <= 102)
		{
			ch += 10;
			return (byte)(ch - 97);
		}
		throw new TProtocolException(1, "Expected hex character");
	}

	private static byte HexChar(byte val)
	{
		val &= 0xF;
		if (val < 10)
		{
			return (byte)(val + 48);
		}
		val -= 10;
		return (byte)(val + 97);
	}

	private void WriteJSONString(byte[] b)
	{
		context.Write();
		trans.Write(QUOTE);
		int num = b.Length;
		for (int i = 0; i < num; i++)
		{
			if ((b[i] & 0xFF) >= 48)
			{
				if (b[i] == BACKSLASH[0])
				{
					trans.Write(BACKSLASH);
					trans.Write(BACKSLASH);
				}
				else
				{
					trans.Write(b, i, 1);
				}
				continue;
			}
			tempBuffer[0] = JSON_CHAR_TABLE[b[i]];
			if (tempBuffer[0] == 1)
			{
				trans.Write(b, i, 1);
				continue;
			}
			if (tempBuffer[0] > 1)
			{
				trans.Write(BACKSLASH);
				trans.Write(tempBuffer, 0, 1);
				continue;
			}
			trans.Write(ESCSEQ);
			tempBuffer[0] = HexChar((byte)(b[i] >> 4));
			tempBuffer[1] = HexChar(b[i]);
			trans.Write(tempBuffer, 0, 2);
		}
		trans.Write(QUOTE);
	}

	private void WriteJSONInteger(long num)
	{
		context.Write();
		string s = num.ToString();
		bool flag = context.EscapeNumbers();
		if (flag)
		{
			trans.Write(QUOTE);
		}
		trans.Write(utf8Encoding.GetBytes(s));
		if (flag)
		{
			trans.Write(QUOTE);
		}
	}

	private void WriteJSONDouble(double num)
	{
		context.Write();
		string text = num.ToString(CultureInfo.InvariantCulture);
		bool flag = false;
		switch (text[0])
		{
		case 'I':
		case 'N':
			flag = true;
			break;
		case '-':
			if (text[1] == 'I')
			{
				flag = true;
			}
			break;
		}
		bool flag2 = flag || context.EscapeNumbers();
		if (flag2)
		{
			trans.Write(QUOTE);
		}
		trans.Write(utf8Encoding.GetBytes(text));
		if (flag2)
		{
			trans.Write(QUOTE);
		}
	}

	private void WriteJSONBase64(byte[] b)
	{
		context.Write();
		trans.Write(QUOTE);
		int num = b.Length;
		int num2 = 0;
		while (num >= 3)
		{
			TBase64Utils.encode(b, num2, 3, tempBuffer, 0);
			trans.Write(tempBuffer, 0, 4);
			num2 += 3;
			num -= 3;
		}
		if (num > 0)
		{
			TBase64Utils.encode(b, num2, num, tempBuffer, 0);
			trans.Write(tempBuffer, 0, num + 1);
		}
		trans.Write(QUOTE);
	}

	private void WriteJSONObjectStart()
	{
		context.Write();
		trans.Write(LBRACE);
		PushContext(new JSONPairContext(this));
	}

	private void WriteJSONObjectEnd()
	{
		PopContext();
		trans.Write(RBRACE);
	}

	private void WriteJSONArrayStart()
	{
		context.Write();
		trans.Write(LBRACKET);
		PushContext(new JSONListContext(this));
	}

	private void WriteJSONArrayEnd()
	{
		PopContext();
		trans.Write(RBRACKET);
	}

	public override void WriteMessageBegin(TMessage message)
	{
		WriteJSONArrayStart();
		WriteJSONInteger(1L);
		byte[] bytes = utf8Encoding.GetBytes(message.Name);
		WriteJSONString(bytes);
		WriteJSONInteger((long)message.Type);
		WriteJSONInteger(message.SeqID);
	}

	public override void WriteMessageEnd()
	{
		WriteJSONArrayEnd();
	}

	public override void WriteStructBegin(TStruct str)
	{
		WriteJSONObjectStart();
	}

	public override void WriteStructEnd()
	{
		WriteJSONObjectEnd();
	}

	public override void WriteFieldBegin(TField field)
	{
		WriteJSONInteger(field.ID);
		WriteJSONObjectStart();
		WriteJSONString(GetTypeNameForTypeID(field.Type));
	}

	public override void WriteFieldEnd()
	{
		WriteJSONObjectEnd();
	}

	public override void WriteFieldStop()
	{
	}

	public override void WriteMapBegin(TMap map)
	{
		WriteJSONArrayStart();
		WriteJSONString(GetTypeNameForTypeID(map.KeyType));
		WriteJSONString(GetTypeNameForTypeID(map.ValueType));
		WriteJSONInteger(map.Count);
		WriteJSONObjectStart();
	}

	public override void WriteMapEnd()
	{
		WriteJSONObjectEnd();
		WriteJSONArrayEnd();
	}

	public override void WriteListBegin(TList list)
	{
		WriteJSONArrayStart();
		WriteJSONString(GetTypeNameForTypeID(list.ElementType));
		WriteJSONInteger(list.Count);
	}

	public override void WriteListEnd()
	{
		WriteJSONArrayEnd();
	}

	public override void WriteSetBegin(TSet set)
	{
		WriteJSONArrayStart();
		WriteJSONString(GetTypeNameForTypeID(set.ElementType));
		WriteJSONInteger(set.Count);
	}

	public override void WriteSetEnd()
	{
		WriteJSONArrayEnd();
	}

	public override void WriteBool(bool b)
	{
		WriteJSONInteger(b ? 1 : 0);
	}

	public override void WriteByte(sbyte b)
	{
		WriteJSONInteger(b);
	}

	public override void WriteI16(short i16)
	{
		WriteJSONInteger(i16);
	}

	public override void WriteI32(int i32)
	{
		WriteJSONInteger(i32);
	}

	public override void WriteI64(long i64)
	{
		WriteJSONInteger(i64);
	}

	public override void WriteDouble(double dub)
	{
		WriteJSONDouble(dub);
	}

	public override void WriteString(string str)
	{
		byte[] bytes = utf8Encoding.GetBytes(str);
		WriteJSONString(bytes);
	}

	public override void WriteBinary(byte[] bin)
	{
		WriteJSONBase64(bin);
	}

	private byte[] ReadJSONString(bool skipContext)
	{
		MemoryStream memoryStream = new MemoryStream();
		if (!skipContext)
		{
			context.Read();
		}
		ReadJSONSyntaxChar(QUOTE);
		while (true)
		{
			byte b = reader.Read();
			if (b == QUOTE[0])
			{
				break;
			}
			if (b == ESCSEQ[0])
			{
				b = reader.Read();
				if (b == ESCSEQ[1])
				{
					ReadJSONSyntaxChar(ZERO);
					ReadJSONSyntaxChar(ZERO);
					trans.ReadAll(tempBuffer, 0, 2);
					b = (byte)((HexVal(tempBuffer[0]) << 4) + HexVal(tempBuffer[1]));
				}
				else
				{
					int num = Array.IndexOf(ESCAPE_CHARS, (char)b);
					if (num == -1)
					{
						throw new TProtocolException(1, "Expected control char");
					}
					b = ESCAPE_CHAR_VALS[num];
				}
			}
			memoryStream.Write(new byte[1] { b }, 0, 1);
		}
		return memoryStream.ToArray();
	}

	private bool IsJSONNumeric(byte b)
	{
		switch (b)
		{
		case 43:
		case 45:
		case 46:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
		case 55:
		case 56:
		case 57:
		case 69:
		case 101:
			return true;
		default:
			return false;
		}
	}

	private string ReadJSONNumericChars()
	{
		StringBuilder stringBuilder = new StringBuilder();
		while (true)
		{
			byte b = reader.Peek();
			if (!IsJSONNumeric(b))
			{
				break;
			}
			stringBuilder.Append((char)reader.Read());
		}
		return stringBuilder.ToString();
	}

	private long ReadJSONInteger()
	{
		context.Read();
		if (context.EscapeNumbers())
		{
			ReadJSONSyntaxChar(QUOTE);
		}
		string s = ReadJSONNumericChars();
		if (context.EscapeNumbers())
		{
			ReadJSONSyntaxChar(QUOTE);
		}
		try
		{
			return long.Parse(s);
		}
		catch (FormatException)
		{
			throw new TProtocolException(1, "Bad data encounted in numeric data");
		}
	}

	private double ReadJSONDouble()
	{
		context.Read();
		if (reader.Peek() == QUOTE[0])
		{
			byte[] array = ReadJSONString(skipContext: true);
			double num = double.Parse(utf8Encoding.GetString(array, 0, array.Length), CultureInfo.InvariantCulture);
			if (!context.EscapeNumbers() && !double.IsNaN(num) && !double.IsInfinity(num))
			{
				throw new TProtocolException(1, "Numeric data unexpectedly quoted");
			}
			return num;
		}
		if (context.EscapeNumbers())
		{
			ReadJSONSyntaxChar(QUOTE);
		}
		try
		{
			return double.Parse(ReadJSONNumericChars());
		}
		catch (FormatException)
		{
			throw new TProtocolException(1, "Bad data encounted in numeric data");
		}
	}

	private byte[] ReadJSONBase64()
	{
		byte[] array = ReadJSONString(skipContext: false);
		int num = array.Length;
		int num2 = 0;
		int num3 = 0;
		while (num >= 4)
		{
			TBase64Utils.decode(array, num2, 4, array, num3);
			num2 += 4;
			num -= 4;
			num3 += 3;
		}
		if (num > 1)
		{
			TBase64Utils.decode(array, num2, num, array, num3);
			num3 += num - 1;
		}
		byte[] array2 = new byte[num3];
		Array.Copy(array, 0, array2, 0, num3);
		return array2;
	}

	private void ReadJSONObjectStart()
	{
		context.Read();
		ReadJSONSyntaxChar(LBRACE);
		PushContext(new JSONPairContext(this));
	}

	private void ReadJSONObjectEnd()
	{
		ReadJSONSyntaxChar(RBRACE);
		PopContext();
	}

	private void ReadJSONArrayStart()
	{
		context.Read();
		ReadJSONSyntaxChar(LBRACKET);
		PushContext(new JSONListContext(this));
	}

	private void ReadJSONArrayEnd()
	{
		ReadJSONSyntaxChar(RBRACKET);
		PopContext();
	}

	public override TMessage ReadMessageBegin()
	{
		TMessage result = default;
		ReadJSONArrayStart();
		if (ReadJSONInteger() != 1)
		{
			throw new TProtocolException(4, "Message contained bad version.");
		}
		byte[] array = ReadJSONString(skipContext: false);
		result.Name = utf8Encoding.GetString(array, 0, array.Length);
		result.Type = (TMessageType)ReadJSONInteger();
		result.SeqID = (int)ReadJSONInteger();
		return result;
	}

	public override void ReadMessageEnd()
	{
		ReadJSONArrayEnd();
	}

	public override TStruct ReadStructBegin()
	{
		ReadJSONObjectStart();
		return default;
	}

	public override void ReadStructEnd()
	{
		ReadJSONObjectEnd();
	}

	public override TField ReadFieldBegin()
	{
		TField result = default;
		byte b = reader.Peek();
		if (b == RBRACE[0])
		{
			result.Type = TType.Stop;
		}
		else
		{
			result.ID = (short)ReadJSONInteger();
			ReadJSONObjectStart();
			result.Type = GetTypeIDForTypeName(ReadJSONString(skipContext: false));
		}
		return result;
	}

	public override void ReadFieldEnd()
	{
		ReadJSONObjectEnd();
	}

	public override TMap ReadMapBegin()
	{
		TMap result = default;
		ReadJSONArrayStart();
		result.KeyType = GetTypeIDForTypeName(ReadJSONString(skipContext: false));
		result.ValueType = GetTypeIDForTypeName(ReadJSONString(skipContext: false));
		result.Count = (int)ReadJSONInteger();
		ReadJSONObjectStart();
		return result;
	}

	public override void ReadMapEnd()
	{
		ReadJSONObjectEnd();
		ReadJSONArrayEnd();
	}

	public override TList ReadListBegin()
	{
		TList result = default;
		ReadJSONArrayStart();
		result.ElementType = GetTypeIDForTypeName(ReadJSONString(skipContext: false));
		result.Count = (int)ReadJSONInteger();
		return result;
	}

	public override void ReadListEnd()
	{
		ReadJSONArrayEnd();
	}

	public override TSet ReadSetBegin()
	{
		TSet result = default;
		ReadJSONArrayStart();
		result.ElementType = GetTypeIDForTypeName(ReadJSONString(skipContext: false));
		result.Count = (int)ReadJSONInteger();
		return result;
	}

	public override void ReadSetEnd()
	{
		ReadJSONArrayEnd();
	}

	public override bool ReadBool()
	{
		if (ReadJSONInteger() != 0)
		{
			return true;
		}
		return false;
	}

	public override sbyte ReadByte()
	{
		return (sbyte)ReadJSONInteger();
	}

	public override short ReadI16()
	{
		return (short)ReadJSONInteger();
	}

	public override int ReadI32()
	{
		return (int)ReadJSONInteger();
	}

	public override long ReadI64()
	{
		return ReadJSONInteger();
	}

	public override double ReadDouble()
	{
		return ReadJSONDouble();
	}

	public override string ReadString()
	{
		byte[] array = ReadJSONString(skipContext: false);
		return utf8Encoding.GetString(array, 0, array.Length);
	}

	public override byte[] ReadBinary()
	{
		return ReadJSONBase64();
	}
}
