namespace Thrift.Protocol
{
	public static class TProtocolUtil
	{
		public static void Skip(TProtocol prot, TType type)
		{
			switch (type)
			{
			case TType.Bool:
				prot.ReadBool();
				break;
			case TType.Byte:
				prot.ReadByte();
				break;
			case TType.I16:
				prot.ReadI16();
				break;
			case TType.I32:
				prot.ReadI32();
				break;
			case TType.I64:
				prot.ReadI64();
				break;
			case TType.Double:
				prot.ReadDouble();
				break;
			case TType.String:
				prot.ReadBinary();
				break;
			case TType.Struct:
				prot.ReadStructBegin();
				while (true)
				{
					TField tField = prot.ReadFieldBegin();
					if (tField.Type == TType.Stop)
					{
						break;
					}
					Skip(prot, tField.Type);
					prot.ReadFieldEnd();
				}
				prot.ReadStructEnd();
				break;
			case TType.Map:
			{
				TMap tMap = prot.ReadMapBegin();
				for (int k = 0; k < tMap.Count; k++)
				{
					Skip(prot, tMap.KeyType);
					Skip(prot, tMap.ValueType);
				}
				prot.ReadMapEnd();
				break;
			}
			case TType.Set:
			{
				TSet tSet = prot.ReadSetBegin();
				for (int j = 0; j < tSet.Count; j++)
				{
					Skip(prot, tSet.ElementType);
				}
				prot.ReadSetEnd();
				break;
			}
			case TType.List:
			{
				TList tList = prot.ReadListBegin();
				for (int i = 0; i < tList.Count; i++)
				{
					Skip(prot, tList.ElementType);
				}
				prot.ReadListEnd();
				break;
			}
			case (TType)5:
			case (TType)7:
			case (TType)9:
				break;
			}
		}
	}
}
