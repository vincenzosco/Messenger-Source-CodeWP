using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class ThreadKey : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool otherUserFbId;

		public bool threadFbId;
	}

	private long _otherUserFbId;

	private long _threadFbId;

	public Isset __isset;

	public long OtherUserFbId
	{
		get
		{
			return _otherUserFbId;
		}
		set
		{
			__isset.otherUserFbId = true;
			_otherUserFbId = value;
		}
	}

	public long ThreadFbId
	{
		get
		{
			return _threadFbId;
		}
		set
		{
			__isset.threadFbId = true;
			_threadFbId = value;
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
				if (tField.Type == TType.I64)
				{
					OtherUserFbId = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 2:
				if (tField.Type == TType.I64)
				{
					ThreadFbId = iprot.ReadI64();
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
		TStruct struc = new TStruct("ThreadKey");
		oprot.WriteStructBegin(struc);
		TField field = default;
		if (__isset.otherUserFbId)
		{
			field.Name = "otherUserFbId";
			field.Type = TType.I64;
			field.ID = 1;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(OtherUserFbId);
			oprot.WriteFieldEnd();
		}
		if (__isset.threadFbId)
		{
			field.Name = "threadFbId";
			field.Type = TType.I64;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(ThreadFbId);
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("ThreadKey(");
		stringBuilder.Append("OtherUserFbId: ");
		stringBuilder.Append(OtherUserFbId);
		stringBuilder.Append(",ThreadFbId: ");
		stringBuilder.Append(ThreadFbId);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
