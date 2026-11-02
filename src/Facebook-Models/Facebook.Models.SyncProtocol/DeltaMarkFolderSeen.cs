using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaMarkFolderSeen : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool folders;

			public bool timestamp;

			public bool irisSeqId;
		}

		private List<ThreadFolder> _folders;

		private long _timestamp;

		private long _irisSeqId;

		public Isset __isset;

		public List<ThreadFolder> Folders
		{
			get
			{
				return _folders;
			}
			set
			{
				__isset.folders = true;
				_folders = value;
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
						Folders = new List<ThreadFolder>();
						TList tList = iprot.ReadListBegin();
						for (int i = 0; i < tList.Count; i++)
						{
							ThreadFolder threadFolder = ThreadFolder.FOLDER_INVALID;
							threadFolder = (ThreadFolder)iprot.ReadI32();
							Folders.Add(threadFolder);
						}
						iprot.ReadListEnd();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.I64)
					{
						Timestamp = iprot.ReadI64();
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
			TStruct struc = new TStruct("DeltaMarkFolderSeen");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (Folders != null && __isset.folders)
			{
				field.Name = "folders";
				field.Type = TType.List;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteListBegin(new TList(TType.I32, Folders.Count));
				foreach (ThreadFolder folder in Folders)
				{
					oprot.WriteI32((int)folder);
				}
				oprot.WriteListEnd();
				oprot.WriteFieldEnd();
			}
			if (__isset.timestamp)
			{
				field.Name = "timestamp";
				field.Type = TType.I64;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(Timestamp);
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
			StringBuilder stringBuilder = new StringBuilder("DeltaMarkFolderSeen(");
			stringBuilder.Append("Folders: ");
			stringBuilder.Append(Folders);
			stringBuilder.Append(",Timestamp: ");
			stringBuilder.Append(Timestamp);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
