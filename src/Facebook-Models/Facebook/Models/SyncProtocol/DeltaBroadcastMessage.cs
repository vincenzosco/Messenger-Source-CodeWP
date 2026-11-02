using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaBroadcastMessage : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool messageMetadatas;

			public bool body;

			public bool coordinates;

			public bool stickerId;

			public bool attachments;

			public bool irisSeqId;
		}

		private List<MessageMetadata> _messageMetadatas;

		private string _body;

		private Coordinates _coordinates;

		private long _stickerId;

		private List<Attachment> _attachments;

		private long _irisSeqId;

		public Isset __isset;

		public List<MessageMetadata> MessageMetadatas
		{
			get
			{
				return _messageMetadatas;
			}
			set
			{
				__isset.messageMetadatas = true;
				_messageMetadatas = value;
			}
		}

		public string Body
		{
			get
			{
				return _body;
			}
			set
			{
				__isset.body = true;
				_body = value;
			}
		}

		public Coordinates Coordinates
		{
			get
			{
				return _coordinates;
			}
			set
			{
				__isset.coordinates = true;
				_coordinates = value;
			}
		}

		public long StickerId
		{
			get
			{
				return _stickerId;
			}
			set
			{
				__isset.stickerId = true;
				_stickerId = value;
			}
		}

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
					if (tField.Type == TType.List)
					{
						MessageMetadatas = new List<MessageMetadata>();
						TList tList2 = iprot.ReadListBegin();
						for (int j = 0; j < tList2.Count; j++)
						{
							MessageMetadata messageMetadata = new MessageMetadata();
							messageMetadata = new MessageMetadata();
							messageMetadata.Read(iprot);
							MessageMetadatas.Add(messageMetadata);
						}
						iprot.ReadListEnd();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.String)
					{
						Body = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.Struct)
					{
						Coordinates = new Coordinates();
						Coordinates.Read(iprot);
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 4:
					if (tField.Type == TType.I64)
					{
						StickerId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 5:
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
		}

		public void Write(TProtocol oprot)
		{
			TStruct struc = new TStruct("DeltaBroadcastMessage");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (MessageMetadatas != null && __isset.messageMetadatas)
			{
				field.Name = "messageMetadatas";
				field.Type = TType.List;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteListBegin(new TList(TType.Struct, MessageMetadatas.Count));
				foreach (MessageMetadata messageMetadata in MessageMetadatas)
				{
					messageMetadata.Write(oprot);
				}
				oprot.WriteListEnd();
				oprot.WriteFieldEnd();
			}
			if (Body != null && __isset.body)
			{
				field.Name = "body";
				field.Type = TType.String;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(Body);
				oprot.WriteFieldEnd();
			}
			if (Coordinates != null && __isset.coordinates)
			{
				field.Name = "coordinates";
				field.Type = TType.Struct;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				Coordinates.Write(oprot);
				oprot.WriteFieldEnd();
			}
			if (__isset.stickerId)
			{
				field.Name = "stickerId";
				field.Type = TType.I64;
				field.ID = 4;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(StickerId);
				oprot.WriteFieldEnd();
			}
			if (Attachments != null && __isset.attachments)
			{
				field.Name = "attachments";
				field.Type = TType.List;
				field.ID = 5;
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
			StringBuilder stringBuilder = new StringBuilder("DeltaBroadcastMessage(");
			stringBuilder.Append("MessageMetadatas: ");
			stringBuilder.Append(MessageMetadatas);
			stringBuilder.Append(",Body: ");
			stringBuilder.Append(Body);
			stringBuilder.Append(",Coordinates: ");
			stringBuilder.Append((Coordinates == null) ? "<null>" : Coordinates.ToString());
			stringBuilder.Append(",StickerId: ");
			stringBuilder.Append(StickerId);
			stringBuilder.Append(",Attachments: ");
			stringBuilder.Append(Attachments);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
