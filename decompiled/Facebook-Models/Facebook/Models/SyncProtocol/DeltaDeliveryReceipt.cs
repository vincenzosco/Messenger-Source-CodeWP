using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class DeltaDeliveryReceipt : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool threadKey;

		public bool actorFbId;

		public bool deviceId;

		public bool appId;

		public bool timestampMs;

		public bool messageIds;

		public bool deliveredWatermarkTimestampMs;

		public bool irisSeqId;
	}

	private ThreadKey _threadKey;

	private long _actorFbId;

	private string _deviceId;

	private long _appId;

	private long _timestampMs;

	private List<string> _messageIds;

	private long _deliveredWatermarkTimestampMs;

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

	public long ActorFbId
	{
		get
		{
			return _actorFbId;
		}
		set
		{
			__isset.actorFbId = true;
			_actorFbId = value;
		}
	}

	public string DeviceId
	{
		get
		{
			return _deviceId;
		}
		set
		{
			__isset.deviceId = true;
			_deviceId = value;
		}
	}

	public long AppId
	{
		get
		{
			return _appId;
		}
		set
		{
			__isset.appId = true;
			_appId = value;
		}
	}

	public long TimestampMs
	{
		get
		{
			return _timestampMs;
		}
		set
		{
			__isset.timestampMs = true;
			_timestampMs = value;
		}
	}

	public List<string> MessageIds
	{
		get
		{
			return _messageIds;
		}
		set
		{
			__isset.messageIds = true;
			_messageIds = value;
		}
	}

	public long DeliveredWatermarkTimestampMs
	{
		get
		{
			return _deliveredWatermarkTimestampMs;
		}
		set
		{
			__isset.deliveredWatermarkTimestampMs = true;
			_deliveredWatermarkTimestampMs = value;
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
				if (tField.Type == TType.I64)
				{
					ActorFbId = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 3:
				if (tField.Type == TType.String)
				{
					DeviceId = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 4:
				if (tField.Type == TType.I64)
				{
					AppId = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 5:
				if (tField.Type == TType.I64)
				{
					TimestampMs = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 6:
				if (tField.Type == TType.List)
				{
					MessageIds = new List<string>();
					TList tList = iprot.ReadListBegin();
					for (int i = 0; i < tList.Count; i++)
					{
						string text = null;
						text = iprot.ReadString();
						MessageIds.Add(text);
					}
					iprot.ReadListEnd();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 7:
				if (tField.Type == TType.I64)
				{
					DeliveredWatermarkTimestampMs = iprot.ReadI64();
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
		TStruct struc = new TStruct("DeltaDeliveryReceipt");
		oprot.WriteStructBegin(struc);
		TField field = default;
		if (ThreadKey != null && __isset.threadKey)
		{
			field.Name = "threadKey";
			field.Type = TType.Struct;
			field.ID = 1;
			oprot.WriteFieldBegin(field);
			ThreadKey.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (__isset.actorFbId)
		{
			field.Name = "actorFbId";
			field.Type = TType.I64;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(ActorFbId);
			oprot.WriteFieldEnd();
		}
		if (DeviceId != null && __isset.deviceId)
		{
			field.Name = "deviceId";
			field.Type = TType.String;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(DeviceId);
			oprot.WriteFieldEnd();
		}
		if (__isset.appId)
		{
			field.Name = "appId";
			field.Type = TType.I64;
			field.ID = 4;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(AppId);
			oprot.WriteFieldEnd();
		}
		if (__isset.timestampMs)
		{
			field.Name = "timestampMs";
			field.Type = TType.I64;
			field.ID = 5;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(TimestampMs);
			oprot.WriteFieldEnd();
		}
		if (MessageIds != null && __isset.messageIds)
		{
			field.Name = "messageIds";
			field.Type = TType.List;
			field.ID = 6;
			oprot.WriteFieldBegin(field);
			oprot.WriteListBegin(new TList(TType.String, MessageIds.Count));
			foreach (string messageId in MessageIds)
			{
				oprot.WriteString(messageId);
			}
			oprot.WriteListEnd();
			oprot.WriteFieldEnd();
		}
		if (__isset.deliveredWatermarkTimestampMs)
		{
			field.Name = "deliveredWatermarkTimestampMs";
			field.Type = TType.I64;
			field.ID = 7;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(DeliveredWatermarkTimestampMs);
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
		StringBuilder stringBuilder = new StringBuilder("DeltaDeliveryReceipt(");
		stringBuilder.Append("ThreadKey: ");
		stringBuilder.Append((ThreadKey == null) ? "<null>" : ThreadKey.ToString());
		stringBuilder.Append(",ActorFbId: ");
		stringBuilder.Append(ActorFbId);
		stringBuilder.Append(",DeviceId: ");
		stringBuilder.Append(DeviceId);
		stringBuilder.Append(",AppId: ");
		stringBuilder.Append(AppId);
		stringBuilder.Append(",TimestampMs: ");
		stringBuilder.Append(TimestampMs);
		stringBuilder.Append(",MessageIds: ");
		stringBuilder.Append(MessageIds);
		stringBuilder.Append(",DeliveredWatermarkTimestampMs: ");
		stringBuilder.Append(DeliveredWatermarkTimestampMs);
		stringBuilder.Append(",IrisSeqId: ");
		stringBuilder.Append(IrisSeqId);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
