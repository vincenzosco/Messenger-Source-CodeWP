using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class AttachmentAppAttribution : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool attributionMetadata;

			public bool attributionAppName;

			public bool attributionAppIconURI;

			public bool androidPackageName;

			public bool iOSStoreId;

			public bool otherUserAppScopedFbIds;

			public bool visibility;
		}

		private string _attributionMetadata;

		private string _attributionAppName;

		private string _attributionAppIconURI;

		private string _androidPackageName;

		private long _iOSStoreId;

		private Dictionary<long, long> _otherUserAppScopedFbIds;

		private AppAttributionVisibility _visibility;

		public Isset __isset;

		public long AttributionAppId { get; set; }

		public string AttributionMetadata
		{
			get
			{
				return _attributionMetadata;
			}
			set
			{
				__isset.attributionMetadata = true;
				_attributionMetadata = value;
			}
		}

		public string AttributionAppName
		{
			get
			{
				return _attributionAppName;
			}
			set
			{
				__isset.attributionAppName = true;
				_attributionAppName = value;
			}
		}

		public string AttributionAppIconURI
		{
			get
			{
				return _attributionAppIconURI;
			}
			set
			{
				__isset.attributionAppIconURI = true;
				_attributionAppIconURI = value;
			}
		}

		public string AndroidPackageName
		{
			get
			{
				return _androidPackageName;
			}
			set
			{
				__isset.androidPackageName = true;
				_androidPackageName = value;
			}
		}

		public long IOSStoreId
		{
			get
			{
				return _iOSStoreId;
			}
			set
			{
				__isset.iOSStoreId = true;
				_iOSStoreId = value;
			}
		}

		public Dictionary<long, long> OtherUserAppScopedFbIds
		{
			get
			{
				return _otherUserAppScopedFbIds;
			}
			set
			{
				__isset.otherUserAppScopedFbIds = true;
				_otherUserAppScopedFbIds = value;
			}
		}

		public AppAttributionVisibility Visibility
		{
			get
			{
				return _visibility;
			}
			set
			{
				__isset.visibility = true;
				_visibility = value;
			}
		}

		public AttachmentAppAttribution()
		{
		}

		public AttachmentAppAttribution(long attributionAppId)
			: this()
		{
			AttributionAppId = attributionAppId;
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
					if (tField.Type == TType.I64)
					{
						AttributionAppId = iprot.ReadI64();
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
						AttributionMetadata = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.String)
					{
						AttributionAppName = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 4:
					if (tField.Type == TType.String)
					{
						AttributionAppIconURI = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 5:
					if (tField.Type == TType.String)
					{
						AndroidPackageName = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 6:
					if (tField.Type == TType.I64)
					{
						IOSStoreId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 7:
					if (tField.Type == TType.Map)
					{
						OtherUserAppScopedFbIds = new Dictionary<long, long>();
						TMap tMap = iprot.ReadMapBegin();
						for (int i = 0; i < tMap.Count; i++)
						{
							long key = iprot.ReadI64();
							long value = iprot.ReadI64();
							OtherUserAppScopedFbIds[key] = value;
						}
						iprot.ReadMapEnd();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 8:
					if (tField.Type == TType.Struct)
					{
						Visibility = new AppAttributionVisibility();
						Visibility.Read(iprot);
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
			TStruct struc = new TStruct("AttachmentAppAttribution");
			oprot.WriteStructBegin(struc);
			TField field = new TField
			{
				Name = "attributionAppId",
				Type = TType.I64,
				ID = 1
			};
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(AttributionAppId);
			oprot.WriteFieldEnd();
			if (AttributionMetadata != null && __isset.attributionMetadata)
			{
				field.Name = "attributionMetadata";
				field.Type = TType.String;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(AttributionMetadata);
				oprot.WriteFieldEnd();
			}
			if (AttributionAppName != null && __isset.attributionAppName)
			{
				field.Name = "attributionAppName";
				field.Type = TType.String;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(AttributionAppName);
				oprot.WriteFieldEnd();
			}
			if (AttributionAppIconURI != null && __isset.attributionAppIconURI)
			{
				field.Name = "attributionAppIconURI";
				field.Type = TType.String;
				field.ID = 4;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(AttributionAppIconURI);
				oprot.WriteFieldEnd();
			}
			if (AndroidPackageName != null && __isset.androidPackageName)
			{
				field.Name = "androidPackageName";
				field.Type = TType.String;
				field.ID = 5;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(AndroidPackageName);
				oprot.WriteFieldEnd();
			}
			if (__isset.iOSStoreId)
			{
				field.Name = "iOSStoreId";
				field.Type = TType.I64;
				field.ID = 6;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(IOSStoreId);
				oprot.WriteFieldEnd();
			}
			if (OtherUserAppScopedFbIds != null && __isset.otherUserAppScopedFbIds)
			{
				field.Name = "otherUserAppScopedFbIds";
				field.Type = TType.Map;
				field.ID = 7;
				oprot.WriteFieldBegin(field);
				oprot.WriteMapBegin(new TMap(TType.I64, TType.I64, OtherUserAppScopedFbIds.Count));
				foreach (long key in OtherUserAppScopedFbIds.Keys)
				{
					oprot.WriteI64(key);
					oprot.WriteI64(OtherUserAppScopedFbIds[key]);
				}
				oprot.WriteMapEnd();
				oprot.WriteFieldEnd();
			}
			if (Visibility != null && __isset.visibility)
			{
				field.Name = "visibility";
				field.Type = TType.Struct;
				field.ID = 8;
				oprot.WriteFieldBegin(field);
				Visibility.Write(oprot);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("AttachmentAppAttribution(");
			stringBuilder.Append("AttributionAppId: ");
			stringBuilder.Append(AttributionAppId);
			stringBuilder.Append(",AttributionMetadata: ");
			stringBuilder.Append(AttributionMetadata);
			stringBuilder.Append(",AttributionAppName: ");
			stringBuilder.Append(AttributionAppName);
			stringBuilder.Append(",AttributionAppIconURI: ");
			stringBuilder.Append(AttributionAppIconURI);
			stringBuilder.Append(",AndroidPackageName: ");
			stringBuilder.Append(AndroidPackageName);
			stringBuilder.Append(",IOSStoreId: ");
			stringBuilder.Append(IOSStoreId);
			stringBuilder.Append(",OtherUserAppScopedFbIds: ");
			stringBuilder.Append(OtherUserAppScopedFbIds);
			stringBuilder.Append(",Visibility: ");
			stringBuilder.Append((Visibility == null) ? "<null>" : Visibility.ToString());
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
