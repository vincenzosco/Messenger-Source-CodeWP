using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class Attachment : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool id;

		public bool mimeType;

		public bool filename;

		public bool fbid;

		public bool fileSize;

		public bool attributionInfo;

		public bool xmaGraphQL;

		public bool imageMetadata;

		public bool videoMetadata;

		public bool haystackHandle;

		public bool genericMetadata;

		public bool hash;

		public bool encryptionKey;

		public bool titanType;

		public bool otherUserFbIds;
	}

	private string _id;

	private string _mimeType;

	private string _filename;

	private long _fbid;

	private long _fileSize;

	private AttachmentAppAttribution _attributionInfo;

	private string _xmaGraphQL;

	private ImageMetadata _imageMetadata;

	private VideoMetadata _videoMetadata;

	private string _haystackHandle;

	private Dictionary<string, string> _genericMetadata;

	private string _hash;

	private string _encryptionKey;

	private int _titanType;

	private THashSet<long> _otherUserFbIds;

	public Isset __isset;

	public string Id
	{
		get
		{
			return _id;
		}
		set
		{
			__isset.id = true;
			_id = value;
		}
	}

	public string MimeType
	{
		get
		{
			return _mimeType;
		}
		set
		{
			__isset.mimeType = true;
			_mimeType = value;
		}
	}

	public string Filename
	{
		get
		{
			return _filename;
		}
		set
		{
			__isset.filename = true;
			_filename = value;
		}
	}

	public long Fbid
	{
		get
		{
			return _fbid;
		}
		set
		{
			__isset.fbid = true;
			_fbid = value;
		}
	}

	public long FileSize
	{
		get
		{
			return _fileSize;
		}
		set
		{
			__isset.fileSize = true;
			_fileSize = value;
		}
	}

	public AttachmentAppAttribution AttributionInfo
	{
		get
		{
			return _attributionInfo;
		}
		set
		{
			__isset.attributionInfo = true;
			_attributionInfo = value;
		}
	}

	public string XmaGraphQL
	{
		get
		{
			return _xmaGraphQL;
		}
		set
		{
			__isset.xmaGraphQL = true;
			_xmaGraphQL = value;
		}
	}

	public ImageMetadata ImageMetadata
	{
		get
		{
			return _imageMetadata;
		}
		set
		{
			__isset.imageMetadata = true;
			_imageMetadata = value;
		}
	}

	public VideoMetadata VideoMetadata
	{
		get
		{
			return _videoMetadata;
		}
		set
		{
			__isset.videoMetadata = true;
			_videoMetadata = value;
		}
	}

	public string HaystackHandle
	{
		get
		{
			return _haystackHandle;
		}
		set
		{
			__isset.haystackHandle = true;
			_haystackHandle = value;
		}
	}

	public Dictionary<string, string> GenericMetadata
	{
		get
		{
			return _genericMetadata;
		}
		set
		{
			__isset.genericMetadata = true;
			_genericMetadata = value;
		}
	}

	public string Hash
	{
		get
		{
			return _hash;
		}
		set
		{
			__isset.hash = true;
			_hash = value;
		}
	}

	public string EncryptionKey
	{
		get
		{
			return _encryptionKey;
		}
		set
		{
			__isset.encryptionKey = true;
			_encryptionKey = value;
		}
	}

	public int TitanType
	{
		get
		{
			return _titanType;
		}
		set
		{
			__isset.titanType = true;
			_titanType = value;
		}
	}

	public THashSet<long> OtherUserFbIds
	{
		get
		{
			return _otherUserFbIds;
		}
		set
		{
			__isset.otherUserFbIds = true;
			_otherUserFbIds = value;
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
				if (tField.Type == TType.String)
				{
					Id = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 2:
				if (tField.Type == TType.String)
				{
					MimeType = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 3:
				if (tField.Type == TType.String)
				{
					Filename = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 4:
				if (tField.Type == TType.I64)
				{
					Fbid = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 5:
				if (tField.Type == TType.I64)
				{
					FileSize = iprot.ReadI64();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 6:
				if (tField.Type == TType.Struct)
				{
					AttributionInfo = new AttachmentAppAttribution();
					AttributionInfo.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 7:
				if (tField.Type == TType.String)
				{
					XmaGraphQL = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 10:
				if (tField.Type == TType.Struct)
				{
					ImageMetadata = new ImageMetadata();
					ImageMetadata.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 11:
				if (tField.Type == TType.Struct)
				{
					VideoMetadata = new VideoMetadata();
					VideoMetadata.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 1000:
				if (tField.Type == TType.String)
				{
					HaystackHandle = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 1001:
				if (tField.Type == TType.Map)
				{
					GenericMetadata = new Dictionary<string, string>();
					TMap tMap = iprot.ReadMapBegin();
					for (int j = 0; j < tMap.Count; j++)
					{
						string key = iprot.ReadString();
						string value = iprot.ReadString();
						GenericMetadata[key] = value;
					}
					iprot.ReadMapEnd();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 1002:
				if (tField.Type == TType.String)
				{
					Hash = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 1003:
				if (tField.Type == TType.String)
				{
					EncryptionKey = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 1004:
				if (tField.Type == TType.I32)
				{
					TitanType = iprot.ReadI32();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 1005:
				if (tField.Type == TType.Set)
				{
					OtherUserFbIds = new THashSet<long>();
					TSet tSet = iprot.ReadSetBegin();
					for (int i = 0; i < tSet.Count; i++)
					{
						long num = 0L;
						num = iprot.ReadI64();
						OtherUserFbIds.Add(num);
					}
					iprot.ReadSetEnd();
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
		TStruct struc = new TStruct("Attachment");
		oprot.WriteStructBegin(struc);
		TField field = default;
		if (Id != null && __isset.id)
		{
			field.Name = "id";
			field.Type = TType.String;
			field.ID = 1;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(Id);
			oprot.WriteFieldEnd();
		}
		if (MimeType != null && __isset.mimeType)
		{
			field.Name = "mimeType";
			field.Type = TType.String;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(MimeType);
			oprot.WriteFieldEnd();
		}
		if (Filename != null && __isset.filename)
		{
			field.Name = "filename";
			field.Type = TType.String;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(Filename);
			oprot.WriteFieldEnd();
		}
		if (__isset.fbid)
		{
			field.Name = "fbid";
			field.Type = TType.I64;
			field.ID = 4;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(Fbid);
			oprot.WriteFieldEnd();
		}
		if (__isset.fileSize)
		{
			field.Name = "fileSize";
			field.Type = TType.I64;
			field.ID = 5;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(FileSize);
			oprot.WriteFieldEnd();
		}
		if (AttributionInfo != null && __isset.attributionInfo)
		{
			field.Name = "attributionInfo";
			field.Type = TType.Struct;
			field.ID = 6;
			oprot.WriteFieldBegin(field);
			AttributionInfo.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (XmaGraphQL != null && __isset.xmaGraphQL)
		{
			field.Name = "xmaGraphQL";
			field.Type = TType.String;
			field.ID = 7;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(XmaGraphQL);
			oprot.WriteFieldEnd();
		}
		if (ImageMetadata != null && __isset.imageMetadata)
		{
			field.Name = "imageMetadata";
			field.Type = TType.Struct;
			field.ID = 10;
			oprot.WriteFieldBegin(field);
			ImageMetadata.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (VideoMetadata != null && __isset.videoMetadata)
		{
			field.Name = "videoMetadata";
			field.Type = TType.Struct;
			field.ID = 11;
			oprot.WriteFieldBegin(field);
			VideoMetadata.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (HaystackHandle != null && __isset.haystackHandle)
		{
			field.Name = "haystackHandle";
			field.Type = TType.String;
			field.ID = 1000;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(HaystackHandle);
			oprot.WriteFieldEnd();
		}
		if (GenericMetadata != null && __isset.genericMetadata)
		{
			field.Name = "genericMetadata";
			field.Type = TType.Map;
			field.ID = 1001;
			oprot.WriteFieldBegin(field);
			oprot.WriteMapBegin(new TMap(TType.String, TType.String, GenericMetadata.Count));
			foreach (string key in GenericMetadata.Keys)
			{
				oprot.WriteString(key);
				oprot.WriteString(GenericMetadata[key]);
			}
			oprot.WriteMapEnd();
			oprot.WriteFieldEnd();
		}
		if (Hash != null && __isset.hash)
		{
			field.Name = "hash";
			field.Type = TType.String;
			field.ID = 1002;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(Hash);
			oprot.WriteFieldEnd();
		}
		if (EncryptionKey != null && __isset.encryptionKey)
		{
			field.Name = "encryptionKey";
			field.Type = TType.String;
			field.ID = 1003;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(EncryptionKey);
			oprot.WriteFieldEnd();
		}
		if (__isset.titanType)
		{
			field.Name = "titanType";
			field.Type = TType.I32;
			field.ID = 1004;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32(TitanType);
			oprot.WriteFieldEnd();
		}
		if (OtherUserFbIds != null && __isset.otherUserFbIds)
		{
			field.Name = "otherUserFbIds";
			field.Type = TType.Set;
			field.ID = 1005;
			oprot.WriteFieldBegin(field);
			oprot.WriteSetBegin(new TSet(TType.I64, OtherUserFbIds.Count));
			foreach (long otherUserFbId in OtherUserFbIds)
			{
				oprot.WriteI64(otherUserFbId);
			}
			oprot.WriteSetEnd();
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("Attachment(");
		stringBuilder.Append("Id: ");
		stringBuilder.Append(Id);
		stringBuilder.Append(",MimeType: ");
		stringBuilder.Append(MimeType);
		stringBuilder.Append(",Filename: ");
		stringBuilder.Append(Filename);
		stringBuilder.Append(",Fbid: ");
		stringBuilder.Append(Fbid);
		stringBuilder.Append(",FileSize: ");
		stringBuilder.Append(FileSize);
		stringBuilder.Append(",AttributionInfo: ");
		stringBuilder.Append((AttributionInfo == null) ? "<null>" : AttributionInfo.ToString());
		stringBuilder.Append(",XmaGraphQL: ");
		stringBuilder.Append(XmaGraphQL);
		stringBuilder.Append(",ImageMetadata: ");
		stringBuilder.Append((ImageMetadata == null) ? "<null>" : ImageMetadata.ToString());
		stringBuilder.Append(",VideoMetadata: ");
		stringBuilder.Append((VideoMetadata == null) ? "<null>" : VideoMetadata.ToString());
		stringBuilder.Append(",HaystackHandle: ");
		stringBuilder.Append(HaystackHandle);
		stringBuilder.Append(",GenericMetadata: ");
		stringBuilder.Append(GenericMetadata);
		stringBuilder.Append(",Hash: ");
		stringBuilder.Append(Hash);
		stringBuilder.Append(",EncryptionKey: ");
		stringBuilder.Append(EncryptionKey);
		stringBuilder.Append(",TitanType: ");
		stringBuilder.Append(TitanType);
		stringBuilder.Append(",OtherUserFbIds: ");
		stringBuilder.Append(OtherUserFbIds);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
