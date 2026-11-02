using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaFolderCount : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool count;

			public bool hasMore;

			public bool counts;

			public bool irisSeqId;
		}

		private int _count;

		private bool _hasMore;

		private Dictionary<FolderCountType, TagCount> _counts;

		private long _irisSeqId;

		public Isset __isset;

		public ThreadFolder ThreadFolder { get; set; }

		public int Count
		{
			get
			{
				return _count;
			}
			set
			{
				__isset.count = true;
				_count = value;
			}
		}

		public bool HasMore
		{
			get
			{
				return _hasMore;
			}
			set
			{
				__isset.hasMore = true;
				_hasMore = value;
			}
		}

		public Dictionary<FolderCountType, TagCount> Counts
		{
			get
			{
				return _counts;
			}
			set
			{
				__isset.counts = true;
				_counts = value;
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

		public DeltaFolderCount()
		{
		}

		public DeltaFolderCount(ThreadFolder threadFolder)
			: this()
		{
			ThreadFolder = threadFolder;
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
					if (tField.Type == TType.I32)
					{
						ThreadFolder = (ThreadFolder)iprot.ReadI32();
						flag = true;
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.I32)
					{
						Count = iprot.ReadI32();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.Bool)
					{
						HasMore = iprot.ReadBool();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 4:
					if (tField.Type == TType.Map)
					{
						Counts = new Dictionary<FolderCountType, TagCount>();
						TMap tMap = iprot.ReadMapBegin();
						for (int i = 0; i < tMap.Count; i++)
						{
							FolderCountType key = (FolderCountType)iprot.ReadI32();
							TagCount tagCount = new TagCount();
							tagCount.Read(iprot);
							Counts[key] = tagCount;
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
			TStruct struc = new TStruct("DeltaFolderCount");
			oprot.WriteStructBegin(struc);
			TField field = new TField
			{
				Name = "threadFolder",
				Type = TType.I32,
				ID = 1
			};
			oprot.WriteFieldBegin(field);
			oprot.WriteI32((int)ThreadFolder);
			oprot.WriteFieldEnd();
			if (__isset.count)
			{
				field.Name = "count";
				field.Type = TType.I32;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteI32(Count);
				oprot.WriteFieldEnd();
			}
			if (__isset.hasMore)
			{
				field.Name = "hasMore";
				field.Type = TType.Bool;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(HasMore);
				oprot.WriteFieldEnd();
			}
			if (Counts != null && __isset.counts)
			{
				field.Name = "counts";
				field.Type = TType.Map;
				field.ID = 4;
				oprot.WriteFieldBegin(field);
				oprot.WriteMapBegin(new TMap(TType.I32, TType.Struct, Counts.Count));
				foreach (FolderCountType key in Counts.Keys)
				{
					oprot.WriteI32((int)key);
					Counts[key].Write(oprot);
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
			StringBuilder stringBuilder = new StringBuilder("DeltaFolderCount(");
			stringBuilder.Append("ThreadFolder: ");
			stringBuilder.Append(ThreadFolder);
			stringBuilder.Append(",Count: ");
			stringBuilder.Append(Count);
			stringBuilder.Append(",HasMore: ");
			stringBuilder.Append(HasMore);
			stringBuilder.Append(",Counts: ");
			stringBuilder.Append(Counts);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
