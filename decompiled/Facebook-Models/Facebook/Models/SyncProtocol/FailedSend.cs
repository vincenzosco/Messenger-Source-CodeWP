using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class FailedSend : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool offlineThreadingId;

		public bool errorMessage;

		public bool isRetryable;

		public bool errorCode;
	}

	private long _offlineThreadingId;

	private string _errorMessage;

	private bool _isRetryable;

	private int _errorCode;

	public Isset __isset;

	public long OfflineThreadingId
	{
		get
		{
			return _offlineThreadingId;
		}
		set
		{
			__isset.offlineThreadingId = true;
			_offlineThreadingId = value;
		}
	}

	public string ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			__isset.errorMessage = true;
			_errorMessage = value;
		}
	}

	public bool IsRetryable
	{
		get
		{
			return _isRetryable;
		}
		set
		{
			__isset.isRetryable = true;
			_isRetryable = value;
		}
	}

	public int ErrorCode
	{
		get
		{
			return _errorCode;
		}
		set
		{
			__isset.errorCode = true;
			_errorCode = value;
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
					OfflineThreadingId = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 2:
				if (tField.Type == TType.String)
				{
					ErrorMessage = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 3:
				if (tField.Type == TType.Bool)
				{
					IsRetryable = iprot.ReadBool();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 4:
				if (tField.Type == TType.I32)
				{
					ErrorCode = iprot.ReadI32();
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
		TStruct struc = new TStruct("FailedSend");
		oprot.WriteStructBegin(struc);
		TField field = default;
		if (__isset.offlineThreadingId)
		{
			field.Name = "offlineThreadingId";
			field.Type = TType.I64;
			field.ID = 1;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(OfflineThreadingId);
			oprot.WriteFieldEnd();
		}
		if (ErrorMessage != null && __isset.errorMessage)
		{
			field.Name = "errorMessage";
			field.Type = TType.String;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(ErrorMessage);
			oprot.WriteFieldEnd();
		}
		if (__isset.isRetryable)
		{
			field.Name = "isRetryable";
			field.Type = TType.Bool;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			oprot.WriteBool(IsRetryable);
			oprot.WriteFieldEnd();
		}
		if (__isset.errorCode)
		{
			field.Name = "errorCode";
			field.Type = TType.I32;
			field.ID = 4;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32(ErrorCode);
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("FailedSend(");
		stringBuilder.Append("OfflineThreadingId: ");
		stringBuilder.Append(OfflineThreadingId);
		stringBuilder.Append(",ErrorMessage: ");
		stringBuilder.Append(ErrorMessage);
		stringBuilder.Append(",IsRetryable: ");
		stringBuilder.Append(IsRetryable);
		stringBuilder.Append(",ErrorCode: ");
		stringBuilder.Append(ErrorCode);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
