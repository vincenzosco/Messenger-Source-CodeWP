using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class MessageMetadata : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool threadKey;

		public bool messageId;

		public bool offlineThreadingId;

		public bool actorFbId;

		public bool timestamp;

		public bool shouldBuzzDevice;

		public bool adminText;

		public bool tags;
	}

	private ThreadKey _threadKey;

	private string _messageId;

	private long _offlineThreadingId;

	private long _actorFbId;

	private long _timestamp;

	private bool _shouldBuzzDevice;

	private string _adminText;

	private List<string> _tags;

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

	public long Timestamp
	{
		get
		{
			return _timestamp;
		}
		set
		{
			__isset.timestamp = true;
			_timestamp = value;
		}
	}

	public bool ShouldBuzzDevice
	{
		get
		{
			return _shouldBuzzDevice;
		}
		set
		{
			__isset.shouldBuzzDevice = true;
			_shouldBuzzDevice = value;
		}
	}

	public string AdminText
	{
		get
		{
			return _adminText;
		}
		set
		{
			__isset.adminText = true;
			_adminText = value;
		}
	}

	public List<string> Tags
	{
		get
		{
			return _tags;
		}
		set
		{
			__isset.tags = true;
			_tags = value;
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
				if (tField.Type == TType.I64)
				{
					OfflineThreadingId = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 4:
				if (tField.Type == TType.I64)
				{
					ActorFbId = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 5:
				if (tField.Type == TType.I64)
				{
					Timestamp = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 6:
				if (tField.Type == TType.Bool)
				{
					ShouldBuzzDevice = iprot.ReadBool();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 7:
				if (tField.Type == TType.String)
				{
					AdminText = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 8:
				if (tField.Type == TType.List)
				{
					Tags = new List<string>();
					TList tList = iprot.ReadListBegin();
					for (int i = 0; i < tList.Count; i++)
					{
						string text = null;
						text = iprot.ReadString();
						Tags.Add(text);
					}
					iprot.ReadListEnd();
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
		TStruct struc = new TStruct("MessageMetadata");
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
		if (MessageId != null && __isset.messageId)
		{
			field.Name = "messageId";
			field.Type = TType.String;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(MessageId);
			oprot.WriteFieldEnd();
		}
		if (__isset.offlineThreadingId)
		{
			field.Name = "offlineThreadingId";
			field.Type = TType.I64;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(OfflineThreadingId);
			oprot.WriteFieldEnd();
		}
		if (__isset.actorFbId)
		{
			field.Name = "actorFbId";
			field.Type = TType.I64;
			field.ID = 4;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(ActorFbId);
			oprot.WriteFieldEnd();
		}
		if (__isset.timestamp)
		{
			field.Name = "timestamp";
			field.Type = TType.I64;
			field.ID = 5;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(Timestamp);
			oprot.WriteFieldEnd();
		}
		if (__isset.shouldBuzzDevice)
		{
			field.Name = "shouldBuzzDevice";
			field.Type = TType.Bool;
			field.ID = 6;
			oprot.WriteFieldBegin(field);
			oprot.WriteBool(ShouldBuzzDevice);
			oprot.WriteFieldEnd();
		}
		if (AdminText != null && __isset.adminText)
		{
			field.Name = "adminText";
			field.Type = TType.String;
			field.ID = 7;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(AdminText);
			oprot.WriteFieldEnd();
		}
		if (Tags != null && __isset.tags)
		{
			field.Name = "tags";
			field.Type = TType.List;
			field.ID = 8;
			oprot.WriteFieldBegin(field);
			oprot.WriteListBegin(new TList(TType.String, Tags.Count));
			foreach (string tag in Tags)
			{
				oprot.WriteString(tag);
			}
			oprot.WriteListEnd();
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("MessageMetadata(");
		stringBuilder.Append("ThreadKey: ");
		stringBuilder.Append((ThreadKey == null) ? "<null>" : ThreadKey.ToString());
		stringBuilder.Append(",MessageId: ");
		stringBuilder.Append(MessageId);
		stringBuilder.Append(",OfflineThreadingId: ");
		stringBuilder.Append(OfflineThreadingId);
		stringBuilder.Append(",ActorFbId: ");
		stringBuilder.Append(ActorFbId);
		stringBuilder.Append(",Timestamp: ");
		stringBuilder.Append(Timestamp);
		stringBuilder.Append(",ShouldBuzzDevice: ");
		stringBuilder.Append(ShouldBuzzDevice);
		stringBuilder.Append(",AdminText: ");
		stringBuilder.Append(AdminText);
		stringBuilder.Append(",Tags: ");
		stringBuilder.Append(Tags);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
