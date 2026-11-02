using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaThreadImage : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool image;

			public bool irisSeqId;
		}

		private Attachment _image;

		private long _irisSeqId;

		public Isset __isset;

		public MessageMetadata MessageMetadata { get; set; }

		public Attachment Image
		{
			get
			{
				return _image;
			}
			set
			{
				__isset.image = true;
				_image = value;
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

		public DeltaThreadImage()
		{
		}

		public DeltaThreadImage(MessageMetadata messageMetadata)
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
					if (tField.Type == TType.Struct)
					{
						Image = new Attachment();
						Image.Read(iprot);
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
			TStruct struc = new TStruct("DeltaThreadImage");
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
			if (Image != null && __isset.image)
			{
				field.Name = "image";
				field.Type = TType.Struct;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				Image.Write(oprot);
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
			StringBuilder stringBuilder = new StringBuilder("DeltaThreadImage(");
			stringBuilder.Append("MessageMetadata: ");
			stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
			stringBuilder.Append(",Image: ");
			stringBuilder.Append((Image == null) ? "<null>" : Image.ToString());
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
