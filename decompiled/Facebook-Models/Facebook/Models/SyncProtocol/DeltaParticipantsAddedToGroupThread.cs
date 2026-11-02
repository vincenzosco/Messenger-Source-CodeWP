using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class DeltaParticipantsAddedToGroupThread : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool addedParticipants;

		public bool irisSeqId;
	}

	private List<ParticipantInfo> _addedParticipants;

	private long _irisSeqId;

	public Isset __isset;

	public MessageMetadata MessageMetadata { get; set; }

	public List<ParticipantInfo> AddedParticipants
	{
		get
		{
			return _addedParticipants;
		}
		set
		{
			__isset.addedParticipants = true;
			_addedParticipants = value;
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

	public DeltaParticipantsAddedToGroupThread()
	{
	}

	public DeltaParticipantsAddedToGroupThread(MessageMetadata messageMetadata)
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
					AddedParticipants = new List<ParticipantInfo>();
					TList tList = iprot.ReadListBegin();
					for (int i = 0; i < tList.Count; i++)
					{
						ParticipantInfo participantInfo = new ParticipantInfo();
						participantInfo = new ParticipantInfo();
						participantInfo.Read(iprot);
						AddedParticipants.Add(participantInfo);
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
		TStruct struc = new TStruct("DeltaParticipantsAddedToGroupThread");
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
		if (AddedParticipants != null && __isset.addedParticipants)
		{
			field.Name = "addedParticipants";
			field.Type = TType.List;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteListBegin(new TList(TType.Struct, AddedParticipants.Count));
			foreach (ParticipantInfo addedParticipant in AddedParticipants)
			{
				addedParticipant.Write(oprot);
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
		StringBuilder stringBuilder = new StringBuilder("DeltaParticipantsAddedToGroupThread(");
		stringBuilder.Append("MessageMetadata: ");
		stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
		stringBuilder.Append(",AddedParticipants: ");
		stringBuilder.Append(AddedParticipants);
		stringBuilder.Append(",IrisSeqId: ");
		stringBuilder.Append(IrisSeqId);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
