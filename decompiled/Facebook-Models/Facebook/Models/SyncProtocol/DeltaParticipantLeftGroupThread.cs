using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaParticipantLeftGroupThread : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool irisSeqId;
		}

		private long _irisSeqId;

		public Isset __isset;

		public MessageMetadata MessageMetadata { get; set; }

		public long LeftParticipantFbId { get; set; }

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

		public DeltaParticipantLeftGroupThread()
		{
		}

		public DeltaParticipantLeftGroupThread(MessageMetadata messageMetadata, long leftParticipantFbId)
			: this()
		{
			MessageMetadata = messageMetadata;
			LeftParticipantFbId = leftParticipantFbId;
		}

		public void Read(TProtocol iprot)
		{
			bool flag = false;
			bool flag2 = false;
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
						MessageMetadata = new MessageMetadata();
						MessageMetadata.Read(iprot);
						flag = true;
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.I64)
					{
						LeftParticipantFbId = iprot.ReadI64();
						flag2 = true;
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
			if (!flag)
			{
				throw new TProtocolException(1);
			}
			if (!flag2)
			{
				throw new TProtocolException(1);
			}
		}

		public void Write(TProtocol oprot)
		{
			TStruct struc = new TStruct("DeltaParticipantLeftGroupThread");
			oprot.WriteStructBegin(struc);
			TField field = new TField
			{
				Name = "messageMetadata",
				Type = TType.Struct,
				ID = 1
			};
			oprot.WriteFieldBegin(field);
			MessageMetadata.Write(oprot);
			oprot.WriteFieldEnd();
			field.Name = "leftParticipantFbId";
			field.Type = TType.I64;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(LeftParticipantFbId);
			oprot.WriteFieldEnd();
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
			StringBuilder stringBuilder = new StringBuilder("DeltaParticipantLeftGroupThread(");
			stringBuilder.Append("MessageMetadata: ");
			stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
			stringBuilder.Append(",LeftParticipantFbId: ");
			stringBuilder.Append(LeftParticipantFbId);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
