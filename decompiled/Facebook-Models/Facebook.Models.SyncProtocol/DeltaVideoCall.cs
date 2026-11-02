using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaVideoCall : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool answered;

			public bool startTime;

			public bool duration;

			public bool irisSeqId;
		}

		private bool _answered;

		private long _startTime;

		private long _duration;

		private long _irisSeqId;

		public Isset __isset;

		public MessageMetadata MessageMetadata { get; set; }

		public bool Answered
		{
			get
			{
				return _answered;
			}
			set
			{
				__isset.answered = true;
				_answered = value;
			}
		}

		public long StartTime
		{
			get
			{
				return _startTime;
			}
			set
			{
				__isset.startTime = true;
				_startTime = value;
			}
		}

		public long Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				__isset.duration = true;
				_duration = value;
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

		public DeltaVideoCall()
		{
		}

		public DeltaVideoCall(MessageMetadata messageMetadata)
			: this()
		{
			MessageMetadata = messageMetadata;
		}

		public void Read(TProtocol iprot)
		{
			bool flag = false;
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
					if (tField.Type == TType.Bool)
					{
						Answered = iprot.ReadBool();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.I64)
					{
						StartTime = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 4:
					if (tField.Type == TType.I64)
					{
						Duration = iprot.ReadI64();
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
		}

		public void Write(TProtocol oprot)
		{
			TStruct struc = new TStruct("DeltaVideoCall");
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
			if (__isset.answered)
			{
				field.Name = "answered";
				field.Type = TType.Bool;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(Answered);
				oprot.WriteFieldEnd();
			}
			if (__isset.startTime)
			{
				field.Name = "startTime";
				field.Type = TType.I64;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(StartTime);
				oprot.WriteFieldEnd();
			}
			if (__isset.duration)
			{
				field.Name = "duration";
				field.Type = TType.I64;
				field.ID = 4;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(Duration);
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
			StringBuilder stringBuilder = new StringBuilder("DeltaVideoCall(");
			stringBuilder.Append("MessageMetadata: ");
			stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
			stringBuilder.Append(",Answered: ");
			stringBuilder.Append(Answered);
			stringBuilder.Append(",StartTime: ");
			stringBuilder.Append(StartTime);
			stringBuilder.Append(",Duration: ");
			stringBuilder.Append(Duration);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
