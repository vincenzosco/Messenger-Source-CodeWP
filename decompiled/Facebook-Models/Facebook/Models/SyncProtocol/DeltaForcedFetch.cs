using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaForcedFetch : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool threadKey;

			public bool messageId;

			public bool isLazy;

			public bool irisSeqId;
		}

		private ThreadKey _threadKey;

		private string _messageId;

		private bool _isLazy;

		private long _irisSeqId;

		public Isset __isset;

		public ThreadKey ThreadKey
		{
			get
			{
				return _threadKey;
			}
			set
			{
				__isset.threadKey = true;
				_threadKey = value;
			}
		}

		public string MessageId
		{
			get
			{
				return _messageId;
			}
			set
			{
				__isset.messageId = true;
				_messageId = value;
			}
		}

		public bool IsLazy
		{
			get
			{
				return _isLazy;
			}
			set
			{
				__isset.isLazy = true;
				_isLazy = value;
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
					if (tField.Type == TType.Struct)
					{
						ThreadKey = new ThreadKey();
						ThreadKey.Read(iprot);
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.String)
					{
						MessageId = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.Bool)
					{
						IsLazy = iprot.ReadBool();
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
			TStruct struc = new TStruct("DeltaForcedFetch");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (ThreadKey != null && __isset.threadKey)
			{
				field.Name = "threadKey";
				field.Type = TType.Struct;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				ThreadKey.Write(oprot);
				oprot.WriteFieldEnd();
			}
			if (MessageId != null && __isset.messageId)
			{
				field.Name = "messageId";
				field.Type = TType.String;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(MessageId);
				oprot.WriteFieldEnd();
			}
			if (__isset.isLazy)
			{
				field.Name = "isLazy";
				field.Type = TType.Bool;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(IsLazy);
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
			StringBuilder stringBuilder = new StringBuilder("DeltaForcedFetch(");
			stringBuilder.Append("ThreadKey: ");
			stringBuilder.Append((ThreadKey == null) ? "<null>" : ThreadKey.ToString());
			stringBuilder.Append(",MessageId: ");
			stringBuilder.Append(MessageId);
			stringBuilder.Append(",IsLazy: ");
			stringBuilder.Append(IsLazy);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
