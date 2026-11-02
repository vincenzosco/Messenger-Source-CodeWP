using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaThreadDelete : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool threadKeys;

			public bool irisSeqId;
		}

		private List<ThreadKey> _threadKeys;

		private long _irisSeqId;

		public Isset __isset;

		public List<ThreadKey> ThreadKeys
		{
			get
			{
				return _threadKeys;
			}
			set
			{
				__isset.threadKeys = true;
				_threadKeys = value;
			}
		}

		public long IrisSeqId
		{
			get
			{
				return _irisSeqId;
			}
			set
			{
				__isset.irisSeqId = true;
				_irisSeqId = value;
			}
		}

		public void Read(TProtocol iprot)
		{
			iprot.ReadStructBegin();
			while (true)
			{
				TField tField = iprot.ReadFieldBegin();
				if (tField.Type == TType.Stop)
				{
					break;
				}
				switch (tField.ID)
				{
				case 1:
					if (tField.Type == TType.List)
					{
						ThreadKeys = new List<ThreadKey>();
						TList tList = iprot.ReadListBegin();
						for (int i = 0; i < tList.Count; i++)
						{
							ThreadKey threadKey = new ThreadKey();
							threadKey = new ThreadKey();
							threadKey.Read(iprot);
							ThreadKeys.Add(threadKey);
						}
						iprot.ReadListEnd();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1000:
					if (tField.Type == TType.I64)
					{
						IrisSeqId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				default:
					TProtocolUtil.Skip(iprot, tField.Type);
					break;
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		public void Write(TProtocol oprot)
		{
			TStruct struc = new TStruct("DeltaThreadDelete");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (ThreadKeys != null && __isset.threadKeys)
			{
				field.Name = "threadKeys";
				field.Type = TType.List;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteListBegin(new TList(TType.Struct, ThreadKeys.Count));
				foreach (ThreadKey threadKey in ThreadKeys)
				{
					threadKey.Write(oprot);
				}
				oprot.WriteListEnd();
				oprot.WriteFieldEnd();
			}
			if (__isset.irisSeqId)
			{
				field.Name = "irisSeqId";
				field.Type = TType.I64;
				field.ID = 1000;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(IrisSeqId);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("DeltaThreadDelete(");
			stringBuilder.Append("ThreadKeys: ");
			stringBuilder.Append(ThreadKeys);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
