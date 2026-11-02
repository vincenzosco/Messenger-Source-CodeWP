using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class DeltaSentMessage : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool attachments;

		public bool irisSeqId;
	}

	private List<Attachment> _attachments;

	private long _irisSeqId;

	public Isset __isset;

	public MessageMetadata MessageMetadata { get; set; }

	public List<Attachment> Attachments
	{
		get
		{
			return _attachments;
		}
		set
		{
			__isset.attachments = true;
			_attachments = value;
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

	public DeltaSentMessage()
	{
	}

	public DeltaSentMessage(MessageMetadata messageMetadata)
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
				if (tField.Type == TType.List)
				{
					Attachments = new List<Attachment>();
					TList tList = iprot.ReadListBegin();
					for (int i = 0; i < tList.Count; i++)
					{
						Attachment attachment = new Attachment();
						attachment = new Attachment();
						attachment.Read(iprot);
						Attachments.Add(attachment);
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
		if (!flag)
		{
			throw new TProtocolException(1);
		}
	}

	public void Write(TProtocol oprot)
	{
		TStruct struc = new TStruct("DeltaSentMessage");
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
		if (Attachments != null && __isset.attachments)
		{
			field.Name = "attachments";
			field.Type = TType.List;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteListBegin(new TList(TType.Struct, Attachments.Count));
			foreach (Attachment attachment in Attachments)
			{
				attachment.Write(oprot);
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
		StringBuilder stringBuilder = new StringBuilder("DeltaSentMessage(");
		stringBuilder.Append("MessageMetadata: ");
		stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
		stringBuilder.Append(",Attachments: ");
		stringBuilder.Append(Attachments);
		stringBuilder.Append(",IrisSeqId: ");
		stringBuilder.Append(IrisSeqId);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
