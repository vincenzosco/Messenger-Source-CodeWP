using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaThreadName : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool name;

			public bool irisSeqId;

			public bool recipientFbIds;
		}

		private string _name;

		private long _irisSeqId;

		private List<long> _recipientFbIds;

		public Isset __isset;

		public MessageMetadata MessageMetadata { get; set; }

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				__isset.name = true;
				_name = value;
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

		public List<long> RecipientFbIds
		{
			get
			{
				return _recipientFbIds;
			}
			set
			{
				__isset.recipientFbIds = true;
				_recipientFbIds = value;
			}
		}

		public DeltaThreadName()
		{
		}

		public DeltaThreadName(MessageMetadata messageMetadata)
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
						Name = iprot.ReadString();
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
				case 1001:
					if (tField.Type == TType.List)
					{
						RecipientFbIds = new List<long>();
						TList tList = iprot.ReadListBegin();
						for (int i = 0; i < tList.Count; i++)
						{
							long num = 0L;
							num = iprot.ReadI64();
							RecipientFbIds.Add(num);
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
			if (!flag)
			{
				throw new TProtocolException(1);
			}
		}

		public void Write(TProtocol oprot)
		{
			TStruct struc = new TStruct("DeltaThreadName");
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
			if (Name != null && __isset.name)
			{
				field.Name = "name";
				field.Type = TType.String;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(Name);
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
			if (RecipientFbIds != null && __isset.recipientFbIds)
			{
				field.Name = "recipientFbIds";
				field.Type = TType.List;
				field.ID = 1001;
				oprot.WriteFieldBegin(field);
				oprot.WriteListBegin(new TList(TType.I64, RecipientFbIds.Count));
				foreach (long recipientFbId in RecipientFbIds)
				{
					oprot.WriteI64(recipientFbId);
				}
				oprot.WriteListEnd();
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("DeltaThreadName(");
			stringBuilder.Append("MessageMetadata: ");
			stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
			stringBuilder.Append(",Name: ");
			stringBuilder.Append(Name);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(",RecipientFbIds: ");
			stringBuilder.Append(RecipientFbIds);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
