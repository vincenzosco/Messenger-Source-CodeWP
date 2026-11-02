using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaNewMessage : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool body;

			public bool coordinates;

			public bool stickerId;

			public bool attachments;

			public bool ttl;

			public bool irisSeqId;
		}

		private string _body;

		private Coordinates _coordinates;

		private long _stickerId;

		private List<Attachment> _attachments;

		private MessageLifetime _ttl;

		private long _irisSeqId;

		public Isset __isset;

		public MessageMetadata MessageMetadata { get; set; }

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

		public MessageLifetime Ttl
		{
			get
			{
				return _ttl;
			}
			set
			{
				__isset.ttl = true;
				_ttl = value;
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

		public DeltaNewMessage()
		{
		}

		public DeltaNewMessage(MessageMetadata messageMetadata)
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
				case 6:
					if (tField.Type == TType.I32)
					{
						Ttl = (MessageLifetime)iprot.ReadI32();
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
			TStruct struc = new TStruct("DeltaNewMessage");
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
			if (__isset.ttl)
			{
				field.Name = "ttl";
				field.Type = TType.I32;
				field.ID = 6;
				oprot.WriteFieldBegin(field);
				oprot.WriteI32((int)Ttl);
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
			StringBuilder stringBuilder = new StringBuilder("DeltaNewMessage(");
			stringBuilder.Append("MessageMetadata: ");
			stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
			stringBuilder.Append(",Body: ");
			stringBuilder.Append(Body);
			stringBuilder.Append(",Coordinates: ");
			stringBuilder.Append((Coordinates == null) ? "<null>" : Coordinates.ToString());
			stringBuilder.Append(",StickerId: ");
			stringBuilder.Append(StickerId);
			stringBuilder.Append(",Attachments: ");
			stringBuilder.Append(Attachments);
			stringBuilder.Append(",Ttl: ");
			stringBuilder.Append(Ttl);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
