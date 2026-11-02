using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaAdminTextMessage : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool type;

			public bool untypedData;

			public bool irisSeqId;
		}

		private string _type;

		private Dictionary<string, string> _untypedData;

		private long _irisSeqId;

		public Isset __isset;

		public MessageMetadata MessageMetadata { get; set; }

		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				__isset.type = true;
				_type = value;
			}
		}

		public Dictionary<string, string> UntypedData
		{
			get
			{
				return _untypedData;
			}
			set
			{
				__isset.untypedData = true;
				_untypedData = value;
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

		public DeltaAdminTextMessage()
		{
		}

		public DeltaAdminTextMessage(MessageMetadata messageMetadata)
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
						Type = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.Map)
					{
						UntypedData = new Dictionary<string, string>();
						TMap tMap = iprot.ReadMapBegin();
						for (int i = 0; i < tMap.Count; i++)
						{
							string key = iprot.ReadString();
							string value = iprot.ReadString();
							UntypedData[key] = value;
						}
						iprot.ReadMapEnd();
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
			TStruct struc = new TStruct("DeltaAdminTextMessage");
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
			if (Type != null && __isset.type)
			{
				field.Name = "type";
				field.Type = TType.String;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(Type);
				oprot.WriteFieldEnd();
			}
			if (UntypedData != null && __isset.untypedData)
			{
				field.Name = "untypedData";
				field.Type = TType.Map;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteMapBegin(new TMap(TType.String, TType.String, UntypedData.Count));
				foreach (string key in UntypedData.Keys)
				{
					oprot.WriteString(key);
					oprot.WriteString(UntypedData[key]);
				}
				oprot.WriteMapEnd();
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
			StringBuilder stringBuilder = new StringBuilder("DeltaAdminTextMessage(");
			stringBuilder.Append("MessageMetadata: ");
			stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
			stringBuilder.Append(",Type: ");
			stringBuilder.Append(Type);
			stringBuilder.Append(",UntypedData: ");
			stringBuilder.Append(UntypedData);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
