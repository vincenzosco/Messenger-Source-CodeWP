using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class DeltaPageAdminReply : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool activityToken;

		public bool replyType;

		public bool irisSeqId;
	}

	private string _activityToken;

	private PageAdminReplyType _replyType;

	private long _irisSeqId;

	public Isset __isset;

	public MessageMetadata MessageMetadata { get; set; }

	public string ActivityToken
	{
		get
		{
			return _activityToken;
		}
		set
		{
			__isset.activityToken = true;
			_activityToken = value;
		}
	}

	public PageAdminReplyType ReplyType
	{
		get
		{
			return _replyType;
		}
		set
		{
			__isset.replyType = true;
			_replyType = value;
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

	public DeltaPageAdminReply()
	{
	}

	public DeltaPageAdminReply(MessageMetadata messageMetadata)
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
				if (tField.Type == TType.String)
				{
					ActivityToken = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 3:
				if (tField.Type == TType.I32)
				{
					ReplyType = (PageAdminReplyType)iprot.ReadI32();
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
		TStruct struc = new TStruct("DeltaPageAdminReply");
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
		if (ActivityToken != null && __isset.activityToken)
		{
			field.Name = "activityToken";
			field.Type = TType.String;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(ActivityToken);
			oprot.WriteFieldEnd();
		}
		if (__isset.replyType)
		{
			field.Name = "replyType";
			field.Type = TType.I32;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32((int)ReplyType);
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
		StringBuilder stringBuilder = new StringBuilder("DeltaPageAdminReply(");
		stringBuilder.Append("MessageMetadata: ");
		stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
		stringBuilder.Append(",ActivityToken: ");
		stringBuilder.Append(ActivityToken);
		stringBuilder.Append(",ReplyType: ");
		stringBuilder.Append(ReplyType);
		stringBuilder.Append(",IrisSeqId: ");
		stringBuilder.Append(IrisSeqId);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
